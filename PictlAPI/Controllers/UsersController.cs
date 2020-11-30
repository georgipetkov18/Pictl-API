using Microsoft.AspNetCore.Mvc;
using PictlCommon.Exceptions;
using PictlData.Services;
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
                var user = await userService.GetUserAsync(email, password);
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
        [Route("register")]
        public async Task<IActionResult> Register()
        {
            //TODO: Get User Info From Request And Create An user

            //var status = await userService.RegisterUser(user);
            //if (status) return this.Ok();

            return this.BadRequest();
        }
    }
}
