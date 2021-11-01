using System;
using System.Collections.Generic;
using AlStudente.Models;
using AlStudente.Models.ViewModels;

namespace AlStudente.Repositories
{
    public interface ITeacherNoteRepository
    {
        void Add(TeacherNote teacherNote);
        TeacherNote GetById(int id);
        List<TeacherNote> GetAllByStudentId(int id);
        void Update(TeacherNote teacherNote);
        void Delete(TeacherNote teacherNote);
    }
}