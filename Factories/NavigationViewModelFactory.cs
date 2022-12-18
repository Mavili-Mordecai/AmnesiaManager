using System;
using System.Collections.Generic;
using AmnesiaManager.ViewModels;

namespace AmnesiaManager.Factories
{
    internal enum ViewModelType
    {
        PasswordList = 0,
        PasswordEditor = 1,
        Settings = 2,
    }

    internal class NavigationViewModelFactory
    {
        #region Private Fields
        private Dictionary<ViewModelType, object> _viewModels = new();
        #endregion

        #region Public Methods
        public object Get(ViewModelType type)
        {
            if (_viewModels.ContainsKey(type)) return _viewModels[type];
            _viewModels.Add(type, type switch
            {
                ViewModelType.PasswordList => new PasswordListViewModel(),
                ViewModelType.PasswordEditor => new PasswordEditorViewModel(),
                ViewModelType.Settings => new SettingsViewModel(),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            });

            return _viewModels[type];
        }
        #endregion
    }
}
