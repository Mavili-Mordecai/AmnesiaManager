using System.Diagnostics;
using AmnesiaManager.Models;
using System.Windows;
using System.Windows.Controls;
using AmnesiaManager.Factories;
using AmnesiaManager.ViewModels;

namespace AmnesiaManager.Views.NavigationControls
{
    /// <summary>
    /// Interaction logic for PasswordEditorControl.xaml
    /// </summary>
    public partial class PasswordEditorControl : UserControl
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
            typeof(PasswordEditorControl),
            new PropertyMetadata(null)
        );
        #endregion

        #region Constructor
        public PasswordEditorControl()
        {
            InitializeComponent();
        }
        #endregion
    }
}
