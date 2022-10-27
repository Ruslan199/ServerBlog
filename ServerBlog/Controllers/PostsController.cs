using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServerBlog.Models;
using ServerBlog.Models.Entities;
using ServerBlog.Models.Request;
using ServerBlog.Models.Response;
using ServerBlog.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerBlog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly ILogger Logger;
        private readonly IUserRepository UserService;
        private readonly IPostRepository PostService;

        public PostsController(PostDBContext context, IUserRepository userService, IPostRepository postRepository, ILogger logger) 
        {
            UserService = userService;
            PostService = postRepository;
            Logger = logger;
        }

        // GET: api/posts/getUserPosts
        [HttpGet("getUserPosts")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Post>>> GetAllUserPosts()
        {
            try
            {
                var userId = HttpContext.User.Identity.Name;
                var allUserPosts = await PostService.AllIncludingAsync(x => x.UserId.ToString() == userId);
                return new JsonResult(new AllPostUser { Success = true, Message = "", Posts = allUserPosts });
            }
            catch (Exception ex)
            {
                Logger.LogError($"Exception occurred with a message: {ex.Message}");
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/posts/getUserPosts
        [HttpGet("getCountPosts")]
        public ActionResult<IEnumerable<Post>> GetCountAllUserPosts()
        {
            try
            {
                var getAllUser = UserService.GetAll().ToList();
                var allUserPosts = new List<AllPosts>();

                foreach (var user in getAllUser)
                {
                    var countPost = PostService.GetAll().Where(x => x.UserId == user.UserId).Count();
                    allUserPosts.Add(new AllPosts { UserName = user.Login, CountPost = countPost });
                }

                return new JsonResult(new AllPostsFromServer { AllPosts = allUserPosts });
            }
            catch (Exception ex)
            {
                Logger.LogError($"Exception occurred with a message: {ex.Message}");
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/DCandidate/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Post>> GetDCandidate(string id)
        {
            try
            {
                var dCandidate = await PostService.GetAsync(x => x.PostId.ToString() == id);

                if (dCandidate == null)
                {
                    return NotFound();
                }

                return dCandidate;
            }

            catch (Exception ex)
            {
                Logger.LogError($"Exception occurred with a message: {ex.Message}");
                return StatusCode(500, ex.Message);
            }
        }

        // PUT: api/DCandidate/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDCandidate(Guid id, Post dCandidate)
        {
            dCandidate.PostId = id;

            PostService.Update(dCandidate);

            try
            {
                await PostService.Commit();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostExists(dCandidate.PostId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/DCandidate
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [Authorize]
        [HttpPost("addPost")]
        public async Task<ActionResult<Post>> CreatePost([FromBody] AddPostRequest postRequest)
        {
            try
            {
                Post post = new Post()
                {
                    PostId = new Guid(),
                    Title = postRequest.Title,
                    Content = postRequest.Content,
                    CreatedOn = DateTime.Now,
                    UserId = UserService.GetSingle(x => x.UserId.ToString() == postRequest.UserId).UserId
                };

                PostService.Add(post);
                await PostService.Commit();

                return CreatedAtAction("GetDCandidate", new { id = post.PostId }, post);
            }
            catch (Exception ex)
            {
                Logger.LogError($"Exception occurred with a message: {ex.Message}");
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE: api/DCandidate/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Post>> DeleteDCandidate(string id)
        {
            try
            {
                var dCandidate = await PostService.GetAsync(x => x.PostId.ToString() == id);
                if (dCandidate == null)
                {
                    return NotFound();
                }

                PostService.Delete(dCandidate);
                await PostService.Commit();

                return dCandidate;
            }
            catch (Exception ex)
            {
                Logger.LogError($"Exception occurred with a message: {ex.Message}");
                return StatusCode(500, ex.Message);
            }
        }

        private bool PostExists(Guid id)
        {
            return PostService.GetAll().Any(x =>x.PostId == id);
        }
    }
}
