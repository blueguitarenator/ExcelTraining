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

        public string HourDisplay
        {
            get { return FormatHour(Hour); }
        }

        private string FormatHour(int hour)
        {
            if (hour < 12)
            {
                return hour.ToString() + ":00 AM";
            }
            else if(hour == 12)
            {
                return "12:00 PM";
            }
            else
            {
                return (hour - 12).ToString() + ":00 PM";
            }
        }
    }
}
