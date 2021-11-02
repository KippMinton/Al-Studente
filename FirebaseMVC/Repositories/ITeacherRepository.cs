using AlStudente.Models;

namespace AlStudente.Repositories
{
    public interface ITeacherRepository
    {
        void Add(Teacher teacher);
        Teacher GetById(int id);
        Teacher GetByUserId(int id);
        void Update(Teacher teacher);
    }
}