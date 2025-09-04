using Microsoft.Extensions.Options;
using SistemaDeCalidad.API.Interfaces.Repositories;
using SistemaDeCalidad.API.Models;
using SistemaDeCalidad.API.Models.Comunicacion;
using SistemaDeCalidad.API.Models.Encuestas;
using SistemaDeCalidad.API.Repositories.NonQueries;
using SistemaDeCalidad.API.Repositories.Queries;
using SistemaDeCalidad.API.Services;
using System.Data.Odbc;

namespace SistemaDeCalidad.API.Repositories
{
    public class EncuestasRepository : IEncuestasRepository
    {
        private readonly SistemaDeCalidadConfiguration _sgcConfigurations;
        public EncuestasRepository(IOptions<SistemaDeCalidadConfiguration> sgcConfigurations)
        {
            _sgcConfigurations = sgcConfigurations.Value;
        }

        public Encuesta EncuestaSegunId(int id)
        {
            var queryExecutor = new QueryExecutor()
            {
                Consulta = Queries.Encuestas.EncuestaSegunId(id),
                DSNs = _sgcConfigurations.SistemaDeCalidadDSNs,
                URL = _sgcConfigurations.SistemaDeCalidadQueryExecutorURL,
                Parametros = new List<(string, object, OdbcType)>()
            };
            var encuesta = EiffelService.SendToDBFDatabase<Encuesta>(queryExecutor);
            return encuesta;
        }

        public Encuesta EncuestaActiva()
        {
            var queryExecutor = new QueryExecutor()
            {
                Consulta = Queries.Encuestas.EncuestaActiva(),
                DSNs = _sgcConfigurations.SistemaDeCalidadDSNs,
                URL = _sgcConfigurations.SistemaDeCalidadQueryExecutorURL,
                Parametros = new List<(string, object, OdbcType)>()
            };
            var encuesta = EiffelService.SendToDBFDatabase<Encuesta>(queryExecutor);
            return encuesta;
        }

        public List<EncuestaPregunta> PreguntasEncuesta(int encuestaId)
        {
            var queryExecutor = new QueryExecutor()
            {
                Consulta = Queries.Encuestas.PreguntasEncuesta(encuestaId),
                DSNs = _sgcConfigurations.SistemaDeCalidadDSNs,
                URL = _sgcConfigurations.SistemaDeCalidadQueryExecutorURL,
                Parametros = new List<(string, object, OdbcType)>()
            };

            var preguntas = EiffelService.SendToDBFDatabase<List<EncuestaPregunta>>(queryExecutor);
            return preguntas;
        }

        public EncuestaPregunta PreguntaSegunId(int id)
        {
            var queryExecutor = new QueryExecutor()
            {
                Consulta = Queries.Encuestas.PreguntaSegunId(id),
                DSNs = _sgcConfigurations.SistemaDeCalidadDSNs,
                URL = _sgcConfigurations.SistemaDeCalidadQueryExecutorURL,
                Parametros = new List<(string, object, OdbcType)>()
            };
            var pregunta = EiffelService.SendToDBFDatabase<EncuestaPregunta>(queryExecutor);
            return pregunta;
        }

        public List<EncuestaPreguntaOpcion> OpcionesPreguntas(List<int> preguntasIds)
        {
            var queryExecutor = new QueryExecutor()
            {
                Consulta = Queries.Encuestas.OpcionesPreguntas(preguntasIds),
                DSNs = _sgcConfigurations.SistemaDeCalidadDSNs,
                URL = _sgcConfigurations.SistemaDeCalidadQueryExecutorURL,
                Parametros = new List<(string, object, OdbcType)>()
            };
            var opciones = EiffelService.SendToDBFDatabase<List<EncuestaPreguntaOpcion>>(queryExecutor);
            return opciones;
        }

