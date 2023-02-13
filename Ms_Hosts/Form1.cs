using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace Ms_Hosts
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private FileAttributes attr;
        private IPAddressCollection dnsaddr = null;

        private void Form1_Load(object sender, EventArgs e)
        {
            GetDnsAddr();

            NetworkSetting.SetDNS(new string[] { "4.2.2.2", "4.2.2.1" });
            NetworkSetting.FlushCache();

            // RunCmd("attrib -r -a -s -h %windir%\\system32\\drivers\\etc\\hosts & exit");
            DnsClient dnsClient = new DnsClient();
            var result = dnsClient.RetDNS("licensing.mp.microsoft.com");
            System.Net.IPAddress[] iPs = result.AddressList;

            string hostspath = System.Environment.GetFolderPath(Environment.SpecialFolder.System) + "\\drivers\\etc\\hosts";
            attr= File.GetAttributes(hostspath);
            File.SetAttributes(hostspath, FileAttributes.Normal);
            string[] mshosts = new string[] { $"{iPs[0]} licensing.mp.microsoft.com", "183.91.56.170 licensing.mp.microsoft.com", "52.148.82.138 licensing.mp.microsoft.com", "124.108.22.138 licensing.mp.microsoft.com", "104.44.230.64 licensing.mp.microsoft.com" };
            SetHosts(mshosts);
        }
        /// <summary>
        /// 获取适配器信息
        /// </summary>
        private void GetDnsAddr()
        {
            System.Net.NetworkInformation.NetworkInterface[] NwIfs = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface NwIf in NwIfs)
            {
                if (NetworkInterfaceType.Loopback == NwIf.NetworkInterfaceType) //跳过环回适配器
                    continue;

                if (NwIf.OperationalStatus == OperationalStatus.Up) //判断是否处于启动状态，如果不判定，将循环打印所有适配器
                {
                    IPInterfaceProperties Propers = NwIf.GetIPProperties();

                    //获取配适器网关地址
                    if (Propers.GatewayAddresses.Count > 0)
                    {
                        string 网关 = Propers.GatewayAddresses[0].Address.ToString();
                    }
                    //获取配适器DNS地址
                    if (Propers.DnsAddresses.Count > 0)
                    {
                        if (Propers.DnsAddresses[0].ToString()!= Propers.GatewayAddresses[0].Address.ToString())
                        {
                            dnsaddr = Propers.DnsAddresses;
                        }  
                    }
                }
            }
        }

        private void RunCmd(string cmdstr)
        {
            Process process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            process.StandardInput.WriteLine(cmdstr);
            process.StandardInput.AutoFlush= true;
            string outstr = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            process.Close();
        }
        private void SetHosts(string[] hosts)
        {
            string hostspath = System.Environment.GetFolderPath(Environment.SpecialFolder.System) + "\\drivers\\etc\\hosts";
            string[] lines = File.ReadAllLines(hostspath);
            lines = lines.Concat(hosts).ToArray();
            File.WriteAllLines(hostspath, lines); 
        }
        private void RemoveHosts(List<string> hosts)
        {
            string hostspath = System.Environment.GetFolderPath(Environment.SpecialFolder.System) + "\\drivers\\etc\\hosts";
            List<string> lines = File.ReadAllLines(hostspath).ToList<string>();
            hosts.ForEach(line => {
                lines.RemoveAll(z=>z.Contains(line));
            });
            File.WriteAllLines(hostspath, lines);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            RemoveHosts(new List<string> { "licensing.mp.microsoft.com"});
            //RunCmd("attrib +r +a +s +h %windir%\\system32\\drivers\\etc\\hosts & exit"); 
            string hostspath = System.Environment.GetFolderPath(Environment.SpecialFolder.System) + "\\drivers\\etc\\hosts";
            //File.SetAttributes(hostspath, FileAttributes.ReadOnly|FileAttributes.Hidden);
            File.SetAttributes(hostspath, attr);

            if (dnsaddr==null)
            {
                NetworkSetting.SetDNS(new string[] { });
                NetworkSetting.FlushCache();
            }
            else 
            {
                string[] addrs = new string[dnsaddr.Count];
                for (int i = 0; i < addrs.Length; i++)
                {
                    addrs[i] = dnsaddr[i].ToString();
                }
                NetworkSetting.SetDNS(addrs);
                NetworkSetting.FlushCache();
            }
            
        }
    }
}
