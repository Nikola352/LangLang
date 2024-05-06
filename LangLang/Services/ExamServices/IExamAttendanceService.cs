﻿using LangLang.Model;
using System.Collections.Generic;
using LangLang.DTO;


namespace LangLang.Services.ExamServices;
public interface IExamAttendanceService
{
    public List<ExamAttendance> GetAttendancesForStudent(string studentId);
    public List<ExamAttendance> GetAttendancesForExam(string examId);
    public ExamAttendance? GetStudentAttendance(string studentId);
    public List<ExamAttendance> GetFinishedExamsStudent(string studentId);
    public ExamAttendance AddAttendance(string studentId, string examId);
    public void RemoveAttendee(string studentId, string examId);
    public void GradeStudent(string studentId, string examId, ExamGradeDto examGradeDto);
    public void RateTutor(ExamAttendance attendance, int rating);

}

