using System.Windows;
using System.Windows.Controls;
using AmnesiaManager.ViewModels;

namespace AmnesiaManager.Views.Controls
{
    /// <summary>
    /// Interaction logic for PasswordControl.xaml
    /// </summary>
    public partial class PasswordControl : UserControl
    {
        #region Private Fields
        private readonly PasswordControlViewModel _viewModel;
        #endregion

        #region Constructor
        public PasswordControl()
        {
            InitializeComponent();
            _viewModel = new PasswordControlViewModel();
        }
        #endregion

        #region Private Methods
        private void CopyText(object sender, RoutedEventArgs e)
        {
            if (sender is not Button { Tag: string tag }) return;
            _viewModel.CopyText(tag);
        }
        #endregion
    }
}
