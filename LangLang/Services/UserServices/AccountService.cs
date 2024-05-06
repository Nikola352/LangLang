﻿using Consts;
using LangLang.Services.CourseServices;
using System;
using System.Collections.Generic;
using LangLang.DAO;
using LangLang.DTO;
using LangLang.Model;
using LangLang.Services.AuthenticationServices;
using LangLang.Services.ExamServices;


namespace LangLang.Services.UserServices
{
    public class AccountService : IAccountService
    {
        private readonly IProfileService _profileService;
        private readonly IStudentService _studentService;
        private readonly ITutorService _tutorService;
        private readonly IStudentCourseCoordinator _studentCourseCoordinator;
        private readonly IExamCoordinator _examCoordinator;
        private readonly IPersonProfileMappingDAO _personProfileMappingDao;
        private readonly IUserProfileMapper _userProfileMapper;

        public AccountService(IProfileService profileService, IStudentService studentService, ITutorService tutorService, IStudentCourseCoordinator studentCourseCoordinator, IPersonProfileMappingDAO personProfileMappingDao, IUserProfileMapper userProfileMapper, IExamCoordinator examCoordinator)
        {
            _profileService = profileService;
            _studentService = studentService;
            _tutorService = tutorService;
            _studentCourseCoordinator = studentCourseCoordinator;
            _personProfileMappingDao = personProfileMappingDao;
            _userProfileMapper = userProfileMapper;
            _examCoordinator = examCoordinator;
        }

        public void UpdateStudent(string studentId, string password, string name, string surname, DateTime birthDate, Gender gender, string phoneNumber)
        {
            if (_studentCourseCoordinator.GetStudentAttendingCourse(studentId) != null)
            {
                throw new ArgumentException("Student applied for courses, editing profile not allowed");
            }
            if (_examCoordinator.GetAttendingExam(studentId) != null)
            {
                throw new ArgumentException("Student applied for exam, editing profile not allowed");
            }

            Student student = _studentService.GetStudentById(studentId)!;
            _studentService.UpdateStudent(student, name, surname, birthDate, gender, phoneNumber);

            Profile profile = _userProfileMapper.GetProfile(new UserDto(student, UserType.Student))
                              ?? throw new InvalidOperationException("No profile associated with student.");
            _profileService.UpdatePassword(profile, password);
        }

        public void DeleteStudent(Student student)
        {
            _studentCourseCoordinator.RemoveAttendee(student.Id);
            _examCoordinator.RemoveAttendee(student.Id);
            _studentService.DeleteAccount(student);
            var profile = _userProfileMapper.GetProfile(new UserDto(student, UserType.Student));
            if(profile != null)
                _profileService.DeleteProfile(profile.Email);
        }

        public void DeactivateStudentAccount(Student student)
        {
            _studentCourseCoordinator.RemoveAttendee(student.Id);
            _examCoordinator.RemoveAttendee(student.Id);
            var profile = _userProfileMapper.GetProfile(new UserDto(student, UserType.Student));
            if(profile != null)
                _profileService.DeactivateProfile(profile);
        }

        public void RegisterStudent(RegisterStudentDto registerDto)
        {
            var profile = _profileService.AddProfile(new Profile(
                registerDto.Email,
                registerDto.Password
            ));
            var student = _studentService.AddStudent(new Student(
                registerDto.Name,
                registerDto.Surname,
                registerDto.BirthDay,
                registerDto.Gender,
                registerDto.PhoneNumber,
                registerDto.EducationLvl,
                0
            ));
            _personProfileMappingDao.AddMapping(new PersonProfileMapping(
                profile.Email,
                UserType.Student,
                student.Id
            ));
        }

        public void RegisterTutor(RegisterTutorDto registerDto)
        {
            var profile = _profileService.AddProfile(new Profile(
                registerDto.Email,
                registerDto.Password
            ));
            var tutor = _tutorService.AddTutor(new Tutor(
                registerDto.Name,
                registerDto.Surname,
                registerDto.BirthDay,
                registerDto.Gender,
                registerDto.PhoneNumber,
                registerDto.KnownLanguages,
                new List<string>(),
                new List<string>(),
                new int[5],
                registerDto.DateAdded
            ));
            _personProfileMappingDao.AddMapping(new PersonProfileMapping(
                profile.Email,
                UserType.Tutor,
                tutor.Id
            ));
        }
    }
}
