using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace EasyImgur
{
    static class CredentialsHelper
    {
        public struct Credentials
        {
            public string Name { get; set; }
            public string Password { get; set; }
        }

        public static void StoreCredentials(string name, string password)
        {
            using (Aes myAes = Aes.Create())
            {
                byte[] key = myAes.Key;
                byte[] iv = myAes.IV;

                byte[] encryptedName = AesHelper.Encrypt(name, key, iv);
                byte[] encryptedP = AesHelper.Encrypt(password, key, iv);

                // Format: 
                // [4 bytes: key size in bytes]
                // [4 bytes: iv size in bytes]
                // [4 bytes: name size]
                // [4 bytes: password size]
                // [n bytes: key (n==keysize)]
                // [n bytes: iv (n==ivsize)]
                // [n bytes: name (n==namesize)]
                // [n bytes: password (n==passwordsize)]

                var finalData = new byte[16 + key.Length + iv.Length + encryptedName.Length + encryptedP.Length];

                BitConverter.GetBytes(key.Length).CopyTo(finalData, 0);
                BitConverter.GetBytes(iv.Length).CopyTo(finalData, 4);
                BitConverter.GetBytes(encryptedName.Length).CopyTo(finalData, 8);
                BitConverter.GetBytes(encryptedP.Length).CopyTo(finalData, 12);
                key.CopyTo(finalData, 16);
                iv.CopyTo(finalData, 16 + key.Length);
                encryptedName.CopyTo(finalData, 16 + key.Length + iv.Length);
                encryptedP.CopyTo(finalData, 16 + key.Length + iv.Length + encryptedName.Length);

                string dataString = Convert.ToBase64String(finalData);
                Properties.Settings.Default.accountInfo = dataString;
                Properties.Settings.Default.Save();
            }
        }

        public static Credentials RetrieveCredentials()
        {
            string dataString = Properties.Settings.Default.accountInfo;
            byte[] data = Convert.FromBase64String(dataString);

            int keySize = BitConverter.ToInt32(data, 0);
            int ivSize = BitConverter.ToInt32(data, 4);
            int nameLength = BitConverter.ToInt32(data, 8);
            int pLength = BitConverter.ToInt32(data, 12);

            var key = new byte[keySize];
            Array.Copy(data, 16, key, 0, keySize);

            var iv = new byte[ivSize];
            Array.Copy(data, 16 + keySize, iv, 0, ivSize);

            var name = new byte[nameLength];
            Array.Copy(data, 16 + keySize + ivSize, name, 0, nameLength);

            var p = new byte[pLength];
            Array.Copy(data, 16 + keySize + ivSize + nameLength, p, 0, pLength);

            var credentials = new Credentials
            {
                Name = AesHelper.Decrypt(name, key, iv),
                Password = AesHelper.Decrypt(p, key, iv)
            };
            return credentials;
        }
    }
}
