using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace AmnesiaManager.Security.EncryptedValue
{
    public abstract class EncryptedValue<T> : IDisposable
    {
        #region Public Properties

        /// <summary>
        /// When receiving a value, decrypts the value and returns it, and encrypts it when setting
        /// </summary>
        public T Value
        {
            get => GetFromBytes(ProtectedData.Unprotect(_encryptedBytes!, _entropy.Get(), _scope));
            set => _encryptedBytes = ProtectedData.Protect(GetBytes(value), _entropy.Get(), _scope);
        }

        /// <summary>
        /// Returns true if the byte array of the encrypted value is null or empty
        /// </summary>
        public bool IsEmpty => _encryptedBytes.Length == 0;

        public int EntropyPower = 256;

        #endregion

        #region Private Fields

        private readonly DataProtectionScope _scope;
        private readonly SecureEntropy _entropy;
        private byte[] _encryptedBytes;

        #endregion

        #region Constructor

        protected EncryptedValue(DataProtectionScope scope = DataProtectionScope.CurrentUser)
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) throw new NotSupportedException();
            _scope = scope;
            _entropy = new SecureEntropy(EntropyPower);
            _encryptedBytes = Array.Empty<byte>();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Clears an array of encrypted bytes
        /// </summary>
        public void Clear()
        {
            Value = GetRandomOverwritingData();
            Array.Clear(_encryptedBytes, 0, _encryptedBytes.Length);
            _entropy.Clear();
        }

        public void Dispose()
        {
            Clear();
            _entropy.Dispose();
        }

        #endregion

        #region Abstract Methods

        protected abstract T GetFromBytes(byte[] bytes);
        protected abstract byte[] GetBytes(in T value);
        protected abstract T GetRandomOverwritingData();

        #endregion
    }
}