using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClientManager.Models
{
    public class JsonReponse
    {
        public string message { get; set; }
        public string status { get; set; }
        public string redirectURL { get; set; }
    }

}