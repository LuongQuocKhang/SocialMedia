using CQRS.Core.Exceptions;
using CQRS.Core.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Post.Cmd.Application.Commands.Message.EditMessageCommand;
using Post.Common.DTOs;
using Post.Common.DTOs.CommandResponse;

namespace Post.Cmd.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly ILogger<MessageController> _logger;
        private readonly ICommandDispatcher _commandDispatcher;

        public MessageController(ILogger<MessageController> logger, ICommandDispatcher commandDispatcher)
        {
            _logger = logger;
            _commandDispatcher = commandDispatcher;
        }

        [HttpPut("{postId}")]
        public async Task<ActionResult> EditMessageAsync(Guid postId, EditMessageCommand command)
        {
            try
            {
                command.Id = postId;
                await _commandDispatcher.SendAsync(command);

                return StatusCode(StatusCodes.Status200OK, new EditMessageResponse()
                {
                    Id = postId,
                    Message = "Edit message request completed successfully!"
                });
            }
            catch(AggregateNotFoundException exception)
            {
                _logger.Log(LogLevel.Warning, exception, "Could not retrieve aggregate, client passed incorrect post ID targetting the aggregate!", new BaseResponse()
                {
                    Message = exception.Message
                });
                 return BadRequest(new BaseResponse()
                {
                    Message = exception.Message
                });
            }
            catch (Exception exception)
            {
                const string SAFE_ERROR_MESSAGE = "Error while processing request to edit post message!";
                _logger.Log(LogLevel.Error, exception, SAFE_ERROR_MESSAGE);

                return StatusCode(StatusCodes.Status500InternalServerError, new NewPostResponse()
                {
                    Id = postId,
                    Message = SAFE_ERROR_MESSAGE
                });
            }
        }
    }
}