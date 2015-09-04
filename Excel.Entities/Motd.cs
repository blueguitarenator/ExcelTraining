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
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:mm-dd-yyyy}")]
        [Display(Name = "Expire Date")]
        public DateTime ExpireDateTime { get; set; }

        public string Message { get; set; }
    }
}
