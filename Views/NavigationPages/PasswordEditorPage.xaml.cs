using System.Windows;
using System.Windows.Controls;
using AmnesiaManager.Models;

namespace AmnesiaManager.Views.NavigationPages
{
    /// <summary>
    /// Interaction logic for PasswordEditorPage.xaml
    /// </summary>
    public partial class PasswordEditorPage : Page
    {
        #region Dependency Property
        public PasswordModel? EditablePassword
        {
            get => (PasswordModel?)GetValue(EditablePasswordProperty);
            set => SetValue(EditablePasswordProperty, value);
        }

        public static readonly DependencyProperty EditablePasswordProperty = DependencyProperty.Register(
            nameof(EditablePassword),
            typeof(PasswordModel),
            typeof(PasswordEditorPage),
            new PropertyMetadata(null)
        );
        #endregion

        #region Constructor
        public PasswordEditorPage()
        {
            InitializeComponent();
        }
        #endregion
    }
}
