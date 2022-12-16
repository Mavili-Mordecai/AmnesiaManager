using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using AmnesiaManager.Models;
using DevExpress.Mvvm;
using DevExpress.Mvvm.Native;

namespace AmnesiaManager.ViewModels
{
    internal class PasswordPageViewModel : ViewModelBase
    {
        #region Public Properties
        public ObservableCollection<PasswordModel> VisiblePasswords { get; set; }
        public ICollectionView? CollectionViewPasswords { get; set; } = null;

        #region Commands
        public DelegateCommand<string> CopyTextCommand { get; set; }
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
        private List<PasswordModel> _allPasswords;
        #endregion

        #region Constructor
        public PasswordPageViewModel()
        {
            _allPasswords = new List<PasswordModel>
            {
                new()
                {
                    Label = "google.com",
                    Login = "Mavili",
                    Password = "pa$$word"
                },
                new()
                {
                    Label = "yandex.ru",
                    Login = "Mavili",
                    Password = "pa$$word"
                }
            };

            VisiblePasswords = new ObservableCollection<PasswordModel>(_allPasswords);

            CopyTextCommand = new DelegateCommand<string>((s =>
            {
                MessageBox.Show(s);
                Clipboard.SetDataObject(s);
            }));
        }
        #endregion

        #region Private Methods
        private void DoSearch(string value)
        {
            if (_allPasswords.Count == 0) return;

            if (string.IsNullOrWhiteSpace(value))
            {
                VisiblePasswords.Clear();

                _allPasswords.ForEach(
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
            
            var foundedPasswords = _allPasswords.Where(
                pwd => 
                    (pwd.Label ?? "").Contains(value) || 
                    (pwd.Login ?? "").Contains(value)
            ).ToList();

            foreach (var pwd in foundedPasswords) 
                VisiblePasswords.Add(pwd);
        }
        #endregion
    }
}
