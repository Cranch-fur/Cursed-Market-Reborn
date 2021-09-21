// THIS CLASS IS UNUSED STARTING FROM 3.6.0.4!!!



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
        private const string SAVEFILE_AESKEY = "5BCC2D6A95D4DF04A005504E59A9B36E"; // <= SaveFile AES KEY in HEX format
        private const string SAVEFILE_INNER = "DbdDAQEB";
        private const string SAVEFILE_OUTER = "DbdDAgAC";

        public static string EncryptSavefile(string input)
        {
            byte[] input_asbyte = Encoding.Unicode.GetBytes(input);
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (ZOutputStream zoutputStream = new ZOutputStream(memoryStream, -1))
                {
                    zoutputStream.Write(input_asbyte, 0, input_asbyte.Length);
                    zoutputStream.Flush();
                }

                string saveFile = Convert.ToBase64String(PaddingWithNumber(memoryStream.ToArray(), input_asbyte.Length));
                int _pad = 16 - ((SAVEFILE_INNER.Length + saveFile.Length) % 16);
                saveFile = SAVEFILE_INNER + saveFile.PadRight(saveFile.Length + _pad, '\u0001');
                string output = null;
                foreach (char c in saveFile)
                {
                    output += (char)(c - '\u0001');
                }
                return SAVEFILE_OUTER + Raw_Encrypt(output);
            }
        }
        private static string Raw_Encrypt(string input)
        {
            byte[] input_asbyte = Encoding.UTF8.GetBytes(input);
            ICryptoTransform transform = new RijndaelManaged
            {
                Mode = CipherMode.ECB,
                Padding = PaddingMode.Zeros
            }.CreateEncryptor(Encoding.ASCII.GetBytes(SAVEFILE_AESKEY), null);


            MemoryStream memoryStream = new MemoryStream(input_asbyte);
            CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Read);
            byte[] array = new byte[input_asbyte.Length];
            int length = cryptoStream.Read(array, 0, array.Length);
            memoryStream.FlushAsync(); memoryStream.Close();
            cryptoStream.Flush(); cryptoStream.Close();
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
