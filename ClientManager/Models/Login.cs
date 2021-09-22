using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClientManager.Models
{
    public class Login
    {
        public string Password { get; set; }
        public string Email { get; set; }
        public bool IsRememberMe { get; set; }
    }
}