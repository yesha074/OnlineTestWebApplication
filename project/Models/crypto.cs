﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;
using System.Configuration;

namespace project.Models
{
    public class crypto
    {
        public static string Encrypt(string toEncrypt, bool useHasing)
        {
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);
            System.Configuration.AppSettingsReader settingsReader = new AppSettingsReader();
            string key = (string)settingsReader.GetValue("SecurityKey",typeof(string));
            if(useHasing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                hashmd5.Clear();
            }
            else
                keyArray = UTF8Encoding.UTF8.GetBytes(key);


           TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
           tdes.Key = keyArray;
           tdes.Mode = CipherMode.ECB;
           tdes.Padding = PaddingMode.PKCS7;
           ICryptoTransform cTransform = tdes.CreateEncryptor();
           byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
           tdes.Clear();
           return Convert.ToBase64String(resultArray, 0, resultArray.Length);
            
        }
    }
}