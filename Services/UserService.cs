using AmnesiaManager.Security;
using AmnesiaManager.Security.EncryptedValue;
using Microsoft.VisualBasic.Logging;
using System.IO;
using System;

namespace AmnesiaManager.Services
{
    internal class UserService
    {
        #region Public Properties
        public bool IsLogged { get; set; } = false;
        public EncryptedString EncryptionKey { get; set; } = new(Entropy.Generate(64));
        #endregion

        #region Singletone
        private static UserService? _current;
        public static UserService Current => _current ??= new UserService();
        #endregion
    }
}
