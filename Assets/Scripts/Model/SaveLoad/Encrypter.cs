using System.Xml.Serialization;
using System.Xml;
using System.IO;
using System.Security.Cryptography;
using System.Security;
using System.Text;
using System;

namespace Scripts.Game.Serialization {

    /// <summary>
    /// Modified slightly from
    /// https://www.codeproject.com/Articles/19538/Encrypt-Decrypt-String-using-DES-in-C
    /// </summary>
    public static class Encrypter {

        /// <span class="code-SummaryComment"><summary></span>
        /// Encrypt a string.
        /// <span class="code-SummaryComment"></summary></span>
        /// <span class="code-SummaryComment"><param name="originalString">The original string.</param></span>
        /// <span class="code-SummaryComment"><returns>The encrypted string.</returns></span>
        /// <span class="code-SummaryComment"><exception cref="ArgumentNullException">This exception will be </span>
        /// thrown when the original string is null or empty.<span class="code-SummaryComment"></exception></span>
        public static string Encrypt(string originalString, string key) {
            if (String.IsNullOrEmpty(originalString)) {
                throw new ArgumentNullException
                       ("The string which needs to be encrypted can not be null.");
            }
            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            MemoryStream memoryStream = new MemoryStream();
            byte[] bytes = ASCIIEncoding.ASCII.GetBytes(key);
            CryptoStream cryptoStream = new CryptoStream(memoryStream,
                cryptoProvider.CreateEncryptor(bytes, bytes), CryptoStreamMode.Write);
            StreamWriter writer = new StreamWriter(cryptoStream);
            writer.Write(originalString);
            writer.Flush();
            cryptoStream.FlushFinalBlock();
            writer.Flush();
            return Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
        }

        /// <span class="code-SummaryComment"><summary></span>
        /// Decrypt an encrypted string.
        /// <span class="code-SummaryComment"></summary></span>
        /// <span class="code-SummaryComment"><param name="cryptedString">The crypted string.</param></span>
        /// <span class="code-SummaryComment"><returns>The decrypted string.</returns></span>
        /// <span class="code-SummaryComment"><exception cref="ArgumentNullException">This exception will be thrown </span>
        /// when the crypted string is null or empty.<span class="code-SummaryComment"></exception></span>
        public static string Decrypt(string cryptedString, string key) {
            if (String.IsNullOrEmpty(cryptedString)) {
                throw new ArgumentNullException
                   ("The string which needs to be decrypted can not be null.");
            }
            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            MemoryStream memoryStream = new MemoryStream
                    (Convert.FromBase64String(cryptedString));
            byte[] bytes = ASCIIEncoding.ASCII.GetBytes(key);
            CryptoStream cryptoStream = new CryptoStream(memoryStream,
                cryptoProvider.CreateDecryptor(bytes, bytes), CryptoStreamMode.Read);
            StreamReader reader = new StreamReader(cryptoStream);
            return reader.ReadToEnd();
        }

    }
}