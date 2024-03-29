﻿using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using AlStudente.Models;

namespace AlStudente.Auth.Models
{
    public class StudentRegistration
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [Compare(nameof(Password))]
        [DisplayName("Confirm Password")]
        public string ConfirmPassword { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string DisplayName { get; set; }
        [Required]
        public DateTime CreateDateTime { get; set; }
        public string ImageLocation { get; set; }
        public int UserTypeId { get; set; }
        public int InstrumentId { get; set; }
        public List<Instrument> Instruments { get; set; }
        public string Bio { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime DOB { get; set; }
        public int PlayingSince { get; set; }
        public int LevelId { get; set; }
        public List<Level> Levels { get; set; }
        public int LessonDayId { get; set; }
        public List<LessonDay> Days { get; set; }
        public int LessonTimeId { get; set; }
        public List<LessonTime> Times { get; set; }
    }
}
