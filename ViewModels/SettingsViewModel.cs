using AmnesiaManager.Models;
using AmnesiaManager.Repository;
using AmnesiaManager.Security.EncryptedValue;
using AmnesiaManager.Services;
using DevExpress.Mvvm;

namespace AmnesiaManager.ViewModels;

internal class SettingsViewModel : ViewModelBase
{
    #region Properties and fields

    public string Password { get; set; } = string.Empty;

    public string ConfirmPassword { get; set; } = string.Empty;

    public Error PasswordFormError { get; set; } = new();

    #endregion

    #region Commands

    public DelegateCommand SaveNewPasswordCommand { get; set; }

    #endregion

    #region Constructor

    public SettingsViewModel()
    {
        SaveNewPasswordCommand = new DelegateCommand(SaveNewPassword);
    }

    #endregion

    #region Private Methods

    private void SaveNewPassword()
    {
        if (string.IsNullOrWhiteSpace(Password))
        {
            PasswordFormError.Message = "You need to enter a password!";
            return;
        }

        if (Password != ConfirmPassword)
        {
            PasswordFormError.Message = "Passwords don't match!";
            return;
        }

        var encryptedPassword = new EncryptedString { Value = Password };

        if (!PasswordRepository.Instance.ChangeEncryptionKey(encryptedPassword)) return;
        UserService.Current.EncryptionKey = encryptedPassword;

        ConfirmPassword = Password = string.Empty;
        PasswordFormError.Message = string.Empty;
    }

    #endregion
}