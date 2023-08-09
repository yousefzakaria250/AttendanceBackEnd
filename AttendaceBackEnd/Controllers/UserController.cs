using Infrastructure.Dtos;
using Infrastructure.Repositories.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AttendaceBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
       private readonly IUserRepo repo ;

        public UserController(IUserRepo repo)
        {
            this.repo = repo;
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
    }
}
