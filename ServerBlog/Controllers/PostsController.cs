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

        /*
          Получение всех постов пользователя
        */
        // GET: api/posts/getUserPosts
        [Authorize]
        [HttpGet("getUserPosts")]
        public async Task<ActionResult<IEnumerable<Post>>> GetAllUserPosts()
        {
            try
            {
                var userId = HttpContext.User.Identity.Name;
                var allUserPosts = PostService.FindBy(x=>x.UserId.ToString() == userId);
                return new JsonResult(new AllPostsUserResponse { Success = true, Message = "", Posts = allUserPosts });
            }
            catch (Exception ex)
            {
                Logger.LogError($"Exception occurred with a message: {ex.Message}");
                return new JsonResult(new BaseResponse { Success = false, Message = ex.Message });
            }
        }

        /*
          Получение количества постов пользователей
        */
        // GET: api/posts/getUserPosts
        [Authorize]
        [HttpGet("getCountPosts")]
        public ActionResult<IEnumerable<Post>> GetCountAllUserPosts()
        {
            try
            {
                var getAllUser = UserService.GetAll().ToList();
                var allUserPosts = new List<CountPostsUser>();

                foreach (var user in getAllUser)
                {
                    var countPost = PostService.FindBy(x => x.UserId == user.UserId).Count();
                    if (countPost > 0)
                    {
                        allUserPosts.Add(new CountPostsUser { UserName = user.Login, CountPost = countPost });
                    }
                }

                return new JsonResult(new CountPostsUserResponse { Success = true, Message = "", AllPosts = allUserPosts });
            }
            catch (Exception ex)
            {
                Logger.LogError($"Exception occurred with a message: {ex.Message}");
                return new JsonResult(new BaseResponse { Success = false, Message = ex.Message });
            }
        }


        /*
          Получение постов других пользователей
        */
        // GET: api/posts/getAllPosts
        [Authorize]
        [HttpGet("getAllPosts")]
        public ActionResult<IEnumerable<Post>> GetAllPosts()
        {
            try
            {
                var allUserPosts = PostService.GetAll().Where(x=>x.UserId.ToString() != HttpContext.User.Identity.Name).ToList();
                var posts = new List<PostsAnotherUsers>();

                foreach (var post in allUserPosts)
                {
                    posts.Add(new PostsAnotherUsers
                    {
                        Author =  GetAuthor(post.UserId),
                        Content = post.Content,
                        Title = post.Title,
                        CreatedOn = post.CreatedOn,
                        PostId = post.PostId
                    });
                }
                return new JsonResult(new PostsAnotherUsersResponse { Success = true, Message = "", Posts = posts });
            }
            catch (Exception ex)
            {
                Logger.LogError($"Exception occurred with a message: {ex.Message}");
                return new JsonResult(new PostsAnotherUsersResponse { Success = false, Message = ex.Message });
            }
        }

        /*
            Получение поста
        */
        // GET: api/posts/{id}
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
                return new JsonResult(new BaseResponse { Success = false, Message = ex.Message });
            }
        }

        /*
            Обновление поста
        */
        // PUT: api/posts/{id}
        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<Post>> PutPost(Guid id,[FromBody] UpdatePostRequest postRequest)
        {
            Post post = new Post()
            {
                PostId = id,
                Title = postRequest.Title,
                Content = postRequest.Content,
                UserId = new Guid(HttpContext.User.Identity.Name),
                CreatedOn = DateTime.Now
            };

            PostService.Update(post);

            try
            {
                await PostService.Commit();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostExists(post.PostId))
                {
                    return new JsonResult(new BaseResponse { Success = false, Message = $"Такого поста с id {post.PostId} не существует" });
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        /*
            Добавление поста
        */
        // POST: api/posts/addPost
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
                return new JsonResult(new BaseResponse { Success = false, Message = ex.Message });
            }
        }

        /*
           Удаление поста 
        */
        // DELETE: api/posts/{id}
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePost(string id)
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

                return NoContent();
            }
            catch (Exception ex)
            {
                Logger.LogError($"Exception occurred with a message: {ex.Message}");
                return new JsonResult(new BaseResponse { Success = false, Message = ex.Message });
            }
        }

        /*
           Проверка существования поста 
        */
        private bool PostExists(Guid id)
        {
            return PostService.GetAll().Any(x =>x.PostId == id);
        }

        /*
           Получение автора поста 
        */
        private string GetAuthor(Guid id)
        {
            return UserService.GetAll().FirstOrDefault(x => x.UserId == id).Login;
        }
    }
}
