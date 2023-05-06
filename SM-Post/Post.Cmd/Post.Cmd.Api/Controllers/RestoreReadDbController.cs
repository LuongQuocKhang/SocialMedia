using CQRS.Core.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Post.Cmd.Application.Commands;
using Post.Common.DTOs;
using Post.Common.DTOs.CommandResponse;
using System;

namespace Post.Cmd.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class RestoreReadDbController : ControllerBase
    {
        private readonly ILogger<RestoreReadDbController> _logger;
        private readonly ICommandDispatcher _commandDispatcher;

        public RestoreReadDbController(ILogger<RestoreReadDbController> logger, ICommandDispatcher commandDispatcher)
        {
            _logger = logger;
            _commandDispatcher = commandDispatcher;
        }
        [HttpPost]
        public async Task<ActionResult> RestoreReadDb()
        {
            var command = new RestoreReadDbCommand();
            try
            {
                await _commandDispatcher.SendAsync(command);
                return StatusCode(StatusCodes.Status202Accepted, new RestoreReadDbResponse()
                {
                    Message = "Read database restore request completed successfully!"
                });
            }
            catch (InvalidOperationException exception)
            {
                _logger.Log(LogLevel.Warning, exception, "Client made bad request!", new BaseResponse()
                {
                    Message = exception.Message
                });
                return BadRequest(new BaseResponse()
                {
                    Message = exception.Message
                });
            }
            catch (Exception ex)
            {
                const string SAFE_ERROR_MESSAGE = "Error while processing request!";
                _logger.Log(LogLevel.Error, ex, SAFE_ERROR_MESSAGE);

                return StatusCode(StatusCodes.Status500InternalServerError, new RestoreReadDbResponse()
                {
                    Message = SAFE_ERROR_MESSAGE
                });
            }
        }
    }
}
