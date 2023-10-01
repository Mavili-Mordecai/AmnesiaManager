using System;
using System.Security;
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
        #region Properties and fields

        public string Title { get; set; }
        public string Password { get; set; } = Empty;
        public string ConfirmPassword { get; set; } = Empty;
        public string ApplicationName => Product.Name;
        public string Version => Product.GetVersion() ?? "N/A";

        public Visibility ConfirmPasswordVisibility { get; set; }

        public Error FormError { get; set; } = new();

        public event EventHandler? OnRequestClose;

        private readonly bool _isRegistration;

        #endregion

        #region Commands

        public DelegateCommand LoginCommand { get; set; }
        public DelegateCommand ExitCommand { get; }

        #endregion

        #region Constructor

        public AuthorizationViewModel()
        {
            LoginCommand = new DelegateCommand(Login);
            ExitCommand = new DelegateCommand(Exit);

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
            if (!ValidateForm()) return;

            UserService.Current.EncryptionKey.Value = Password.Trim();

            if (_isRegistration)
            {
                if (PasswordRepository.Instance.MarkAsRegistered())
                {
                    SwitchToMainWindow();
                    return;
                }

                FormError.Message = "Failed to create file!";
                return;
            }

            if (PasswordRepository.Instance.GetAll() == null)
            {
                FormError.Message = "Invalid password!";
                return;
            }

            SwitchToMainWindow();
        }

        private void SwitchToMainWindow()
        {
            ResetForm();

            var window = new MainWindow();
            Application.Current.MainWindow = window;
            window.Show();
            OnRequestClose?.Invoke(this, EventArgs.Empty);
        }

        private void Exit()
        {
            if (Application.Current.MainWindow is AuthorizationWindow window) window.TaskbarIcon.Dispose();
            Environment.Exit(0);
        }

        #endregion
    }
}