// Decompiled with JetBrains decompiler
// Type: Ticketing_Dashboard.Utility.CryptographyManager
// Assembly: Ticketing Dashboard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E2BA73D6-734D-47BB-8E89-378BFC2CE176
// Assembly location: C:\Users\PenTester07\wwwroot\bin\Ticketing Dashboard.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Ticketing_Dashboard.Utility
{
    public static class CryptographyManager
    {
        private const int Keysize = 256;
        private const int DerivationIterations = 1000;

        public static string EncryptString(string plainText, string passPhrase = "Du@9]$^f6._;pqvP")
        {
            byte[] numArray1 = CryptographyManager.Generate256BitsOfRandomEntropy();
            byte[] numArray2 = CryptographyManager.Generate256BitsOfRandomEntropy();
            char[] chArray = new char[1] { '=' };
            byte[] bytes1 = Encoding.UTF8.GetBytes(plainText);
            using (Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(passPhrase, numArray1, 1000))
            {
                byte[] bytes2 = rfc2898DeriveBytes.GetBytes(32);
                using (RijndaelManaged rijndaelManaged = new RijndaelManaged())
                {
                    rijndaelManaged.BlockSize = 256;
                    rijndaelManaged.Mode = CipherMode.CBC;
                    rijndaelManaged.Padding = PaddingMode.PKCS7;
                    using (ICryptoTransform encryptor = rijndaelManaged.CreateEncryptor(bytes2, numArray2))
                    {
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                            {
                                cryptoStream.Write(bytes1, 0, bytes1.Length);
                                cryptoStream.FlushFinalBlock();
                                byte[] array = ((IEnumerable<byte>)((IEnumerable<byte>)numArray1).Concat<byte>((IEnumerable<byte>)numArray2).ToArray<byte>()).Concat<byte>((IEnumerable<byte>)memoryStream.ToArray()).ToArray<byte>();
                                memoryStream.Close();
                                cryptoStream.Close();
                                return Convert.ToBase64String(array).TrimEnd(chArray).Replace('+', '-').Replace('/', '_');
                            }
                        }
                    }
                }
            }
        }

        public static string DecryptString(string cipherText, string passPhrase = "Du@9]$^f6._;pqvP")
        {
            string s = cipherText.Replace('_', '/').Replace('-', '+');
            switch (cipherText.Length % 4)
            {
                case 2:
                    s += "==";
                    break;
                case 3:
                    s += "=";
                    break;
            }
            byte[] source = Convert.FromBase64String(s);
            byte[] array1 = ((IEnumerable<byte>)source).Take<byte>(32).ToArray<byte>();
            byte[] array2 = ((IEnumerable<byte>)source).Skip<byte>(32).Take<byte>(32).ToArray<byte>();
            byte[] array3 = ((IEnumerable<byte>)source).Skip<byte>(64).Take<byte>(source.Length - 64).ToArray<byte>();
            using (Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(passPhrase, array1, 1000))
            {
                byte[] bytes = rfc2898DeriveBytes.GetBytes(32);
                using (RijndaelManaged rijndaelManaged = new RijndaelManaged())
                {
                    rijndaelManaged.BlockSize = 256;
                    rijndaelManaged.Mode = CipherMode.CBC;
                    rijndaelManaged.Padding = PaddingMode.PKCS7;
                    using (ICryptoTransform decryptor = rijndaelManaged.CreateDecryptor(bytes, array2))
                    {
                        using (MemoryStream memoryStream = new MemoryStream(array3))
                        {
                            using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                            {
                                byte[] numArray = new byte[array3.Length];
                                int count = cryptoStream.Read(numArray, 0, numArray.Length);
                                memoryStream.Close();
                                cryptoStream.Close();
                                return Encoding.UTF8.GetString(numArray, 0, count);
                            }
                        }
                    }
                }
            }
        }

        private static byte[] Generate256BitsOfRandomEntropy()
        {
            byte[] data = new byte[32];
            using (RNGCryptoServiceProvider cryptoServiceProvider = new RNGCryptoServiceProvider())
                cryptoServiceProvider.GetBytes(data);
            return data;
        }

        public static string ValidatedPasswordChecks(string password)
        {
            if (password.Length < 8)
                return "Password Length should be greater than 8";
            if (!password.Any<char>(new Func<char, bool>(char.IsUpper)))
                return "Password must have at least one uppercase letter";
            if (!password.Any<char>(new Func<char, bool>(char.IsLower)))
                return "Password must have at least one lowercase letter";
            if (!password.Any<char>(new Func<char, bool>(char.IsDigit)))
                return "Password must have at least one number";
            char[] charArray = "%!@#$%^&*()?/>.<,:;'\\|}]{[_~`+=-\"".ToCharArray();
            bool flag = false;
            foreach (char ch in charArray)
            {
                if (password.Contains<char>(ch))
                    flag = true;
            }
            return !flag ? "Password must have at least one special character" : "Correct";
        }

        public static string GenerateUUID() => Guid.NewGuid().ToString();

        public static void CheckLinkValidity(DateTime time)
        {
            do
                ;
            while (DateTime.Now.Subtract(time).TotalMinutes <= 10.0);
        }

        public static string ComputeSha256Hash(string rawData)
        {
            using (SHA256 shA256 = SHA256.Create())
            {
                byte[] hash = shA256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                StringBuilder stringBuilder = new StringBuilder();
                for (int index = 0; index < hash.Length; ++index)
                    stringBuilder.Append(hash[index].ToString("x2"));
                return stringBuilder.ToString();
            }
        }

        public static string ComputeSha265HashWithSalt(string rawData)
        {
            string str = "Du@9]$^f6._;pqvP";
            using (SHA256 shA256 = SHA256.Create())
            {
                byte[] hash = shA256.ComputeHash(Encoding.UTF8.GetBytes(rawData + str));
                StringBuilder stringBuilder = new StringBuilder();
                for (int index = 0; index < hash.Length; ++index)
                    stringBuilder.Append(hash[index].ToString("x2"));
                return stringBuilder.ToString();
            }
        }

        public static bool VerifySha265HashWithSalt(string rawData, string comapareWith)
        {
            string str = "Du@9]$^f6._;pqvP";
            using (SHA256 shA256 = SHA256.Create())
            {
                byte[] hash = shA256.ComputeHash(Encoding.UTF8.GetBytes(rawData + str));
                StringBuilder stringBuilder = new StringBuilder();
                for (int index = 0; index < hash.Length; ++index)
                    stringBuilder.Append(hash[index].ToString("x2"));
                return stringBuilder.ToString() == comapareWith;
            }
        }
    }
}
