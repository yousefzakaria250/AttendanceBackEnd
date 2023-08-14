using Data;
using Infrastructure;
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
    public class EmployeeController : ControllerBase
    {
        private readonly UserManager<Employee> userManager;
        private readonly IEmployeeRepo employeeRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public EmployeeController(IEmployeeRepo employeeRepo, UserManager<Employee> userManager, IHttpContextAccessor httpContextAccessor)
        {
            this.userManager = userManager;
            this.employeeRepo = employeeRepo;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost("AddEmployee")]
        [Authorize]
        public async Task<IActionResult> Add(EmployeeDto dto , bool flag)
        { 
            var Emp = await employeeRepo.Add(dto , flag);
            return Ok(Emp);
        }

        [HttpGet("GetAllEmployee")]
        public async Task<IActionResult> GetAllEmployee()
        {
            var emp = await employeeRepo.GetAll();
            return Ok(emp);
        }
        [HttpGet("GetAllSupervisior")]
        public async Task<IActionResult> GetAllSupervisior()
        {
            var emp = await employeeRepo.GetAllSupervisior();
            return Ok(emp);
        }

        [HttpPut("UpdateBalance")]

        public async Task<IActionResult> UpdateBalance(string UserId, int balance)
        {
            var res = await employeeRepo.Update(UserId, balance);
            return Ok(res);

        }


    }
}
