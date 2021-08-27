//Original script by Marsik

using Org.BouncyCastle.Utilities.Zlib;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Cursed_Market_Reborn
{
    public static class SaveFile
    {
        private const string SAVEFILE_AESKEY = "5BCC2D6A95D4DF04A005504E59A9B36E";
        private const string SAVEFILE_INNER = "DbdDAQEB";
        private const string SAVEFILE_OUTER = "DbdDAgAC";

        public static string EncryptSavefile(string content)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(content);
            MemoryStream memoryStream = new MemoryStream();
            ZOutputStream zoutputStream = new ZOutputStream(memoryStream, -1);
            zoutputStream.Write(bytes, 0, bytes.Length);
            zoutputStream.Flush();
            zoutputStream.Finish();
            string text = Convert.ToBase64String(PaddingWithNumber(memoryStream.ToArray(), bytes.Length));
            int num = SAVEFILE_INNER.Length + text.Length;
            int num2 = 16 - num % 16;
            text = SAVEFILE_INNER + text.PadRight(text.Length + num2, '\u0001');
            string text2 = "";
            string text3 = text;
            foreach (char c in text3)
            {
                text2 += (char)(c - '\u0001');
            }
            return SAVEFILE_OUTER + Raw_Encrypt(text2, SAVEFILE_AESKEY);
        }

        private static string Raw_Encrypt(string text, string key)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            byte[] bytes2 = Encoding.ASCII.GetBytes(key);
            ICryptoTransform transform = new RijndaelManaged
            {
                Mode = CipherMode.ECB,
                Padding = PaddingMode.Zeros
            }.CreateEncryptor(bytes2, null);
            MemoryStream memoryStream = new MemoryStream(bytes);
            CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Read);
            byte[] array = new byte[bytes.Length];
            int length = cryptoStream.Read(array, 0, array.Length);
            memoryStream.Close();
            cryptoStream.Close();
            return Convert.ToBase64String(array, 0, length);
        }

        private static byte[] PaddingWithNumber(byte[] buffer, int num)
        {
            byte[] bytes = BitConverter.GetBytes(num);
            byte[] array = new byte[bytes.Length + buffer.Length];
            Buffer.BlockCopy(bytes, 0, array, 0, bytes.Length);
            Buffer.BlockCopy(buffer, 0, array, bytes.Length, buffer.Length);
            return array;
        }
    }
}
