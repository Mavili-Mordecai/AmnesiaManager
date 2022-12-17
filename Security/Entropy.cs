using System.Security.Cryptography;

namespace AmnesiaManager.Security
{
    /// <summary>
    /// A class for generating cryptographically secure entropy
    /// </summary>
    public static class Entropy
    {
        #region Private Fields
        private static readonly RandomNumberGenerator RandomNumberGenerator = RandomNumberGenerator.Create();
        #endregion

        #region Public Methods
        /// <summary>
        /// Generates random entropy
        /// </summary>
        /// <param name="power"></param>
        /// <returns>Returns an array of size equal to power consisting of random bytes</returns>
        public static byte[] Generate(int power = 32)
        {
            var entropy = new byte[power];
            RandomNumberGenerator.GetBytes(entropy);
            return entropy;
        }
        #endregion
    }
}
