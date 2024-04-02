﻿using System;

namespace Consts
{
    public static class Constants
    {
        public const int ClassroomsNumber = 2;
        
        public static readonly TimeSpan ExamDuration = TimeSpan.FromHours(4);
        public static readonly TimeSpan LessonDuration = TimeSpan.FromMinutes(90);
        
        public static readonly TimeSpan ConfirmableExamTime = new(7, 0, 0, 0);

        public const string StudentFilePath = "../../../Data/Students.json";
        public const string CourseFilePath = "../../../Data/Courses.json";
        public const string ExamFilePath = "../../../Data/Exams.json";
        public const string LanguageFilePath = "../../../Data/Languages.json";
        public const string TutorFilePath = "../../../Data/Tutors.json";
        public const string DirectorFilePath = "../../../Data/Directors.json";
        public const string LastIdFilePath = "../../../Data/LastId.json";
    }


    public enum Gender
    {
        Female, Male, Other
    }

    public enum Role
    {
        Student, Tutor, Director
    }

    public enum LanguageLvl
    {
        A1, A2, B1, B2, C1, C2
    }

    public enum CourseState
    {
        Active, Canceled, Finished
    }
    public enum WorkDay
    {
        Monday,Tuesday,Wednesday,Thursday,Friday
    }


    
}

