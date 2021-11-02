using System;
using System.ComponentModel.DataAnnotations;

namespace AlStudente.Models
{
    public class TeacherNote
    {
        public int Id { get; set; }
        public int TeacherId { get; set; }
        public int StudentId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime CreateDateTime { get; set; }
    }
}