using Org.BouncyCastle.Crypto.Fips;
using Org.BouncyCastle.Crypto.Utilities;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;
using Pcg;
using System;

namespace BSS.Encryption.Fips
{
    public static partial class xFips
    {
        public static FipsSecureRandom GenerateSecureRandom(FipsDrbgType fipsSourceClass = FipsDrbgType.Sha512HMac)
        {
            PcgRandom pcgRandom = new();

            Byte[] personalizationStringBytes = new Byte[420];
            pcgRandom.NextBytes(personalizationStringBytes);

            pcgRandom = new();

            Byte[] nonce = new Byte[32];
            pcgRandom.NextBytes(nonce);

            return GenerateSecureRandom(ref personalizationStringBytes, ref nonce, ref fipsSourceClass);
        }

        public static FipsSecureRandom GenerateSecureRandom(ref Byte[] personalizationStringBytes, ref Byte[] nonce, ref FipsDrbgType fipsSourceClass)
        {
            BasicEntropySourceProvider entropySource = new(new SecureRandom(), true);

            IDrbgBuilder<FipsSecureRandom> drbgBuilder = fipsSourceClass switch
            {
                FipsDrbgType.Sha1 => CryptoServicesRegistrar.CreateService(FipsDrbg.Sha1).FromEntropySource(entropySource),
                FipsDrbgType.Sha224 => CryptoServicesRegistrar.CreateService(FipsDrbg.Sha224).FromEntropySource(entropySource),
                FipsDrbgType.Sha256 => CryptoServicesRegistrar.CreateService(FipsDrbg.Sha256).FromEntropySource(entropySource),
                FipsDrbgType.Sha384 => CryptoServicesRegistrar.CreateService(FipsDrbg.Sha384).FromEntropySource(entropySource),
                FipsDrbgType.Sha512 => CryptoServicesRegistrar.CreateService(FipsDrbg.Sha512).FromEntropySource(entropySource),
                FipsDrbgType.Sha512_224 => CryptoServicesRegistrar.CreateService(FipsDrbg.Sha512_224).FromEntropySource(entropySource),
                FipsDrbgType.Sha512_256 => CryptoServicesRegistrar.CreateService(FipsDrbg.Sha512_256).FromEntropySource(entropySource),
                FipsDrbgType.Sha1HMac => CryptoServicesRegistrar.CreateService(FipsDrbg.Sha1HMac).FromEntropySource(entropySource),
                FipsDrbgType.Sha224HMac => CryptoServicesRegistrar.CreateService(FipsDrbg.Sha224HMac).FromEntropySource(entropySource),
                FipsDrbgType.Sha256HMac => CryptoServicesRegistrar.CreateService(FipsDrbg.Sha256HMac).FromEntropySource(entropySource),
                FipsDrbgType.Sha384HMac => CryptoServicesRegistrar.CreateService(FipsDrbg.Sha384HMac).FromEntropySource(entropySource),
                FipsDrbgType.Sha512HMac => CryptoServicesRegistrar.CreateService(FipsDrbg.Sha512HMac).FromEntropySource(entropySource),
                FipsDrbgType.Sha512_224HMac => CryptoServicesRegistrar.CreateService(FipsDrbg.Sha512_224HMac).FromEntropySource(entropySource),
                FipsDrbgType.Sha512_256HMac => CryptoServicesRegistrar.CreateService(FipsDrbg.Sha512_256HMac).FromEntropySource(entropySource),
                FipsDrbgType.CtrTripleDes168 => CryptoServicesRegistrar.CreateService(FipsDrbg.CtrTripleDes168).FromEntropySource(entropySource),
                FipsDrbgType.CtrAes128 => CryptoServicesRegistrar.CreateService(FipsDrbg.CtrAes128).FromEntropySource(entropySource),
                FipsDrbgType.CtrAes192 => CryptoServicesRegistrar.CreateService(FipsDrbg.CtrAes192).FromEntropySource(entropySource),
                FipsDrbgType.CtrAes256 => CryptoServicesRegistrar.CreateService(FipsDrbg.CtrAes256).FromEntropySource(entropySource),

                _ => throw new InvalidOperationException("enum was modified?")
            };

            drbgBuilder.SetSecurityStrength(256);
            drbgBuilder.SetEntropyBitsRequired(256);
            drbgBuilder.SetPersonalizationString(personalizationStringBytes);

            return drbgBuilder.Build(nonce, true);
        }

        //

        public enum FipsDrbgType
        {
            Sha1 = 0,
            Sha224 = 1,
            Sha256 = 2,
            Sha384 = 3,
            Sha512 = 4,
            Sha512_224 = 5,
            Sha512_256 = 6,
            Sha1HMac = 7,
            Sha224HMac = 8,
            Sha256HMac = 9,
            Sha384HMac = 10,
            Sha512HMac = 11,
            Sha512_224HMac = 12,
            Sha512_256HMac = 13,
            CtrTripleDes168 = 14,
            CtrAes128 = 15,
            CtrAes192 = 16,
            CtrAes256 = 17,
        }
    }
}