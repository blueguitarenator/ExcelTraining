using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Excel.Entities
{
    public class InjuryNote
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Note Date")]
        public DateTime NoteDate { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Message { get; set; }

        public virtual Athlete Athlete { get; set; }
    }
}
