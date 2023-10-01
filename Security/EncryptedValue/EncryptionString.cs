using System;
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

        public EncryptedString(Encoding? encoding = null, DataProtectionScope scope = DataProtectionScope.CurrentUser)
            : base(scope)
        {
            _encoding = encoding ?? Encoding.UTF8;
        }

        #endregion

        #region Override Methods

        protected override string GetFromBytes(byte[] bytes) => _encoding.GetString(bytes);

        protected override byte[] GetBytes(in string value) => _encoding.GetBytes(value);

        protected override string GetRandomOverwritingData()
        {
            using var random = RandomNumberGenerator.Create();
            var randomData = new byte[Value.Length];
            random.GetBytes(randomData);
            var randomDataString = BitConverter.ToString(randomData).Replace("-", "");
            return randomDataString;
        }

        #endregion
    }
}