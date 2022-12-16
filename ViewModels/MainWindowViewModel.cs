using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using AmnesiaManager.Models;
using AmnesiaManager.Models.KeyboardHotkeys;
using AmnesiaManager.Views;
using AmnesiaManager.Views.Pages;
using DevExpress.Mvvm;
using KeyDownTester.Keys;

namespace AmnesiaManager.ViewModels
{
    internal enum PageType
    {
        Passwords = 0,
        Settings = 1,
        PasswordEditor = 2
    }

    internal class MainWindowViewModel
    {
        #region Public Properties
        public string ApplicationName => AppDomain.CurrentDomain.FriendlyName;
        public DelegateCommand ExitCommand { get; }
        public Page CurrentPage { get; set; }
        public ObservableCollection<PasswordModel> Passwords;
        #endregion

        #region Private Fields
        private Dictionary<PageType, Page> _pages;
        #endregion

        #region Constructor
        public MainWindowViewModel()
        {
            Passwords = new ObservableCollection<PasswordModel>();

            _pages = new Dictionary<PageType, Page>
            {
                { PageType.Passwords, new PasswordPage() }
            };

            CurrentPage = _pages[PageType.Passwords];

            ExitCommand = new DelegateCommand(Exit);

            HotkeysManager.SetupSystemHook();
            HotkeysManager.AddHotkey(
                new GlobalHotkey(
                    ModifierKeys.Alt, 
                    Key.P,
                    () =>
                    {
                        if (
                            Application.Current.MainWindow is MainWindow window
                        ) window.ToggleWindow(this, new RoutedEventArgs());
                    }
                )
            );
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
