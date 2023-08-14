using Data;
using Infrastructure.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public interface IEmployeeRepo
    {
        public Task<Employee> Add(EmployeeDto dto , bool flag);
        public Task<List<Employee>> GetAll();
        public Task<List<Employee>> GetAllSupervisior();
        public Task<Employee> GetEmployee(string UserId);
        public Task<Employee> Update(string UserId , int balance);
    }
}
