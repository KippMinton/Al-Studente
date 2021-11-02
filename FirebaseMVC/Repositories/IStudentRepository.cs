using System;
using System.Collections.Generic;
using AlStudente.Models;
using AlStudente.Models.ViewModels;

namespace AlStudente.Repositories
{
    public interface IStudentRepository
    {
        void Add(Student student);
        Student GetById(int id);
        Student GetByUserId(int id);
        List<StudentUserViewModel> GetAllByTeacher(int teacherId);
        void Update(Student student);
        void DeleteFromRoster(Student student);
    }
}