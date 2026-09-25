namespace SistemaDeCalidad.API.Repositories.Queries
{
    public static class Encuestas
    {
        public static string EncuestaSegunId(int id)
        {
            var consulta = $"SELECT " +
                $"Id, " +
                $"Titulo, " +
                $"encuestas.introduccion, " +
                $"encuestas.cierre, " +
                $"Activa " +
                $"FROM encuestas " +
                $"WHERE id = {id}";

            return consulta;
        }

        public static string EncuestaActiva()
        {
            var consulta = "SELECT " +
                "encuestas.id, " +
                "ALLTRIM(encuestas.titulo) as titulo, " +
                "encuestas.introduccion, " +
                "encuestas.cierre, " +
                "encuestas.activa " +
                "FROM encuestas " +
                "WHERE " +
                "encuestas.activa ";

            return consulta;
        }

        public static string PreguntasEncuesta(long encuestaId)
        {
            var consulta = $"SELECT " +
                $"encuestasPreguntas.id, " +
                $"encuestasPreguntas.encuestaId, " +
                $"ALLTRIM(encuestasPreguntas.titulo) as titulo, " +
                $"encuestasPreguntas.tipoControlId " +
                $"FROM encuestasPreguntas " +
                $"WHERE " +
                $"encuestasPreguntas.encuestaId = {encuestaId}";

            return consulta;
        }

        public static string PreguntaSegunId(int id)
        {
            var consulta = $"SELECT " +
                $"Id, " +
                $"Titulo, " +
                $"TipoControlId " +
                $"FROM encuestasPreguntas " +
                $"WHERE id = {id}";

            return consulta;
        }

        public static string OpcionesPreguntas(List<int> preguntasIds)
        {
            if (preguntasIds != null && preguntasIds.Count != 0)
            {
                var filtro = preguntasIds.Count == 1 ?
                    $"encuestasPreguntasOpciones.encuestaPreguntaId = {preguntasIds.FirstOrDefault()}" :
                    $"encuestasPreguntasOpciones.encuestaPreguntaId IN ({string.Join(",", preguntasIds)})";

                var consulta = $"SELECT " +
                    $"encuestasPreguntasOpciones.id, " +
                    $"encuestasPreguntasOpciones.encuestaPreguntaId, " +
                    $"ALLTRIM(encuestasPreguntasOpciones.opcion) as opcion " +
                    $"FROM encuestasPreguntasOpciones " +
                    $"WHERE " +
                    $"{filtro}";

                return consulta;
            }
            else
                throw new ArgumentNullException("No se recibieron preguntas sobre las que buscar las opciones");

        }

        public static string OpcionSegunId(int id)
        {
            var consulta = $"SELECT " +
                $"EncuestasPreguntasOpciones.Id, " +
                $"EncuestasPreguntasOpciones.Opcion, " +
                $"EncuestasPreguntasOpciones.EncuestaPreguntaId " +
                $"FROM EncuestasPreguntasOpciones " +
                $"WHERE EncuestasPreguntasOpciones.Id = {id}";

            return consulta;

        }

        public static string EncuestaContestada(string hash)
        {
            var consulta = $"SELECT " +
                $"encuestasRespuestas.id, " +
                $"encuestasRespuestas.hash, " +
                $"encuestasRespuestas.encuestaId, " +
                $"encuestasRespuestas.NombreUsuario, " +
                $"encuestasRespuestas.ApellidoUsuario, " +
                $"encuestasRespuestas.Fecha, " +
                $"encuestasRespuestas.Hora " +
                $"FROM encuestasRespuestas " +
                $"WHERE " +
                $"encuestasRespuestas.hash = '{hash}'";

            return consulta;
        }

        public static string EncuestaConRespuestas(int id)
        {
            var consulta = $"SELECT " +
                $"encuestasRespuestas.id, " +
                $"encuestasRespuestas.hash, " +
                $"encuestasRespuestas.encuestaId, " +
                $"encuestasRespuestas.NombreUsuario, " +
                $"encuestasRespuestas.ApellidoUsuario, " +
                $"encuestasRespuestas.Fecha, " +
                $"encuestasRespuestas.Hora " +
                $"FROM encuestasRespuestas " +
                $"WHERE " +
                $"encuestasRespuestas.encuestaId = {id}";

            return consulta;
        }
        public static string ControlesEncuesta()
        {
            var consulta = $"SELECT " +
                $"encuestasTiposControles.id, " +
                $"encuestasTiposControles.control, " +
                $"encuestasTiposControles.tipo, " +
                $"encuestasTiposControles.opciones " +
                $"FROM " +
                $"encuestasTiposControles";

            return consulta;

        }

        public static string UltimoId(string tabla)
        {
            var consulta = $"SELECT " +
                $"id " +
                $"FROM {tabla} " +
                $"ORDER BY id DESC " +
                $"TOP 1";

            return consulta;
        }

        public static string RespuestaContestada(string hash, int preguntaId)
        {
            var consulta = $"SELECT " +
                $"encuestasRespuestasPreguntas.id, " +
                $"encuestasRespuestasPreguntas.hash, " +
                $"encuestasRespuestasPreguntas.encuestaPreguntaId " +
                $"FROM encuestasRespuestasPreguntas " +
                $"WHERE " +
                $"encuestasRespuestasPreguntas.hash = '{hash}' AND " +
                $"encuestasRespuestasPreguntas.encuestaPreguntaId = {preguntaId}";

            return consulta;
        }

        public static string UltimaPreguntaEncuesta(int encuestaId)
        {
            var consulta = $"SELECT " +
                $"encuestasPreguntas.id, " +
                $"encuestasPreguntas.encuestaId, " +
                $"ALLTRIM(encuestasPreguntas.titulo) as titulo, " +
                $"encuestasPreguntas.tipoControlId " +
                $"FROM encuestasPreguntas " +
                $"WHERE " +
                $"encuestasPreguntas.encuestaId = {encuestaId} " +
                $"ORDER BY encuestasPreguntas.id DESC " +
                $"TOP 1";

            return consulta;
        }

        public static string AdicionalesPreguntas(List<int> preguntasIds)
        {
            if (preguntasIds != null && preguntasIds.Count != 0)
            {
                var filtro = preguntasIds.Count == 1 ?
                    $"encuestasPreguntasAdicionales.encuestaPreguntaId = {preguntasIds.FirstOrDefault()}" :
                    $"encuestasPreguntasAdicionales.encuestaPreguntaId IN ({string.Join(",", preguntasIds)})";

                var consulta = $"SELECT " +
                    $"preguntasAdicionales.id, " +
                    $"preguntasAdicionales.encuestaId, " +
                    $"encuestasPreguntasAdicionales.encuestaPreguntaId, " +
                    $"encuestasPreguntasAdicionales.orden, " +
                    $"ALLTRIM(preguntasAdicionales.titulo) as titulo, " +
                    $"preguntasAdicionales.tipoControlId, " +
                    $"preguntasAdicionales.obligatoria " +
                    $"FROM encuestasPreguntasAdicionales " +
                    $"INNER JOIN preguntasAdicionales ON preguntasAdicionales.id = encuestasPreguntasAdicionales.preguntaAdicionalId " +
                    $"WHERE " +
                    $"{filtro} " +
                    $"ORDER BY encuestasPreguntasAdicionales.encuestaPreguntaId, encuestasPreguntasAdicionales.orden";

                return consulta;
            }
            else
                throw new ArgumentNullException("No se recibieron preguntas sobre las que buscar las adicionales");
        }

        public static string OpcionesAdicionales(List<int> adicionalesIds)
        {
            if (adicionalesIds != null && adicionalesIds.Count != 0)
            {
                var filtro = adicionalesIds.Count == 1 ?
                    $"preguntasAdicionalesOpciones.preguntaAdicionalId = {adicionalesIds.FirstOrDefault()}" :
                    $"preguntasAdicionalesOpciones.preguntaAdicionalId IN ({string.Join(",", adicionalesIds)})";

                var consulta = $"SELECT " +
                    $"preguntasAdicionalesOpciones.id, " +
                    $"preguntasAdicionalesOpciones.preguntaAdicionalId, " +
                    $"ALLTRIM(preguntasAdicionalesOpciones.opcion) as opcion " +
                    $"FROM preguntasAdicionalesOpciones " +
                    $"WHERE " +
                    $"{filtro}";

                return consulta;
            }
            else
                throw new ArgumentNullException("No se recibieron preguntas adicionales sobre las que buscar las opciones");
        }

        public static string RespuestaAdicionalContestada(string hash, int encuestaPreguntaId, int preguntaAdicionalId)
        {
            var consulta = $"SELECT " +
                $"encuestasRespuestasAdicionales.id, " +
                $"encuestasRespuestasAdicionales.hash, " +
                $"encuestasRespuestasAdicionales.encuestaPreguntaId, " +
                $"encuestasRespuestasAdicionales.preguntaAdicionalId " +
                $"FROM encuestasRespuestasAdicionales " +
                $"WHERE " +
                $"encuestasRespuestasAdicionales.hash = '{hash}' AND " +
                $"encuestasRespuestasAdicionales.encuestaPreguntaId = {encuestaPreguntaId} AND " +
                $"encuestasRespuestasAdicionales.preguntaAdicionalId = {preguntaAdicionalId}";

            return consulta;
        }
    }
}
