using DnsClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ms_Hosts
{
    public class DnsClient
    {
        /// <summary>
        /// dns 解析
        /// </summary>
        /// <param name="domain">域名</param>
        /// <param name="dns">指定解析dns</param>
        /// <returns></returns>
        public IPHostEntry RetDNS(string domain, string dns="4.2.2.2")
        {
            var client = new LookupClient(IPAddress.Parse(dns), 53);
           // var client = new LookupClient(IPAddress.Parse(dns));
            var result = client.GetHostEntryAsync(domain);
            result.Wait();
            return result.Result;
        }
    }
}
