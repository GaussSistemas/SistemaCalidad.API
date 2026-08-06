using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaDeCalidad.API.DTOs.Input;
using SistemaDeCalidad.API.DTOs.Input.Bloqueo;
using SistemaDeCalidad.API.DTOs.Output.Bloqueo;
using SistemaDeCalidad.API.Interfaces.Services.Bloqueo;
using SistemaDeCalidad.API.Models;

namespace SistemaDeCalidad.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IUsersService _service;
        private readonly IMapper _mapper;

        public UsersController(ILogger<UsersController> logger, IUsersService service, IMapper mapper)
        {
            _logger = logger;
            _service = service;
            _mapper = mapper;
        }

        [HttpPost(Name = "Create user")]
        public async Task<ActionResult> Post(UserInput newUserInput)
        {
            try
            {
                var newUser = _mapper.Map<Persistence.Entities.User>(newUserInput);
                var createdUser = await _service.CreateUser(newUser).ConfigureAwait(false);
                return Created(createdUser.Id.ToString(), _mapper.Map<UserOutput>(createdUser));
            }
            catch (BadHttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Authenticate", Name = "Create token")]
        public async Task<ActionResult> CreateToken(LoginInput credentials)
        {
            try
            {
                var loginCredentials = _mapper.Map<Login>(credentials);
                var token = await _service.CreateToken(loginCredentials).ConfigureAwait(false);
                return Ok(token);
            }
            catch (BadHttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
