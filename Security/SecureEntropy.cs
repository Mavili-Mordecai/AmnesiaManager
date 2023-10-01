using System;
using System.Security.Cryptography;

namespace AmnesiaManager.Security
{
    public class SecureEntropy : IDisposable
    {
        #region Public Fields

        public int Power { get; set; }
        public bool IsEmpty => _entropy.Length == 0;

        #endregion

        #region Private Fields

        private byte[] _entropy;

        #endregion

        #region Constructor

        public SecureEntropy(int power)
        {
            _entropy = RandomNumberGenerator.GetBytes(Power = power);
        }

        #endregion

        #region Public Methods

        public byte[] Get() => (byte[])_entropy.Clone();
        public byte[] Regenerate() => _entropy = RandomNumberGenerator.GetBytes(Power);
        public void Clear() => Array.Clear(_entropy, 0, _entropy.Length);
        public void Dispose() => Clear();

        #endregion

        #region Desctructor

        ~SecureEntropy() => Dispose();

        #endregion
    }
}