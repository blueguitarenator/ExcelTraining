using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Excel.Entities
{
    
    public enum AthleteTypes
    {
        [Display(Name = "Personal Training")]
        PersonalTraining,
        [Display(Name = "Sports Training")]
        SportsTraining
    }

    public enum UserTypes
    {
        Trainer,
        Athlete,
        Trial
    }

}
