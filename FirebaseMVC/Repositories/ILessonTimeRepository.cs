using System.Collections.Generic;
using AlStudente.Models;

namespace AlStudente.Repositories
{
    public interface ILessonTimeRepository
    {
        LessonTime GetById(int id);
        List<LessonTime> GetAll();
    }
}