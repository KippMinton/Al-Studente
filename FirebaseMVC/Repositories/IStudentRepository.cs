using System;
using System.Collections.Generic;
using AlStudente.Models;

namespace AlStudente.Repositories
{
    public interface IStudentRepository
    {
        //void Add(Student student);
        Student GetByUserId(int id);
        List<Student> GetAllByTeacher(int teacherId);
    }
}