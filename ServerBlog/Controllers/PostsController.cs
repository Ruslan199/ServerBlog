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
        private readonly ILogger<PostsController> Logger;
        private readonly IUserRepository UserService;
        private readonly IPostRepository PostService;

        public PostsController(IUserRepository userService, IPostRepository postRepository, ILogger<PostsController> logger) 
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
                var allUserPosts = PostService.FindBy(x=>x.UserId.ToString() == userId);
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
        [Authorize]
        public ActionResult<IEnumerable<Post>> GetCountAllUserPosts()
        {
            try
            {
                var getAllUser = UserService.GetAll().ToList();
                var allUserPosts = new List<AllPosts>();

                foreach (var user in getAllUser)
                {
                    var countPost = PostService.FindBy(x => x.UserId == user.UserId).Count();
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

        [HttpGet("getAllPosts")]
        [Authorize]
        public ActionResult<IEnumerable<Post>> GetAllPosts()
        {
            try
            {
                var allUserPosts = PostService.GetAll().ToList();
                var posts = new List<AllPostFromBlog>();

                foreach (var post in allUserPosts)
                {
                    posts.Add(new AllPostFromBlog
                    {
                        Author =  GetAuthor(post.UserId),
                        Content = post.Content,
                        Title = post.Title,
                        CreatedOn = post.CreatedOn,
                        PostId = post.PostId
                    });
                }
                return new JsonResult(new AllPostsFromServerResponse { Success = true, Message = "", Posts = posts });
            }
            catch (Exception ex)
            {
                Logger.LogError($"Exception occurred with a message: {ex.Message}");
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/DCandidate/5
        [HttpGet("GetPostById/{id}")]
        public async Task<ActionResult<Post>> GetPost(string id)
        {
            try
            {
                var post = await PostService.GetAsync(x => x.PostId.ToString() == id);

                if (post == null)
                {
                    return NotFound();
                }

                return post;
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
        public async Task<IActionResult> PutPost(Guid id, Post post)
        {
            post.PostId = id;

            PostService.Update(post);

            try
            {
                await PostService.Commit();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostExists(post.PostId))
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

                return new JsonResult(new BaseResponse { Success = true, Message = "Успешно добавлена запись"});
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
                var post = await PostService.GetAsync(x => x.PostId.ToString() == id);
                if (post == null)
                {
                    return NotFound();
                }

                PostService.Delete(post);
                await PostService.Commit();

                return post;
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

        private string GetAuthor(Guid id)
        {
            return UserService.GetAll().FirstOrDefault(x => x.UserId == id).Login;
        }
    }
}
