using System.Collections.Generic;
using AlStudente.Models;

namespace AlStudente.Repositories
{
    public interface ILevelRepository
    {
        Level GetById(int id);
        List<Level> GetAll();
    }
}