        public EncuestaPreguntaOpcion OpcionSegunId(int id)
        {
            var queryExecutor = new QueryExecutor()
            {
                Consulta = Queries.Encuestas.OpcionSegunId(id),
                DSNs = _sgcConfigurations.SistemaDeCalidadDSNs,
                URL = _sgcConfigurations.SistemaDeCalidadQueryExecutorURL,
                Parametros = new List<(string, object, OdbcType)>()
            };
            var opcion = EiffelService.SendToDBFDatabase<EncuestaPreguntaOpcion>(queryExecutor);
            return opcion;
        }

        public List<EncuestaTipoControl> ControlesEncuestas()
        {
            var queryExecutor = new QueryExecutor()
            {
                Consulta = Queries.Encuestas.ControlesEncuesta(),
                DSNs = _sgcConfigurations.SistemaDeCalidadDSNs,
                URL = _sgcConfigurations.SistemaDeCalidadQueryExecutorURL,
                Parametros = new List<(string, object, OdbcType)>()
            };
            var controles = EiffelService.SendToDBFDatabase<List<EncuestaTipoControl>>(queryExecutor);
            return controles;
        }

        public EncuestaRespuestaPregunta RespuestaPregunta(string hash, int preguntaId)
        {
            var queryExecutor = new QueryExecutor()
            {
                Consulta = Queries.Encuestas.RespuestaContestada(hash, preguntaId),
                DSNs = _sgcConfigurations.SistemaDeCalidadDSNs,
                URL = _sgcConfigurations.SistemaDeCalidadQueryExecutorURL,
                Parametros = new List<(string, object, OdbcType)>()
            };
            var respuesta = EiffelService.SendToDBFDatabase<EncuestaRespuestaPregunta>(queryExecutor);
            return respuesta;
        }

        public EncuestaPregunta UltimaPreguntaEncuesta(int encuestaId)
        {
            var queryExecutor = new QueryExecutor()
            {
                Consulta = Queries.Encuestas.UltimaPreguntaEncuesta(encuestaId),
                DSNs = _sgcConfigurations.SistemaDeCalidadDSNs,
                URL = _sgcConfigurations.SistemaDeCalidadQueryExecutorURL,
                Parametros = new List<(string, object, OdbcType)>()
            };
            var pregunta = EiffelService.SendToDBFDatabase<EncuestaPregunta>(queryExecutor);
            return pregunta;
        }

        public bool EncuestaContestada(string id)
        {
            var queryExecutor = new QueryExecutor()
            {
                Consulta = Queries.Encuestas.EncuestaContestada(id),
                DSNs = _sgcConfigurations.SistemaDeCalidadDSNs,
                URL = _sgcConfigurations.SistemaDeCalidadQueryExecutorURL,
                Parametros = new List<(string, object, OdbcType)>()
            };

            var encuesta = EiffelService.SendToDBFDatabase<EncuestaRespuesta>(queryExecutor);
            return encuesta != null && !string.IsNullOrEmpty(encuesta.Hash);
        }

        public bool EncuestaConRespuestas(int id)
        {
            var queryExecutor = new QueryExecutor()
            {
                Consulta = Queries.Encuestas.EncuestaConRespuestas(id),
                DSNs = _sgcConfigurations.SistemaDeCalidadDSNs,
                URL = _sgcConfigurations.SistemaDeCalidadQueryExecutorURL,
                Parametros = new List<(string, object, OdbcType)>()
            };

            var respuestas = EiffelService.SendToDBFDatabase<List<EncuestaRespuesta>>(queryExecutor);
            return respuestas != null && respuestas.Count() > 0;
        }

