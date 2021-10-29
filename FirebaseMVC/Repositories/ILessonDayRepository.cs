using System.Collections.Generic;
using AlStudente.Models;

namespace AlStudente.Repositories
{
    public class ILessonDayRepository
    {
        LessonDay GetById(int id);
        List<LessonDay> GetAll();
    }
}