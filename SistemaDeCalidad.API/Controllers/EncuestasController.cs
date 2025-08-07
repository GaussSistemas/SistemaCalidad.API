using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SistemaDeCalidad.API.DTOs.Input;
using SistemaDeCalidad.API.DTOs.Output;
using SistemaDeCalidad.API.Interfaces.Services;
using SistemaDeCalidad.API.Models.Encuestas;

namespace SistemaDeCalidad.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EncuestasController : ControllerBase
    {
        private readonly ILogger<EncuestasController> _logger;
        private readonly IEncuestasService _service;
        private readonly IMapper _mapper;
        public EncuestasController(ILogger<EncuestasController> logger, IEncuestasService encuestasService, IMapper mapper)
        {
            _logger = logger;
            _service = encuestasService;
            _mapper = mapper;
        }

        [HttpGet(Name = "Encuesta")]
        public async Task<ActionResult> Get(string hash)
        {
            try
            {
                var encuesta = await _service.RecuperarEncuesta(hash);
                var dto = _mapper.Map<EncuestaConSoporteOutput>(encuesta);
                return Ok(dto);
            }
            catch (BadHttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet("Activa", Name = "Encuesta activa")]
        public async Task<EncuestaOutput> GetActiva()
        {
            var encuesta = await _service.RecuperarEncuestaActiva();
            var dto = _mapper.Map<EncuestaOutput>(encuesta);

            return dto;
        }

        [HttpPost(Name = "Nueva encuesta")]
        public async Task<ActionResult> Post([FromBody] EncuestaInput encuestaInput)
        {
            try
            {
                var encuesta = _mapper.Map<Encuesta>(encuestaInput);
                var resultado = _service.GrabarEncuesta(encuesta);
                return Created();
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (IOException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Respuesta", Name = "Respuesta encuesta")]
        public async Task<ActionResult> PostRespuesta([FromBody] EncuestaRespuestaInput respuestaInput)
        {
            try
            {
                var respuesta = _mapper.Map<EncuestaRespuesta>(respuestaInput);
                await _service.GrabarRespuestaEncuesta(respuesta);
                return Created();
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (BadHttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (IOException ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPut(Name = "Modificar encuesta")]
        public async Task<ActionResult> UpdateEncuesta(int idEncuesta, [FromBody] EncuestaInput encuestaInput)
        {
            try
            {
                var encuesta = _mapper.Map<Encuesta>(encuestaInput);
                encuesta.Id = idEncuesta;

                var resultado = _service.ActualizarEncuesta(encuesta);
                return Ok();
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (IOException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete(Name = "Eliminar encuesta")]
        public async Task<ActionResult> EliminarEncuesta(int idEncuesta)
        {
            try
            {
                _service.EliminarEncuesta(idEncuesta); 
                return Ok();
            }
            catch(ArgumentNullException ex)
            {
                return BadRequest(ex.Message); 
            }
            catch (BadHttpRequestException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch(IOException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Controles", Name = "Controles encuesta")]
        public async Task<List<EncuestaTipoControlOutput>> GetControles()
        {
            var controles = await _service.RecuperarControlesEncuesta();
            var dto = _mapper.Map<List<EncuestaTipoControlOutput>>(controles);
            return dto;
        }

        [HttpPost("Pregunta", Name = "Nueva pregunta")]
        public async Task<ActionResult> PostPregunta(int idEncuesta, [FromBody] EncuestaPreguntaInput encuestaPreguntaInput)
        {
            try
            {
                var encuestaPregunta = _mapper.Map<EncuestaPregunta>(encuestaPreguntaInput);
                encuestaPregunta.EncuestaId = idEncuesta;

                var resultado = _service.GrabarPregunta(encuestaPregunta);
                return Created();
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (IOException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Opcion", Name = "Nueva opción")]
        public async Task<ActionResult> PostOpcion(int idPregunta, [FromBody] EncuestaPreguntaOpcionInput encuestaPreguntaOpcionInput)
        {
            try
            {
                var opcion = _mapper.Map<EncuestaPreguntaOpcion>(encuestaPreguntaOpcionInput);
                opcion.EncuestaPreguntaId = idPregunta;

                var resultado = _service.GrabarOpcion(opcion);
                return Created();
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (IOException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("Pregunta", Name = "Modificar pregunta")]
        public async Task<ActionResult> UpdatePregunta(int idPregunta, [FromBody] EncuestaPreguntaInput encuestaPreguntaInput)
        {
            try
            {
                var pregunta = _mapper.Map<EncuestaPregunta>(encuestaPreguntaInput);
                pregunta.Id = idPregunta;

                var resultado = _service.ActualizarPregunta(pregunta);
                return Ok();
            }
            catch (BadHttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (IOException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("Opcion", Name = "Modificar opcion")]
        public async Task<ActionResult> UpdateOpcion(int idOpcion, [FromBody] EncuestaPreguntaOpcionInput encuestaPreguntaOpcionInput)
        {
            try
            {
                var opcion = _mapper.Map<EncuestaPreguntaOpcion>(encuestaPreguntaOpcionInput);
                opcion.Id = idOpcion;

                var resultado = _service.ActualizarOpcion(opcion);
                return Ok();
            }
            catch (BadHttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (IOException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
