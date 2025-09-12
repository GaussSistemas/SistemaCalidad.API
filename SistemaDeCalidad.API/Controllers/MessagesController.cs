using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaDeCalidad.API.DTOs.Input.Bloqueo;
using SistemaDeCalidad.API.DTOs.Output.Bloqueo;
using SistemaDeCalidad.API.Interfaces.Services.Bloqueo;
using SistemaDeCalidad.API.Persistence.Entities;

namespace SistemaDeCalidad.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class MessagesController : ControllerBase
    {
        private readonly ILogger<MessagesController> _logger;
        private readonly IMessagesService _service;
        private readonly IMapper _mapper;

        public MessagesController(ILogger<MessagesController> logger, IMessagesService service, IMapper mapper)
        {
            _logger = logger;
            _service = service;
            _mapper = mapper;
        }

        [HttpGet(Name = "Get all messages")]
        public async Task<ActionResult> Get()
        {
            try
            {
                var messages = await _service.GetAllMessages().ConfigureAwait(false);
                return Ok(_mapper.Map<List<MessageOutput>>(messages));
            }
            catch (BadHttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetAllForCustomerBusiness", Name = "Get all messages for customer business")]
        public async Task<ActionResult> GetAllForCustomerCompany(string customerId, string companyId)
        {
            try
            {
                var messages = await _service.GetAllMessagesForCustomerCompany(customerId, companyId).ConfigureAwait(false);
                return Ok(_mapper.Map<List<MessageOutput>>(messages));
            }
            catch (BadHttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost(Name = "Create message")]
        public async Task<ActionResult> Post(MessageInput newMessageInput)
        {
            try
            {
                var newMessage = _mapper.Map<Persistence.Entities.Message>(newMessageInput);
                var createdMessage = await _service.CreateMessage(newMessage).ConfigureAwait(false);
                return Created(createdMessage.Id.ToString(), _mapper.Map<MessageOutput>(createdMessage));
            }
            catch (BadHttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut(Name = "Update message")]
        public async Task<ActionResult> Update(int messageId, int stepId)
        {
            try
            {
                var message = await _service.UpdateMessage(messageId, stepId).ConfigureAwait(false);
                return Ok(_mapper.Map < List<MessageOutput>>(message));
            }
            catch (BadHttpRequestException ex)
            {

                throw;
            }
        }

        [HttpGet("User", Name = "Get message for user")]
        public async Task<ActionResult> GetMessageForUser(string customerId, string companyId, string eiffelUserId)
        {
            try
            {
                var message = await _service.GetMessageForEiffelUser(customerId, companyId, eiffelUserId).ConfigureAwait(false);
                if(message == null)
                    return NoContent();
                
                return Ok(_mapper.Map<MessageOutput>(message));
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
