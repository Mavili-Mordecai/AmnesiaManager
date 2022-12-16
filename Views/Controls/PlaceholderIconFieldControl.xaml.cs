using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace AmnesiaManager.Views.Controls
{
    /// <summary>
    /// Interaction logic for PlaceholderIconFieldControl.xaml
    /// </summary>
    public partial class PlaceholderIconFieldControl : UserControl
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
                typeof(PlaceholderIconFieldControl), 
                new PropertyMetadata(string.Empty)
            );
        #endregion

        #region Text
        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(
                nameof(Text), 
                typeof(string), 
                typeof(PlaceholderIconFieldControl), 
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
                typeof(PlaceholderIconFieldControl), 
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
                typeof(PlaceholderIconFieldControl),
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
                typeof(PlaceholderIconFieldControl), 
                new PropertyMetadata(50)
            );
        #endregion
        #endregion

        #region Constructor
        public PlaceholderIconFieldControl()
        {
            InitializeComponent();
        }
        #endregion
    }
}
