using CQRS.Core.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Post.Common.DTOs;
using Post.Common.DTOs.QueryResponse;
using Post.Query.Application.Queries.Post;
using Post.Query.Domain.Entities;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Post.Query.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PostController : ControllerBase
    {
        private readonly ILogger<PostController> _logger;
        private readonly IQueryDispatcher<PostEntity> _queryDispatcher;

        public PostController(ILogger<PostController> logger, IQueryDispatcher<PostEntity> queryDispatcher)
        {
            _logger = logger;
            _queryDispatcher = queryDispatcher;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllPostsAsync()
        {
            var getAllPostsQuery = new FindAllPostsQuery();
            try
            {
                var posts = await _queryDispatcher.SendAsync(getAllPostsQuery);

                if(posts == null || !posts.Any())
                {
                    return NoContent();
                }

                var postsCount = posts.Count;

                return Ok(new PostLookUpResponse()
                {
                    Posts = posts,
                    Message = $"Successfully returned {postsCount} post{(postsCount > 1 ? "s" : string.Empty)}"
                });
            }
            catch (Exception exception)
            {
                const string SAFE_ERROR_MESSAGE = "Error while processing request to retrive all posts!";
                _logger.Log(LogLevel.Error, exception, SAFE_ERROR_MESSAGE);

                return StatusCode(StatusCodes.Status500InternalServerError, new PostLookUpResponse()
                {
                    Message = SAFE_ERROR_MESSAGE
                });
            }
        }

        [HttpGet("[Action]/{postId}")]
        public async Task<ActionResult> GetAllPostByIdAsync(Guid postId)
        {
            var getPostsByIdQuery = new FindPostByIdQuery()
            {
                Id = postId
            };
            try
            {
                var posts = await _queryDispatcher.SendAsync(getPostsByIdQuery);

                if (posts == null || !posts.Any())
                {
                    return NoContent();
                }

                var postsCount = posts.Count;

                return Ok(new PostLookUpResponse()
                {
                    Posts = posts,
                    Message = $"Successfully returned post"
                });
            }
            catch (Exception exception)
            {
                const string SAFE_ERROR_MESSAGE = "Error while processing request to retrive all posts!";
                _logger.Log(LogLevel.Error, exception, SAFE_ERROR_MESSAGE);

                return StatusCode(StatusCodes.Status500InternalServerError, new PostLookUpResponse()
                {
                    Message = SAFE_ERROR_MESSAGE
                });
            }
        }

        [HttpGet("[Action]/{author}")]
        public async Task<ActionResult> GetAllPostsByAuthorAsync(string author)
        {
            try
            {
                var posts = await _queryDispatcher.SendAsync(new FindPostsByAuthorQuery()
                {
                    Author = author
                });

                if (posts == null || !posts.Any())
                {
                    return NoContent();
                }

                var postsCount = posts.Count;

                return Ok(new PostLookUpResponse()
                {
                    Posts = posts,
                    Message = $"Successfully returned {postsCount} post{(postsCount > 1 ? "s" : string.Empty)}"
                });
            }
            catch (Exception exception)
            {
                const string SAFE_ERROR_MESSAGE = "Error while processing request to retrive all posts!";
                _logger.Log(LogLevel.Error, exception, SAFE_ERROR_MESSAGE);

                return StatusCode(StatusCodes.Status500InternalServerError, new PostLookUpResponse()
                {
                    Message = SAFE_ERROR_MESSAGE
                });
            }
        }

        [HttpGet("[Action]")]
        public async Task<ActionResult> GetAllPostsWithCommentAsync()
        {
            try
            {
                var posts = await _queryDispatcher.SendAsync(new FindPostsWithCommentsQuery());

                if (posts == null || !posts.Any())
                {
                    return NoContent();
                }

                var postsCount = posts.Count;

                return Ok(new PostLookUpResponse()
                {
                    Posts = posts,
                    Message = $"Successfully returned {postsCount} post{(postsCount > 1 ? "s" : string.Empty)}"
                });
            }
            catch (Exception exception)
            {
                const string SAFE_ERROR_MESSAGE = "Error while processing request to retrive all posts!";
                _logger.Log(LogLevel.Error, exception, SAFE_ERROR_MESSAGE);

                return StatusCode(StatusCodes.Status500InternalServerError, new PostLookUpResponse()
                {
                    Message = SAFE_ERROR_MESSAGE
                });
            }
        }

        [HttpGet("[Action]")]
        public async Task<ActionResult> GetAllPostsWithLikesAsync(int numberOfLikes)
        {
            try
            {
                var posts = await _queryDispatcher.SendAsync(new FindPostsWithLikesQuery()
                {
                    NumberOfLikes = numberOfLikes
                });

                if (posts == null || !posts.Any())
                {
                    return NoContent();
                }

                var postsCount = posts.Count;

                return Ok(new PostLookUpResponse()
                {
                    Posts = posts,
                    Message = $"Successfully returned {postsCount} post{(postsCount > 1 ? "s" : string.Empty)}"
                });
            }
            catch (Exception exception)
            {
                const string SAFE_ERROR_MESSAGE = "Error while processing request to retrive all posts!";
                _logger.Log(LogLevel.Error, exception, SAFE_ERROR_MESSAGE);

                return StatusCode(StatusCodes.Status500InternalServerError, new PostLookUpResponse()
                {
                    Message = SAFE_ERROR_MESSAGE
                });
            }
        }
    }
}