using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApp.Models
{
    public class CommonHttpHeaders
    {
        public string Connection { get; set; }
        public string Accept { get; set; }
        public string AcceptCharset { get; set; }
        public string AcceptEncoding { get; set; }
        public string AcceptLanguage { get; set; }
        public string Host { get; set; }
        public string UserAgent { get; set; }
    }
}