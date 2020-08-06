using ChecksumFiles.Static;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ChecksumFiles.BusinessLogic
{
  internal  class ChecksumCalculation
    {
        public string CalculateSelectedAlgoritham(string path)
        {
            string selectedAlgoritham = RadioButtonStaticVariables.ChecksumType;
            switch (selectedAlgoritham)
            {
                case "SHA1":
                    return CalculateSHA1(path);
                case "SHA256":
                    return CalculateSHA256(path);
                case "SHA384":
                    return CalculateSHA384(path);
                case "SHA512":
                    return CalculateSHA512(path);
                case "MD5":
                    return CalculateMD5(path);

            }
            return CalculateMD5(path);
        }

        public string CalculateSHA384(string path)
        {
            using (var sha384 = SHA384.Create())
            {
                using (var stream = File.OpenRead(path))
                {
                    var hash = sha384.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }

        public string CalculateSHA1(string path)
        {
            using (var sha1 = SHA1.Create())
            {
                using (var stream = File.OpenRead(path))
                {
                    var hash = sha1.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }

        public string CalculateSHA256(string path)
        {
            using (var sha256 = SHA256.Create())
            {
                using (var stream = File.OpenRead(path))
                {
                    var hash = sha256.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }

        public string CalculateMD5(string filename)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filename))
                {
                    var hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }

        public string CalculateSHA512(string filename)
        {
            using (var sha512 = SHA512.Create())
            {
                using (var stream = File.OpenRead(filename))
                {
                    var hash = sha512.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }
    }
}
