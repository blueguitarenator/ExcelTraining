using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Excel.Web.Models
{
    public class SessionTableAthletes
    {
        [DataType(DataType.DateTime)]
        public DateTime SessionDate { get; set; }

        public string[] SessionAthletes;
        public int hour;
    }
}