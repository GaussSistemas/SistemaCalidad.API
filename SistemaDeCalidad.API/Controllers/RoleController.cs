using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SistemaDeCalidad.API.DTOs.Input.Bloqueo;
using SistemaDeCalidad.API.DTOs.Output.Bloqueo;
using SistemaDeCalidad.API.Interfaces.Services.Bloqueo;

namespace SistemaDeCalidad.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly ILogger<RoleController> _logger;
        private readonly IRoleService _service;
        private readonly IMapper _mapper;

        public RoleController(ILogger<RoleController> logger, IRoleService service, IMapper mapper)
        {
            _logger = logger;
            _service = service;
            _mapper = mapper;
        }

        [HttpPost(Name = "Create role")]
        public async Task<ActionResult> Post(RoleInput newRoleInput)
        {
            try
            {
                var newRole = _mapper.Map<Persistence.Entities.Role>(newRoleInput);
                var createdRole = await _service.CreateRole(newRole).ConfigureAwait(false);
                return Created(createdRole.Id.ToString(), _mapper.Map<RoleOutput>(createdRole));
            }
            catch (BadHttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("AssignSteps", Name = "Asign steps to role")]
        public async Task<ActionResult> AssignStepsToRole(int roleId, List<int> stepsIds)
        {
            try
            {
                var updatedRole = await _service.AssignStepsToRole(roleId, stepsIds).ConfigureAwait(false); 
                return Ok(_mapper.Map<RoleOutput>(updatedRole));
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
