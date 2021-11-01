using System.Collections.Generic;

namespace AlStudente.Models.ViewModels
{
    public class TeacherUserViewModel
    {
        public UserProfile UserProfile { get; set; }
        public Teacher Teacher { get; set; }
        public StudentUserViewModel StudentVM { get; set; }
        public List<StudentUserViewModel> Students { get; set; }
        public List<Instrument> Instruments { get; set; }
        public Instrument Instrument { get; set; }
    }
}