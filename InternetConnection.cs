using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;

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
    public class NetworkInterfaceAttributes
    {
        public static string GetMacAddress()
        {
            // Looping through all interfaces to get the active one ..
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                // Considering active Ethernet and WiFi network interface/adapter
                if ((nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet ||
                    nic.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
                    && (nic.OperationalStatus == OperationalStatus.Up))
                {
                    return nic.GetPhysicalAddress().ToString();
                }
            }
            return null;
        }
        public static List<int> GetActivePortNumbers()
        {
            List<int> portNumbers = new List<int>();

            using (Process process = new Process())
            {
                ProcessStartInfo processInfo = new ProcessStartInfo();
                processInfo.WindowStyle = ProcessWindowStyle.Hidden;
                processInfo.RedirectStandardError = true;
                processInfo.RedirectStandardOutput = true;
                processInfo.RedirectStandardInput = true;
                processInfo.UseShellExecute = false;
                processInfo.Arguments = "-a -n -o";
                processInfo.FileName = "netstat.exe";
                processInfo.CreateNoWindow = true;
                process.StartInfo = processInfo;
                process.Start();

                StreamReader stdOutput = process.StandardOutput;

                // Splitting the rows of the result into a grid-like style
                string[] portsResultSet = Regex.Split(stdOutput.ReadToEnd(), "\r\n");
                foreach (string portRecord in portsResultSet)
                {
                    string[] tokens = Regex.Split(portRecord, "\\s+");
                    if (tokens.Length > 4 && (tokens[1].Equals("UDP") || tokens[1].Equals("TCP")))
                        portNumbers.Add(int.Parse(Regex.Replace(tokens[2], @"\[(.*?)\]", "1.1.1.1").Split(':')[1]));
                }
            }
            return portNumbers;
        }
    }
}
