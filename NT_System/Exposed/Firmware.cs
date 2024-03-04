using System;
using System.Management.Automation;

namespace BSS.System.Windows
{
    /// <summary></summary>
    public static partial class xFirmware
    {
        /// <summary></summary>
        public enum FirmwareType
        {
            /// <summary></summary>
            UEFI = 0,
            /// <summary></summary>
            Legacy_BIOS = 1
        }

        /// <summary>Checks if the System is running on UEFI or legacy BIOS</summary>
        public static FirmwareType GetFWType()
        {
            String env = PSQuery("$env:firmware_type");

            return env switch
            {
                "uefi" => FirmwareType.UEFI,
                "legacy" => FirmwareType.Legacy_BIOS,
                _ => throw new InvalidPowerShellStateException("Returned unexpected data: " + env)
            };
        }

        /// <summary>Checks if Secure Boot is enabled on the system</summary>
        public static Boolean GetSecureBootStatus()
        {
            String env = PSQuery("Confirm-SecureBootUEFI");

            return env switch
            {
                "true" => true,
                "false" => false,
                _ => throw new InvalidPowerShellStateException("Returned unexpected data: " + env)
            };
        }
    }
}