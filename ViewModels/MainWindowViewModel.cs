using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using AmnesiaManager.Views;
using AmnesiaManager.Views.Pages;
using DevExpress.Mvvm;

namespace AmnesiaManager.ViewModels
{
    internal enum PageType
    {
        Passwords = 0,
        Cards = 1,
        Settings = 2,
        NewPassword = 3
    }

    internal class MainWindowViewModel
    {
        #region Public Fields
        public string ApplicationName => AppDomain.CurrentDomain.FriendlyName;
        public DelegateCommand ExitCommand { get; }
        public Page CurrentPage { get; set; }
        #endregion

        #region Private Fields
        private Dictionary<PageType, Page> _pages;
        #endregion

        #region Constructor
        public MainWindowViewModel()
        {
            _pages = new Dictionary<PageType, Page>
            {
                { PageType.Passwords, new PasswordPage() }
            };

            CurrentPage = _pages[PageType.Passwords];

            ExitCommand = new DelegateCommand(Exit);
        }
        #endregion

        #region Private Methods
        private void Exit()
        {
            if (Application.Current.MainWindow is MainWindow window) 
                window.TaskbarIcon.Dispose();
            Environment.Exit(0);
        }
        #endregion
    }
}
