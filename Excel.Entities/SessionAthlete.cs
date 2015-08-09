using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Excel.Entities
{
    public class SessionAthlete
    {
        [Key, Column(Order = 0)]
        public int SessionId { get; set; }
        [Key, Column(Order = 1)]
        public int AthleteId { get; set; }

        public virtual Session Session { get; set; }
        public virtual Athlete Athlete { get; set; }

        public bool Confirmed { get; set; }



    }
}
