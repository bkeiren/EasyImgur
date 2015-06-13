using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyImgur
{
    static class CredentialsHelper
    {
        public struct Credentials
        {
            public string name;
            public string p;
        }

        public static void StoreCredentials( string _Name, string _P )
        {
            using (System.Security.Cryptography.Aes myAes = System.Security.Cryptography.Aes.Create())
            {
                byte[] key = myAes.Key;
                byte[] iv = myAes.IV;

                byte[] encryptedName = AesHelper.EncryptStringToBytes_Aes(_Name, key, iv);
                byte[] encryptedP = AesHelper.EncryptStringToBytes_Aes(_P, key, iv);

                // Format: 
                // [4 bytes: key size in bytes]
                // [4 bytes: iv size in bytes]
                // [4 bytes: name size]
                // [4 bytes: p size]
                // [n bytes: key (n==keysize)]
                // [n bytes: iv (n==ivsize)]
                // [n bytes: name (n==namesize)]
                // [n bytes: p (n==psize)]

                byte[] finalData = new byte[16 + key.Length + iv.Length + encryptedName.Length + encryptedP.Length];

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

            byte[] key = new byte[keySize];
            Array.Copy(data, 16, key, 0, keySize);

            byte[] iv = new byte[ivSize];
            Array.Copy(data, 16 + keySize, iv, 0, ivSize);

            byte[] name = new byte[nameLength];
            Array.Copy(data, 16 + keySize + ivSize, name, 0, nameLength);

            byte[] p = new byte[pLength];
            Array.Copy(data, 16 + keySize + ivSize + nameLength, p, 0, pLength);

            Credentials credentials = new Credentials();
            credentials.name = AesHelper.DecryptStringFromBytes_Aes(name, key, iv);
            credentials.p = AesHelper.DecryptStringFromBytes_Aes(p, key, iv);
            return credentials;
        }
    }
}
