using DevExpress.Mvvm;
using System.Windows;

namespace AmnesiaManager.ViewModels
{
    internal class PasswordControlViewModel : ViewModelBase
    {
        #region Public Methods
        public void CopyText(string text) => Clipboard.SetDataObject(text);
        #endregion
    }
}
        