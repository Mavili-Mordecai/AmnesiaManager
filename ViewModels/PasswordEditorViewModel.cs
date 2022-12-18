using System.Diagnostics;
using System.Windows;
using AmnesiaManager.Models;
using AmnesiaManager.Repository;
using AmnesiaManager.Security;
using AmnesiaManager.Security.EncryptedValue;
using AmnesiaManager.Views;
using DevExpress.Mvvm;

namespace AmnesiaManager.ViewModels
{
    internal class PasswordEditorViewModel : ViewModelBase
    {
        #region Public Properties
        public string Label
        {
            get => GetProperty(() => Label);
            set => SetProperty(() => Label, value);
        }

        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public Error FormError
        {
            get => GetProperty(() => FormError);
            set => SetProperty(() => FormError, value);
        }

        #region Commands
        public DelegateCommand SaveCommand { get; }
        public DelegateCommand CancelCommand { get; }
        #endregion
        #endregion

        #region Private Fields
        private PasswordModel? _editablePassword;
        #endregion

        #region Constructor
        public PasswordEditorViewModel()
        {
            Label = string.Empty;
            Login = string.Empty;
            Password = string.Empty;

            FormError = new Error();
            CancelCommand = new DelegateCommand(ClearAndExit);
            SaveCommand = new DelegateCommand(Save);
        }
        #endregion

        #region Public Methods
        public void SetEditablePassword(PasswordModel? editablePassword)
        { 
            _editablePassword = editablePassword;
            Label = _editablePassword?.Label ?? string.Empty;
            Login = _editablePassword?.Login ?? string.Empty;
            Password = _editablePassword?.Password.Value ?? string.Empty;
        }
        #endregion

        #region Private Methods
        private void Save()
        {
            if (!ValidateForm()) return;

            if (_editablePassword != null)
            {
                // Editable mode
                _editablePassword.Label = Label;
                _editablePassword.Login = Login;
                _editablePassword.Password.Value = Password;

                if (!PasswordRepository.Instance.Update(_editablePassword)) FormError.Message = "Failed to save password!";
                else ClearAndExit();
                
                return;
            }

            // Creation mode
            var password = new PasswordModel(new EncryptedString(Entropy.Generate()) { Value = Password}) 
            { 
                Label = Label,
                Login = Login
            };

            if (!PasswordRepository.Instance.Create(password)) FormError.Message = "Failed to create password!";
            else ClearAndExit();
        }

        private void ClearAndExit()
        {
            ResetForm();
            SetEditablePassword(null);
            if (Application.Current.MainWindow is not MainWindow
                {
                    DataContext: MainWindowViewModel mainViewModel
                }) return;
            mainViewModel.ChangeViewModel(0);
        }

        private bool ValidateForm()
        {
            if (
                !string.IsNullOrWhiteSpace(Label) &&
                !string.IsNullOrWhiteSpace(Login) &&
                !string.IsNullOrWhiteSpace(Password)
            ) return true;

            FormError.Message = "It is necessary to fill in all the fields!";
            return false;

        }

        private void ResetForm() => Label = Login = Password = string.Empty;
        #endregion
    }
}
