using System;

namespace AlStudente.Models
{
    public class LessonNotes
    {
        public int Id { get; set; }
        public int TeacherId { get; set; }
        public int StudentId { get; set; }
        public int LessonNumber { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime LessonDate { get; set; }
    }
}