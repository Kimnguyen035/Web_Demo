using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Demo.Models;
using MySql.Data.MySqlClient;
using System.Threading.Tasks;
using System.Configuration;

namespace Demo.Controllers
{
    public class TaskQueueController : Controller
    {
        public string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;

        public MySqlConnection CreateConnection()
        {
            var con = new MySqlConnection(constr);
            return con;
        }

        public Queue<string> Insert_Queue(Product p, int length)
        {
            Queue<string> products = new Queue<string>();
            string query = "insert into product (title, status, created_at, updated_at, price, direction)" +
            " values";

            string created_at = p.Created_at.ToString("yyyy-MM-dd hh:mm:ss");
            string updated_at = p.Updated_at.ToString("yyyy-MM-dd hh:mm:ss");
            for (int i = 0; i < length; i++)
            {
                products.Enqueue(query + String.Format("(\"{0}\",{1}, '{2}', '{3}', {4}, \"{5}\")"
                , p.Name + i, p.Status, created_at, updated_at, p.Price + i, p.Direction));
            }
            return products;
        }

        public void Run_Queue(Product p, int len)
        {
            Task.Run(() =>
            {
                var products = Insert_Queue(p, len);
                int length = products.Count;
                var conn = CreateConnection();
                conn.Open();
                for (int i = 0; i < length; i++)
                {
                    string query = products.Peek();
                    products.Dequeue();
                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    string created_at = p.Created_at.ToString("yyyy-MM-dd hh:mm:ss");
                    string updated_at = p.Updated_at.ToString("yyyy-MM-dd hh:mm:ss");

                    cmd.Parameters.AddWithValue("@_name", p.Name);
                    cmd.Parameters.AddWithValue("@_status", p.Status);
                    cmd.Parameters.AddWithValue("@_created_at", created_at);
                    cmd.Parameters.AddWithValue("@_updated_at", updated_at);
                    cmd.Parameters.AddWithValue("@_price", p.Price);
                    cmd.Parameters.AddWithValue("@_direction", p.Direction);

                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                conn.Close();
            });
        }

        public ActionResult Action_Queue()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Action_Queue(int len, Product p)
        {
            Run_Queue(p, len);
            return RedirectToAction("Index", "Home");
        }
    }
}