        public bool GrabarRespuesta(EncuestaRespuesta respuestaEncuesta, string tipoId, int numero)
        {
            var ultimaPregunta = UltimaPreguntaEncuesta(respuestaEncuesta.EncuestaId);
            var idCabecera = ProximoId(respuestaEncuesta.NombreTabla());

            var parametros = new List<(string tabla, List<(String, Object, OdbcType)> parametros)>();
            parametros.Add(NonQueries.Encuestas.ParametrosCabeceraRespuesta(idCabecera, respuestaEncuesta));

            var idRespuestaPregunta = 0;
            foreach (var respuestaPregunta in respuestaEncuesta.RespuestasPreguntas)
            {
                if (idRespuestaPregunta == 0)
                    idRespuestaPregunta = ProximoId(respuestaPregunta.NombreTabla());
                else
                    idRespuestaPregunta++;

                parametros.Add(NonQueries.Encuestas.ParametrosRespuestaPregunta(idRespuestaPregunta, respuestaEncuesta.Hash, respuestaPregunta));
            }

            if(ultimaPregunta != null && respuestaEncuesta.RespuestasPreguntas.Any(rp => rp.EncuestaPreguntaId == ultimaPregunta.Id))
                parametros.Add(GrabarLog(tipoId, numero));

            var statements = EiffelService.ArmarStatements(parametros, _sgcConfigurations.SistemaDeCalidadQueryExecutorURL, _sgcConfigurations.SistemaDeCalidadDSNs);
            var resultado = EiffelService.SendTransactionToDBFDatabase(statements, _sgcConfigurations.SistemaDeCalidadQueryExecutorURL, _sgcConfigurations.SistemaDeCalidadDSNs);

            return resultado;
        }

        public bool ActualizarRespuesta(EncuestaRespuesta respuestaEncuesta, string tipoId, int numero)
        {
            var ultimaPregunta = UltimaPreguntaEncuesta(respuestaEncuesta.EncuestaId);

            var statements = new List<(string statement, List<(string, object, OdbcType)> parametros)>();
            var resultado = false;
            var preguntaContestada = RespuestaPregunta(respuestaEncuesta.Hash, respuestaEncuesta.RespuestasPreguntas.First().EncuestaPreguntaId);

            if (preguntaContestada != null)
            {
                var parametrosUpdate = new List<(string tabla, List<(String, Object)> parametros, List<(String, Object)> filtros)>();
                preguntaContestada.EncuestaPreguntaOpcionId = respuestaEncuesta.RespuestasPreguntas.First().EncuestaPreguntaOpcionId;
                preguntaContestada.Valor = respuestaEncuesta.RespuestasPreguntas.First().Valor;
                parametrosUpdate.Add(NonQueries.Encuestas.ParametrosUpdateRespuestaPregunta(preguntaContestada));
                statements = EiffelService.ArmarStatementUpdate(parametrosUpdate, _sgcConfigurations.SistemaDeCalidadQueryExecutorURL, _sgcConfigurations.SistemaDeCalidadDSNs);
            }
            else
            {
                var parametros = new List<(string tabla, List<(String, Object, OdbcType)> parametros)>();
                var idRespuestaPregunta = ProximoId(respuestaEncuesta.RespuestasPreguntas.First().NombreTabla());
                parametros.Add(NonQueries.Encuestas.ParametrosRespuestaPregunta(idRespuestaPregunta, respuestaEncuesta.Hash, respuestaEncuesta.RespuestasPreguntas.First()));
                statements = EiffelService.ArmarStatements(parametros, _sgcConfigurations.SistemaDeCalidadQueryExecutorURL, _sgcConfigurations.SistemaDeCalidadDSNs);
            }

            if (ultimaPregunta != null && respuestaEncuesta.RespuestasPreguntas.Any(rp => rp.EncuestaPreguntaId == ultimaPregunta.Id))
            {
                var logParametros = new List<(string tabla, List<(String, Object, OdbcType)> parametros)>();
                logParametros.Add(GrabarLog(tipoId, numero));
                var logStatement = EiffelService.ArmarStatements(logParametros, _sgcConfigurations.SistemaDeCalidadQueryExecutorURL, _sgcConfigurations.SistemaDeCalidadDSNs);
                statements.AddRange(logStatement);
            }

            resultado = EiffelService.SendTransactionToDBFDatabase(statements, _sgcConfigurations.SistemaDeCalidadQueryExecutorURL, _sgcConfigurations.SistemaDeCalidadDSNs);
            return resultado;
        }

