using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DefaultBrowserFilter
{
    enum BrowserApplication
    {
        Unknown,
        InternetExplorer,
        Firefox,
        Chrome,
        Opera,
        Safari,
        Edge
    }

    class Program
    {
        const string programName = "DefaultBrowserURLFilter";

        static internal void PrintHelp()
        {
            // TODO: print something here
        }

        static void Main(string[] args)
        {
            // parameter handling: 
            // 1) -Install 
            // 2) -Open
            if (args.Length == 0)
            {
                PrintHelp();
            }
            
            var actionName = args[0];
            if (actionName == "-Install")
            {
                // 1) HKEY_LOCAL_MACHINE\SOFTWARE\Clients\StartMenuInternet
                //    HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Clients\StartMenuInternet
                string contents = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "StartMenuInternet.temp"));
                string localExecutableName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AppDomain.CurrentDomain.FriendlyName);
                localExecutableName = localExecutableName.Replace(@"\", @"\\");
                localExecutableName = "\\\"" + localExecutableName + "\\\"";
                contents = contents.Replace("$PATH", localExecutableName);
                File.WriteAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "StartMenuInternet.reg"), contents);
                Process regeditProcess = Process.Start("regedit.exe", "/s " + Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "StartMenuInternet.reg"));
                regeditProcess.WaitForExit();

                const string startMenuInternet = @"SOFTWARE\Clients\StartMenuInternet";
                using (RegistryKey startMenuInternetKey = Registry.LocalMachine.OpenSubKey(startMenuInternet))
                {
                }
                

                // 2) HKEY_CURRENT_USER\Software\Microsoft\Windows\Shell\Associations\UrlAssociations\https
                string[] protocols = new string[] { "http", "https" };
                foreach (var protocol in protocols)
                {

                }
                // 3) HKEY_CLASSES_ROOT\ChromeHTML
            }
            else if (actionName == "-Open")
            {

            }
            else
            {
                // TODO log/output error message
                PrintHelp();
            }
        }
    }
}
