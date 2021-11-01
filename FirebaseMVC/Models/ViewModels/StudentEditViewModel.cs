using System.Collections.Generic;
using AlStudente.Auth.Models;

namespace AlStudente.Models.ViewModels
{
    public class StudentEditViewModel
    {
        public Registration Registration { get; set; }
        public UserProfile UserProfile { get; set; }
        public Student Student { get; set; }
        public Teacher Teacher { get; set; }
        public UserProfile TeacherUser { get; set; }
        public List<Instrument> Instruments { get; set; }
        public List<Level> Levels { get; set; }
        public List<LessonDay> LessonDays { get; set; }
        public List<LessonTime> LessonTimes { get; set; }
    }
}