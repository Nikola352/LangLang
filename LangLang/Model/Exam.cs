﻿using System;
using Consts;

namespace LangLang.Model
{
    public class Exam
    {
        public Language Language { get; set; }
        public LanguageLvl LanguageLvl { get; set; }
        public DateTime Time { get; set; }
        public int ClassroomNumber { get; set; }
        public int MaxStudents { get; set; }
        public int NumStudents { get; set; }

        public Exam(Language language, LanguageLvl languageLvl, DateTime time, int classroomNumber, int maxStudents, int numStudents=0)
        {
            Language = language;
            LanguageLvl = languageLvl;
            Time = time;
            ClassroomNumber = classroomNumber;
            MaxStudents = maxStudents;
            NumStudents = numStudents;
        }
        
        public void AddAttendance()
        {
            NumStudents++;
        }

        public void CancelAttendance()
        {
            NumStudents--;
        }

        public bool IsFull()
        {
            return NumStudents == MaxStudents;
        }
    }
}