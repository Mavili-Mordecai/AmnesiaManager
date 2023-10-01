namespace AmnesiaManager.Security.EncryptionAlgorithms
{
    interface ISymmetricEncryptionAlgorithm
    {
        public byte[] Encrypt(string data, string salt);
        public string Decrypt(byte[] bytes, string salt);
    }
}
