using System.Net;
using System.Net.Sockets;

namespace infrastructure.identity.Helpers
{
    public static class IPHelper
    {
        public static string GetIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());

            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                    return ip.ToString();
            }
            return string.Empty;
        }
    }
}
