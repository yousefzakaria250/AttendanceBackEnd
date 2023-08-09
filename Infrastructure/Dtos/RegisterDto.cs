using Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Infrastructure.Dtos
{
    public class RegisterDto
    {
        public string Fname { set; get; }
        public string Lname { set; get; }
        public string UserName { set; get;  }
        public string Email { set; get; }
        public string Password { set; get; }
        public string ConfirmPassword { set; get; }
        public DateTime DateOfHiring { set; get; }
        public int Balance { set; get; }
        public int DepartmentId { set; get; }
        public string? SupervisiorId { set; get; }
        

    }
}
