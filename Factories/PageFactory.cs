using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using AmnesiaManager.Views.Pages;

namespace AmnesiaManager.Factories
{
    internal enum PageType
    {
        Passwords = 0,
        PasswordEditor = 2,
        Settings = 3,
    }

    internal class PageFactory
    {
        #region Private Fields
        private Dictionary<PageType, Page> _pages = new();
        #endregion

        #region Public Methods
        public Page Get(PageType type)
        {
            if (_pages.ContainsKey(type)) return _pages[type];
            _pages.Add(type, type switch
            {
                PageType.Passwords => new PasswordPage(),
                PageType.PasswordEditor => new Page(),
                PageType.Settings => new Page(),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            });

            return _pages[type];
        }
        #endregion
    }
}
