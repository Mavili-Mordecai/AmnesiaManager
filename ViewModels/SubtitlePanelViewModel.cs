using System.Windows;
using AmnesiaManager.Factories;
using AmnesiaManager.Views;
using DevExpress.Mvvm;

namespace AmnesiaManager.ViewModels
{
    internal class SubtitlePanelViewModel : ViewModelBase
    {
        #region Commands
        public DelegateCommand OpenPasswordListControlCommand { get; }
        #endregion

        #region Constructor

        public SubtitlePanelViewModel()
        {
            OpenPasswordListControlCommand = new DelegateCommand(OpenPasswordListControl);
        }

        #endregion

        #region Private Methods

        private void OpenPasswordListControl()
        {
            if (Application.Current.MainWindow is not MainWindow { DataContext: MainWindowViewModel viewModel }) return;
            viewModel.ChangeViewModel(ViewModelType.PasswordList.GetHashCode());
        }
        #endregion
    }
}
