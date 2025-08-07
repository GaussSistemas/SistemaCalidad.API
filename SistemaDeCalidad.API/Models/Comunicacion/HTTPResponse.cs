using System.Data;

namespace SistemaDeCalidad.API.Models.Comunicacion
{
    public class HTTPResponse
    {
        public bool Success { get; set; }
        public bool BadRequest { get; set; }
        public bool NotFound { get; set; }
        public string Response { get; set; }
        public DataTable Data { get; set; }
        public int Status { get; set; }
        public string Dsn { get; set; }
        public string Message { get; set; }
        public List<Column> Estructura { get; set; }
        public string Error { get; set; }
        public byte[] File { get; set; }

        public class Column
        {
            public string Name { get; set; }
            public Type DataType { get; set; }
            public string ValorNeutro { get; set; }
        }
    }
}