        public (string tabla, List<(String, Object, OdbcType)> parametros) GrabarLog(string tipoId, int numero)
        {
            var log = new EncuestaLog()
            {
                UsuarioId = "AUT",
                EstadoId = "FIN",
                Observacion = "ENCUESTA FINALIZADA",
                Fecha = DateTime.Now,
                Hora = DateTime.Now.ToString("HH:mm:ss"),
                Numero = numero,
                SoporteId = tipoId
            };
            return NonQueries.Encuestas.ParametrosLog(log);
        }

        public bool GrabarEncuesta(Encuesta encuesta)
        {
            var idEncuesta = ProximoId(encuesta.NombreTabla());
            var parametros = new List<(string tabla, List<(String, Object, OdbcType)> parametros)>();
            parametros.Add(NonQueries.Encuestas.ParametrosEncuesta(idEncuesta, encuesta));

            var idPregunta = 0;
            var idOpcion = 0;
            foreach (var pregunta in encuesta.Preguntas)
            {
                if (idPregunta == 0)
                    idPregunta = ProximoId(pregunta.NombreTabla());
                else
                    idPregunta++;

                pregunta.EncuestaId = idEncuesta;
                parametros.Add(NonQueries.Encuestas.ParametrosEncuestaPregunta(idPregunta, pregunta));
                if (pregunta.Opciones != null && pregunta.Opciones.Count > 0)
                    foreach (var opcion in pregunta.Opciones)
                    {
                        if (idOpcion == 0)
                            idOpcion = ProximoId(opcion.NombreTabla());
                        else
                            idOpcion++;

                        opcion.EncuestaPreguntaId = idPregunta;
                        parametros.Add(NonQueries.Encuestas.ParametrosEncuestaPreguntaOpcion(idOpcion, opcion));
                    }
            }

            var statements = EiffelService.ArmarStatements(parametros, _sgcConfigurations.SistemaDeCalidadQueryExecutorURL, _sgcConfigurations.SistemaDeCalidadDSNs);
            var resultado = EiffelService.SendTransactionToDBFDatabase(statements, _sgcConfigurations.SistemaDeCalidadQueryExecutorURL, _sgcConfigurations.SistemaDeCalidadDSNs);

            return resultado;
        }

        public bool GrabarPregunta(EncuestaPregunta pregunta)
        {
            var idPregunta = ProximoId(pregunta.NombreTabla());
            var parametros = new List<(string tabla, List<(String, Object, OdbcType)> parametros)>();
            parametros.Add(NonQueries.Encuestas.ParametrosEncuestaPregunta(idPregunta, pregunta));

            var idOpcion = 0;
            if (pregunta.Opciones != null && pregunta.Opciones.Count > 0)
                foreach (var opcion in pregunta.Opciones)
                {
                    if (idOpcion == 0)
                        idOpcion = ProximoId(opcion.NombreTabla());
                    else
                        idOpcion++;

                    opcion.EncuestaPreguntaId = idPregunta;
                    parametros.Add(NonQueries.Encuestas.ParametrosEncuestaPreguntaOpcion(idOpcion, opcion));
                }

            var statements = EiffelService.ArmarStatements(parametros, _sgcConfigurations.SistemaDeCalidadQueryExecutorURL, _sgcConfigurations.SistemaDeCalidadDSNs);
            var resultado = EiffelService.SendTransactionToDBFDatabase(statements, _sgcConfigurations.SistemaDeCalidadQueryExecutorURL, _sgcConfigurations.SistemaDeCalidadDSNs);

            return resultado;
        }

        public bool GrabarOpcion(EncuestaPreguntaOpcion opcion)
        {
            var idOpcion = ProximoId(opcion.NombreTabla());
            var parametros = new List<(string tabla, List<(String, Object, OdbcType)> parametros)>();
            parametros.Add(NonQueries.Encuestas.ParametrosEncuestaPreguntaOpcion(idOpcion, opcion));

            var statements = EiffelService.ArmarStatements(parametros, _sgcConfigurations.SistemaDeCalidadQueryExecutorURL, _sgcConfigurations.SistemaDeCalidadDSNs);
            var resultado = EiffelService.SendTransactionToDBFDatabase(statements, _sgcConfigurations.SistemaDeCalidadQueryExecutorURL, _sgcConfigurations.SistemaDeCalidadDSNs);
            return resultado;
        }

