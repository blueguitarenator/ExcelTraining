using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Excel.Entities
{
    public class Schedule
    {
        public int Id { get; set; }

        [Required]
        public int Hour { get; set; }

        [Required]
        public bool IsAvailable { get; set; }

        [Required]
        public AthleteTypes AthleteType { get; set; }

        [Required]
        public Location Location { get; set; }
    }
}
