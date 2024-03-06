using Org.BouncyCastle.Crypto.Fips;
using Org.BouncyCastle.Crypto;
using System;
using System.IO;

namespace BSS.Encryption.Fips
{
    public static class xHMAC_Fips
    {
        public static Byte[] ComputeHMAC_512(ref Byte[] data, ref Byte[] hmacKey)
        {
            IMacFactory<FipsShs.AuthenticationParameters> macFactory = CryptoServicesRegistrar
                .CreateService(new FipsShs.Key(FipsShs.Sha512HMac, hmacKey))
                .CreateMacFactory(FipsShs.Sha512HMac
                .WithMacSize(FipsShs.Sha512HMac.MacSizeInBits));

            IStreamCalculator<IBlockResult> macCalculator = macFactory.CreateCalculator();
            using Stream macStream = macCalculator.Stream;

            macStream.Write(data, 0, data.Length);

            return macCalculator.GetResult().Collect();
        }
    }
}