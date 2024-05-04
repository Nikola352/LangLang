﻿using Consts;
using LangLang.Model;
using LangLang.MVVM;
using LangLang.Services.AuthenticationServices;
using LangLang.Services.CourseServices;
using LangLang.Services.ExamServices;
using LangLang.Services.NavigationServices;
using LangLang.Services.UserServices;
using LangLang.Services.UtilityServices;
using LangLang.Stores;
using LangLang.ViewModel.Factories;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;


namespace LangLang.ViewModel
{
    public class StudentViewModel : ViewModelBase, INavigableDataContext
    {
        private readonly Student _loggedInUser;
        private readonly ICourseService _courseService;
        private readonly ILanguageService _languageService;
        private readonly IStudentService _studentService;
        private readonly IExamService _examService;
        private readonly IStudentCourseCoordinator _courseCoordinator;
        private readonly IAccountService _accountService;
        public ICommand ClearExamFiltersCommand { get; }
        public ICommand ClearCourseFiltersCommand { get; }
        public ICommand LogOutCommand { get; }
        public ICommand DeleteProfileCommand { get; }
        public ICommand OpenStudentProfileCommand { get; }
        public ICommand ApplyCourseCommand { get; }
        public ICommand CancelCourseCommand{ get; }
        public ICommand ApplyExamCommand { get; }
        public ICommand CancelExamCommand { get; }
        public ICommand RateTutorCommand { get; }
        public ICommand CancelAttendingCourseCommand { get; }
        public ICommand CancelAttendingExamCommand { get; }
        public ICommand OpenNotificationWindowCommand { get; }
        public ObservableCollection<Course> Courses { get; set; }
        public ObservableCollection<Course> FinishedCourses { get; set; }
        public ObservableCollection<Course> AppliedCourses { get; set; }
        public ObservableCollection<Course> AttendingCourse { get; set; }
        public ObservableCollection<Exam> Exams { get; set; }
        public ObservableCollection<Exam> FinishedExams { get; set; }
        public ObservableCollection<Exam> AttendingExam { get; set; }
        public ObservableCollection<string?> Languages { get; set; }
        public ObservableCollection<LanguageLvl> Levels { get; set; }
        public ObservableCollection<int?> Durations { get; set; }

