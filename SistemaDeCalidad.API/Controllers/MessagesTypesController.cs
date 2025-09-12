using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SistemaDeCalidad.API.DTOs.Input.Bloqueo;
using SistemaDeCalidad.API.DTOs.Output.Bloqueo;
using SistemaDeCalidad.API.Interfaces.Services.Bloqueo;

namespace SistemaDeCalidad.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class MessagesTypesController : ControllerBase
    {
        private readonly ILogger<MessagesController> _logger;
        private readonly IMessagesTypesService _service;
        private readonly IMapper _mapper;

        public MessagesTypesController(ILogger<MessagesController> logger, IMessagesTypesService service, IMapper mapper)
        {
            _logger = logger;
            _service = service;
            _mapper = mapper;
        }

        [HttpGet(Name = "Get all message types")]
        public async Task<ActionResult> Get()
        {
            try
            {
                var messagesTypes = await _service.GetAllMessageTypes().ConfigureAwait(false); 
                return Ok(_mapper.Map<List<MessageTypeOutput>>(messagesTypes));
            }
            catch (BadHttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost(Name = "Create message type")]
        public async Task<ActionResult> Post(MessageTypeInput newMessageTypeInput)
        {
            try
            {
                var newMessageType = _mapper.Map<Persistence.Entities.MessageType>(newMessageTypeInput);
                var createdMessageType = await _service.CreateMessageType(newMessageType).ConfigureAwait(false);
                return Created(createdMessageType.Id.ToString(), _mapper.Map<MessageTypeOutput>(createdMessageType));
            }
            catch (BadHttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("ModifyTemplate", Name = "Modify template")]
        public async Task<ActionResult> ModifyTemplate(int messageTypeId, string newTemplate)
        {
            try
            {
                var modifiedMessageType = await _service.ModifyTemplate(messageTypeId, newTemplate).ConfigureAwait(false);
                return Ok(_mapper.Map<MessageTypeOutput>(modifiedMessageType));
            }
            catch (BadHttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
