using SistemaDeCalidad.API.Models.Encuestas;
using SistemaDeCalidad.API.Repositories.Queries;
using System.Data.Odbc;

namespace SistemaDeCalidad.API.Repositories.NonQueries
{
    public static class Encuestas
    {
        public static (string tabla, List<(string, object, OdbcType)> parametros) ParametrosEncuesta(int id, Encuesta encuesta)
        {
            var parametros = new List<(string, object, OdbcType)>()
            {
                ("id", id, OdbcType.Int),
                ("titulo", $"'{encuesta.Titulo}'", OdbcType.Text),
                ("activa", encuesta.Activa ? ".T." : ".F.", OdbcType.Bit)
            };
            return (encuesta.NombreTabla(), parametros);
        }

        public static (string tabla, List<(string, object, OdbcType)> parametros) ParametrosEncuestaPregunta(int id, EncuestaPregunta pregunta)
        {
            var parametros = new List<(string, object, OdbcType)>()
            {
                ("id", id, OdbcType.Int),
                ("encuestaId", pregunta.EncuestaId, OdbcType.Int),
                ("titulo", $"'{pregunta.Titulo}'", OdbcType.Text),
                ("tipoControlId", pregunta.TipoControlId, OdbcType.Int)
            };

            return (pregunta.NombreTabla(), parametros);
        }

        public static (string tabla, List<(string, object, OdbcType)> parametros) ParametrosEncuestaPreguntaOpcion(int id, EncuestaPreguntaOpcion opcion)
        {
            var parametros = new List<(string, object, OdbcType)>()
            {
                ("id", id, OdbcType.Int),
                ("encuestaPreguntaId", opcion.EncuestaPreguntaId, OdbcType.Int),
                ("opcion", $"'{opcion.Opcion}'", OdbcType.Text)
            };

            return (opcion.NombreTabla(), parametros); 
        }
        
        public static (string tabla, List<(string, object, OdbcType)> parametros) ParametrosCabeceraRespuesta(int id, EncuestaRespuesta respuesta)
        {
            var parametros = new List<(string, object, OdbcType)>()
            {
                ("id", id, OdbcType.Int),
                ("hash", $"'{respuesta.Hash}'", OdbcType.Text),
                ("encuestaId", $"{respuesta.EncuestaId}", OdbcType.Int),
                ("nombreUsuario", $"'{respuesta.NombreUsuario}'", OdbcType.Text),
                ("apellidoUsuario", $"'{respuesta.ApellidoUsuario}'", OdbcType.Text),
                ("fecha", $"CTOD('{respuesta.Fecha.ToString("MM-dd-yyyy")}')", OdbcType.Text),
                ("hora", $"'{respuesta.Hora}'", OdbcType.Text)
            };

            return (respuesta.NombreTabla(), parametros);
        }

        public static (string tabla, List<(string, object, OdbcType)> parametros) ParametrosRespuestaPregunta(int id, string hash, EncuestaRespuestaPregunta respuesta)
        {
            var parametros = new List<(String, object, OdbcType)>()
            {
                ("id", id, OdbcType.Int),
                ("hash", $"'{hash}'", OdbcType.Text),
                ("encuestaPreguntaId", respuesta.EncuestaPreguntaId, OdbcType.Int),
                ("valor", $"'{respuesta.Valor}'", OdbcType.Text)
            };

            if (respuesta.EncuestaPreguntaOpcionId != 0)
                parametros.Add(("encuestaPreguntaOpcionId", respuesta.EncuestaPreguntaOpcionId, OdbcType.Int));

            return (respuesta.NombreTabla(), parametros);

        }
        
        public static (string tabla, List<(string, object, OdbcType)> parametros) ParametrosLog(EncuestaLog log)
        {
            var parametros = new List<(String, object, OdbcType)>()
            {
                ("fecha", $"CTOD('{log.Fecha.ToString("MM-dd-yyyy")}')", OdbcType.Text),
                ("hora", $"'{log.Hora}'", OdbcType.Text),
                ("obs", $"'{log.Observacion}'", OdbcType.Text),
                ("id_estado", $"'{log.EstadoId}'", OdbcType.Text),
                ("id_usuario", $"'{log.UsuarioId}'", OdbcType.Text),
                ("id_tipo", $"'{log.SoporteId}'", OdbcType.Text),
                ("numero", $"{log.Numero}", OdbcType.BigInt),
            };

            return (log.NombreTabla(), parametros);

        }
        
        public static (string tabla, List<(string, object)> parametros, List<(string, object)> filtros) ParametrosUpdateEncuesta(Encuesta encuesta)
        {
            var parametros = new List<(string, object)>()
            {
                ("Titulo", $"'{encuesta.Titulo}'"),
                ("Activa", encuesta.Activa ? ".T." : ".F.")
            };

            var filtros = new List<(string, object)>()
            {
                ("id", encuesta.Id ),
            };

            return (encuesta.NombreTabla(), parametros, filtros);
        }

        public static (string tabla, List<(string, object)> parametros, List<(string, object)> filtros) ParametrosUpdatePregunta(EncuestaPregunta pregunta)
        {
            var parametros = new List<(string, object)>()
            {
                ("Titulo", $"'{pregunta.Titulo}'"),
            };

            var filtros = new List<(string, object)>()
            {
                ("id", pregunta.Id ),
            };

            return (pregunta.NombreTabla(), parametros, filtros);
        }

        public static (string tabla, List<(string, object)> parametros, List<(string, object)> filtros) ParametrosUpdateOpcion(EncuestaPreguntaOpcion opcion)
        {
            var parametros = new List<(string, object)>()
            {
                ("Opcion", $"'{opcion.Opcion}'"),
            };

            var filtros = new List<(string, object)>()
            {
                ("id", opcion.Id ),
            };

            return (opcion.NombreTabla(), parametros, filtros);
        }

        public static (string tabla, List<(string, object)> parametros, List<(string, object)> filtros) ParametrosUpdateRespuestaPregunta(EncuestaRespuestaPregunta respuesta)
        {
            var parametros = new List<(string, object)>()
            {
                ("EncuestaPreguntaOpcionId", $"{respuesta.EncuestaPreguntaOpcionId}"),
                ("Valor", $"'{respuesta.Valor}'"),
            };

            var filtros = new List<(string, object)>()
            {
                ("Hash", $"'{respuesta.Hash}'"),
                ("Id", respuesta.Id),
                ("EncuestaPreguntaId", respuesta.EncuestaPreguntaId)  
            };

            return (respuesta.NombreTabla(), parametros, filtros);
        }


        public static (string tabla, List<(string, object)> filtros) ParametrosDeleteEncuesta(Encuesta encuesta)
        {
            var filtros = new List<(string, object)>()
            {
                ("id", encuesta.Id ),
            };

            return (encuesta.NombreTabla(), filtros);
        }

        public static (string tabla, List<(string, object)> filtros) ParametrosDeleteEncuestaPregunta(EncuestaPregunta encuestaPregunta)
        {
            var filtros = new List<(string, object)>()
            {
                ("encuestaId", encuestaPregunta.EncuestaId ),
            };

            return (encuestaPregunta.NombreTabla(), filtros);
        }

        public static (string tabla, List<(string, object)> filtros) ParametrosDeleteEncuestaPreguntasOpcion(EncuestaPreguntaOpcion encuestaPreguntaOpcion)
        {
            var filtros = new List<(string, object)>()
            {
                ("encuestaPreguntaId", encuestaPreguntaOpcion.EncuestaPreguntaId),
            };

            return (encuestaPreguntaOpcion.NombreTabla(), filtros);
        }
    }
}
