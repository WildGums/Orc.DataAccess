// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringExtensions.cs" company="WildGums">
//   Copyright (c) 2008 - 2019 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.DataAccess
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;

    public static class StringExtensions
    {
        #region Constants
        private const int Keysize = 256; // This constant is used to determine the keysize of the encryption algorithm.
        public const string InitVector = "tu89geji340t89u2";
        #endregion

        #region Methods       
        //TODO:Remove
        public static string Encrypt(this string plainText) /////to encrypt password
        {
            var passPhrase = "FG_EncryptionKey"; /////encryption Key text
            var initVectorBytes = Encoding.UTF8.GetBytes(InitVector);
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            //var salt1 = new byte[8];
            //using (var rngCsp = new RNGCryptoServiceProvider())
            //{
            //    // Fill the array with a random value.
            //    rngCsp.GetBytes(salt1);
            //}

            var password = new Rfc2898DeriveBytes(passPhrase, new byte[] {1, 2, 3, 4, 5, 6, 7, 8});
            var keyBytes = password.GetBytes(Keysize / 8);
            var symmetricKey = new RijndaelManaged
            {
                Mode = CipherMode.CBC
            };
            var encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes); ////To encrypt
            var memoryStream = new MemoryStream();
            var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
            cryptoStream.FlushFinalBlock();
            var cipherTextBytes = memoryStream.ToArray();
            memoryStream.Close();
            cryptoStream.Close();
            return Convert.ToBase64String(cipherTextBytes);
        }

        public static string Decrypt(this string cipherText)
        {
            try
            {
                var passPhrase = "FG_EncryptionKey"; /////encryption Key text same 
                //// as using in encryption if key change then it will not decrypt proper
                var initVectorBytes = Encoding.ASCII.GetBytes(InitVector);
                var cipherTextBytes = Convert.FromBase64String(cipherText);

                var password = new Rfc2898DeriveBytes(passPhrase, new byte[] {1, 2, 3, 4, 5, 6, 7, 8});
                var keyBytes = password.GetBytes(Keysize / 8);
                var symmetricKey = new RijndaelManaged
                {
                    Mode = CipherMode.CBC
                };
                var decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);
                var memoryStream = new MemoryStream(cipherTextBytes);
                var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
                var plainTextBytes = new byte[cipherTextBytes.Length];
                var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                memoryStream.Close();
                cryptoStream.Close();
                return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion
    }
}
