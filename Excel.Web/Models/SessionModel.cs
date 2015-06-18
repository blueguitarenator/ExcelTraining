using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Excel.Web.Models
{
    public class SessionModel
    {
        [DisplayFormat(DataFormatString = "{0:mm-dd-yyyy}",
               ApplyFormatInEditMode = true)]
        public DateTime SessionDateTime { get; set; }

        public int Hour { get; set; }
    }
}