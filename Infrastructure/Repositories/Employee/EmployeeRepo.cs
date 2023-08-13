using Data;
using Infrastructure.Dtos;
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

        public EmployeeRepo(AttendanceContext Context)
        {
            this.Context = Context;
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

        

        public Task<List<Employee>> GetAll()
        {
            var Employees = Context.Employee.ToListAsync();
            return Employees;

        }

        public Task<List<Employee>> GetAllSupervisior()
        {
            var Employees = Context.Employee.Where( emp=> emp.SupervisiorId == null).ToListAsync();
            return Employees;
        }

        public async Task<Employee> GetEmployee(string UserId)
        {
            var Emp = await Context.Employee.Include(r=>r.Requests).Where(e=> e.Id == UserId).FirstOrDefaultAsync();
            return Emp;
        }
    }
}
