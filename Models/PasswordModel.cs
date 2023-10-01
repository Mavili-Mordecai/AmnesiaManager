using System;
using System.Runtime.CompilerServices;
using System.Windows;
using AmnesiaManager.Repository;
using AmnesiaManager.Security.EncryptedValue;
using AmnesiaManager.ViewModels;
using AmnesiaManager.Views;
using DevExpress.Mvvm;
using Newtonsoft.Json;

namespace AmnesiaManager.Models
{
    public class PasswordModel : ViewModelBase, IDisposable
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
        [JsonIgnore] public DelegateCommand CopyLabelCommand { get; }
        [JsonIgnore] public DelegateCommand CopyLoginCommand { get; }
        [JsonIgnore] public DelegateCommand CopyPasswordCommand { get; }
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

            CopyLabelCommand = new DelegateCommand(() => { Copy(Label); });
            CopyLoginCommand = new DelegateCommand(() => { Copy(Login); });
            CopyPasswordCommand = new DelegateCommand(() => { Copy(Password.Value); });
        }
        #endregion

        #region Public Methods
        public void Dispose()
        {
            Password.Dispose();
        }
        #endregion

        #region Private Methods
        private void Copy(string? text)
        {
            if (string.IsNullOrWhiteSpace(text)) return;
            try
            {
                Clipboard.SetDataObject(text);
            }
            catch (Exception)
            {
                // TODO: Log this
            }
        }
        #endregion
    }
}