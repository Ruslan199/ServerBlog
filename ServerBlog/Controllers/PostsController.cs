using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServerBlog.Models;
using ServerBlog.Models.Request;
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
        private readonly PostDBContext Context;
        private readonly IUserRepository UserService;

        public PostsController(PostDBContext context, IUserRepository userService) 
        {
            Context = context;
            UserService = userService;
        }

        // GET: api/DCandidate
        [HttpGet("getPosts")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Post>>> GetDCandidates()
        {
            var userId = HttpContext.User.Identity.Name;
            return await Context.Posts.Where(x=>x.UserId.ToString() == userId).ToListAsync();
        }

        // GET: api/DCandidate/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Post>> GetDCandidate(int id)
        {
            var dCandidate = await Context.Posts.FindAsync(id);

            if (dCandidate == null)
            {
                return NotFound();
            }

            return dCandidate;
        }

        // PUT: api/DCandidate/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDCandidate(Guid id, Post dCandidate)
        {
            dCandidate.PostId = id;

            Context.Entry(dCandidate).State = EntityState.Modified;

            try
            {
                await Context.SaveChangesAsync();
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
        public async Task<ActionResult<Post>> PostDCandidate([FromBody] AddPostRequest postRequest)
        {
            Post post = new Post()
            {
                PostId = new Guid(),
                Title = postRequest.Title,
                Content = postRequest.Content,
                CreatedOn = DateTime.Now,
                UserId = UserService.GetSingle(x=>x.UserId.ToString() == postRequest.UserId).UserId
            };


            Context.Posts.Add(post);
            await Context.SaveChangesAsync();

            return CreatedAtAction("GetDCandidate", new { id = post.PostId }, post);
        }

        // DELETE: api/DCandidate/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Post>> DeleteDCandidate(Guid id)
        {
            var dCandidate = await Context.Posts.FindAsync(id);
            if (dCandidate == null)
            {
                return NotFound();
            }

            Context.Posts.Remove(dCandidate);
            await Context.SaveChangesAsync();

            return dCandidate;
        }

        private bool PostExists(Guid id)
        {
            return Context.Posts.Any(e => e.PostId == id);
        }
    }
}
