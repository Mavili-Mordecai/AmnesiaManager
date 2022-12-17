using System.Security.Cryptography;
using System.Text;

namespace AmnesiaManager.Security.EncryptedValue
{
    /// <summary>
    /// Stores the string in encrypted mode using DPAPI. Supports Windows only.
    /// </summary>
    public sealed class EncryptedString : EncryptedValue<string>
    {
        #region Private Fields
        private readonly Encoding _encoding;
        #endregion

        #region Constructor
        public EncryptedString(byte[] entropy, Encoding? encoding = null, DataProtectionScope scope = DataProtectionScope.CurrentUser)
            : base(entropy, scope)
        {
            _encoding = encoding ?? Encoding.UTF8;
        }
        #endregion

        #region Override Methods
        protected override string GetFromBytes(byte[] bytes) => _encoding.GetString(bytes);
        
        protected override byte[] GetBytes(in string value) => _encoding.GetBytes(value);
        #endregion
    }
}
