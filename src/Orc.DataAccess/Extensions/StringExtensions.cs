﻿namespace Orc.DataAccess;

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public static class StringExtensions
{
    private static readonly HashAlgorithmName HashAlgorithm = HashAlgorithmName.SHA256;
    private const int Iterations = 80234;
    private const int Keysize = 256; // This constant is used to determine the keysize of the encryption algorithm.
    public const string InitVector = "tu89geji340t89u2";

    public static string Encrypt(this string plainText) /////to encrypt password
    {
        ArgumentNullException.ThrowIfNull(plainText);

        const string passPhrase = "FG_EncryptionKey"; /////encryption Key text
        var initVectorBytes = Encoding.UTF8.GetBytes(InitVector);
        var plainTextBytes = Encoding.UTF8.GetBytes(plainText);

        using var password = new Rfc2898DeriveBytes(passPhrase, new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 }, Iterations, HashAlgorithm);
        var keyBytes = password.GetBytes(Keysize / 8);
        using var symmetricKey = Aes.Create();
        var encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes); ////To encrypt

        using var memoryStream = new MemoryStream();
        using var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
        cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
        cryptoStream.FlushFinalBlock();
        var cipherTextBytes = memoryStream.ToArray();
        memoryStream.Close();
        cryptoStream.Close();
        return Convert.ToBase64String(cipherTextBytes);
    }

    public static string? Decrypt(this string cipherText)
    {
        ArgumentNullException.ThrowIfNull(cipherText);

        try
        {
            const string passPhrase = "FG_EncryptionKey"; /////encryption Key text same 
            //// as using in encryption if key change then it will not decrypt proper
            var initVectorBytes = Encoding.ASCII.GetBytes(InitVector);
            var cipherTextBytes = Convert.FromBase64String(cipherText);

            using var password = new Rfc2898DeriveBytes(passPhrase, new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 }, Iterations, HashAlgorithm);
            var keyBytes = password.GetBytes(Keysize / 8);
            using var symmetricKey = Aes.Create();
            var decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);

            using var memoryStream = new MemoryStream(cipherTextBytes);
            using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
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
}
