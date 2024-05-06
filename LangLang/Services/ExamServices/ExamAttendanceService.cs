﻿using LangLang.DAO;
using LangLang.Model;
using LangLang.Services.UserServices;
using System.Collections.Generic;
using static LangLang.Model.Exam;


namespace LangLang.Services.ExamServices
{
    public class ExamAttendanceService : IExamAttendanceService
    {
        private readonly IExamService _examService;
        private readonly IStudentService _studentService;
        private readonly ITutorService _tutorService;
        private readonly IExamAttendanceDAO _examAttendanceDAO;

        public ExamAttendanceService(IExamService examService, IStudentService studentService, ITutorService tutorService, IExamAttendanceDAO examAttendanceDAO)
        {
            _examService = examService;
            _studentService = studentService;
            _tutorService = tutorService;
            _examAttendanceDAO = examAttendanceDAO;
        }

        public List<ExamAttendance> GetAttendancesForStudent(string studentId)
        {
            return _examAttendanceDAO.GetExamAttendancesForStudent(studentId);
        }

        public List<ExamAttendance> GetAttendancesForExam(string examId)
        {
            return _examAttendanceDAO.GetExamAttendancesForExam(examId);
        }

        public ExamAttendance? GetStudentAttendance(string studentId)
        {
            List<ExamAttendance> attendances = _examAttendanceDAO.GetExamAttendancesForStudent(studentId);
            foreach (ExamAttendance attendance in attendances)
            {
                Exam exam = _examService.GetExamById(attendance.ExamId)!;
                if (exam.ExamState != State.NotStarted && exam.ExamState != State.Graded && exam.ExamState != State.Reported)
                {
                    return attendance;
                }
            }
            return null;
        }
        public List<ExamAttendance> GetFinishedExamsStudent(string studentId)
        {
            List<ExamAttendance> allAttendances = _examAttendanceDAO.GetExamAttendancesForStudent(studentId);
            List<ExamAttendance> finishedAttendances = new();
            foreach (ExamAttendance attendance in allAttendances)
            {
                Exam exam = _examService.GetExamById(attendance.ExamId)!;
                if (exam.ExamState == State.Finished || exam.ExamState == State.Graded || exam.ExamState == State.Reported)
                    finishedAttendances.Add(attendance);
            }
            return finishedAttendances;
        }

        public ExamAttendance AddAttendance(string studentId, string examId)
        {
            ExamAttendance attendance = new ExamAttendance(examId, studentId, false, false,0, 0);
            _examAttendanceDAO.AddExamAttendance(attendance);
            return attendance;
        }

        public void RemoveAttendee(string studentId, string examId)
        {
            List<ExamAttendance> attendances = _examAttendanceDAO.GetExamAttendancesForStudent(studentId);
            foreach (ExamAttendance attendance in attendances)
            {
                if (attendance.ExamId == examId)
                {
                    if(_examService.GetExamById(examId)!.ExamState != State.NotStarted)
                    {
                        _examService.GetExamById(examId)!.CancelAttendance();
                    }
                    _examAttendanceDAO.DeleteExamAttendance(attendance.Id);
                }
            }
        }

        public void GradeStudent(string studentId, string ExamId, int knowledgeGrade, int activityGrade)
        {
            //predavac 6., i dont see this being used anywhere later on
        }

        public void RateTutor(ExamAttendance attendance, int rating)
        {
            if (!attendance.isRated)
            {
                attendance.AddRating();
                Exam exam = _examService.GetExamById(attendance.ExamId)!;
                //Tutor tutor = _tutorService.GetTutor(exam.TutorId);
                Tutor tutor = _tutorService.GetTutorForExam(exam.Id)!;
               _tutorService.AddRating(tutor, rating);  //after tutor id gets added to course/exam
                                                        //i will only pass tutor id and then the service will findById
            }
        }


    }
}
