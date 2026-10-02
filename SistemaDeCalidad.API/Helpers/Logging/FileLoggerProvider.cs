using System.Collections.Concurrent;
using System.Text;

namespace SistemaDeCalidad.API.Helpers.Logging
{
    public sealed class FileLoggerProvider : ILoggerProvider
    {
        private readonly string _carpeta;
        private readonly object _lock = new();
        private readonly ConcurrentDictionary<string, FileLogger> _loggers = new();

        public FileLoggerProvider(string carpeta)
        {
            _carpeta = carpeta;
            try
            {
                Directory.CreateDirectory(_carpeta);
            }
            catch
            {
            }
        }

        public ILogger CreateLogger(string categoryName) =>
            _loggers.GetOrAdd(categoryName, nombre => new FileLogger(nombre, this));

        internal void Escribir(string linea)
        {
            try
            {
                var archivo = Path.Combine(_carpeta, $"SistemaDeCalidad-{DateTime.Now:yyyyMMdd}.log");
                lock (_lock)
                {
                    File.AppendAllText(archivo, linea, Encoding.UTF8);
                }
            }
            catch
            {
            }
        }

        public void Dispose() => _loggers.Clear();

        private sealed class FileLogger : ILogger
        {
            private readonly string _categoria;
            private readonly FileLoggerProvider _provider;

            public FileLogger(string categoria, FileLoggerProvider provider)
            {
                _categoria = categoria;
                _provider = provider;
            }

            public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

            public bool IsEnabled(LogLevel logLevel) => logLevel != LogLevel.None;

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
            {
                if (!IsEnabled(logLevel))
                    return;

                var sb = new StringBuilder()
                    .Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"))
                    .Append(" [").Append(logLevel.ToString().ToUpperInvariant()).Append("] ")
                    .Append(_categoria).Append(": ")
                    .AppendLine(formatter(state, exception));

                if (exception != null)
                    sb.AppendLine(exception.ToString());

                _provider.Escribir(sb.ToString());
            }
        }
    }
}
