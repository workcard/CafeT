using CafeT.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace CafeT.Objects
{
    public class ComputerObject
    {
        public string Name { set; get; }
        public string Ip { set; get; }
        public string Domain { set; get; }

        public bool IsConnectedToInternet
        {
            get
            {
                Uri url = new Uri("www.google.com");
                string pingurl = string.Format("{0}", url.Host);
                string host = pingurl;
                bool result = false;
                Ping p = new Ping();
                try
                {
                    PingReply reply = p.Send(host, 3000);
                    if (reply.Status == IPStatus.Success)
                        return true;
                }
                catch { }
                return result;
            }
        }

        public bool IsConnectedToUrl(string url)
        {
            bool result = false;
            Ping p = new Ping();
            try
            {
                PingReply reply = p.Send(url, 3000);
                if (reply.Status == IPStatus.Success)
                    return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return result;
        }

        public bool IsConnectedToIp(string ip)
        {
            bool result = false;
            Ping p = new Ping();
            try
            {
                PingReply reply = p.Send(ip.ToIp(), 3000);
                if (reply.Status == IPStatus.Success)
                    return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return result;
        }

        //http://www.ryadel.com/en/asp-net-c-helper-class-to-get-web-server-ip-address-and-other-network-related-methods/
        /// <summary>
        /// Gets all the IP addresses of the server machine hosting the application.
        /// </summary>
        /// <returns>a string array containing all the IP addresses of the server machine</returns>
        public IPAddress[] GetIPAddresses()
        {
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName()); // `Dns.Resolve()` method is deprecated.
            return ipHostInfo.AddressList;
        }

        /// <summary>
        /// Gets the IP address of the server machine hosting the application.
        /// </summary>
        /// <param name="num">if set, it will return the Nth available IP address: if not set, the first available one will be returned.</param>
        /// <returns>the (first available or chosen) IP address of the server machine</returns>
        public IPAddress GetIPAddress(int num = 0)
        {
            return GetIPAddresses()[num];
        }

        /// <summary>
        /// Checks if the given IP address is one of the IP addresses registered to the server machine hosting the application.
        /// </summary>
        /// <param name="ipAddress">the IP Address to check</param>
        /// <returns>TRUE if the IP address is registered, FALSE otherwise</returns>
        public bool HasIPAddress(IPAddress ipAddress)
        {
            return GetIPAddresses().Contains(ipAddress);
        }

        /// <summary>
        /// Checks if the given IP address is one of the IP addresses registered to the server machine hosting the application.
        /// </summary>
        /// <param name="ipAddress">the IP Address to check</param>
        /// <returns>TRUE if the IP address is registered, FALSE otherwise</returns>
        public bool HasIPAddress(string ipAddress)
        {
            return HasIPAddress(IPAddress.Parse(ipAddress));
        }
    }
}
