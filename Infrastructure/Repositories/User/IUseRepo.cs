using Data;
using Infrastructure.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.User
{
    public interface IUserRepo
    {
        public  Task<AuthResponse> RegisetrAsync(RegisterDto userDTO);
        public Task<AuthResponse> Login(LoginDto loginDto);
        public Task<List<Request>> GetCurrentUser(string UserId);

    }
}
