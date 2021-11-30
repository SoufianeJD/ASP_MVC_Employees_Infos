using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace T4EmployeesMVC.Models
{


    public class user
    {
        public string IdUser { get; set; }
        public string UserName { get; set; }
        public string Passwrd { get; set; }
        public string Surname { get; set; }
        public string EmailAddress { get; set; }
    }
}