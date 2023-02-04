using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using AmnesiaManager.Models;
using AmnesiaManager.Repository;
using AmnesiaManager.Views;
using DevExpress.Mvvm;

namespace AmnesiaManager.ViewModels
{
    internal class PasswordListViewModel : ViewModelBase
    {
        #region Public Properties
        public ObservableCollection<PasswordModel> VisiblePasswords
        {
            get => GetProperty(() => VisiblePasswords);
            set => SetProperty(() => VisiblePasswords, value);
        }

        #region Commands
        public DelegateCommand<int> SwitchPageToCommand { get; }
        #endregion
        #endregion

        #region Backing Properties
        private string _searchText = string.Empty;
        public string SearchText
        {
            get => _searchText;
            set
            {
                DoSearch(value);
                _searchText = value;
            }
        }
        #endregion

        #region Private Fields
        private List<PasswordModel> AllPasswords { get; set; }
        #endregion

        #region Constructor
        public PasswordListViewModel()
        {
            var passwords = PasswordRepository.Instance.GetAll() ?? new List<PasswordModel>();
            var passwordModels = passwords as PasswordModel[] ?? passwords.ToArray();

            AllPasswords = new List<PasswordModel>(passwordModels.ToList());
            VisiblePasswords = new ObservableCollection<PasswordModel>(passwordModels.ToList());
            SwitchPageToCommand = new DelegateCommand<int>(SwitchPageTo);

            PasswordRepository.Instance.OnPasswordCreated += OnPasswordCreated;
            PasswordRepository.Instance.OnPasswordDeleted += OnPasswordDeleted;
            PasswordRepository.Instance.OnPasswordUpdated += OnPasswordUpdated;
        }
        #endregion

        #region Private Methods
        private void DoSearch(string value)
        {
            if (AllPasswords.Count == 0) return;

            if (string.IsNullOrWhiteSpace(value))
            {
                VisiblePasswords.Clear();

                AllPasswords.ForEach(
                    pwd => VisiblePasswords.Add(pwd)
                );

                return;
            }
            
            VisiblePasswords.Clear();

            var foundedPasswords = AllPasswords.Where(
                pwd => 
                    (pwd.Label ?? "").Contains(value) || 
                    (pwd.Login ?? "").Contains(value)
            ).ToList();

            foreach (var pwd in foundedPasswords) 
                VisiblePasswords.Add(pwd);
        }

        private void SwitchPageTo(int type)
        {
            if (Application.Current.MainWindow is not MainWindow
                {
                    DataContext: MainWindowViewModel mainViewModel
                }
               ) return;

            mainViewModel.ChangePage(type);
        }

        private void OnPasswordCreated(PasswordModel password)
        {
            AllPasswords.Add(password);
            VisiblePasswords.Add(password); 
        }

        private void OnPasswordDeleted(PasswordModel password)
        {
            AllPasswords.Remove(password);
            VisiblePasswords.Remove(password);
        }

        private void OnPasswordUpdated(PasswordModel password)
        {
            for (var i = 0; i < AllPasswords.Count; i++)
                if (AllPasswords[i].Guid == password.Guid)
                {
                    AllPasswords.RemoveAt(i);
                    AllPasswords.Insert(i, password);
                    break;
                }

            for (var i = 0; i < VisiblePasswords.Count; i++)
                if (VisiblePasswords[i].Guid == password.Guid)
                {
                    VisiblePasswords.RemoveAt(i);
                    VisiblePasswords.Insert(i, password);
                    break;
                }
        }
        #endregion
    }
}
