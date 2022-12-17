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
        public ObservableCollection<PasswordModel> VisiblePasswords { get; set; }

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
        private readonly IRepository<PasswordModel> _repository;
        #endregion

        #region Constructor
        public PasswordListViewModel()
        {
            _repository = new LocalPasswordRepository();

            var passwords = _repository.GetAll() ?? new List<PasswordModel>();
            var passwordModels = passwords as PasswordModel[] ?? passwords.ToArray();

            AllPasswords = new List<PasswordModel>(passwordModels.ToList());
            VisiblePasswords = new ObservableCollection<PasswordModel>(passwordModels.ToList());
            SwitchPageToCommand = new DelegateCommand<int>(SwitchPageTo);
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

                //CollectionViewSource.GetDefaultView(VisiblePasswords).Refresh();
                return;
            }
            
            VisiblePasswords.Clear();

            /*foreach (var pwd in _allPasswords.Where(
                         pwd => 
                             (pwd.Label?.Trim().ToLower() ?? "").Contains(value) ||
                             (pwd.Login?.Trim().ToLower() ?? "").Contains(value))
                    ) VisiblePasswords.Add(pwd);*/

            //CollectionViewSource.GetDefaultView(VisiblePasswords).Refresh();
            
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
        #endregion
    }
}
