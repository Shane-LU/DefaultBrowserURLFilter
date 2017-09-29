﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DefaultBrowserFilter
{
    static class Utility
    {
        static internal string ExpandTemplateString(this string regTemplate
                                          , string oldString
                                          , string newString)
        {
            return regTemplate.Replace(oldString, newString);
        }
    }

    class Program
    {
        const string programNamePlaceholder = "$ApplicationName";
        const string programName = "DefaultBrowserURLFilter";
        const string applicationClassNamePlaceholder = "$ApplicationClass";
        const string applicationClassName = "DefaultBrowserURLFilterURL";
        const string applicationPathPlaceholder = "$Path";

        static internal void RegImport(string regFileName)
        {
            Process regeditProcess = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    FileName = "reg.exe",
                    Arguments = "import " + regFileName,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                }
            };
            regeditProcess.Start();
#if DEBUG
            while (!regeditProcess.StandardOutput.EndOfStream)
            {
                string line = regeditProcess.StandardOutput.ReadLine();
                Console.WriteLine(line);
            }

            Console.WriteLine("Error: ");
            while (!regeditProcess.StandardError.EndOfStream)
            {
                string line = regeditProcess.StandardError.ReadLine();
                Console.WriteLine(line);
            }
#endif
            regeditProcess.WaitForExit();
        }

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
                // HKEY_LOCAL_MACHINE\SOFTWARE\Clients\StartMenuInternet
                // HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Clients\StartMenuInternet
                string localExecutableName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AppDomain.CurrentDomain.FriendlyName);
                localExecutableName = localExecutableName.Replace(@"\", @"\\");

                string contents = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "StartMenuInternet.temp"));
                contents = contents.ExpandTemplateString(applicationClassNamePlaceholder, applicationClassName)
                                   .ExpandTemplateString(applicationPathPlaceholder, localExecutableName)
                                   .ExpandTemplateString(programNamePlaceholder, programName);
                File.WriteAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "StartMenuInternet.reg"), contents);
                RegImport(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "StartMenuInternet.reg"));

                // create the RegisteredApplications sub value
                using (var registeredApplications = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\RegisteredApplications", true))
                {
                    if (registeredApplications != null)
                    {
                        registeredApplications.SetValue(programName, $"SOFTWARE\\Clients\\StartMenuInternet\\{programName}.EXE\\Capabilities");
                    }
                }

                // HKEY_CURRENT_USER\Software\Microsoft\Windows\Shell\Associations\UrlAssociations\http
                // HKEY_CURRENT_USER\Software\Microsoft\Windows\Shell\Associations\UrlAssociations\https
                string[] protocols = new string[] { "http", "https" };
                foreach (var protocol in protocols)
                {

                }
                // 3) HKEY_CLASSES_ROOT\ChromeHTML
                // HKEY_LOCAL_MACHINE\SOFTWARE\Classes
                // 4) DefaultBrowserURLFilterURL
                //    
            }
            else if (actionName == "-Open")
            {

            }
            else if (actionName == "-HideShortcuts")
            {

            }
            else if (actionName == "-ShowShortcuts")
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
