﻿using System;
using Consts;
using System.Collections.Generic;
using LangLang.Model;
using LangLang.Util;

namespace LangLang.DAO
{
    internal class CourseDAO
    {
        private static CourseDAO? instance;
        private Dictionary<string, Course>? courses;
        private Dictionary<string, LastId> lastId;
        private static LastIdDAO lastIdDAO = LastIdDAO.GetInstance();

        private CourseDAO()
        {
            lastId = new Dictionary<string, LastId>();
        }
        public static CourseDAO getInstance()
        {
            if(instance == null)
            {
                instance = new CourseDAO();
            }
            return instance;
        }
        public Dictionary<string, Course> getAllCourses()
        {
            if(courses == null)
            {
                courses = JsonUtil.ReadFromFile<Course>(Constants.CourseFilePath);
            }
            return courses;
        }
        public void AddCourse(Course course)
        {
            if(courses != null)
            {
                string id = lastIdDAO.GetCourseId();
                lastIdDAO.IncrementCourseId();
                course.Id = id;
                courses[id] = course;
                JsonUtil.WriteToFile<Course>(courses, Constants.CourseFilePath);
        }
        }
        public Course GetCourseById(string id)
        {
            if(courses == null)
            {
                courses = new Dictionary<string, Course>();
            }
            return courses[id];
        }
        public void DeleteCourse(string id)
        {
            if(courses != null)
            {
                courses.Remove(id);
                JsonUtil.WriteToFile<Course>(courses, Constants.CourseFilePath);
            }
        }
        public void UpdateCourse(Course course)
        {
            if(courses != null)
            {
                courses[course.Id] = course;
                JsonUtil.WriteToFile<Course>(courses, Constants.CourseFilePath);
            }
        }


    }
}
