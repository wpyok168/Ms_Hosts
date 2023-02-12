using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
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

        private void Form1_Load(object sender, EventArgs e)
        {
           // RunCmd("attrib -r -a -s -h %windir%\\system32\\drivers\\etc\\hosts & exit");
            string hostspath = System.Environment.GetFolderPath(Environment.SpecialFolder.System) + "\\drivers\\etc\\hosts";
            File.SetAttributes(hostspath, FileAttributes.Normal);
            string[] mshosts = new string[] { "183.91.56.170 licensing.mp.microsoft.com", "52.148.82.138 licensing.mp.microsoft.com", "124.108.22.138 licensing.mp.microsoft.com", "104.44.230.64 licensing.mp.microsoft.com" };
            SetHosts(mshosts);
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
            File.SetAttributes(hostspath, FileAttributes.ReadOnly);
        }
    }
}
