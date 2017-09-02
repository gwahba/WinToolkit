using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace WinToolkit
{
	public class InternetConnectionCheker
	{
		private static bool InternalIsConnectionAvailable(int timeout)
		{
			const string host = "google.com";

			var ping = new Ping();
			var buffer = new byte[32];
			var pingOptions = new PingOptions();

			try
			{
				var reply = ping.Send(host, timeout, buffer, pingOptions);
				return (reply != null && reply.Status == IPStatus.Success);
			}
			catch (Exception)
			{
				return false;
			}
		}

		public static bool IsConnectionAvailable(int retryCount = 0, int timeoutInMSec = 3000)
		{
			bool status = false;
            do
            {
                status = InternalIsConnectionAvailable(timeoutInMSec);
            } while (retryCount-- > 0);

			return status;
		}
	}
    public class NetworkInterfaceCardAttributes
    {
        public static string GetMacAddress()
        {
            // Looping through all interfaces to get the active one ..
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                // Only consider Ethernet network interfaces
                if ((nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet ||
                    nic.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
                    && (nic.OperationalStatus == OperationalStatus.Up))
                {
                    return nic.GetPhysicalAddress().ToString();
                }
            }
            return null;
        }
    }
}
