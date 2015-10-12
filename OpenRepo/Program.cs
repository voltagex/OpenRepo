using System.Configuration;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Reflection;
using OpenRepo.Handler;

namespace OpenRepo
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("OpenRepo github-windows:// shim");
                Console.ForegroundColor = ConsoleColor.White;

                Console.WriteLine("Launching this with an argument of github-windows://<path> should open SourceTree to clone the repo");
                Console.WriteLine("To set this up, run openrepo.exe /register-github (as current user, or Administrator)");

                Environment.Exit(1);
            }

            if (args[0].Contains("register-github"))
            {
               var registration = new HandlerRegistration() { Description = "Github for Windows (OpenRepo 1.01 Shim)", ProtocolName = "github-windows", HandlerPath = Assembly.GetEntryAssembly().Location };
               var provider = new WindowsRegistry();

                provider.SetHandlerForProtocol(registration);
                Console.WriteLine("Registered github-windows:// protocol handler");
                Environment.Exit(0);
            }

            if (!args[0].StartsWith("github-windows://"))
            {
                Environment.Exit(1);
            }

            else
            {
                string[] split = args[0].ToLowerInvariant().Split(new[] { "github-windows://openrepo/" }, StringSplitOptions.RemoveEmptyEntries); //there has to be a nicer way to do this
                bool useShellExecute = true; //if I'm starting another handler, I can use shellexecute. If I'm starting an exe out of PATH I can't.
                string handler = "sourcetree://cloneRepo/"; //default config, try using SourceTree
                string commandLine = split[0];
                //for example, github-windows://openRepo/https://github.com/voltagex/junkcode

                
                var info = new ProcessStartInfo()
                {
                    UseShellExecute = useShellExecute,
                    FileName = handler,
                    Arguments = commandLine,
                    RedirectStandardOutput = false
                };

                Process.Start(info);
            }
        }
    }
}
