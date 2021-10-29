using System.Collections.Generic;
using AlStudente.Models;

namespace AlStudente.Repositories
{
    public interface ILessonDayRepository
    {
        LessonDay GetById(int id);
        List<LessonDay> GetAll();
    }
}