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
        [Authorize]
        public async Task<IActionResult> GetAllEmployee()
        {
            var emp = await employeeRepo.GetAll();
            return Ok(emp);
        }
        [HttpGet("GetAllSupervisior")]
        [Authorize]
        public async Task<IActionResult> GetAllSupervisior()
        {
            var emp = await employeeRepo.GetAllSupervisior();
            return Ok(emp);
        }

        [HttpGet("GetAllSupervisiorByDeptId")]
        [Authorize]
        public async Task<IActionResult> GetAllSupervisiorByDeptId(int DeptId)
        {
            var emp = await employeeRepo.GetAllSupervisiorByDeptId(DeptId);
            return Ok(emp);
        }

        [HttpPut("UpdateEmployee")]
        [Authorize(Roles = "HR")]
        public async Task<IActionResult> UpdateEmployee(string UserId, EmpDto empDto)
        {
            var res = await employeeRepo.Update(UserId, empDto);
            return Ok(res);
        }

        [HttpPatch("DeleteEmplyee")]
        [Authorize(Roles = "HR")]
        public async Task<IActionResult> DeleteEmployee(string UserId)
        {
            var res = await employeeRepo.Delete(UserId);
            return Ok(res);

        }

        [HttpGet("GetAllDeleteEmployee")]
        [Authorize]
        public async Task<IActionResult> GetAllLeavingEmployee()
        {
            var res = await employeeRepo.GetAllLeavingEmployee();
            return Ok(res);

        }


    }
}
