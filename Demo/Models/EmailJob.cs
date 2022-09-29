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
        public void Execute(IJobExecutionContext context)
        {
            string MailServer = ConfigurationManager.AppSettings["MailServer"];
            int Mailport = Convert.ToInt32(ConfigurationManager.AppSettings["Mailport"]);
            string MailPNC = ConfigurationManager.AppSettings["MailPNC"];
            string PassworkMailPNC = ConfigurationManager.AppSettings["PassworkMailPNC"];
            using (var mail = new MailMessage(MailPNC, "kimnguyen035171@gmail.com"))
            {
                mail.Subject = "Phuong Nam Telecom";
                mail.Body = "Hello, Today: " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
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