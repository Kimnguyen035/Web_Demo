using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Demo.Models
{
    public class PNC_Mail
    {
        [Required(ErrorMessage = "You need enter receiver")]
        public string Receiver { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        //public HttpPostedFileBase Attachment { get; set; }
    }
}