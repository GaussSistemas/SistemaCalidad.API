using SistemaDeCalidad.API.Models.Encuestas;

namespace SistemaDeCalidad.API.Interfaces.Services
{
    public interface IEncuestasService
    {
        public Task<EncuestaConSoporte> RecuperarEncuesta(string id);
        public Task<Encuesta> RecuperarEncuestaActiva();
        public Task<List<EncuestaTipoControl>> RecuperarControlesEncuesta();
        public Task GrabarRespuestaEncuesta(EncuestaRespuesta respuesta);
        public Task GrabarEncuesta(Encuesta encuesta); 
        public Task GrabarPregunta(EncuestaPregunta pregunta);
        public Task GrabarOpcion(EncuestaPreguntaOpcion opcion);
        public Task ActualizarEncuesta(Encuesta encuesta);
        public Task ActualizarPregunta(EncuestaPregunta pregunta);
        public Task ActualizarOpcion(EncuestaPreguntaOpcion opcion);
        public Task EliminarEncuesta(int id);
        public Task EliminarPregunta(int id);
        public Task EliminarOpcion(int id);
        public Task EliminarEncuestas(List<int> ids);
        public Task EliminarPreguntas(List<int> ids);
        public Task EliminarOpciones(List<int> ids);
    }
}
