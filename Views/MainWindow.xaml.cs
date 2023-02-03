using AmnesiaManager.Animations;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using AmnesiaManager.ViewModels;

namespace AmnesiaManager.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Public Fields
        public static int Offset => 15;
        public bool IsAnimating;
        #endregion

        #region Private Fields
        private bool _isOpened;
        private bool _isNeedToClose;
        private MainWindowViewModel? _viewModel;
        #endregion

        #region Constructor
        public MainWindow()
        {
            InitializeComponent();

            var monitorArea = SystemParameters.WorkArea;
            Left = monitorArea.Right - Width - Offset;
            Top = monitorArea.Bottom - Height - Offset;

            ShowInTaskbar = false;
        }
        #endregion

        #region Override Methods
        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = !_isNeedToClose;
            
            if (!e.Cancel && !TaskbarIcon.IsDisposed) TaskbarIcon.Dispose();
            
            base.OnClosing(e);
        }

        protected override void OnActivated(EventArgs e)
        {
            if (_isOpened || IsAnimating) return;

            IsAnimating = true;
            Topmost = true;
            Slide.Left(BorderContent, (_, _) =>
            {
                IsAnimating = false;
                _isOpened = true;
            });
        }
        #endregion

        #region Public Methods
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
        #endregion

        #region Private Methods
        private void AnimationHide(EventHandler? completed = null)
        {
            if (!_isOpened || IsAnimating) return;

            IsAnimating = true;
            Topmost = true;
            Slide.Right(BorderContent, (_, _) =>
            {
                Hide();
                IsAnimating = false;
                _isOpened = false;
                Topmost = false;
                completed?.Invoke(this, EventArgs.Empty);
            });
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            _viewModel = DataContext as MainWindowViewModel;
            if (_viewModel == null) return;

            _viewModel.OnRequestLock += (o, args) =>
            {
                Application.Current.MainWindow = new AuthorizationWindow();
                _isNeedToClose = true;

                AnimationHide((s, e) => { Close(); });
            };
        }        
        #endregion
    }
}
