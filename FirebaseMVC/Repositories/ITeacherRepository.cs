using AlStudente.Models;

namespace AlStudente.Repositories
{
    public interface ITeacherRepository
    {
        void Add(Teacher teacher);
        Teacher GetByUserId(int id);
    }
}