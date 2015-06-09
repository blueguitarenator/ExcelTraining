using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Excel.Entities
{
    public class Session
    {
  
        public int Id { get; set; }
        
        [Required]
        public int Hour { get; set; }
        
        [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-mm-dd}")]
        public DateTime Day { get; set; }
        
        public virtual ICollection<Athlete> Athletes { get; set; }
    }
}
