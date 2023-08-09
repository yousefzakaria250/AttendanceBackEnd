using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Data
{
    public class Employee :IdentityUser
    {
       // public int Id { set; get; }
        public string Fname { set; get; }
        public string Lname { set; get; }
        public DateTime DateOfHiring { set; get; }
        public int Balance { set; get; }
        public ICollection<Request> Requests { set; get; }

        [ForeignKey("Department")]
        public int DepartmentId { set; get; }
        [JsonIgnore]
        public Department Department { set; get; }
       
        [ForeignKey("Supervisior")]
       public string? SupervisiorId { set; get; }
        [JsonIgnore]
       public Employee? Supervisior { set; get; }
    }
}
