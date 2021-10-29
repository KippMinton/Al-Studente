using System.Collections.Generic;
using AlStudente.Models;

namespace AlStudente.Repositories
{
    public class IInstrumentRepository
    {
        Instrument GetById(int id);
        List<Instrument> GetAll();
    }
}