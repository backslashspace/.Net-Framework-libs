using Org.BouncyCastle.Crypto.Fips;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Utilities.IO;
using System;
using System.IO;
using Pcg;

namespace BSS.Encryption.Fips
{
    public static class xAES_Fips
    {
        public static class CTR
        {
            /// <returns>Byte[] (cipher text + iv)</returns>
            public static Byte[] Encrypt(Byte[] plainBytes, ref Byte[] key)
            {
                Byte[] iv = new Byte[16];
                PcgRandom random = new();
                random.NextBytes(iv);

                FipsAes.Key iKey = new(key);
                IBlockCipherService provider = CryptoServicesRegistrar.CreateService(iKey);

                using MemoryOutputStream outputCipherText = new();

                ICipherBuilder<IParameters<Algorithm>> encryptorBuilder = provider.CreateEncryptorBuilder(FipsAes.Ctr.WithIV(iv));
                ICipher encryptor = encryptorBuilder.BuildCipher(outputCipherText);

                using (Stream encryptorStream = encryptor.Stream)
                {
                    encryptorStream.Write(plainBytes, 0, plainBytes.Length);
                }

                Byte[] cipherText = outputCipherText.ToArray();

                Byte[] cipherTextPlusIV = new Byte[cipherText.Length + 16];

                Array.Copy(cipherText, cipherTextPlusIV, cipherText.Length);
                Array.Copy(iv, 0, cipherTextPlusIV, cipherText.Length, iv.Length);

                return cipherTextPlusIV;
            }

            public static Byte[] Decrypt(ref Byte[] cipherTextPlusIV, ref Byte[] key)
            {
                Byte[] cipherText = new Byte[cipherTextPlusIV.Length - 16];
                Byte[] iv = new Byte[16];

                Array.Copy(cipherTextPlusIV, cipherText, cipherTextPlusIV.Length - 16);
                Array.Copy(cipherTextPlusIV, cipherTextPlusIV.Length - 16, iv, 0, 16);

                FipsAes.Key iKey = new(key);

                IBlockCipherService provider = CryptoServicesRegistrar.CreateService(iKey);
                ICipherBuilder<IParameters<Algorithm>> decryptorBuilder = provider.CreateDecryptorBuilder(FipsAes.Ctr.WithIV(iv));

                ICipher decryptor = decryptorBuilder.BuildCipher(new MemoryInputStream(cipherText));

                using Stream decryptorStream = decryptor.Stream;

                return Streams.ReadAll(decryptorStream);
            }
        }
    }
}