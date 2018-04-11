using System.Net;

namespace CafeT.Text
{
    public static class IpOnText
    {
        public static IPAddress ToIp(this string text)
        {
            System.Net.IPAddress _ipAddress = System.Net.IPAddress.Parse(text);
            return _ipAddress;
        }
    }
}
