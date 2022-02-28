﻿using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;

namespace ExtensionMethods
{
    public static class ProcessExtensions
    {
        /// <summary>Computes the MD5 hash formatted as a simple, lowercased string.</summary>
        /// <param name="process">The <see cref="Process"/> instance</param>
        /// <returns>If the file executing <paramref name="process"/> is found: the lowercased, invariant string representing its MD5 hash; otherwise, <see langword="null"/></returns>
        public static string GetMd5Hash(this Process process)
        {
            string exePath = process?.MainModule?.FileName;
            if (!string.IsNullOrEmpty(exePath))
            {
                string md5Hash;
                using (var md5 = MD5.Create())
                {
                    using (var stream = File.Open(exePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        var hash = md5.ComputeHash(stream);
                        md5Hash = BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                    }
                }
                return md5Hash;
            }
            
            return null;
        }
    }
}
