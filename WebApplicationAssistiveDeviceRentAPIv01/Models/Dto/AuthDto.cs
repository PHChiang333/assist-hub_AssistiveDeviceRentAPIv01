using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplicationAssistiveDeviceRentAPIv01.Class;

namespace WebApplicationAssistiveDeviceRentAPIv01.Models.Dto
{
    public class AuthDto
    {
    }

    public class requestRegister
    {
        public string Name { get; set; }
        //public string Phone { get; set; }
        public string Email { get; set; }  //unique
        public string Password { get; set; }
    }


    public class requestLogin
    {
        public string Email { get; set; }  //unique
        public string Password { get; set; }
    }

}