using AspNetCore.ReportingServices.ReportProcessing.ReportObjectModel;
using Data;
using Infrastructure.Dtos;
using Infrastructure.Repositories.User;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;

namespace AttendaceBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
       private readonly IUserRepo repo ;
        private readonly IHttpContextAccessor httpContextAccessor ;
        private readonly Microsoft.AspNetCore.Identity.UserManager<Employee> userManager;
        public UserController(IUserRepo repo, IHttpContextAccessor httpContextAccessor, Microsoft.AspNetCore.Identity.UserManager<Employee> userManager)
        {
            this.repo = repo;
            this.httpContextAccessor = httpContextAccessor;
            this.userManager = userManager;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDto userDTO)
        {
            if (ModelState.IsValid)
            {
                var authenticationModel = await repo.RegisetrAsync(userDTO);
                if (authenticationModel.IsAuthenticated)
                {
                    return Ok(new { Token = authenticationModel.Token, Expiration = authenticationModel.ExpiresOn });
                }
                return
                    BadRequest(authenticationModel.Message);
            }
            return BadRequest(ModelState);
        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto userDTO)
        {
            if (ModelState.IsValid)
            {
                var authenticationModel = await repo.Login(userDTO);
                if (authenticationModel.IsAuthenticated)
                {
                    return Ok(new { Token = authenticationModel.Token, Expiration = authenticationModel.ExpiresOn });
                }
                return
                    BadRequest(authenticationModel.Message);
            }
            return BadRequest(ModelState);
        }

        
        [HttpGet("GetCurrentUser")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await userManager.FindByNameAsync(userName);
            var res = await repo.GetCurrentUser(user.Id);
            return Ok(user);
        }

    }
}
