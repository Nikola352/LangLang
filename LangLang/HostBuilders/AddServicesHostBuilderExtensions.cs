﻿using LangLang.Services.AuthenticationServices;
using LangLang.Services.NavigationServices;
using LangLang.Services.UserServices;
using LangLang.Services.UtilityServices;
using LangLang.Services.CourseServices;
using LangLang.Services.ExamServices;
using LangLang.Services.NotificationServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using LangLang.Services.DropRequestServices;

namespace LangLang.HostBuilders;

public static class AddServicesHostBuilderExtensions
{
    public static IHostBuilder AddServices(this IHostBuilder host)
    {
        host.ConfigureServices(services =>
        {
            services.AddSingleton<ILoginService, LoginService>();
            services.AddSingleton<IRegisterService, RegisterService>();
            services.AddSingleton<IStudentService, StudentService>();
            services.AddSingleton<ICourseService, CourseService>();
            services.AddSingleton<IExamService, ExamService>();
            services.AddSingleton<ITutorService, TutorService>();
            services.AddSingleton<IDirectorService, DirectorService>();
            services.AddSingleton<ILanguageService, LanguageService>();
            services.AddSingleton<ITimetableService, TimetableService>();
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<IPopupNavigationService, PopupNavigationService>();
            services.AddSingleton<IClosePopupNavigationService, ClosePopupNavigationService>();
            services.AddSingleton<ICourseAttendanceService, CourseAttendanceService>();
            services.AddSingleton<ICourseApplicationService, CourseApplicationService>();
            services.AddSingleton<IStudentCourseCoordinator, StudentCourseCoordinator>();
            services.AddSingleton<IAccountService, AccountService>();
            services.AddSingleton<IProfileService, ProfileService>();
            services.AddSingleton<IUserProfileMapper, UserProfileMapper>();
            services.AddSingleton<IPenaltyService, PenaltyService>();
            services.AddSingleton<IUserValidator, UserValidator>();
            services.AddSingleton<INotificationService, NotificationService>();
            services.AddSingleton<INotificationInfoService, NotificationInfoService>();
            services.AddSingleton<IExamApplicationService, ExamApplicationService>();
            services.AddSingleton<IExamAttendanceService, ExamAttendanceService>();
            services.AddSingleton<IExamCoordinator, ExamCoordinator>();
            services.AddSingleton<IDropRequestService, DropRequestService>();
            services.AddSingleton<IDropRequestInfoService, DropRequestInfoService>();
        });
        
        return host;
    }
}