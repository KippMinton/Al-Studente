using System;

namespace AlStudente.Models
{
    public class Student
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int TeacherId { get; set; }
        public DateTime DOB { get; set; }
        public DateTime StartDate { get; set; }
        public int PlayingSince { get; set; }
        public int Level { get; set; }
        public int LessonDayId { get; set; }
        public int LessonTimeId { get; set; }
    }
}