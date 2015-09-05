using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Excel.Web.Models
{
    public class MotdViewModel
    {

        public int DaysToLive { get; set; }

        public string Motd { get; set; }
    }
}