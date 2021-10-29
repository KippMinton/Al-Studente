using System.Collections.Generic;

namespace AlStudente.Models.ViewModels
{
    public class TeacherFormViewModel
    {
        public UserProfile UserProfile { get; set; }
        public Teacher Teacher { get; set; }
        public List<Instrument> Instruments { get; set; }
    }
}