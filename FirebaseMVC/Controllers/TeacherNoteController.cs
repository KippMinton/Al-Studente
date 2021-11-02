using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlStudente.Repositories;
using AlStudente.Models;

namespace AlStudente.Controllers
{
    public class TeacherNoteController : Controller
    {
        private readonly ITeacherNoteRepository _teacherNoteRepository;
        private readonly ITeacherRepository _teacherRepository;
        private readonly IStudentRepository _studentRepository;

        public TeacherNoteController(ITeacherNoteRepository teacherNoteRepository,
                                     ITeacherRepository teacherRepository,
                                     IStudentRepository studentRepository)
        {
            _teacherNoteRepository = teacherNoteRepository;
            _teacherRepository = teacherRepository;
            _studentRepository = studentRepository;
        }

        // GET: TeacherNoteController
        public ActionResult Index()
        {
            return View();
        }

        // GET: TeacherNoteController/Details/5
        public ActionResult Details(int id)
        {
            var note = _teacherNoteRepository.GetById(id);
            var student = _studentRepository.GetById(note.StudentId);

            var noteVM = new TeacherNoteViewModel
            {
                TeacherNote = note,
                Student = student
            };

            return View(noteVM);
        }

        // GET: TeacherNoteController/Create
        public ActionResult Create(int id)
        {
            var student = _studentRepository.GetByUserId(id);
            TeacherNoteViewModel newNoteVM = new TeacherNoteViewModel
            {
                Student = student,
                TeacherNote = new TeacherNote
                {
                    StudentId = student.Id,
                    TeacherId = student.TeacherId
                }

            };
           
            return View(newNoteVM);
        }

        // POST: TeacherNoteController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int id, TeacherNoteViewModel noteVM)
        {
            var student = _studentRepository.GetByUserId(id);
            
            
                var newNote = new TeacherNote
                {
                    StudentId = student.Id,
                    TeacherId = student.TeacherId,
                    Title = noteVM.TeacherNote.Title,
                    Content = noteVM.TeacherNote.Content
                };

                _teacherNoteRepository.Add(newNote);

                return RedirectToAction("StudentDetails", "Home", new { id = id });
            
            //catch
            //{
            //    return View(noteVM);
            //}
        }

        // GET: TeacherNoteController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TeacherNoteController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TeacherNoteController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TeacherNoteController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
