using Org.BouncyCastle.Crypto;
using System;

namespace BSS.Encryption.Fips
{
    public static partial class xFips
    {
        public static void SetApprovedOnlyMode(Boolean approvalOnlyMode = true)
        {
            CryptoServicesRegistrar.SetApprovedOnlyMode(approvalOnlyMode);
        }
    }
}