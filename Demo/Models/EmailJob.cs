using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using Quartz;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace Demo.Models
{
    public class EmailJob : IJob
    {
        public string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;

        public List<MailModel> GetAllMailJob()
        {
            List<MailModel> list_mailjob = new List<MailModel>();
            var conn = CreateConnection();
            conn.Open();

            string query = "select * from cronjob";
            MySqlCommand cmd = new MySqlCommand(query, conn);

            using (var rd = cmd.ExecuteReader())
            {
                while (rd.Read())
                {
                    list_mailjob.Add(new MailModel
                    {
                        ID = Convert.ToInt32(rd["id"]),
                        Data_job = Convert.ToString(rd["data_job"]),
                        Time_start = Convert.ToString(rd["time_start"]),
                        Type_start = Convert.ToString(rd["type_start"])
                    });
                }
            }
            
            return list_mailjob;
        }

        public MySqlConnection CreateConnection()
        {
            var con = new MySqlConnection(constr);
            return con;
        }

        public void Execute(IJobExecutionContext context)
        {
            var lst = GetAllMailJob();
            JObject json = new JObject();
            json = JObject.Parse(lst[0].Data_job);
            string email = json.GetValue("email").ToString();
            string title = json.GetValue("title").ToString();
            string content = json.GetValue("content").ToString();

            string MailServer = ConfigurationManager.AppSettings["MailServer"];
            int Mailport = Convert.ToInt32(ConfigurationManager.AppSettings["Mailport"]);
            string MailPNC = ConfigurationManager.AppSettings["MailPNC"];
            string PassworkMailPNC = ConfigurationManager.AppSettings["PassworkMailPNC"];
            using (var mail = new MailMessage(MailPNC, email))
            {
                mail.Subject = title;
                mail.Body = content + ", Today: " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
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
        }
    }
}