using Data;
using Infrastructure.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class EmployeeRepo : IEmployeeRepo
    {
        private readonly AttendanceContext Context;
        private readonly UserManager<Employee>  userManager;

        public EmployeeRepo(AttendanceContext Context, UserManager<Employee> userManager)
        {
            this.Context = Context;
            this.userManager = userManager;
        }
        public async Task<Employee> Add(EmployeeDto dto , bool flag)
        {
            var Emp = new Employee();
            if (flag)
            {
                 Emp = new Employee()
                {
                    Fname = dto.Fname,
                    Lname = dto.Lname,
                    DateOfHiring = dto.DateOfHiring,
                    DepartmentId = dto.DepartmentId,
                    SupervisiorId = null
                };
                await Context.AddAsync(Emp);
                Context.SaveChanges();
                return Emp;
            }
             Emp = new Employee()
            {
                Fname = dto.Fname,
                Lname = dto.Lname,
                DateOfHiring = dto.DateOfHiring,
                DepartmentId = dto.DepartmentId,
                SupervisiorId = dto.SupervisiorId
            };

            await Context.AddAsync(Emp);
            Context.SaveChanges();
            return Emp;
        }

        public async Task<Employee> Delete(string UserId)
        {
            var emp = await GetEmployee(UserId);
            emp.IsDeleted = 1;
            emp.DateofLeaving = DateTime.Now;
            Context.Entry(emp).State = EntityState.Modified;
            Context.SaveChanges();
            return emp;
        }
        public async Task<List<Employee>> GetAll()
        {
            
            var Employees = await Context.Employee.Where(s=> s.IsDeleted != 1).ToListAsync();
            return Employees;

        }
        public async Task<dynamic> GetAllLeavingEmployee()
        {
            var res = await Context.Employee.Include(d=>d.Department).Where(s=> s.IsDeleted == 1).Select(emp =>new 
            {
                FName = emp.Fname,
                LName = emp.Lname ,
                deptName = emp.Department.Name ,
                DateofHiring = emp.DateOfHiring ,
                DateOfLeaving = emp.DateofLeaving ,
                Time = emp.DateofLeaving - emp.DateOfHiring
            }).ToListAsync();
            return res;
        }
        public async Task<List<Employee>> GetAllSupervisior()
        {
            var Employees = await Context.Employee.Where( emp=> ( emp.SupervisiorId == null && emp.IsDeleted != 1 ) ).ToListAsync();
            return Employees;
        }

        public async Task<List<Employee>> GetAllSupervisiorByDeptId(int DeptId)
        {
            var Employees = await Context.Employee.Where(emp => (emp.SupervisiorId == null && emp.IsDeleted != 1 && emp.DepartmentId == DeptId)).ToListAsync();
            return Employees;
        }

        public async Task<Employee> GetEmployee(string UserId)
        {
            var Emp = await Context.Employee.Include(r=>r.Requests).Where(e=> e.Id == UserId).FirstOrDefaultAsync();
            return Emp;
        }
        public async Task<Employee> Update(string UserId , EmpDto empDto)
        {
            var Emp = await GetEmployee(UserId);
            Emp.Fname = empDto.Fname;
            Emp.Lname = empDto.Lname;
            Emp.Balance = empDto.Balance;
            Context.Entry(Emp).State = EntityState.Modified;
            Context.SaveChanges();
            return Emp; 
        }

        


    }
}
