using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace OpenRepo.Handler
{
    public class WindowsRegistry : IRegistryProvider
    {
        public bool SetHandlerForProtocol(HandlerRegistration handler)
        {
            Trace.WriteLine(string.Format("Attempting to register as {0}:// handler",handler.ProtocolName));
            RegistryKey softwareClasses =
                Registry.CurrentUser.OpenSubKey("Software\\Classes", true).CreateSubKey(handler.ProtocolName);
            softwareClasses.SetValue("", "URL:" + handler.Description);
            softwareClasses.SetValue("URL Protocol", "");


            RegistryKey shell = softwareClasses.CreateSubKey("shell");
            RegistryKey open = shell.CreateSubKey("open");
            RegistryKey command = open.CreateSubKey("command");
            command.SetValue("", System.Reflection.Assembly.GetEntryAssembly().Location + " %1");


            RegistryKey defaultIcon = softwareClasses.CreateSubKey("DefaultIcon");
            defaultIcon.SetValue("", Path.GetFileName(string.IsNullOrEmpty(handler.DefaultIconPath) ? handler.HandlerPath : handler.DefaultIconPath));
            return
                Registry.CurrentUser.OpenSubKey("Software\\Classes", true)
                    .GetSubKeyNames()
                    .Contains(handler.ProtocolName);

        }

        public bool RemoveHandlerForProtocol(string ProtocolName)
        {
            throw new NotImplementedException();
        }
    }
}
