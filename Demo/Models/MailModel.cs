using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Demo.Models
{
    public class MailModel
    {
        public int ID { get; set; }
        public string Data_job { get; set; }
        public string Time_start { get; set; }
        public string Type_start { get; set; }
        public DateTime Created_at { get; set; }
        public DateTime Updated_at { get; set; }
        public DateTime Delete_at { get; set; }
        public string Updated_by { get; set; }
        public string Key { get; set; }
    }

    public class Data_job
    {
        public string Receiver { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}