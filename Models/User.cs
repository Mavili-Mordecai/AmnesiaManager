using AmnesiaManager.Security;
using AmnesiaManager.Security.EncryptedValue;

namespace AmnesiaManager.Models
{
    internal class User
    {
        #region Public Properties
        public bool IsLogged { get; set; } = false;
        public EncryptedString EncryptionKey { get; set; } = new(Entropy.Generate(64));
        #endregion

        #region Singletone
        private static User? _current;
        public static User Current => _current ??= new User();
        #endregion
    }
}
