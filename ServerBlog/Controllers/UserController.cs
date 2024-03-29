﻿using Microsoft.AspNetCore.Authorization;
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
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ServerBlog.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository UserService;
        private readonly IAuthService AuthService;
        private readonly ILogger<UserController> Logger;

        public UserController(IAuthService authService, IUserRepository userService, ILogger<UserController> logger)
        {
            UserService = userService;
            AuthService = authService;
            Logger = logger;
        }

        /*
          Метод регистрации пользователя 
        */
        [HttpPost("registration")]
        public async Task<ActionResult<User>> Registration([FromBody] UserRegistrationRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var users = UserService.GetAll().ToList();
                if (users.FirstOrDefault(x => x.Login == request.Login) != null)
                {
                    return new JsonResult(new BaseResponse { Success = false, Message = "Пользователь с таким логином уже существует!" });
                }

                var user = new User()
                {
                    UserId = new Guid(),
                    Login = request.Login,
                    Email = request.Email,
                    Password = request.Password
                };

                UserService.Add(user);
                await UserService.Commit();

                return new JsonResult(new BaseResponse { Success = true, Message = "Вы успешно зарегестрировались" });
            }
            catch (Exception ex)
            {
                Logger.LogError($"Exception occurred with a message: {ex.Message}");
                return StatusCode(500, ex.Message);
            }
        }
        /*
           Метод авторизации пользователя 
        */
        [HttpPost("authorization")]
        public ActionResult Authorization([FromBody] AuthRequest request)
        {
            try
            {
                var user = UserService.GetSingle(u => u.Login == request.Login);

                var userValid = UserService.GetAll().Where(x => x.Password == request.Password && x.Login == request.Login).SingleOrDefault();
                if (userValid == null)
                {
                    return new JsonResult(new AuthResponse { Success = false, Message = $"Не найден пользователь с логином {request.Login} проверьте логин/пароль" });
                }

                var authInfo = AuthService.GetAuthData(user.UserId);

                return new JsonResult(new AuthResponse { Success = true, Token = authInfo.Token, UserId = authInfo.Id });
            }

            catch (Exception ex)
            {
                Logger.LogError($"Exception occurred with a message: {ex.Message}");
                return StatusCode(500, ex.Message);
            }
        }
    }
}
