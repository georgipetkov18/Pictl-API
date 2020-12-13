using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PictlData.Attributes;
using PictlData.Models;
using PictlData.Services;
using PictlHelpers;
using PictlHelpers.Exceptions;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace PictlAPI.Controllers
{
    [ApiController]
    [Route("[action]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService userService;

        public UsersController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> LogIn([FromBody] string rawInfo)
        {
            try
            {
                var parameters = rawInfo.GetParameters("email", "password");

                var user = await userService.LogInAsync(parameters["email"], parameters["password"]);
                return this.Ok(user);
            }

            catch (ArgumentNullException e)
            {
                return this.BadRequest(e.Message);
            }

            catch (WrongPasswordException e)
            {
                return this.BadRequest(e.Message);
            }
             
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] string rawInfo)
        {
            var parameters = rawInfo.GetParameters("email", "password", "firstName", "lastname");

            var user = new User()
            {
                Email = parameters["email"],
                FirstName = parameters["firstName"],
                LastName = parameters["lastname"],
                Password = parameters["password"],
                IsDeleted = false
            };

            var status = await userService.RegisterUser(user);
            if (status) return this.Ok();

            return this.BadRequest();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> GetUserInfo()
        {
            var token = this.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var handler = new JwtSecurityTokenHandler();
            var userID = int.Parse(handler.ReadJwtToken(token).Claims.First().Value);
            var user = await this.userService.GetUserAsync(userID);
            return new JsonResult(new { firstName = user.FirstName, lastName = user.LastName, email = user.Email });
        }
    }
}
