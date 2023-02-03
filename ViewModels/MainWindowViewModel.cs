using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using AmnesiaManager.Factories;
using AmnesiaManager.Models;
using AmnesiaManager.Services;
using AmnesiaManager.Views;
using AmnesiaManager.Views.NavigationPages;
using DevExpress.Mvvm;

namespace AmnesiaManager.ViewModels
{
    
    public class MainWindowViewModel : ViewModelBase
    {
        #region Public Properties
        public string ApplicationName => Product.Name;
        public ObservableCollection<PasswordModel> Passwords;
        public event EventHandler? OnRequestLock;

        public Page CurrentPage { 
            get => GetProperty(() => CurrentPage);
            set => SetProperty(() => CurrentPage, value);
        }

        #region Commands
        public DelegateCommand LockTheAppCommand { get; }
        public DelegateCommand ExitCommand { get; }
        public DelegateCommand<int> ChangeViewModelCommand { get; }
        #endregion
        #endregion
        
        #region Private Fields
        private readonly NavigationPageFactory _pageFactory;
        #endregion

        #region Constructor
        public MainWindowViewModel()
        {
            Passwords = new ObservableCollection<PasswordModel>();
            _pageFactory = new NavigationPageFactory();

            ChangeViewModelCommand = new DelegateCommand<int>(ChangePage);
            ExitCommand = new DelegateCommand(Exit);
            LockTheAppCommand = new DelegateCommand(() =>
            {
                OnRequestLock?.Invoke(this, EventArgs.Empty);
            });

            CurrentPage = _pageFactory.Get(PageType.PasswordList);
            
            HotkeyService.SetupSystemHook();
            HotkeyService.AddHotkey(
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

        #region Public Methods
        public void ChangePage(int type) => CurrentPage = _pageFactory.Get((PageType)type);

        public void EditPassword(PasswordModel password)
        {
            var page = _pageFactory.Get(PageType.PasswordEditor, false) as PasswordEditorPage;
            if (page?.DataContext is not PasswordEditorViewModel viewModel) return;
            viewModel.SetEditablePassword(password);
            CurrentPage = page;
        }
        #endregion

        #region Private Methods
        private void Exit()
        {
            if (Application.Current.MainWindow is MainWindow window) window.TaskbarIcon.Dispose();
            Environment.Exit(0);
        }
        #endregion
    }
}
