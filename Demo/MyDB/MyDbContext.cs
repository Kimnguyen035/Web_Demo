using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Demo.MyDB
{
    public class MyDbContext
    {
        private string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
        public MySqlConnection CreateConnection()
        {
            var con = new MySqlConnection(constr);
            return con;
        }
    }
}