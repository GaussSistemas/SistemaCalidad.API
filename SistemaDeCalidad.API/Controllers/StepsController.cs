using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SistemaDeCalidad.API.DTOs.Input.Bloqueo;
using SistemaDeCalidad.API.DTOs.Output.Bloqueo;
using SistemaDeCalidad.API.Interfaces.Services.Bloqueo;

namespace SistemaDeCalidad.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StepsController : ControllerBase
    {
        private readonly ILogger<StepsController> _logger;
        private readonly IStepsService _service;
        private readonly IMapper _mapper;

        public StepsController(ILogger<StepsController> logger, IStepsService service, IMapper mapper)
        {
            _logger = logger;
            _service = service;
            _mapper = mapper;
        }

        [HttpPost(Name = "Create step")]
        public async Task<ActionResult> Post(StepInput newStepInput)
        {
            try
            {
                var newStep = _mapper.Map<Persistence.Entities.Step>(newStepInput);
                var createdStep = await _service.CreateStep(newStep).ConfigureAwait(false);
                return Created(createdStep.Id.ToString(), _mapper.Map<StepOutput>(createdStep));
            }
            catch (BadHttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}