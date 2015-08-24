using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Excel.Entities
{
    public class SessionAthlete
    {
        public int SessionId { get; set; }
        public int AthleteId { get; set; }
        public bool Confirmed { get; set; }

        public virtual Session Session { get; set; }
        public virtual Athlete Athlete { get; set; }
    }
}
