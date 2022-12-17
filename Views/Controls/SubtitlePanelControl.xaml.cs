using System.Windows;
using System.Windows.Controls;

namespace AmnesiaManager.Views.Controls
{
    /// <summary>
    /// Interaction logic for SubtitlePanelControl.xaml
    /// </summary>
    public partial class SubtitlePanelControl : UserControl
    {
        #region Dependency Properties
        #region Text
        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            nameof(Text), 
            typeof(string), 
            typeof(SubtitlePanelControl), 
            new PropertyMetadata("Subtitle")
        );
        #endregion
        #endregion

        #region Constructor
        public SubtitlePanelControl()
        {
            InitializeComponent();
        }
        #endregion
    }
}
