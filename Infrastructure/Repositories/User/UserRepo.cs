using Data;
using Infrastructure.Constants;
using Infrastructure.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static OfficeOpenXml.ExcelErrorValue;

namespace Infrastructure.Repositories.User
{
    public class UserRepo :IUserRepo
    {
        private readonly UserManager<Employee> userManger;
        private readonly IConfiguration config;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly JWT jwt;
        private readonly AttendanceContext context;

        public UserRepo(UserManager<Employee> _userManger,
            IConfiguration _config, RoleManager<IdentityRole> _roleManager, AttendanceContext _context, IOptions<JWT> _jwt)
        {
            this.context = _context;
            userManger = _userManger;
            config = _config;
            roleManager = _roleManager;
            jwt = _jwt.Value;
          //  this.jwt = jwt;
        }



        public async Task<AuthResponse> RegisetrAsync(RegisterDto userDTO)
        {
            //check if user has email registered before
            if (await userManger.FindByEmailAsync(userDTO.Email) is not null)
                return new AuthResponse { Message = "هذا البريد الالكتروني مستخدم من قبل" };
            //check if any user uses the same UserName
            if (await userManger.FindByNameAsync(userDTO.UserName) is not null)
                return new AuthResponse { Message = "اسم المستخم موجود بالفعل" };
            Employee user = new Employee();
            user.Id = Guid.NewGuid().ToString();
            user.UserName = userDTO.UserName;
            user.Email = userDTO.Email;
            user.DepartmentId = userDTO.DepartmentId;
            user.SupervisiorId = userDTO.SupervisiorId;
            user.DateOfHiring = userDTO.DateOfHiring;
            user.Balance = userDTO.Balance;
            user.Lname = userDTO.Lname;
            user.Fname = userDTO.Fname;
            user.PasswordHash = userDTO.Password;            
            //create user in database
            IdentityResult result = await userManger.CreateAsync(user, userDTO.Password);
            if (result.Succeeded)
            {
                // assign  new user to Role As user
                result = await userManger.AddToRoleAsync(user,Roles.UserRole);
                if (!result.Succeeded)
                    return new AuthResponse { Message = "Not Role Added" };
                var jwtSecurityToken = await this.CreateJwtToken(user);
                return new AuthResponse
                {
                    Email = user.Email,
                    ExpiresOn = jwtSecurityToken.ValidTo,
                    IsAuthenticated = true,
                    Roles = new List<string> { Roles.UserRole },
                    Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                    Username = user.UserName
                };
            }

            return new AuthResponse { Message = "User Not Added" };

        }



        public async Task<AuthResponse> Login(LoginDto userDto)
        {
            var AuthModel = new AuthResponse();
            Employee user = await userManger.FindByNameAsync(userDto.UserName);
            if (user == null)
            {
                AuthModel.Message = "اسم المستخدم غير صحيح";
                return AuthModel;
            }
            else
            {
                bool found = await userManger.CheckPasswordAsync(user, userDto.Password);
                if (found)
                {
                    var myToken = await CreateJwtToken(user);

                    var roleList = await userManger.GetRolesAsync(user);
                    AuthModel.IsAuthenticated = true;
                    AuthModel.Token = new JwtSecurityTokenHandler().WriteToken(myToken);
                    AuthModel.Email = user.Email;
                    AuthModel.Username = user.UserName;
                    AuthModel.Roles = roleList.ToList();
                    AuthModel.ExpiresOn = myToken.ValidTo;
                    return AuthModel;
                }
                else
                {
                    AuthModel.Message = "كلمة المرور غير صحيحه";
                    return AuthModel;
                }

            }
        }



        private async Task<JwtSecurityToken> CreateJwtToken(Employee user)
        {
            //get claims
            var UserClaims = await this.userManger.GetClaimsAsync(user);

            //get Role
            var roles = await userManger.GetRolesAsync(user);
            var roleClaims = new List<Claim>();
            foreach (var role in roles)
            {
                roleClaims.Add(new Claim(ClaimTypes.Role, role));
            }
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email,user.Email)
            }.Union(UserClaims).Union(roleClaims);
            SecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:secret"]));
            SigningCredentials signingCred = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            //create Token
            JwtSecurityToken myToken = new JwtSecurityToken(
            issuer: jwt.ValidIssuer,
            audience: jwt.ValidAudiance,
            claims: claims,
            expires: DateTime.Now.AddDays(jwt.DurationInDays),
            signingCredentials: signingCred
         );
            return myToken;
        }
    }
}

