using System;

namespace AlStudente.Models
{
    public class LessonComment
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreateDateTime { get; set; }
    }
}