        private string name = "";
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged();
            }
        }

        private string languageName = "";
        public string LanguageName
        {
            get { return languageName; }
            set
            {
                languageName = value;
                OnPropertyChanged();
            }
        }

        private LanguageLvl level;
        public LanguageLvl Level
        {
            get { return level; }
            set
            {
                level = value;
                OnPropertyChanged();
            }
        }

        private int? duration;
        public int? Duration
        {
            get { return duration; }
            set
            {
                duration = value;
                OnPropertyChanged();
            }
        }

        private DateTime? start;
        public DateTime? Start
        {
            get { return start; }
            set
            {
                start = value;
                OnPropertyChanged();
            }
        }

        private bool online;
        public bool Online
        {
            get { return online; }
            set
            {
                online = value;
                OnPropertyChanged();
            }
        }

        // FILTER VALUES
        private string courseLanguageFilter = "";
        public string CourseLanguageFilter
        {
            get { return courseLanguageFilter; }
            set
            {
                courseLanguageFilter = value;
                FilterCourses();
                OnPropertyChanged();
            }
        }

        private LanguageLvl? courseLevelFilter;
        public LanguageLvl? CourseLevelFilter
        {
            get { return courseLevelFilter; }
            set
            {
                courseLevelFilter = value;
                FilterCourses();
                OnPropertyChanged();
            }
        }

        private DateTime? courseStartFilter;
        public DateTime? CourseStartFilter
        {
            get { return courseStartFilter; }
            set
            {
                courseStartFilter = value;
                FilterCourses();
                OnPropertyChanged();
            }
        }

        private bool? courseOnlineFilter;
        public bool? CourseOnlineFilter
        {
            get { return courseOnlineFilter; }
            set
            {
                courseOnlineFilter = value;
                FilterCourses();
                OnPropertyChanged();
            }
        }

        private int courseDurationFilter;
        public int CourseDurationFilter
        {
            get { return courseDurationFilter; }
            set
            {
                courseDurationFilter = value;
                FilterCourses();
                OnPropertyChanged();
            }
        }

        private Course? selectedItem;
        public Course? SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
                if (selectedItem != null)
                {
                    Name = selectedItem.Name;
                    LanguageName = selectedItem.Language.Name;
                    Level = selectedItem.Level;
                    Duration = selectedItem.Duration;
                    Start = selectedItem.Start;
                    Online = selectedItem.Online;
                }
                OnPropertyChanged();
            }
        }

        private readonly ILoginService _loginService;
        private readonly INavigationService _navigationService;
        private readonly IPopupNavigationService _popupNavigationService;
        public NavigationStore NavigationStore { get; }
        
        public StudentViewModel(IStudentService studentService,IAccountService accountService, ILoginService loginService, IStudentCourseCoordinator courseCoordinator, INavigationService navigationService, IPopupNavigationService popupNavigationService, NavigationStore navigationStore, ICourseService courseService, ILanguageService languageService, IExamService examService, IAuthenticationStore authenticationStore)
        {
            _loggedInUser = (Student?)authenticationStore.CurrentUser.Person ??
                                throw new InvalidOperationException(
                                    "Cannot create StudentViewModel without currently logged in student");
            NavigationStore = navigationStore;
            _courseService = courseService;
            _accountService = accountService;
            _languageService = languageService;
            _examService = examService;
            _studentService = studentService;
            _loginService = loginService;
            _courseCoordinator = courseCoordinator;
            _navigationService = navigationService;
            _popupNavigationService = popupNavigationService;

            Courses = new ObservableCollection<Course>();
            FinishedCourses = new ObservableCollection<Course>();
            AttendingCourse = new ObservableCollection<Course>(); 
            AppliedCourses = new ObservableCollection<Course>();
            Exams = new ObservableCollection<Exam>();
            AttendingExam = new ObservableCollection<Exam>();
            FinishedExams = new ObservableCollection<Exam>();
            Languages = new ObservableCollection<string?>();
            Levels = new ObservableCollection<LanguageLvl>();
            Durations = new ObservableCollection<int?> { null, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            Start = DateTime.Now;

            LoadExams();
            //LoadAttendingExam();
            //LoadFinishedExams();
            LoadLanguages();
            LoadCourses();
            LoadAttendingCourse();
            LoadAppliedCourses();
            LoadFinishedCourses();
            LoadLanguageLevels();

            //initialize commands
            ClearCourseFiltersCommand = new RelayCommand(ClearCourseFilters);
            ClearExamFiltersCommand = new RelayCommand(ClearExamFilters);
            LogOutCommand = new RelayCommand(_ => LogOut());
            DeleteProfileCommand = new RelayCommand(_ => DeleteProfile());
            OpenStudentProfileCommand = new RelayCommand(_ => OpenStudentProfile());
            ApplyCourseCommand = new RelayCommand<string>(ApplyCourse);
            CancelCourseCommand = new RelayCommand<string>(CancelCourse);
            ApplyExamCommand = new RelayCommand<string>(ApplyExam);
            CancelExamCommand = new RelayCommand<string>(CancelExam);
            RateTutorCommand = new RelayCommand<string>(RateTutor);
            CancelAttendingCourseCommand = new RelayCommand<string>(CancelAttendingCourse!);
            CancelAttendingExamCommand = new RelayCommand(CancelAttendingExam!);
            OpenNotificationWindowCommand = new RelayCommand(_ => OpenNotificationWindow());
        }


        private void CancelAttendingCourse(string courseId)
        {
            Course course = _courseService.GetCourseById(courseId)!;
            try
            {
                _courseCoordinator.DropCourse(_loggedInUser.Id);
                MessageBox.Show($"You've successfully dropped {course.Name} course.", "Success");
                AttendingCourse.Remove(course);
            }
            catch
            {
                MessageBox.Show($"It's too early to drop {course.Name}. Wait before trying again.", "Success");

            }
        }

        private void CancelAttendingExam(object parameter)
        {
            MessageBox.Show($"cancelled exam sent!", "Success");

        }



        private void ApplyCourse(string courseId)
        {
            try
            {
                _courseCoordinator.ApplyForCourse(courseId, _loggedInUser.Id);
                Course course = _courseService.GetCourseById(courseId)!;
                MessageBox.Show($"Application sent! You've successfully applied for {course.Name}!", "Success");

                Course appliedCourse = _courseService.GetCourseById(courseId)!;
                Courses.Remove(appliedCourse);
                AppliedCourses.Add(appliedCourse);
            }
            catch
            {
                MessageBox.Show($"Can't apply to other courses when already attending {AttendingCourse[0].Name} course!", "Fail");
            }
        }


        private void CancelCourse(string courseId)
        {
            Course course = _courseService.GetCourseById(courseId)!;
            MessageBox.Show($"Your application for {course.Name} has been cancelled.", "Success");
            _courseCoordinator.CancelApplication(_loggedInUser.Id, courseId);

            Course cancelledCourse = _courseService.GetCourseById(courseId)!;
            Courses.Add(cancelledCourse);
            AppliedCourses.Remove(cancelledCourse);
        }


        private void ApplyExam(string examId)
        {
            MessageBox.Show($"Successful apply for exam {examId}", "Success");
        }


        private void CancelExam(string examId)
        {
            MessageBox.Show($"Successful cancel for exam {examId}", "Success");
        }

        private void RateTutor(string courseId)
        {
            MessageBox.Show($"Successful rating for course {courseId}", "Success");
        }

        private void ClearCourseFilters(object? obj)
        {
            CourseLanguageFilter = "";
            CourseLevelFilter = null;
            CourseStartFilter = null;
            CourseOnlineFilter = null;
            CourseDurationFilter = 0;
            Courses.Clear();
            LoadCourses();
            OnPropertyChanged();
        }

        private void ClearExamFilters(object? obj)
        {
            ExamLanguageFilter = "";
            ExamLevelFilter = null;
            ExamStartFilter = null;
            Exams.Clear();
            LoadExams();
            OnPropertyChanged();
        }

        public void LoadCourses()
        {
            var availableCourses = _courseCoordinator.GetAvailableCourses(_loggedInUser.Id);
            foreach (Course course in availableCourses)
            {
                Courses.Add(course);
            }
        }

        private void LoadAttendingCourse()
        {
            //when trying to test attendance, apply for course
            //in files change application state to 2 (accepted), then change course state to 4 (In progress so i can drop it)
            //_courseCoordinator.GenerateAttendance("30");
            Course attendingCourse = _courseCoordinator.GetStudentAttendingCourse(_loggedInUser.Id)!;
            if(attendingCourse != null)
            {
                AttendingCourse.Add(attendingCourse);
            }
        }

        private void LoadAppliedCourses()
        {
            foreach (Course course in _courseCoordinator.GetAppliedCoursesStudent(_loggedInUser.Id))
            {
                AppliedCourses.Add(course);
            }
        }

        private void LoadFinishedCourses()
        {
            var finishedCourses = _courseCoordinator.GetFinishedCoursesStudent(_loggedInUser.Id);
            foreach (Course course in finishedCourses)
            {
                FinishedCourses.Add(course);
            }
        }

        public void LoadExams()
        {
            var examsDictionary = _examService.GetAvailableExamsForStudent(_loggedInUser);
            foreach (Exam exam in examsDictionary)
            {
                Exams.Add(exam);
            }
        }

        public void LoadLanguages()
        {
            var languages = _languageService.GetAll();
            foreach (Language language in languages)
            {
                Languages.Add(language.Name);
            }
            Languages.Add("");
        }

        public void LoadLanguageLevels()
        {
            foreach (LanguageLvl lvl in Enum.GetValues(typeof(LanguageLvl)))
            {
                Levels.Add(lvl);
            }
        }

        public void RemoveInputs()
        {
            Name = "";
            LanguageName = "";
            Level = LanguageLvl.A1;
            Duration = null;
            Online = false;
            selectedItem = null;
            Start = DateTime.Now;
        }

        public void FilterCourses()
        {
            Courses.Clear();
            var courses = _courseService.GetAll();
            foreach (Course course in courses.Values)
            {
                if ((course.Language.Name == CourseLanguageFilter || CourseLanguageFilter == "") && (course.Level == CourseLevelFilter || CourseLevelFilter == null))
                {
                    if (CourseStartFilter == null || (CourseStartFilter != null && course.Start == ((DateTime)CourseStartFilter)))
                    {
                        if (course.Online == CourseOnlineFilter || CourseOnlineFilter == null)
                        {
                            if (course.Duration == CourseDurationFilter || CourseDurationFilter == 0)
                            {
                                Courses.Add(course);
                            }
                        }
                    }
                }
            }
        }


        //FILTERING EXAM
        // Add properties for exam filtering criteria
        // Define new filter properties for exams
        private string examLanguageFilter = "";
        public string ExamLanguageFilter
        {
            get { return examLanguageFilter; }
            set
            {
                examLanguageFilter = value;
                FilterExams();
                OnPropertyChanged();
            }
        }

        private LanguageLvl? examLevelFilter;
        public LanguageLvl? ExamLevelFilter
        {
            get { return examLevelFilter; }
            set
            {
                examLevelFilter = value;
                FilterExams();
                OnPropertyChanged();
            }
        }

        private DateTime? examStartFilter;
        public DateTime? ExamStartFilter
        {
            get { return examStartFilter; }
            set
            {
                examStartFilter = value;
                FilterExams();
                OnPropertyChanged();
            }
        }

        // Modify FilterExams method to filter exams based on new criteria
        public void FilterExams()
        {
            // Clear existing exams from the list
            Exams.Clear();

            // Get all exams
            var exams = _examService.GetAllExams();

            // Filter exams based on criteria
            foreach (Exam exam in exams)
            {
                if ((exam.Language.Name == ExamLanguageFilter || ExamLanguageFilter == "") &&
                    (exam.LanguageLvl == ExamLevelFilter || ExamLevelFilter == null))
                {
                    if (ExamStartFilter == null || (ExamStartFilter != null && exam.Time.Date == ExamStartFilter.Value.Date))
                    {
                        Exams.Add(exam);
                    }
                }
            }
        }

        private void LogOut()
        {
            _loginService.LogOut();
            _navigationService.Navigate(ViewType.Login);
        }

        private void DeleteProfile()
        {
            _accountService.DeleteStudent(_loggedInUser);
            MessageBox.Show("Your profile has been successfully deleted");
            _navigationService.Navigate(ViewType.Login);
        }

        private void OpenStudentProfile()
        {
            _popupNavigationService.Navigate(ViewType.StudentAccount);
        }

        private void OpenNotificationWindow()
        {
            _popupNavigationService.Navigate(ViewType.Notifications);
        }
    }
}