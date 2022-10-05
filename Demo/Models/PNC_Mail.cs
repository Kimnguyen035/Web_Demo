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
        [Display(Name = "Email")]
        public string Receiver { get; set; }
        [Display(Name = "Title")]
        public string Subject { get; set; }
        [Display(Name = "Content")]
        public string Body { get; set; }
        //public HttpPostedFileBase Attachment { get; set; }
    }
}