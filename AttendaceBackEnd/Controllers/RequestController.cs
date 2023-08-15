using Data;
using Infrastructure;
using Infrastructure.Constants;
using Infrastructure.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AttendaceBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        private readonly IRequestRepo requestRepo;
        private readonly UserManager<Employee> userManager;

        public RequestController(IRequestRepo requestRepo, UserManager<Employee> userManager)
        {
            this.requestRepo = requestRepo;
            this.userManager = userManager;
        }

        [HttpPost("AddRequest")]
        public async Task<IActionResult> Add( RequestDto requestDto)
        {
            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await userManager.FindByNameAsync(userName);
           // var res = await repo.GetCurrentUser(user.Id);
            var res = await requestRepo.Add(requestDto , user.Id);
            return Ok(res);
        }
        ///
        [HttpGet("GetAllRequestsSupervisior")]
        [Authorize(Roles = "DM")]
        public async Task<IActionResult> GetAll(int deptNo)
        {
            var res = await requestRepo.GetAllRequestOfSupervisior(deptNo);
            return Ok(res);
        }


        [HttpPatch("ChangeRequestStateOfSupervisior")]
        [Authorize(Roles = "DM")]
        public async Task<IActionResult> UpdateState( int Id , int State)
        {
            var res = await requestRepo.ChangeRequestStateBySupervisior(Id, State);
            if (res == null)
                return NotFound("This Request Not Found .");
            return Ok(res); 
        }

        [HttpGet("GetAllRequestOfGm")]
        [Authorize(Roles = "GM")]
        public async Task<IActionResult> GetAllOfGM(DateTime date ,int deptNo = 0 )
        {
            var res = await requestRepo.GetAllRequestOfGM( date ,deptNo);
            return Ok(res);
        }


        [HttpPatch("ChangeRequestStateByGM")]
        [Authorize(Roles = "GM")]
        public async Task<IActionResult> UpdateStateByGm(int Id, int State)
        {
            var res = await requestRepo.ChangeRequestStateByGM(Id, State);
            if(res== null)
                return NotFound("This Request Not Found .");
            return Ok(res);
        }

        [HttpDelete("DeleteRequest")]
        public async Task<IActionResult> DeleteRequest(int Id)
        {
            var res = await requestRepo.Delete(Id);
            return Ok(res);
        }

    }
}
