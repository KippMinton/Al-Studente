using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlStudente.Models.ViewModels
{
    public class StudentUserViewModel
    {
        public UserProfile UserProfile { get; set; }
        public Student Student { get; set; }
    }
}