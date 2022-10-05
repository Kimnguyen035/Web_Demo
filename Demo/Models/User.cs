using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Demo.Models
{
    public class User
    {
        public int ID { get; set; }
        public string User_name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime Created_at { get; set; }
    }
}