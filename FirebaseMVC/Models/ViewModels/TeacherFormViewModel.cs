﻿using System.Collections.Generic;
using AlStudente.Auth.Models;

namespace AlStudente.Models.ViewModels
{
    public class TeacherFormViewModel
    {
        public Registration Registration { get; set; }
        public UserProfile UserProfile { get; set; }
        public Teacher Teacher { get; set; }
        public List<Instrument> Instruments { get; set; }
    }
}