using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Ms_Hosts
{
    public class PingHelp
    {
        /// <summary>
        /// ping
        /// </summary>
        /// <param name="host">域名或ip</param>
        /// <param name="echoNum">ping次数</param>
        /// <returns></returns>
        public static double PingTimeAverage(string host, int echoNum)
        {
            long totalTime = 0;
            int timeout = 120;
            Ping pingSender = new Ping();

            for (int i = 0; i < echoNum; i++)
            {
                PingReply reply = pingSender.Send(host, timeout);
                if (reply.Status == IPStatus.Success)
                {
                    totalTime += reply.RoundtripTime;
                }
            }
            return totalTime / echoNum;
        }
        /// <summary>
        /// ping
        /// </summary>
        /// <param name="host">域名或ip</param>
        /// <param name="echoNum">ping次数</param>
        /// <returns></returns>
        public static bool PingAverage(string host, int echoNum)
        {
            int timeout = 120;
            Ping pingSender = new Ping();

            for (int i = 0; i < echoNum; i++)
            {
                PingReply reply = pingSender.Send(host, timeout);
                if (reply.Status == IPStatus.Success)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
