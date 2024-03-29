﻿using System.Collections.Generic;

namespace AlStudente.Models.ViewModels
{
    public class StudentUserViewModel
    {
        public UserProfile UserProfile { get; set; }
        public Student Student { get; set; }
        public Teacher Teacher { get; set; }
        public UserProfile TeacherUser { get; set; }
        public LessonDay LessonDay { get; set; }
        public LessonTime LessonTime { get; set; }
        public Instrument Instrument { get; set; }
        public Level Level { get; set; }
        public List<TeacherNote> Notes { get; set; }
        public TeacherNote TeacherNote { get; set; }
    }
}