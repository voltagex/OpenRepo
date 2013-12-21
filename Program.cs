using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenRepo
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args[0].Contains("register-github"))
            {
                RegistryKey fakeHubRegistry = Registry.CurrentUser.OpenSubKey("Software\\Classes",true).CreateSubKey("github-windows");
                fakeHubRegistry.SetValue("", "URL:Github for Windows");
                fakeHubRegistry.SetValue("URL Protocol", "");

                //RegistryKey defaultIcon = helpDesk.CreateSubKey("DefaultIcon");
                //defaultIcon.SetValue("", Path.GetFileName(Application.ExecutablePath));

                RegistryKey shell = fakeHubRegistry.CreateSubKey("shell");
                RegistryKey open = shell.CreateSubKey("open");
                RegistryKey command = open.CreateSubKey("command");
                command.SetValue("", System.Reflection.Assembly.GetEntryAssembly().Location + " %1");
            }
            if (!args[0].StartsWith("github-windows://"))
            {
                Environment.Exit(1);
            }

            else
            {
                //for example, github-windows://openRepo/https://github.com/voltagex/junkcode
               var split = args[0].ToLowerInvariant().Split(new string[] { "github-windows://openrepo/", }, StringSplitOptions.RemoveEmptyEntries); //there has to be a nicer way to do this
               Process.Start(new ProcessStartInfo() {UseShellExecute = true,FileName="sourcetree://cloneRepo/"+split[0]});
            }


        }
    }
}
