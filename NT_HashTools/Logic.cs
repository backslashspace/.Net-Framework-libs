using System;
using System.IO;
using System.Security.Cryptography;

namespace BSS.HashTools
{
    public static partial class xHash
    {
        private static String ComputeFileHash(ref String filePath, ref Algorithm algorithm)
        {
            FileStream fileStream;

            try
            {
                fileStream = File.OpenRead(filePath);
            }
            catch (Exception ex)
            {
                throw new FileLoadException($"Unable to open file :{filePath}\n{ex.Message}");
            }

            Byte[] rawHash;

            switch (algorithm)
            {
                case Algorithm.MD5:
                    MD5 md5 = MD5.Create();
                    rawHash = md5.ComputeHash(fileStream);
                    md5.Clear();
                    md5.Dispose();
                    break;

                case Algorithm.SHA1:
                    SHA1 sha1 = SHA1.Create();
                    rawHash = sha1.ComputeHash(fileStream);
                    sha1.Clear();
                    sha1.Dispose();
                    break;

                case Algorithm.SHA256:
                    SHA256 sha256 = SHA256.Create();
                    rawHash = sha256.ComputeHash(fileStream);
                    sha256.Clear();
                    sha256.Dispose();
                    break;

                case Algorithm.SHA384:
                    SHA384 sha384 = SHA384.Create();
                    rawHash = sha384.ComputeHash(fileStream);
                    sha384.Clear();
                    sha384.Dispose();
                    break;

                case Algorithm.SHA512:
                    SHA512 sha512 = SHA512.Create();
                    rawHash = sha512.ComputeHash(fileStream);
                    sha512.Clear();
                    sha512.Dispose();
                    break;

                case Algorithm.RIPEMD160:
                    RIPEMD160 ripemd160 = RIPEMD160.Create();
                    rawHash = ripemd160.ComputeHash(fileStream);
                    ripemd160.Clear();
                    ripemd160.Dispose();
                    break;

                default:
                    throw new Exception("invalid hash algorithm: " + algorithm);
            }

            fileStream.Close();
            fileStream.Dispose();

            return BitConverter.ToString(rawHash).Replace("-", String.Empty).ToLower();
        }
    }
}