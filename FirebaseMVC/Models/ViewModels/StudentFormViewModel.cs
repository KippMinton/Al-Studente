using System.Collections.Generic;
using AlStudente.Auth.Models;

namespace AlStudente.Models.ViewModels
{
    public class StudentFormViewModel
    {
        public Registration Registration { get; set; }
        public UserProfile UserProfile { get; set; }
        public Student Student { get; set; }
        public Teacher Teacher { get; set; }
        public List<Instrument> Instruments { get; set; }
        public List<LessonDay> LessonDays { get; set; }
    }
}