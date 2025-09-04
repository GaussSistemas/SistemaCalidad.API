using SistemaDeCalidad.API.Interfaces.Repositories;
using SistemaDeCalidad.API.Interfaces.Services;
using SistemaDeCalidad.API.Models.Encuestas;

namespace SistemaDeCalidad.API.Services
{
    public class EncuestasService : IEncuestasService
    {
        private readonly IEncuestasRepository _encuestasRepository;
        private readonly ISoportesService _soportesService;
        public EncuestasService(IEncuestasRepository encuestasRepository, ISoportesService soportesService)
        {
            _encuestasRepository = encuestasRepository;
            _soportesService = soportesService;
        }

        public async Task GrabarRespuestaEncuesta(EncuestaRespuesta respuesta)
        {
            if (respuesta == null)
                throw new ArgumentNullException("La respuesta no puede estar vacía");

            if (_encuestasRepository.EncuestaContestada(respuesta.Hash))
                throw new BadHttpRequestException("La encuesta ya se encuentra respondida.");
            
            var soporte = await _soportesService.GetSoporte(respuesta.Hash);
            if (soporte == null)
                throw new BadHttpRequestException("El soporte asociado a la encuesta no existe.");

            var encuesta = _encuestasRepository.EncuestaSegunId(respuesta.EncuestaId);
            if (encuesta == null)
                throw new BadHttpRequestException("La encuesta no existe.");

            if (respuesta?.RespuestasPreguntas == null || respuesta?.RespuestasPreguntas.Count == 0)
                throw new BadHttpRequestException("La respuesta debe contar con al menos una respuesta de pregunta.");

            var resultado = _encuestasRepository.GrabarRespuesta(respuesta, soporte.TipoId, (int)soporte.Numero);
            if (!resultado)
                throw new IOException("No se pudo insertar la respuesta en la base de datos.");

            await _soportesService.FinalizarSoporte(respuesta.Hash);
            return;
        }

        public async Task AgregarRespuestaEncuesta(EncuestaRespuesta respuesta)
        {
            if (respuesta == null)
                throw new ArgumentNullException("La respuesta no puede estar vacía");

            var soporte = await _soportesService.GetSoporte(respuesta.Hash);
            if (soporte == null)
                throw new BadHttpRequestException("El soporte asociado a la encuesta no existe.");

            var encuesta = _encuestasRepository.EncuestaSegunId(respuesta.EncuestaId);
            if (encuesta == null)
                throw new BadHttpRequestException("La encuesta no existe.");

            if (respuesta?.RespuestasPreguntas == null || respuesta?.RespuestasPreguntas.Count == 0)
                throw new BadHttpRequestException("La respuesta debe contar con al menos una respuesta de pregunta.");

            var resultado = _encuestasRepository.ActualizarRespuesta(respuesta, soporte.TipoId, (int)soporte.Numero);
            if (!resultado)
                throw new IOException("No se pudo insertar la respuesta en la base de datos.");

            return;
        }
        public async Task GrabarEncuesta(Encuesta encuesta)
        {
            if (encuesta == null)
                throw new ArgumentNullException("La encuesta no puede estar vacía");

            if (encuesta.Preguntas == null || encuesta.Preguntas.Count == 0)
                throw new ArgumentNullException("La encuesta debe tener al menos 1 pregunta");

            var resultado = _encuestasRepository.GrabarEncuesta(encuesta);

            if (!resultado)
                throw new IOException("No se pudo insertar la encuesta en la base de datos");

            return;
        }

        public async Task GrabarPregunta(EncuestaPregunta pregunta)
        {
            if (pregunta == null)
                throw new ArgumentNullException("La pregunta no puede estar vacía.");

            var encuesta = _encuestasRepository.EncuestaSegunId(pregunta.EncuestaId);
            if (encuesta == null)
                throw new ArgumentNullException("La encuesta no existe.");

            var resultado = _encuestasRepository.GrabarPregunta(pregunta);

            if (!resultado)
                throw new IOException("No se pudo insertar la encuesta en la base de datos");

            return;
        }

        public async Task GrabarOpcion(EncuestaPreguntaOpcion opcion)
        {
            if (opcion == null)
                throw new ArgumentNullException("La opción no puede estar vacía.");

            var resultado = _encuestasRepository.GrabarOpcion(opcion);

            if (!resultado)
                throw new IOException("No se pudo insertar la encuesta en la base de datos");

            return;
        }

        public async Task ActualizarEncuesta(Encuesta encuesta)
        {
            if (encuesta == null)
                throw new ArgumentNullException("La encuesta no puede estar vacía.");

            var encuestaDB = _encuestasRepository.EncuestaSegunId(encuesta.Id);
            if (encuestaDB == null)
                throw new ArgumentNullException("La encuesta no existe.");


            var resultado = _encuestasRepository.ActualizarEncuesta(encuesta);
            if (!resultado)
                throw new IOException("No se pudo actualizar la encuesta.");

            return;
        }

