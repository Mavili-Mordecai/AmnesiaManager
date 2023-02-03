using System;
using System.Collections.Generic;
using System.Windows.Controls;
using AmnesiaManager.Views.NavigationPages;

namespace AmnesiaManager.Factories
{
    public enum PageType
    {
        PasswordList = 0,
        PasswordEditor = 1,
        Settings = 2,
    }

    internal class NavigationPageFactory
    {
        #region Private Fields
        private readonly Dictionary<PageType, Page> _pages = new();
        #endregion

        #region Public Methods
        public Page Get(PageType type, bool isCached = true)
        {
            if (_pages.ContainsKey(type) && isCached) return _pages[type];

            Page page = type switch
            {
                PageType.PasswordList => new PasswordListPage(),
                PageType.PasswordEditor => new PasswordEditorPage(),
                PageType.Settings => new SettingsPage(),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };

            if (isCached) _pages.Add(type, page);

            return page;
        }
        #endregion
    }
}
