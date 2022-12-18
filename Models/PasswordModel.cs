using System;
using System.Windows;
using AmnesiaManager.Repository;
using AmnesiaManager.Security.EncryptedValue;
using AmnesiaManager.ViewModels;
using AmnesiaManager.Views;
using DevExpress.Mvvm;
using Newtonsoft.Json;

namespace AmnesiaManager.Models
{
    public class PasswordModel : ViewModelBase
    {
        #region Public Properties
        [JsonProperty("guid")] public Guid Guid { get; set; }
        [JsonProperty("label")] public string? Label { get; set; }
        [JsonProperty("login")] public string? Login { get; set; }
        [JsonProperty("password")] public EncryptedString Password { get; set; }

        #region Commands
        // TODO: Need to put these commands in the ViewModel
        [JsonIgnore] public DelegateCommand RemoveCommand { get; }
        [JsonIgnore] public DelegateCommand EditCommand { get; }
        #endregion
        #endregion

        #region Constructor
        public PasswordModel(EncryptedString password)
        {
            Password = password;
            Guid = Guid.NewGuid();

            RemoveCommand = new DelegateCommand(() =>
            {
                if (MessageBox.Show(
                        "Are you sure you want to delete this password?", 
                        "Confirmation of deletion", 
                        MessageBoxButton.YesNo, 
                        MessageBoxImage.Question
                    ) == MessageBoxResult.Yes) PasswordRepository.Instance.Delete(this);
            });

            EditCommand = new DelegateCommand(() =>
            {
                if (Application.Current.MainWindow is not MainWindow
                    {
                        DataContext: MainWindowViewModel viewModel
                    }) return;
                viewModel.EditPassword(this);
            });
        }
        #endregion
    }
}