        public async Task ActualizarPregunta(EncuestaPregunta pregunta)
        {
            if (pregunta == null)
                throw new ArgumentNullException("La pregunta no puede estar vacía.");

            var preguntaDB = _encuestasRepository.PreguntaSegunId(pregunta.Id);
            if (preguntaDB == null)
                throw new ArgumentNullException("La pregunta no existe.");

            var tiposControles = _encuestasRepository.ControlesEncuestas();

            if (tiposControles == null)
                throw new ArgumentNullException("No se pudieron llevar a cabo las validaciones necesarias.");

            if (!tiposControles.Any(control => control.Id == pregunta.TipoControlId))
                throw new BadHttpRequestException("El código de control de la pregunta no existe.");
            else
            {
                var tipoControlNuevo = tiposControles.First(control => control.Id == pregunta.TipoControlId);
                var tipoControlAnterior = tiposControles.First(control => control.Id == preguntaDB.TipoControlId);
                if ((!tipoControlNuevo.Opciones && tipoControlAnterior.Opciones) || (tipoControlNuevo.Opciones && !tipoControlAnterior.Opciones))
                    throw new BadHttpRequestException("Hay una diferencia entre el tipo de control actual y el anterior.");

                var resultado = _encuestasRepository.ActualizarPregunta(pregunta);

                if (!resultado)
                    throw new IOException("No se pudo actualizar la pregunta.");
            }

            return;
        }

        public async Task ActualizarOpcion(EncuestaPreguntaOpcion opcion)
        {
            if (opcion == null)
                throw new ArgumentNullException("La opción no puede estar vacía.");

            var preguntaDB = _encuestasRepository.OpcionSegunId(opcion.Id);
            if (preguntaDB == null)
                throw new ArgumentNullException("La opción no existe.");

            var resultado = _encuestasRepository.ActualizarOpcion(opcion);

            if (!resultado)
                throw new IOException("No se pudo actualizar la pregunta.");
        }

        public async Task<List<EncuestaTipoControl>> RecuperarControlesEncuesta()
        {
            var controles = _encuestasRepository.ControlesEncuestas();
            return controles;
        }

        public async Task<EncuestaConSoporte> RecuperarEncuesta(string id)
        {
            if (!_encuestasRepository.EncuestaContestada(id))
            {
                var soporte = await _soportesService.GetSoporte(id).ConfigureAwait(false);
                if (soporte == null)
                    throw new BadHttpRequestException("El soporte con el hash indicado no se encuentra en el sistema");

                var encuesta = await this.RecuperarEncuestaActiva().ConfigureAwait(false);
                if (encuesta == null)
                    throw new ArgumentException("No hay ninguna encuesta activa");

                return new EncuestaConSoporte() { Soporte = soporte, Encuesta = encuesta };
            }
            return new EncuestaConSoporte() { Contestada = true };
        }

        public async Task<Encuesta> RecuperarEncuestaActiva()
        {
            var encuesta = _encuestasRepository.EncuestaActiva();
            var preguntas = _encuestasRepository.PreguntasEncuesta(encuesta.Id);
            var opciones = _encuestasRepository.OpcionesPreguntas(preguntas.Select(pregunta => pregunta.Id).ToList());

            encuesta.Preguntas = preguntas;
            foreach (var pregunta in preguntas)
            {
                var opcionesPregunta = opciones.Where(opcion => opcion.EncuestaPreguntaId == pregunta.Id).ToList();
                if (opcionesPregunta.Any())
                    pregunta.Opciones = opcionesPregunta;
            }

            return encuesta;
        }

        public async Task EliminarEncuesta(int id)
        {
            var encuestaDB = _encuestasRepository.EncuestaSegunId(id);
            if (encuestaDB == null)
                throw new ArgumentNullException("La encuesta no existe.");

            if (_encuestasRepository.EncuestaConRespuestas(id))
                throw new ArgumentNullException("La encuesta ya cuenta con respuestas.");

            var preguntas = _encuestasRepository.PreguntasEncuesta(id);
            var opciones = _encuestasRepository.OpcionesPreguntas(preguntas.Select(pregunta => pregunta.Id).ToList());

            if (preguntas != null)
            {
                encuestaDB.Preguntas = preguntas;
                foreach (var pregunta in preguntas)
                {
                    var opcionesPregunta = opciones.Where(opcion => pregunta.Id == opcion.EncuestaPreguntaId).ToList();
                    if (opcionesPregunta.Any())
                        pregunta.Opciones = new List<EncuestaPreguntaOpcion>(opcionesPregunta);
                }
            }

            var result = _encuestasRepository.EliminarEncuesta(encuestaDB);
            if (!result)
                throw new IOException("No se pudo eliminar la encuesta.");

            return;
        }

        public Task EliminarPregunta(int id)
        {
            throw new NotImplementedException();
        }

        public Task EliminarOpcion(int id)
        {
            throw new NotImplementedException();
        }

        public Task EliminarEncuestas(List<int> ids)
        {
            throw new NotImplementedException();
        }

        public Task EliminarPreguntas(List<int> ids)
        {
            throw new NotImplementedException();
        }

        public Task EliminarOpciones(List<int> ids)
        {
            throw new NotImplementedException();
        }
    }
}
