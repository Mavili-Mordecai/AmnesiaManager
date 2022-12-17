using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using AmnesiaManager.Factories;
using AmnesiaManager.Models;
using AmnesiaManager.Models.KeyboardHotkeys;
using AmnesiaManager.Views;
using AmnesiaManager.Views.Pages;
using DevExpress.Mvvm;
using KeyDownTester.Keys;

namespace AmnesiaManager.ViewModels
{
    internal class MainWindowViewModel
    {
        #region Public Properties
        public string ApplicationName => Product.Name;
        public Page CurrentPage { get; set; }

        public event EventHandler OnRequestLock;

        public DelegateCommand LockTheAppCommand { get; }
        public DelegateCommand ExitCommand { get; }

        public ObservableCollection<PasswordModel> Passwords;
        #endregion

        #region Private Fields
        private PageFactory _pageFactory;
        #endregion

        #region Constructor
        public MainWindowViewModel()
        {
            Passwords = new ObservableCollection<PasswordModel>();
            _pageFactory = new PageFactory();

            CurrentPage = _pageFactory.Get(PageType.Passwords);

            ExitCommand = new DelegateCommand(Exit);
            LockTheAppCommand = new DelegateCommand(() => { OnRequestLock?.Invoke(this, EventArgs.Empty); });

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
