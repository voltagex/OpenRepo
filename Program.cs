using System.Configuration;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;
using System;
using System.Diagnostics;
namespace OpenRepo
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Environment.Exit(1); //TODO: usage message
            }

            if (args[0].Contains("register-github"))
            {
                Console.WriteLine("Attempting to register as github-windows:// handler");
                RegistryKey fakeHubRegistry = Registry.CurrentUser.OpenSubKey("Software\\Classes", true).CreateSubKey("github-windows");
                fakeHubRegistry.SetValue("", "URL:Github for Windows");
                fakeHubRegistry.SetValue("URL Protocol", "");

                //RegistryKey defaultIcon = fakeHubRegistry.CreateSubKey("DefaultIcon");
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
                string[] split = args[0].ToLowerInvariant().Split(new[] { "github-windows://openrepo/" }, StringSplitOptions.RemoveEmptyEntries); //there has to be a nicer way to do this
                bool useShellExecute = true; //if I'm starting another handler, I can use shellexecute. If I'm starting an exe out of PATH I can't.
                string handler = "sourcetree://cloneRepo/"; //default config, try using SourceTree
                string commandLine = split[0];
                //for example, github-windows://openRepo/https://github.com/voltagex/junkcode

                switch (ConfigurationManager.AppSettings.Get("Handler"))
                {
                    case "Git":

                        handler = GetGitPath();
                        commandLine = "clone " + split[0] + " " + GetClonePath(split[0]);
                        useShellExecute = useShellExecute;
                        break;
                }

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

        private static string GetGitPath()
        {
            string defaultGitPath = "C:\\Program Files (x86)\\Git\\cmd\\git.exe";
            string path = ConfigurationManager.AppSettings.Get("GitPath");
            if (string.IsNullOrEmpty(path)) //try the default path if it's not in config
            {
                var settings = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                if (File.Exists(defaultGitPath)) //found it, save it to config so it'll be faster next time
                {
                    settings.AppSettings.Settings.Add("GitPath", defaultGitPath);
                    settings.Save();
                    return defaultGitPath;
                }

                else //exhaustive search
                {
                    //msysgit uses the System/Machine PATH variable, so look there
                    var values = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Machine);
                    foreach (var envPath in values.Split(';'))
                    {
                        var fullPath = Path.Combine(envPath, "git.exe");
                        if (File.Exists(fullPath))
                        {
                            settings.AppSettings.Settings.Add("GitPath", defaultGitPath);
                            settings.Save();
                            return fullPath;
                        }
                    }
                    MessageBox.Show("Sorry, you've asked to open github-windows:// links with git.exe, but I can't find it in PATH.");
                    Environment.Exit(1);
                }
            }
            return path;
        }

        private static string GetClonePath(string repoPath)
        {
            string[] repoParts = repoPath.Split(new[] { '/' });
            string repoName = repoParts[repoParts.Length - 1];
            string clonePath = ConfigurationManager.AppSettings.Get("ClonePath");
            if (string.IsNullOrEmpty(clonePath))
            {
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), repoName);
            }
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), clonePath);
        }
    }
}
