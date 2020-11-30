using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PictlData.Attributes;
using PictlData.Models;
using PictlData.Services;
using PictlHelpers.Exceptions;
using System;
using System.Threading.Tasks;

namespace PictlAPI.Controllers
{
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService userService;

        public UsersController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpPost]
        [Route("login/{email}/{password}")]
        public async Task<IActionResult> LogIn(string email, string password)
        {
            try
            {
                //TODO: get parameters from request body
                var user = await userService.LogInAsync(email, password);
                return this.Ok(user);
            }

            catch (NullReferenceException e)
            {
                return this.BadRequest(e.Message);
            }

            catch (WrongPasswordException e)
            {
                return this.BadRequest(e.Message);
            }
            
            //var user = new User() { Email = "a", ID = 1 };  
        }

        [HttpPost]
        [Route("register/{email}/{password}/{fName}/{lName}")]
        public async Task<IActionResult> Register(string email, string password, string fName, string lname)
        {
            //TODO: Get User Info From Request And Create An user
            //TODO: get parameters from request body

            //var status = await userService.RegisterUser(user);
            //if (status) return this.Ok;
            var user = new User()
            {
                Email = email,
                FirstName = fName,
                LastName = lname,
                Password = password,
                IsDeleted = false
            };

            var status = await userService.RegisterUser(user);
            if (status) return this.Ok();

            return this.BadRequest();
        }

        [Authorize]
        [HttpPost]
        [Route("test")]
        public IActionResult Test()
        {
            return new JsonResult(new { message = "Authorized" }) { StatusCode = StatusCodes.Status200OK };
        }
    }
}
