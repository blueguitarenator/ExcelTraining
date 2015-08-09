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
        public virtual int LocationId { get; set; }
        public virtual Location Location { get; set; }
        
        [Required]
        public int Hour { get; set; }
        
        [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:mm-dd-yyyy}")]
        [Display(Name = "Session Date")]
        public DateTime Day { get; set; }

        [Required]
        public AthleteTypes AthleteType { get; set; }

        public virtual ICollection<SessionAthlete> SessionAthletes { get; set; }
    }
}
