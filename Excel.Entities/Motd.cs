using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Excel.Entities
{
    public class Motd
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Display Date")]
        public DateTime DisplayDate { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Message { get; set; }

        [Display(Name = "Days To Live")]
        public int DaysToLive { get; set; }
    }
}
