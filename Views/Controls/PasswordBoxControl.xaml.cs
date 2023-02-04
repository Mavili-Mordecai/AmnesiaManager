using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace AmnesiaManager.Views.Controls
{
    /// <summary>
    /// Interaction logic for PasswordBoxControl.xaml
    /// </summary>
    public partial class PasswordBoxControl : UserControl
    {
        #region Dependency Properties
        #region Placeholder
        public string Placeholder
        {
            get => (string)GetValue(PlaceholderProperty);
            set => SetValue(PlaceholderProperty, value);
        }

        public static readonly DependencyProperty PlaceholderProperty =
            DependencyProperty.Register(
                nameof(Placeholder),
                typeof(string),
                typeof(PasswordBoxControl),
                new PropertyMetadata(string.Empty)
            );
        #endregion

        #region Password
        public string Password
        {
            get => (string)GetValue(PasswordProperty);
            set => SetValue(PasswordProperty, value);
        }

        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register(
                nameof(Password),
                typeof(string),
                typeof(PasswordBoxControl),
                new PropertyMetadata(string.Empty)
            );
        #endregion

        #region Icon
        public BitmapImage Icon
        {
            get => (BitmapImage)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register(
                nameof(Icon),
                typeof(BitmapImage),
                typeof(PasswordBoxControl),
                new PropertyMetadata(null)
            );
        #endregion

        #region Corner Radius
        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(
                nameof(CornerRadius),
                typeof(CornerRadius),
                typeof(PasswordBoxControl),
                new PropertyMetadata(new CornerRadius(0))
            );
        #endregion

        #region MaxLength
        public int MaxLength
        {
            get => (int)GetValue(MaxLengthProperty);
            set => SetValue(MaxLengthProperty, value);
        }

        public static readonly DependencyProperty MaxLengthProperty =
            DependencyProperty.Register(
                nameof(MaxLength),
                typeof(int),
                typeof(PasswordBoxControl),
                new PropertyMetadata(50)
            );
        #endregion

        #region FontSizePassword
        public int FontSizePassword
        {
            get => (int)GetValue(FontSizePasswordProperty);
            set => SetValue(FontSizePasswordProperty, value);
        }

        public static readonly DependencyProperty FontSizePasswordProperty = DependencyProperty.Register(
            nameof(FontSizePassword), 
            typeof(int),
            typeof(PasswordBoxControl),
            new PropertyMetadata(8)
        );
        #endregion
        #endregion

        #region Constructor
        public PasswordBoxControl()
        {
            InitializeComponent();
        }
        #endregion
        private void PasswordChanged(object sender, RoutedEventArgs e)
        {
            Password = ((PasswordBox)sender).Password;
            PlaceholderBox.Foreground = string.IsNullOrWhiteSpace(TextSource.Password)
                ? TextSource.Foreground
                : System.Windows.Media.Brushes.Transparent;
        }
    }
}