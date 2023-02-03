using AmnesiaManager.Animations;
using System;
using System.ComponentModel;
using System.Windows;
using AmnesiaManager.ViewModels;

namespace AmnesiaManager.Views
{
    /// <summary>
    /// Interaction logic for AuthorizationWindow.xaml
    /// </summary>
    public partial class AuthorizationWindow : Window
    {
        #region Private Fields
        private bool _isOpened;
        private bool _isAnimating;
        private AuthorizationViewModel? _viewModel;
        #endregion

        #region Constructor
        public AuthorizationWindow()
        {
            InitializeComponent();

            var monitorArea = SystemParameters.WorkArea;
            Left = monitorArea.Right - Width - MainWindow.Offset;
            Top = monitorArea.Bottom - Height - MainWindow.Offset;

            ShowInTaskbar = false;
        }
        #endregion

        #region Override Methods
        protected override void OnActivated(EventArgs e)
        {
            if (_isOpened || _isAnimating) return;
            _isAnimating = true;
            Topmost = true;

            Slide.Left(BorderContent, (_, _) =>
            {
                _isAnimating = false;
                _isOpened = true;
            });
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            TaskbarIcon.Dispose();
        }
        #endregion

        #region Private Methods
        private void AnimationHide()
        {
            if (!_isOpened || _isAnimating) return;

            _isAnimating = true;
            Topmost = true;
            Slide.Right(BorderContent, (_, _) =>
            {
                Hide();
                _isAnimating = false;
                _isOpened = false;
                Topmost = false;
            });
        }

        public void ToggleWindow(object sender, RoutedEventArgs? e)
        {
            //if (IsAnimating) return;
            if (!IsVisible)
            {
                Show();
                Activate();
            }
            else
            {
                AnimationHide();
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            _viewModel = DataContext as AuthorizationViewModel;
            if (_viewModel == null) return;

            _viewModel.OnRequestClose += (_, _) => { Close(); };
        }
        #endregion
    }
}
