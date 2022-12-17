using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace AmnesiaManager.Security.EncryptedValue
{
    public abstract class EncryptedValue<T>
    {
        #region Public Properties
        /// <summary>
        /// When receiving a value, decrypts the value and returns it, and encrypts it when setting
        /// </summary>
        public T Value
        {
            get => GetFromBytes(ProtectedData.Unprotect(_encryptedBytes!, _entropy, _scope));
            set => _encryptedBytes = ProtectedData.Protect(GetBytes(value), _entropy, _scope);
        }

        /// <summary>
        /// Returns true if the byte array of the encrypted value is null or empty
        /// </summary>
        public bool IsEmpty => _encryptedBytes == null || _encryptedBytes.Length == 0;
        #endregion

        #region Private Fields
        private readonly DataProtectionScope _scope;
        private byte[] _entropy;
        private byte[]? _encryptedBytes;
        #endregion

        #region Constructor
        protected EncryptedValue(byte[] entropy, DataProtectionScope scope = DataProtectionScope.CurrentUser)
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) throw new NotSupportedException();
            _scope = scope;
            _entropy = entropy;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Clears an array of encrypted bytes
        /// </summary>
        public void Clear() => _encryptedBytes = null;
        
        /// <summary>
        /// Replaces the old entropy with a new one
        /// </summary>
        /// <param name="newEntropy"></param>
        /// <returns>True if entropy is successfully changed, otherwise false</returns>
        public bool ChangeEntropy(byte[] newEntropy)
        {
            if (IsEmpty)
            {
                _entropy = newEntropy;
                return true;
            }

            try
            {
                var decryptedBytes = ProtectedData.Unprotect(_encryptedBytes!, _entropy, _scope);
                _encryptedBytes = ProtectedData.Protect(decryptedBytes, newEntropy, _scope);
                return true;
            }
            catch (CryptographicException)
            {
                return false;
            }
        }
        #endregion

        #region Abstract Methods
        protected abstract T GetFromBytes(byte[] bytes);
        protected abstract byte[] GetBytes(in T value);
        #endregion
    }
}
