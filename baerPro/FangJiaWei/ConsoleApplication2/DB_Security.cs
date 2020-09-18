using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ConsoleApplication2
{
    /// <summary>
    /// 从单点登录中拷贝的加解密算法
    /// </summary>
    public class DB_Security
    {
        #region 加减密算法二(用此算法)
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="strEncrypt">被加密的字符串</param>
        /// <param name="encryptionKey">密钥</param>
        /// /// <param name="encoding">编码格式</param>
        /// <returns></returns>
        public static string DESEnCode(string strEncrypt, string encryptionKey, string encoding)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray = Encoding.GetEncoding(encoding).GetBytes(strEncrypt);

            //建立加密对象的密钥和偏移量    
            //原文使用ASCIIEncoding.ASCII方法的GetBytes方法    
            //使得输入密码必须输入英文文本    
            des.Key = ASCIIEncoding.ASCII.GetBytes(encryptionKey);
            des.IV = ASCIIEncoding.ASCII.GetBytes(encryptionKey);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);

            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();

            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            return ret.ToString();
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="pToDecrypt">被解密的字符串</param>
        /// <param name="sKey"></param>
        /// <returns></returns>
        public static string DESDeCode(string strDecrypt, string encryptionKey, string encoding)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            byte[] inputByteArray = new byte[strDecrypt.Length / 2];
            for (int x = 0; x < strDecrypt.Length / 2; x++)
            {
                int i = (Convert.ToInt32(strDecrypt.Substring(x * 2, 2), 16));
                inputByteArray[x] = (byte)i;
            }

            des.Key = ASCIIEncoding.ASCII.GetBytes(encryptionKey);
            des.IV = ASCIIEncoding.ASCII.GetBytes(encryptionKey);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();

            return System.Text.Encoding.GetEncoding(encoding).GetString(ms.ToArray());
        }
        #endregion
    }
}
