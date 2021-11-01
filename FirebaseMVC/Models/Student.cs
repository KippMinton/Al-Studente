using System;
using System.ComponentModel.DataAnnotations;

namespace AlStudente.Models
{
    public class Student
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int TeacherId { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime DOB { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]

        public DateTime StartDate { get; set; }
        public int PlayingSince { get; set; }
        public int LevelId { get; set; }
        public int LessonDayId { get; set; }
        public int LessonTimeId { get; set; }
    }
}