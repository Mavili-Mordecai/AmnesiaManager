using AmnesiaManager.Animations;
using System;
using System.ComponentModel;
using System.Windows;

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

        #region MyRegion
        private bool _isOpened = false;
        #endregion

        #region Override Methods
        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
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
        public void AnimationHide()
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
        #endregion
    }
}
