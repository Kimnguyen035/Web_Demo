﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using MySql.Data.MySqlClient;
using Demo.Models;
using ServiceStack.Redis;
using StackExchange.Redis;
using Newtonsoft.Json;
using PagedList.Mvc;
using PagedList;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using System.IO;

namespace Demo.Controllers
{
    public class HomeController : Controller
    {
        public string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;

        //private static Lazy<ConnectionMultiplexer> lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
        //{
        //    string cacheConnection = ConfigurationManager.AppSettings["CacheConnection"].ToString();
        //    return ConnectionMultiplexer.Connect(cacheConnection);
        //});

        //public static ConnectionMultiplexer Connection
        //{
        //    get
        //    {
        //        return lazyConnection.Value;
        //    }
        //}

        public MySqlConnection CreateConnection()
        {
            var con = new MySqlConnection(constr);
            return con;
        }

        //public List<Product> GetFromList()
        //{
        //    List<Product> product = null;

        //    IDatabase cache = Connection.GetDatabase();
        //    string serializedTeams = cache.StringGet("products");
        //    if (!String.IsNullOrEmpty(serializedTeams))
        //    {
        //        product = JsonConvert.DeserializeObject<List<Product>>(serializedTeams);

        //        ViewBag.msg += "List read from cache. ";
        //    }
        //    else
        //    {
        //        ViewBag.msg += "Teams list cache miss. ";
        //        // Get from database and store in cache
        //        product = GetData();

        //        ViewBag.msg += "Storing results to cache. ";
        //        cache.StringSet("products", JsonConvert.SerializeObject(product));
        //    }

        //    return product;
        //}

        //public ActionResult Counter()
        //{
        //    var cacheKey = "product";
        //    var host = ConfigurationManager.AppSettings["host"].ToString();
        //    var port = Convert.ToInt32(ConfigurationManager.AppSettings["port"]);
        //    RedisEndpoint redisEndpoint = new RedisEndpoint(host, port);
        //    List<Product> lst = GetData();

        //    using (var client = new RedisClient(redisEndpoint))
        //    {
        //        ViewBag.Visit = client.Increment(cacheKey, 3);
        //    }

        //    return View();
        //}

        
        public List<Product> GetData()
        {
            List<Product> list = new List<Product>();
            var conn = CreateConnection();
            conn.Open();

            string query = "select * from product";
            MySqlCommand cmd = new MySqlCommand(query, conn);

            using (var rd = cmd.ExecuteReader())
            {
                while (rd.Read())
                {
                    list.Add(new Product()
                    {
                        ID = Convert.ToInt32(rd["id"]),
                        Name = Convert.ToString(rd["title"]),
                        Status = Convert.ToInt32(rd["status"]),
                        Created_at = Convert.ToDateTime(rd["created_at"]),
                        Updated_at = Convert.ToDateTime(rd["updated_at"]),
                        Price = Convert.ToDouble(rd["price"]),
                        Direction = Convert.ToString(rd["direction"])
                    });
                }
            }
            cmd.Dispose();
            conn.Close();

            return list;
        }

        public ActionResult Index(int? pg)
        {
            var lst = GetData();

            var data = lst.ToList().ToPagedList(pg ?? 1, 3);

            return View(data);
        }

        public ActionResult Create()
        {
            return View(new Product());
        }

        [HttpPost]
        public ActionResult Create(Product p)
        {
            if (p.Created_at < DateTime.Now || p.Updated_at < DateTime.Now)
            {
                ViewBag.Message = "You need enter today or feature";
            }
            try
            {
                if (ModelState.IsValid)
                {
                    var conn = CreateConnection();
                    conn.Open();
                    string query = "insert into product (title, status, created_at, updated_at, price, direction)" +
                    " values(@_name,@_status,@_created_at,@_updated_at,@_price,@_direction)";
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
                    conn.Close();

                    //Run_Queue(p);
                    
                    return RedirectToAction("Index", "Home");
                }
                return View(p);
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Update(int id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Update(Product p)
        {
            var conn = CreateConnection();
            conn.Open();

            string query = "update product set " +
            "title=@_name,status=@_status,created_at=@_created_at,updated_at=@_updated_at,price=@_price,direction=@_direction " +
            "where id=@_id";
            MySqlCommand cmd = new MySqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@_name", p.Name);
            cmd.Parameters.AddWithValue("@_status", p.Status);
            cmd.Parameters.AddWithValue("@_created_at", p.Created_at);
            cmd.Parameters.AddWithValue("@_updated_at", p.Updated_at);
            cmd.Parameters.AddWithValue("@_price", p.Price);
            cmd.Parameters.AddWithValue("@_direction", p.Direction);
            cmd.Parameters.AddWithValue("@_id", p.ID);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            conn.Close();
            return RedirectToAction("Index", "Home");
        }

        public void GetProductDetail(Product p)
        {
            var conn = CreateConnection();
            conn.Open();

            MySqlCommand cmd = new MySqlCommand("select * from product where id = @_id", conn);
            cmd.Parameters.AddWithValue("@_id", Convert.ToInt32(p.ID));
            using (var rd = cmd.ExecuteReader())
            {
                while (rd.Read())
                {
                    p.ID = Convert.ToInt32(rd["id"]);
                    p.Name = Convert.ToString(rd["title"]);
                    p.Status = Convert.ToInt32(rd["status"]);
                    p.Created_at = Convert.ToDateTime(rd["created_at"]);
                    p.Updated_at = Convert.ToDateTime(rd["updated_at"]);
                    p.Price = Convert.ToDouble(rd["price"]);
                    p.Direction = Convert.ToString(rd["direction"]);
                }
            }
            cmd.Dispose();

            conn.Close();
        }

        public ActionResult Detail(int? _id, Product p)
        {
            GetProductDetail(p);
            return View(p);
        }

        public ActionResult RecycleBin()
        {

            return View();
        }

        public ActionResult Delete(int id)
        {
            return View();
        }
        [HttpPost]
        public ActionResult Delete(Product p)
        {
            var conn = CreateConnection();
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("delete from product where id = @_id", conn);

            cmd.Parameters.AddWithValue("@_id", p.ID);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            conn.Close();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult SentMail()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SentMail(PNC_Mail m)
        {
            string MailServer = ConfigurationManager.AppSettings["MailServer"].ToString();
            int Mailport = Convert.ToInt32(ConfigurationManager.AppSettings["Mailport"]);
            string MailPNC = ConfigurationManager.AppSettings["MailPNC"].ToString();
            string PassworkMailPNC = ConfigurationManager.AppSettings["PassworkMailPNC"].ToString();
            try
            {
                using (var mail = new MailMessage(MailPNC, m.Receiver))
                {
                    mail.Subject = m.Subject;
                    mail.Body = m.Body;
                    //if (m.Attachment != null)
                    //{
                    //    string file_name = Path.GetFileName(m.Attachment.FileName);
                    //    mail.Attachments.Add(new Attachment(m.Attachment.InputStream, file_name));
                    //}
                    mail.IsBodyHtml = false;
                    using (SmtpClient smtp = new SmtpClient())
                    {
                        smtp.Host = MailServer;
                        smtp.EnableSsl = true;
                        NetworkCredential _networkCredential = new NetworkCredential(MailPNC, PassworkMailPNC);
                        smtp.UseDefaultCredentials = true;
                        smtp.Credentials = _networkCredential;
                        smtp.Port = Mailport;
                        smtp.Send(mail);
                    }
                }
                ViewBag.Message = "Send mail sucessfully";
            }
            catch
            {
                ViewBag.Error = "Send mail failled";
            }

            return View();
        }
    }
}