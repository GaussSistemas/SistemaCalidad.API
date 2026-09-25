using SistemaDeCalidad.API.Models.Encuestas;

namespace SistemaDeCalidad.API.Interfaces.Repositories
{
    public interface IEncuestasRepository
    {
        bool EncuestaContestada(string id);
        Encuesta EncuestaSegunId(int id);
        Encuesta EncuestaActiva();
        EncuestaPregunta PreguntaSegunId(int id); 
        List<EncuestaPregunta> PreguntasEncuesta(int encuestaId);
        List<EncuestaPreguntaOpcion> OpcionesPreguntas(List<int> preguntasIds);
        List<PreguntaAdicional> AdicionalesPreguntas(List<int> preguntasIds);
        List<PreguntaAdicionalOpcion> OpcionesAdicionales(List<int> adicionalesIds);
        EncuestaPreguntaOpcion OpcionSegunId(int id);
        List<EncuestaTipoControl> ControlesEncuestas();
        bool GrabarRespuesta(EncuestaRespuesta respuesta, string tipoId, int soporteNumero);
        bool GrabarEncuesta(Encuesta encuesta);
        bool GrabarPregunta(EncuestaPregunta pregunta);
        bool GrabarOpcion(EncuestaPreguntaOpcion opcion);
        bool ActualizarEncuesta(Encuesta encuesta);
        bool ActualizarPregunta(EncuestaPregunta pregunta);
        bool ActualizarOpcion(EncuestaPreguntaOpcion opcion);
        bool EncuestaConRespuestas(int id);
        bool EliminarEncuesta(Encuesta encuesta);
        bool ActualizarRespuesta(EncuestaRespuesta respuestaEncuesta, string tipoId, int numero);
    }
}