        public bool ActualizarEncuesta(Encuesta encuesta)
        {
            var parametros = new List<(string tabla, List<(string, object)> parametros, List<(string, object)> filtros)>();
            parametros.Add(NonQueries.Encuestas.ParametrosUpdateEncuesta(encuesta));

            var statements = EiffelService.ArmarStatementUpdate(parametros, _sgcConfigurations.SistemaDeCalidadQueryExecutorURL, _sgcConfigurations.SistemaDeCalidadDSNs);
            var resultado = EiffelService.SendTransactionToDBFDatabase(statements, _sgcConfigurations.SistemaDeCalidadQueryExecutorURL, _sgcConfigurations.SistemaDeCalidadDSNs);

            return resultado;
        }

        private int ProximoId(string tabla)
        {
            var queryExecutor = new QueryExecutor()
            {
                Consulta = Queries.Encuestas.UltimoId(tabla),
                DSNs = _sgcConfigurations.SistemaDeCalidadDSNs,
                URL = _sgcConfigurations.SistemaDeCalidadQueryExecutorURL,
                Parametros = new List<(string, object, OdbcType)>()
            };
            var id = EiffelService.SendToDBFDatabase<Base>(queryExecutor);
            if (id != null && id.Id != 0)
                return id.Id + 1;
            else
                return 1;
        }

        public bool ActualizarPregunta(EncuestaPregunta pregunta)
        {
            var parametros = new List<(string tabla, List<(string, object)> parametros, List<(string, object)> filtros)>();
            parametros.Add(NonQueries.Encuestas.ParametrosUpdatePregunta(pregunta));

            var statements = EiffelService.ArmarStatementUpdate(parametros, _sgcConfigurations.SistemaDeCalidadQueryExecutorURL, _sgcConfigurations.SistemaDeCalidadDSNs);
            var resultado = EiffelService.SendTransactionToDBFDatabase(statements, _sgcConfigurations.SistemaDeCalidadQueryExecutorURL, _sgcConfigurations.SistemaDeCalidadDSNs);

            return resultado;
        }

        public bool ActualizarOpcion(EncuestaPreguntaOpcion opcion)
        {
            var parametros = new List<(string tabla, List<(string, object)> parametros, List<(string, object)> filtros)>();
            parametros.Add(NonQueries.Encuestas.ParametrosUpdateOpcion(opcion));

            var statements = EiffelService.ArmarStatementUpdate(parametros, _sgcConfigurations.SistemaDeCalidadQueryExecutorURL, _sgcConfigurations.SistemaDeCalidadDSNs);
            var resultado = EiffelService.SendTransactionToDBFDatabase(statements, _sgcConfigurations.SistemaDeCalidadQueryExecutorURL, _sgcConfigurations.SistemaDeCalidadDSNs);

            return resultado;
        }

        public bool EliminarEncuesta(Encuesta encuesta)
        {
            var parametros = new List<(string tabla, List<(string, object)> filtros)>();
            parametros.Add(NonQueries.Encuestas.ParametrosDeleteEncuesta(encuesta));
            if (encuesta.Preguntas != null)
            {
                parametros.Add(NonQueries.Encuestas.ParametrosDeleteEncuestaPregunta(encuesta.Preguntas.First()));

                foreach (var pregunta in encuesta.Preguntas.Where(pregunta => pregunta.Opciones != null && pregunta.Opciones.Count > 0))
                    parametros.Add(NonQueries.Encuestas.ParametrosDeleteEncuestaPreguntasOpcion(pregunta.Opciones.FirstOrDefault()));
            }

            var statements = EiffelService.ArmarStatementDelete(parametros);
            var resultado = EiffelService.SendTransactionToDBFDatabase(statements, _sgcConfigurations.SistemaDeCalidadQueryExecutorURL, _sgcConfigurations.SistemaDeCalidadDSNs);

            return resultado;
        }
    }
}
