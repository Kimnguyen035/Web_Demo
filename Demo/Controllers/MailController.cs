using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Net.Mail;
using System.Configuration;
using Demo.Models;

namespace Demo.Controllers
{
    public class MailController : Controller
    {
        // GET: Mail
        public ActionResult SendMail()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SendMail(PNC_Mail m)
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