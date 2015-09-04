using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Excel.Web.Models
{
    public class MotdViewModel
    {
        public String Motd { get; set; }
        public DateTime ExpireDateTime { get; set; }
    }
}