using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using AmnesiaManager.Factories;
using AmnesiaManager.Models;
using AmnesiaManager.Services;
using AmnesiaManager.Views;
using DevExpress.Mvvm;

namespace AmnesiaManager.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region Public Properties
        public string ApplicationName => Product.Name;
        public ObservableCollection<PasswordModel> Passwords;
        public event EventHandler? OnRequestLock;

        public object? CurrentViewModel
        {
            get => GetProperty(() => CurrentViewModel);
            set { SetProperty(() => CurrentViewModel, value); }
        }

        #region Commands
        public DelegateCommand LockTheAppCommand { get; }
        public DelegateCommand ExitCommand { get; }
        public DelegateCommand<int> ChangeViewModelCommand { get; }
        #endregion
        #endregion
        
        #region Private Fields
        private readonly NavigationViewModelFactory _viewModelFactory;
        #endregion

        #region Constructor
        public MainWindowViewModel()
        {
            Passwords = new ObservableCollection<PasswordModel>();
            _viewModelFactory = new NavigationViewModelFactory();

            CurrentViewModel = _viewModelFactory.Get(ViewModelType.PasswordList);

            ChangeViewModelCommand = new DelegateCommand<int>(ChangeViewModel);
            ExitCommand = new DelegateCommand(Exit);
            LockTheAppCommand = new DelegateCommand(() => { OnRequestLock?.Invoke(this, EventArgs.Empty); });

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
        public void ChangeViewModel(int type) => CurrentViewModel = _viewModelFactory.Get((ViewModelType)type);

        public void EditPassword(PasswordModel password)
        {
            var viewModel = _viewModelFactory.Get(ViewModelType.PasswordEditor) as PasswordEditorViewModel;
            viewModel?.SetEditablePassword(password);
            CurrentViewModel = viewModel;
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
