namespace AlStudente.Models
{
    public class Teacher
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public bool AcceptingStudents { get; set; }
        public double LessonRate { get; set; }
    }
}