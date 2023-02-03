using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace AmnesiaManager.Views.Controls
{
    /// <summary>
    /// Interaction logic for HyperlinkControl.xaml
    /// </summary>
    public partial class HyperlinkControl : UserControl
    {
        #region Dependency properties
        #region Text
        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            nameof(Text), 
            typeof(string), 
            typeof(HyperlinkControl), 
            new PropertyMetadata(string.Empty)
        );
        #endregion

        #region Url
        public string Url
        {
            get => (string)GetValue(UrlProperty);
            set => SetValue(UrlProperty, value);
        }

        public static readonly DependencyProperty UrlProperty = DependencyProperty.Register(
            nameof(Url),
            typeof(string),
            typeof(HyperlinkControl),
            new PropertyMetadata(string.Empty)
        );
        #endregion
        #endregion

        #region Constructor
        public HyperlinkControl()
        {
            InitializeComponent();
        }
        #endregion

        private void LinkClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Url)) return;
            try
            {
                Process.Start(new ProcessStartInfo(Url)
                {
                    UseShellExecute = true
                });
            }
            catch (Exception)
            {
                // TODO: Log this exception
            }
        }
    }
}
