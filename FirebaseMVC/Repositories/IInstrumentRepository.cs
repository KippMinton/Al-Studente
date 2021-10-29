using System.Collections.Generic;
using AlStudente.Models;

namespace AlStudente.Repositories
{
    public interface IInstrumentRepository
    {
        Instrument GetById(int id);
        List<Instrument> GetAll();
    }
}