using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRS.Core.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Post.Cmd.Application.Commands.Comment.AddCommentCommand;
using Post.Cmd.Application.Commands.Comment.EditCommentCommand;
using Post.Cmd.Application.Commands.Comment.RemoveCommentComand;
using Post.Common.DTOs;

namespace Post.Cmd.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CommentController : ControllerBase
    {
        private readonly ILogger<CommentController> _logger;
        private readonly ICommandDispatcher _commandDispatcher;

        public CommentController(ILogger<CommentController> logger, ICommandDispatcher commandDispatcher)
        {
            _logger = logger;
            _commandDispatcher = commandDispatcher;
        }

        [HttpPost("{postId}")]
        public async Task<ActionResult> NewCommentAsync(Guid postId, AddCommentCommand command)
        {
            try
            {
                command.Id = postId;
                await _commandDispatcher.SendAsync(command);

                return StatusCode(StatusCodes.Status201Created, new NewCommentResponse()
                {
                    Id = postId,
                    Message = "New comment creation request completed successfully!"
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
                const string SAFE_ERROR_MESSAGE = "Error while processing request to create a new comment!";
                _logger.Log(LogLevel.Error, exception, SAFE_ERROR_MESSAGE);

                return StatusCode(StatusCodes.Status500InternalServerError, new NewCommentResponse()
                {
                    Id = postId,
                    Message = SAFE_ERROR_MESSAGE
                });
            }
        } 

        [HttpPut("{postId}")]
        public async Task<ActionResult> EditCommentAsync(Guid postId, EditCommentCommand command)
        {
            try
            {
                command.Id = postId;
                await _commandDispatcher.SendAsync(command);

                return StatusCode(StatusCodes.Status201Created, new EditCommentResponse()
                {
                    Id = command.CommentId,
                    Message = "Edit Comment request completed successfully!"
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
                const string SAFE_ERROR_MESSAGE = "Error while processing request to edit a comment!";
                _logger.Log(LogLevel.Error, exception, SAFE_ERROR_MESSAGE);

                return StatusCode(StatusCodes.Status500InternalServerError, new EditCommentResponse()
                {
                    Id = postId,
                    Message = SAFE_ERROR_MESSAGE
                });
            }
        } 


        [HttpDelete("{postId}")]
        public async Task<ActionResult> DeleteCommentAsync(Guid postId, RemoveCommentComand command)
        {
            try
            {
                command.Id = postId;
                await _commandDispatcher.SendAsync(command);

                return StatusCode(StatusCodes.Status201Created, new RemoveCommentResponse()
                {
                    Id = command.CommentId,
                    Message = "Remove Comment request completed successfully!"
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
                const string SAFE_ERROR_MESSAGE = "Error while processing request to remove a comment!";
                _logger.Log(LogLevel.Error, exception, SAFE_ERROR_MESSAGE);

                return StatusCode(StatusCodes.Status500InternalServerError, new RemoveCommentResponse()
                {
                    Id = postId,
                    Message = SAFE_ERROR_MESSAGE
                });
            }
        } 
    }
}