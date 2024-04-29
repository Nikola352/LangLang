﻿namespace LangLang.Services.CourseServices;

public interface IStudentCourseCoordinator
{
    public void Accept(string applicationId);
    public void CancelAplication(string applicationId);
    public void RemoveAttendee(string courseId, string studentId);
    public void ApplyForCourse(string courseId, string studentId);
    public void FinishCourse(string courseId, string studentId);
    public void GenerateAttendance();
    public void DropCourse(string courseId);

}
