﻿using System;
using System.ComponentModel;
using System.Security;
using System.Windows;
using System.Windows.Input;
using LangLang.MVVM;
using LangLang.Services;
using LangLang.View;

namespace LangLang.ViewModel
{
    public class RegisterViewModel : INotifyPropertyChanged
    {
        private string _email;
        private string _password;
        private string _name;
        private string _surname;
        private string _phoneNumber;
        private string _gender;
        private string _errorMessage;


        private readonly Window _window;

        public RegisterViewModel()
        {
            SignUpCommand = new RelayCommand(SignUp);
        }


        public RegisterViewModel(Window window)
        {
            _window = window;
            SignUpCommand = new RelayCommand(SignUp);
        }

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public string Surname
        {
            get => _surname;
            set
            {
                _surname = value;
                OnPropertyChanged(nameof(Surname));
            }
        }

        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                _phoneNumber = value;
                OnPropertyChanged(nameof(PhoneNumber));
            }
        }

        public string Gender
        {
            get => _gender;
            set
            {
                _gender = value;
                OnPropertyChanged(nameof(Gender));
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        public ICommand SignUpCommand { get; }

        private void SignUp(object parameter)
        {
            ErrorMessage = "";

            // Directly access the properties
            string email = Email;
            string password = Password;
            string name = Name;
            string surname = Surname;
            string phoneNumber = PhoneNumber;
            string gender = Gender;

            bool successful = RegisterService.RegisterStudent(email, password, name, surname, DateTime.Now, Consts.Gender.Other, phoneNumber, "");

            if (successful)
            {
                _window.Close();
                MessageBox.Show($"Succesfull registration");
            }
            else
            {
                MessageBox.Show($"Fail");
            }
        }






        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }











    }
}
