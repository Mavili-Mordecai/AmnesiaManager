using System;
using System.Windows;
using AmnesiaManager.Models;
using AmnesiaManager.Repository;
using AmnesiaManager.Services;
using AmnesiaManager.Views;
using DevExpress.Mvvm;
using static System.String;

namespace AmnesiaManager.ViewModels
{
    internal class AuthorizationViewModel : ViewModelBase
    {
        #region Public Properties
        public string Title { get; set; }
        public string Password { get; set; } = Empty;
        public string ConfirmPassword { get; set; } = Empty;
        public string ApplicationName => Product.Name;

        public Visibility ConfirmPasswordVisibility { get; set; }

        public Error FormError { get; set; } = new();

        #region Commands
        public DelegateCommand LoginCommand { get; set; }
        #endregion
        #endregion

        #region Events
        public event EventHandler? OnRequestClose;
        #endregion

        #region Private Fields
        private readonly bool _isRegistration;
        #endregion

        #region Constructor
        public AuthorizationViewModel()
        {
            LoginCommand = new DelegateCommand(Login);

            _isRegistration = !PasswordRepository.Instance.IsExists();

            ConfirmPasswordVisibility = _isRegistration
                ? Visibility.Visible
                : Visibility.Collapsed;

            Title = _isRegistration 
                ? "Registration"
                : "Authorization";
        }
        #endregion

        #region Private Methods
        private void ResetForm()
        {
            Password = ConfirmPassword = Empty;
            FormError.Clear();
        } 

        private bool ValidateForm()
        {
            if (IsNullOrWhiteSpace(Password))
            {
                FormError.Message = "You need to enter a password!";
                return false;
            }

            if (_isRegistration && Password != ConfirmPassword)
            {
                FormError.Message = "Passwords don't match!";
                return false;
            }

            return true;
        }

        private void Login()
        {
            FormError.Message = "test";
            FormError.Visibility = Visibility.Visible;

            if (!ValidateForm()) return;

            UserService.Current.EncryptionKey.Value = Password.Trim();

            if (_isRegistration)
            {
                GoToMainWindow();
                return;
            }

            if (PasswordRepository.Instance.GetAll() == null)
            {
                FormError.Message = "Invalid password!";
                return;
            }

            GoToMainWindow();
        }

        private void GoToMainWindow()
        {
            ResetForm();

            var window = new MainWindow();
            Application.Current.MainWindow = window;
            window.Show();
            OnRequestClose?.Invoke(this, EventArgs.Empty);
        }
        #endregion
    }
}
