using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace AmnesiaManager.Security
{
    /// <summary>
    /// A class for encrypting data using the AES algorithm
    /// </summary>
    public static class SymmetricEncryptor
    {
        #region Private Feilds
        private const int AesBlockByteSize = 128 / 8;

        private const int PasswordSaltByteSize = 128 / 8;
        private const int PasswordByteSize = 256 / 8;
        private const int PasswordIterationCount = 100_000;

        private const int SignatureByteSize = 256 / 8;

        private const int MinimumEncryptedMessageByteSize = PasswordSaltByteSize + // auth salt
                                                            PasswordSaltByteSize + // key salt
                                                            AesBlockByteSize +     // IV
                                                            AesBlockByteSize +     // cipher text min length
                                                            SignatureByteSize;     // signature tag

        private static readonly Encoding StringEncoding = Encoding.UTF8;
        private static readonly RandomNumberGenerator Random = RandomNumberGenerator.Create();
        #endregion

        #region Public Methods
        /// <summary>
        /// Encrypts data with salt as the key
        /// </summary>
        /// <param name="data"></param>
        /// <param name="salt"></param>
        /// <returns>Returns an array of encrypted bytes</returns>
        public static byte[] EncryptString(string data, string salt)
        {
            // encrypt
            var keySalt = GenerateRandomBytes(PasswordSaltByteSize);
            var key = GetKey(salt, keySalt);
            var iv = GenerateRandomBytes(AesBlockByteSize);

            byte[] cipherText;
            using (var aes = CreateAes())
            using (var encryptor = aes.CreateEncryptor(key, iv))
            {
                var plainText = StringEncoding.GetBytes(data);
                cipherText = encryptor.TransformFinalBlock(
                    plainText, 0, plainText.Length
                );
            }

            // sign
            var authKeySalt = GenerateRandomBytes(PasswordSaltByteSize);
            var authKey = GetKey(salt, authKeySalt);

            var result = MergeArrays(
                additionalCapacity: SignatureByteSize,
                authKeySalt, keySalt, iv, cipherText
            );

            using (var hmac = new HMACSHA256(authKey))
            {
                var payloadToSignLength = result.Length - SignatureByteSize;
                var signatureTag = hmac.ComputeHash(result, 0, payloadToSignLength);
                signatureTag.CopyTo(result, payloadToSignLength);
            }

            return result;
        }

        /// <summary>
        /// Decrypts data with salt as the key
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="salt"></param>
        /// <returns>Returns an array of decrypted bytes</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="CryptographicException"></exception>
        public static string DecryptToString(byte[] bytes, string salt)
        {
            if (bytes is null || bytes.Length < MinimumEncryptedMessageByteSize)
                throw new ArgumentException("Invalid length of encrypted data");
            
            var authKeySalt = new byte[PasswordSaltByteSize];
            Array.Copy(bytes, 0, authKeySalt, 0, PasswordSaltByteSize);

            var keySalt = new byte[PasswordSaltByteSize];
            Array.Copy(bytes, PasswordSaltByteSize, keySalt, 0, PasswordSaltByteSize);

            var iv = new byte[AesBlockByteSize];
            Array.Copy(bytes, 2 * PasswordSaltByteSize, iv, 0, AesBlockByteSize);

            var signatureTag = new byte[SignatureByteSize];
            Array.Copy(bytes, bytes.Length - SignatureByteSize, signatureTag, 0, SignatureByteSize);

            var cipherTextIndex = authKeySalt.Length + keySalt.Length + iv.Length;
            var cipherTextLength =
                bytes.Length - cipherTextIndex - signatureTag.Length;

            var authKey = GetKey(salt, authKeySalt);
            var key = GetKey(salt, keySalt);

            // verify signature
            using (var hmac = new HMACSHA256(authKey))
            {
                var payloadToSignLength = bytes.Length - SignatureByteSize;
                var signatureTagExpected = hmac.ComputeHash(bytes, 0, payloadToSignLength);

                // constant time checking to prevent timing attacks
                var signatureVerificationResult = 0;
                for (var i = 0; i < signatureTag.Length; i++)
                    signatureVerificationResult |= signatureTag[i] ^ signatureTagExpected[i];
                
                if (signatureVerificationResult != 0)
                    throw new CryptographicException("Invalid signature");
            }

            // decrypt
            using var aes = CreateAes();
            using var encryptor = aes.CreateDecryptor(key, iv);
            var decryptedBytes = encryptor.TransformFinalBlock(
                bytes, 
                cipherTextIndex, 
                cipherTextLength
            );

            return StringEncoding.GetString(decryptedBytes);
        }
        #endregion

        #region Private Methods
        private static Aes CreateAes()
        {
            var aes = Aes.Create();
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            return aes;
        }

        private static byte[] GetKey(string password, byte[] passwordSalt)
        {
            var keyBytes = StringEncoding.GetBytes(password);

            using var derivator = new Rfc2898DeriveBytes(
                keyBytes, passwordSalt, 
                PasswordIterationCount, HashAlgorithmName.SHA256);
            return derivator.GetBytes(PasswordByteSize);
        }

        private static byte[] GenerateRandomBytes(int numberOfBytes)
        {
            var randomBytes = new byte[numberOfBytes];
            Random.GetBytes(randomBytes);
            return randomBytes;
        }

        private static byte[] MergeArrays(int additionalCapacity = 0, params byte[][] arrays)
        {
            var merged = new byte[arrays.Sum(a => a.Length) + additionalCapacity];
            var mergeIndex = 0;

            for (var i = 0; i < arrays.GetLength(0); i++)
            {
                arrays[i].CopyTo(merged, mergeIndex);
                mergeIndex += arrays[i].Length;
            }

            return merged;
        }
        #endregion
    }
}