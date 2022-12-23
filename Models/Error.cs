using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace AmnesiaManager.Models
{
    public class Error : INotifyPropertyChanged
    {
        #region Fields
        public Visibility Visibility { get; set; }

        private string _message = string.Empty;
        public string Message
        {
            get => _message;
            set
            {
                _message = value.Trim();
                Visibility = string.IsNullOrEmpty(_message)
                    ? Visibility.Collapsed
                    : Visibility.Visible;

                OnPropertyChanged();
                OnPropertyChanged(nameof(Visibility));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        #endregion

        #region Public Methods
        public void Clear() => Message = string.Empty;
        #endregion

        #region Protected Methods
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

    }
}
