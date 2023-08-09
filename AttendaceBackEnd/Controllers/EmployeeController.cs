using Data;
using Infrastructure;
using Infrastructure.Dtos;
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

        public EmployeeController(IEmployeeRepo employeeRepo , UserManager<Employee> userManager)
        {
            this.userManager = userManager;
            this.employeeRepo = employeeRepo;
        }


      

        [HttpPost("AddEmployee")]

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

        [HttpGet("GetEmployee")]
        public async Task<IActionResult> Get()
        {
            string UserId =  userManager.GetUserId(HttpContext.User);
            .
            var res = await employeeRepo.GetEmployee(UserId);
            return Ok(res);
        }
    }
}
