using CQRS.Core.Exceptions;
using CQRS.Core.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Post.Cmd.Application.Commands.Post.DeletePostCommand;
using Post.Cmd.Application.Commands.Post.LikePostCommand;
using Post.Cmd.Application.Commands.Post.NewPostCommand;
using Post.Common.DTOs;

namespace Post.Cmd.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PostController : ControllerBase
    {
        private readonly ILogger<PostController> _logger;
        private readonly ICommandDispatcher _commandDispatcher;

        public PostController(ILogger<PostController> logger, ICommandDispatcher commandDispatcher)
        {
            _logger = logger;
            _commandDispatcher = commandDispatcher;
        }

        [HttpPost(Name = "Add New Post")]
        public async Task<ActionResult> NewPostAsync(NewPostCommand command)
        {
            var id = Guid.NewGuid();
            try
            {
                command.Id = id;
                await _commandDispatcher.SendAsync(command);

                return StatusCode(StatusCodes.Status201Created, new NewPostResponse()
                {
                    Id = id,
                    Message = "New post creation request completed successfully!"
                });
            }
            catch (InvalidOperationException exception)
            {
                _logger.Log(LogLevel.Warning, exception, "Client made bad request!", new BaseResponse()
                {
                    Message = exception.Message
                });
                return BadRequest(new BaseResponse(){
                    Message = exception.Message
                });
            }
            catch (Exception exception)
            {
                const string SAFE_ERROR_MESSAGE = "Error while processing request to create a new post!";
                _logger.Log(LogLevel.Error, exception, SAFE_ERROR_MESSAGE);

                return StatusCode(StatusCodes.Status500InternalServerError, new NewPostResponse()
                {
                    Id = id,
                    Message = SAFE_ERROR_MESSAGE
                });
            }
        }

        [HttpPut("{postId}")]
        public async Task<ActionResult> LikePostAsync(Guid postId)
        {
            try
            {
                LikePostCommand command = new LikePostCommand() {
                    Id = postId
                };
                await _commandDispatcher.SendAsync(command);

                return StatusCode(StatusCodes.Status200OK, new EditMessageResponse()
                {
                    Id = postId,
                    Message = "Like Post request completed successfully!"
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
    
        [HttpDelete("{postId}")]
        public async Task<ActionResult> DeletePostAsync(Guid postId, DeletePostCommand command) 
        {
            try
            {
                command.Id = postId;
                
                await _commandDispatcher.SendAsync(command);

                return StatusCode(StatusCodes.Status200OK, new DeleteMessageResponse()
                {
                    Id = postId,
                    Message = "Delete Post request completed successfully!"
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

                return StatusCode(StatusCodes.Status500InternalServerError, new DeleteMessageResponse()
                {
                    Id = postId,
                    Message = SAFE_ERROR_MESSAGE
                });
            }
        }
    }
}