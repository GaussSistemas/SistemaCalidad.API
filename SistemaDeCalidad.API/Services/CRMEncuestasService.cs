using Microsoft.Extensions.Options;
using SistemaDeCalidad.API.DTOs.Input;
using SistemaDeCalidad.API.DTOs.Output;
using SistemaDeCalidad.API.Interfaces.Repositories;
using SistemaDeCalidad.API.Interfaces.Services;
using SistemaDeCalidad.API.Models;
using SistemaDeCalidad.API.Models.Encuestas;
using SistemaDeCalidad.API.Models.Soportes;
using System.Diagnostics;
using System.Globalization;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace SistemaDeCalidad.API.Services
{
    public class CRMEncuestasService : ICRMEncuestasService
    {
        public const string HttpClientName = "CRMEncuestas";

        private const int MaximoCaracteresBodyEnLog = 500;

        private const string CampoAtencion = "atencion";
        private const string CampoTiempoRespuesta = "tiempoRespuesta";
        private const string CampoRespuestaClara = "respuestaClara";
        private const string CampoRespuestaClaraComentario = "respuestaClaraComentario";
        private const string CampoContactoPrevio = "contactoPrevio";
        private const string CampoResuelto = "resuelto";
        private const string CampoComentario = "comentario";

        private static readonly string[] CamposRequeridos =
        {
            CampoAtencion, CampoTiempoRespuesta, CampoRespuestaClara,
            CampoRespuestaClaraComentario, CampoContactoPrevio, CampoComentario
        };

        private static readonly JsonSerializerOptions JsonOptions =
            new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IEncuestasRepository _encuestasRepository;
        private readonly CRMConfiguration _crmConfiguration;
        private readonly ILogger<CRMEncuestasService> _logger;

        public CRMEncuestasService(
            IHttpClientFactory httpClientFactory,
            IEncuestasRepository encuestasRepository,
            IOptions<CRMConfiguration> crmConfiguration,
            ILogger<CRMEncuestasService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _encuestasRepository = encuestasRepository;
            _crmConfiguration = crmConfiguration.Value;
            _logger = logger;
        }

        /// <summary>Publishes the survey to the CRM once its last question is answered. Never throws.</summary>
        public async Task<bool> PublicarSiFinalizo(EncuestaRespuesta respuesta, Soporte soporte, Encuesta encuesta)
        {
            try
            {
                if (!_crmConfiguration.Habilitado || string.IsNullOrWhiteSpace(_crmConfiguration.IngestaURL))
                {
                    _logger.LogDebug("Push al CRM deshabilitado (Habilitado={Habilitado}, IngestaURL vacía={SinUrl}). Hash {Hash}",
                        _crmConfiguration.Habilitado, string.IsNullOrWhiteSpace(_crmConfiguration.IngestaURL), respuesta?.Hash);
                    return false;
                }

                if (respuesta == null || soporte == null)
                {
                    _logger.LogWarning("No se puede publicar en el CRM sin respuesta y soporte.");
                    return false;
                }

                var ultimaPregunta = _encuestasRepository.UltimaPreguntaEncuesta(respuesta.EncuestaId);
                var finalizo = ultimaPregunta != null &&
                    (respuesta.RespuestasPreguntas ?? new List<EncuestaRespuestaPregunta>())
                        .Any(rp => rp != null && rp.EncuestaPreguntaId == ultimaPregunta.Id);
                if (!finalizo)
                {
                    _logger.LogDebug("Respuesta parcial, todavía no se publica en el CRM. Hash {Hash}", respuesta.Hash);
                    return false;
                }

                var mapeo = await ObtenerMapeo(respuesta.Hash);
                if (mapeo == null)
                    return false;

                var faltantes = CamposRequeridos
                    .Where(campo => !mapeo.ContainsKey(campo))
                    .ToList();
                if (faltantes.Count > 0)
                {
                    _logger.LogError("Mapeo de preguntas incompleto en el CRM: faltan {Campos} en encuesta_mapeo_preguntas. No se publica la encuesta. Hash {Hash}",
                        string.Join(", ", faltantes), respuesta.Hash);
                    return false;
                }

                var payload = ArmarPayload(respuesta, soporte, encuesta, mapeo);
                return await Postear(payload);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado publicando la encuesta en el CRM. Hash {Hash}", respuesta?.Hash);
                return false;
            }
        }

        private EncuestaCRMOutput ArmarPayload(EncuestaRespuesta respuesta, Soporte soporte, Encuesta encuesta,
            Dictionary<string, CRMMapeoPreguntaInput> mapeo)
        {
            var cabecera = _encuestasRepository.CabeceraRespuesta(respuesta.Hash) ?? respuesta;
            var respuestas = _encuestasRepository.RespuestasPreguntas(respuesta.Hash);
            if (respuestas == null || respuestas.Count == 0)
                respuestas = respuesta.RespuestasPreguntas ?? new List<EncuestaRespuestaPregunta>();
            var adicionales = _encuestasRepository.RespuestasAdicionales(respuesta.Hash)
                ?? new List<EncuestaRespuestaAdicional>();

            var opciones = ResolverOpciones(mapeo.Values
                .Where(fila => !EsAdicional(fila))
                .Select(fila => BuscarRespuesta(respuestas, fila.EncuestaPreguntaId)));

            string? Valor(string campo)
            {
                if (!mapeo.TryGetValue(campo, out var fila))
                    return null;

                return EsAdicional(fila)
                    ? ResolverValorAdicional(BuscarAdicional(adicionales, fila.EncuestaPreguntaId, fila.PreguntaAdicionalId!.Value), campo)
                    : ResolverValor(BuscarRespuesta(respuestas, fila.EncuestaPreguntaId), opciones, campo);
            }

            return new EncuestaCRMOutput
            {
                OrigenHash = respuesta.Hash?.Trim(),
                OrigenTicket = $"{soporte.TipoId?.Trim()}-{(int)soporte.Numero}",
                Titulo = soporte.Tarea?.Trim(),
                IdCliente = soporte.ClienteId?.Trim(),
                NombreCliente = soporte.NombreCliente?.Trim(),
                Respondente = ArmarRespondente(cabecera, soporte),
                MailRespondente = soporte.MailUsuario?.Trim(),
                FechaHora = FormatearFechaHora(cabecera),
                Responsable = soporte.Responsable?.Trim(),
                Atencion = Valor(CampoAtencion),
                TiempoRespuesta = Valor(CampoTiempoRespuesta),
                RespuestaClara = Valor(CampoRespuestaClara),
                RespuestaClaraComentario = Valor(CampoRespuestaClaraComentario),
                ContactoPrevio = Valor(CampoContactoPrevio),
                Resuelto = Valor(CampoResuelto),
                Comentario = Valor(CampoComentario),
                TipoEncuesta = string.IsNullOrWhiteSpace(encuesta?.Titulo) ? "Encuesta de soporte" : encuesta.Titulo.Trim()
            };
        }

        private static EncuestaRespuestaPregunta? BuscarRespuesta(List<EncuestaRespuestaPregunta> respuestas, int preguntaId)
        {
            if (preguntaId <= 0)
                return null;

            return respuestas.FirstOrDefault(r => r != null && r.EncuestaPreguntaId == preguntaId);
        }

        private static EncuestaRespuestaAdicional? BuscarAdicional(List<EncuestaRespuestaAdicional> adicionales, int encuestaPreguntaId, int preguntaAdicionalId)
        {
            if (preguntaAdicionalId <= 0)
                return null;

            return adicionales.FirstOrDefault(r => r != null
                && r.EncuestaPreguntaId == encuestaPreguntaId
                && r.PreguntaAdicionalId == preguntaAdicionalId);
        }

        private static bool EsAdicional(CRMMapeoPreguntaInput fila) =>
            fila.PreguntaAdicionalId.HasValue && fila.PreguntaAdicionalId.Value > 0;

        private string? ResolverValorAdicional(EncuestaRespuestaAdicional? adicional, string campo)
        {
            if (adicional == null)
            {
                _logger.LogDebug("Pregunta adicional mapeada {Campo} sin respuesta en la encuesta.", campo);
                return null;
            }

            if (!string.IsNullOrWhiteSpace(adicional.Valor))
                return adicional.Valor.Trim();

            if (adicional.PreguntaAdicionalOpcionId <= 0)
                return null;

            var opcion = _encuestasRepository.OpcionesAdicionales(new List<int> { adicional.PreguntaAdicionalId })
                ?.FirstOrDefault(o => o != null && o.Id == adicional.PreguntaAdicionalOpcionId);
            if (opcion == null)
            {
                _logger.LogWarning("No se encontró la opción {OpcionId} de la pregunta adicional {PreguntaAdicionalId} ({Campo}).",
                    adicional.PreguntaAdicionalOpcionId, adicional.PreguntaAdicionalId, campo);
                return null;
            }

            return opcion.Opcion?.Trim();
        }

        private static string? ArmarRespondente(EncuestaRespuesta respuesta, Soporte soporte)
        {
            var partes = new[] { respuesta.NombreUsuario, respuesta.ApellidoUsuario }
                .Where(parte => !string.IsNullOrWhiteSpace(parte))
                .Select(parte => parte.Trim());

            var nombre = string.Join(" ", partes);

            return string.IsNullOrWhiteSpace(nombre) ? soporte?.UsuarioCliente?.Trim() : nombre;
        }

        /// <summary>Loads in a single query the options of mapped answers that have no text value.</summary>
        private List<EncuestaPreguntaOpcion> ResolverOpciones(IEnumerable<EncuestaRespuestaPregunta?> mapeadas)
        {
            var preguntasIds = mapeadas
                .OfType<EncuestaRespuestaPregunta>()
                .Where(r => string.IsNullOrWhiteSpace(r.Valor) && r.EncuestaPreguntaOpcionId > 0)
                .Select(r => r.EncuestaPreguntaId)
                .Distinct()
                .ToList();

            if (preguntasIds.Count == 0)
                return new List<EncuestaPreguntaOpcion>();

            return _encuestasRepository.OpcionesPreguntas(preguntasIds) ?? new List<EncuestaPreguntaOpcion>();
        }

        private string? ResolverValor(EncuestaRespuestaPregunta? respuestaPregunta, List<EncuestaPreguntaOpcion> opciones, string campo)
        {
            if (respuestaPregunta == null)
            {
                _logger.LogDebug("Pregunta mapeada {Campo} sin respuesta en la encuesta.", campo);
                return null;
            }

            if (!string.IsNullOrWhiteSpace(respuestaPregunta.Valor))
                return respuestaPregunta.Valor.Trim();

            if (respuestaPregunta.EncuestaPreguntaOpcionId <= 0)
                return null;

            var opcion = opciones?.FirstOrDefault(o => o != null && o.Id == respuestaPregunta.EncuestaPreguntaOpcionId);
            if (opcion == null)
            {
                _logger.LogWarning("No se encontró la opción {OpcionId} de la pregunta {PreguntaId} ({Campo}).",
                    respuestaPregunta.EncuestaPreguntaOpcionId, respuestaPregunta.EncuestaPreguntaId, campo);
                return null;
            }

            return opcion.Opcion?.Trim();
        }

        /// <summary>Formats the response date and time as ISO 8601 with the configured UTC offset.</summary>
        private string FormatearFechaHora(EncuestaRespuesta respuesta)
        {
            var hora = respuesta.Hora;
            if (hora < TimeSpan.Zero || hora >= TimeSpan.FromDays(1))
                hora = respuesta.Fecha.TimeOfDay;

            var local = DateTime.SpecifyKind(respuesta.Fecha.Date.Add(hora), DateTimeKind.Unspecified);
            var offset = TimeSpan.FromHours(_crmConfiguration.OffsetHorasUTC);

            return new DateTimeOffset(local, offset)
                .ToString("yyyy-MM-ddTHH:mm:sszzz", CultureInfo.InvariantCulture);
        }

        /// <summary>Fetches the question-to-field mapping from the CRM; returns null if it can't be retrieved.</summary>
        private async Task<Dictionary<string, CRMMapeoPreguntaInput>?> ObtenerMapeo(string? hash)
        {
            const string sufijoIngesta = "/ingesta";
            var ingestaURL = _crmConfiguration.IngestaURL!.Trim().TrimEnd('/');
            if (!ingestaURL.EndsWith(sufijoIngesta, StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogError("No se puede derivar la URL del mapeo: IngestaURL '{Url}' no termina en '{Sufijo}'. No se publica la encuesta. Hash {Hash}",
                    _crmConfiguration.IngestaURL, sufijoIngesta, hash);
                return null;
            }
            var mapeoURL = ingestaURL.Substring(0, ingestaURL.Length - sufijoIngesta.Length) + "/mapeo";

            try
            {
                var client = _httpClientFactory.CreateClient(HttpClientName);
                using var request = new HttpRequestMessage(HttpMethod.Get, mapeoURL);
                if (!AgregarToken(request, hash, string.Empty))
                    return null;

                using var response = await client.SendAsync(request, CancellationToken.None);
                var body = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("El CRM rechazó la lectura del mapeo de preguntas. No se publica la encuesta. Hash {Hash} URL {Url} Status {Status} Body {Body}",
                        hash, mapeoURL, (int)response.StatusCode, Truncar(body));
                    return null;
                }

                var filas = JsonSerializer.Deserialize<List<CRMMapeoPreguntaInput>>(body, JsonOptions)
                    ?? new List<CRMMapeoPreguntaInput>();

                var mapeo = new Dictionary<string, CRMMapeoPreguntaInput>(StringComparer.OrdinalIgnoreCase);
                foreach (var fila in filas.Where(f => f != null && !string.IsNullOrWhiteSpace(f.CampoCrm) && f.EncuestaPreguntaId > 0))
                    mapeo[fila.CampoCrm!.Trim()] = fila;

                return mapeo;
            }
            catch (TaskCanceledException)
            {
                _logger.LogError("Timeout ({Segundos}s) leyendo el mapeo de preguntas del CRM. No se publica la encuesta. Hash {Hash} URL {Url}",
                    _crmConfiguration.TimeoutSegundos, hash, mapeoURL);
                return null;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error de red leyendo el mapeo de preguntas del CRM. No se publica la encuesta. Hash {Hash} URL {Url}",
                    hash, mapeoURL);
                return null;
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "El mapeo de preguntas del CRM no es un JSON válido. No se publica la encuesta. Hash {Hash} URL {Url}",
                    hash, mapeoURL);
                return null;
            }
        }

        /// <summary>Adds the bearer token to the request; returns false if the token is invalid.</summary>
        private bool AgregarToken(HttpRequestMessage request, string? hash, string payloadParaLog)
        {
            if (string.IsNullOrWhiteSpace(_crmConfiguration.Token))
                return true;

            var token = _crmConfiguration.Token.Trim();
            if (!token.All(char.IsAscii))
            {
                _logger.LogError("El Token del CRM tiene caracteres no ASCII (largo {Largo}). Revisar CRMConfiguration:Token en el appsettings. Hash {Hash}{Payload}",
                    token.Length, hash, payloadParaLog);
                return false;
            }

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return true;
        }

        private async Task<bool> Postear(EncuestaCRMOutput payload)
        {
            var json = JsonSerializer.Serialize(payload, JsonOptions);
            var cronometro = Stopwatch.StartNew();

            try
            {
                var client = _httpClientFactory.CreateClient(HttpClientName);

                using var request = new HttpRequestMessage(HttpMethod.Post, _crmConfiguration.IngestaURL)
                {
                    Content = new StringContent(json, Encoding.UTF8, "application/json")
                };

                if (!AgregarToken(request, payload.OrigenHash, PayloadParaLog(json)))
                    return false;

                using var response = await client.SendAsync(request, CancellationToken.None);
                cronometro.Stop();

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Encuesta publicada en el CRM. Hash {Hash} Ticket {Ticket} Status {Status} Duracion {Ms}ms",
                        payload.OrigenHash, payload.OrigenTicket, (int)response.StatusCode, cronometro.ElapsedMilliseconds);
                    return true;
                }

                var body = await response.Content.ReadAsStringAsync();
                _logger.LogWarning("El CRM rechazó la ingesta. Hash {Hash} Ticket {Ticket} Status {Status} Body {Body}{Payload}",
                    payload.OrigenHash, payload.OrigenTicket, (int)response.StatusCode, Truncar(body), PayloadParaLog(json));
                return false;
            }
            catch (TaskCanceledException)
            {
                cronometro.Stop();
                _logger.LogWarning("Timeout ({Segundos}s) publicando la encuesta en el CRM. Hash {Hash}{Payload}",
                    _crmConfiguration.TimeoutSegundos, payload.OrigenHash, PayloadParaLog(json));
                return false;
            }
            catch (HttpRequestException ex)
            {
                cronometro.Stop();
                _logger.LogWarning(ex, "Error de red publicando la encuesta en el CRM. Hash {Hash} URL {Url}{Payload}",
                    payload.OrigenHash, _crmConfiguration.IngestaURL, PayloadParaLog(json));
                return false;
            }
        }

        private string PayloadParaLog(string json)
        {
            return _crmConfiguration.LoguearPayloadEnFallo ? $" Payload {json}" : string.Empty;
        }

        private static string? Truncar(string? texto)
        {
            if (string.IsNullOrEmpty(texto) || texto.Length <= MaximoCaracteresBodyEnLog)
                return texto;

            return texto.Substring(0, MaximoCaracteresBodyEnLog) + "...";
        }
    }
}
