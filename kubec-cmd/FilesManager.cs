// FilesManager.cs
// kubec-cmd
// Created by Eddy Wister on 25/11/22

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using kubec_cmd;

class FilesManager
{
    private static string _target;
    private static string _context;
    private static string userfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
    private static List<string> _ConfigFound = new List<string>();
    private static string dirbk = Path.Join(userfile, ".kube",".bk"); // Define this path
    private static string kubeconfig = Path.Join(userfile, ".kube");
    private static DirectoryInfo kubeconfigDir = new DirectoryInfo("path_to_kubeconfig_directory"); // Define this path

    public static void SearchFiles(string target, string context, List<string> configFile)
    {
        _target = target;
        _context = context;
        _ConfigFound = configFile;
        CreateBackUpDirectory();
        Makebackup();
        GetConfig();
        Clean();
        SwitcherConfig();
    }

    public static void CreateBackUpDirectory()
    {
        // create directory
        try
        {
            Directory.CreateDirectory(dirbk);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    public static void GetConfig()
    {
        // list of string
        // Select config files
        foreach (var file in Directory.EnumerateFiles(kubeconfig, "*"))
        {
            if (file.Contains(Program.GlobalVariables.configsuffix))
            {
                if (!file.Contains("bk") && !file.Contains(".back"))
                {
                    _ConfigFound.Add(file);
                }
            }
        }
    }

    public static void Makebackup()
    {
        Console.OutputEncoding = Encoding.Unicode;
        // date format date + time
        string result = DateTime.Now.ToString("dd-MM-yyyy_HH-mm-ss");

        // copy files .kube to .kube/bk and append _date + time
        foreach (var file in _ConfigFound)
        {
            string source = Path.Combine(kubeconfig, file);
            string destination = Path.Combine(dirbk, file + "_" + result);
            try
            {
                File.Copy(source, destination);
            }
            catch (Exception)
            {
                Console.WriteLine("Error on Create backup ⚠️");
            }
        }
    }

    public static void Clean()
    {
        Console.OutputEncoding = Encoding.Unicode;
        // If exist file .kube/config
        bool exist = File.Exists(Path.Join(kubeconfig, "config"));

        // delete file .kube/config
        if (exist)
        {
            try
            {
                File.Delete(Path.Join(kubeconfig, "config"));
            }
            catch (Exception)
            {
                Console.WriteLine("Error on Create backup ⚠️");
            }
        }
    }

    public static void SwitcherConfig()
    {
        Console.OutputEncoding = Encoding.Unicode;
        bool existTarget = File.Exists((Path.Join(kubeconfig, "config") + "_" + _target));

        if (existTarget)
        {
            Console.WriteLine("File exist");
            string targetfile = Program.GlobalVariables.configsuffix + _target;
            string source = Path.Combine(kubeconfig, targetfile);
            string destination = Path.Join(kubeconfig, "config");
            try
            {
                File.Copy(source, destination);
            }
            catch (Exception e)
            {
                Console.WriteLine("Ooops! Something went wrong: " + e.Message);
            }
            Console.WriteLine("Completed ✅");

            // Cambia el contexto si se especificó
            if (!string.IsNullOrEmpty(_context))
            {
                string changeContextCommand = $"config use-context {_context}";
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "/opt/homebrew/bin/kubectl",
                        Arguments = changeContextCommand,
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };
                process.Start();
                process.WaitForExit();
                Console.WriteLine("Context switched to: " + _context);
            }
        }
        else
        {
            Console.WriteLine("Target no exist ❌ " + _target);
        }
    }
}
