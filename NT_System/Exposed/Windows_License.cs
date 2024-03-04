using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace BSS.System.Windows
{
    /// <summary></summary>
    public static class xWinLicense
    {
        /// <summary>Retrieves the Windows activation status using the WMI</summary>
        public static Boolean GetStatus(out String licenseMessage)
        {
            Byte status = QueryStatus();

            licenseMessage = LicenseStatusToString(status);

            if (status == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>Maps the WMI license status to its representative string</summary>
        public static String LicenseStatusToString(Byte status)
        {
            return (status) switch
            {
                0 => "Unlicensed: 0",
                1 => "Licensed: 1",
                2 => "OOBGrace: 2",
                3 => "OOTGrace: 3",
                4 => "NonGenuineGrace: 4",
                5 => "Notification: 5",
                6 => "ExtendedGrace: 6",
                _ => "unknown_status: error",
            };
        }

        /// <summary>Retrieves the Windows activation status as byte using WMI</summary>
        public static Byte QueryStatus()
        {
            SelectQuery licenseQuery = new(@"SELECT LicenseStatus
                                             FROM SoftwareLicensingProduct
                                             WHERE PartialProductKey is not null AND Name LIKE 'Windows%'");

            ManagementObjectSearcher searcher = new(licenseQuery);

            ManagementBaseObject wmiObject = searcher.Get().OfType<ManagementBaseObject>().FirstOrDefault();

            return Byte.Parse($"{wmiObject["LicenseStatus"]}");
        }
    }
}