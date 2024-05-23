// FilesManager.cs
// kubec-cmd
// Created by Eddy Wister on 25/11/22

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

class FilesManager
{
    private static string _target;
    private static string _context;
    private static List<string> ConfigFound = new List<string>();
    private static string dirbk = "path_to_backup_directory"; // Define this path
    private static string kubeconfig = "path_to_kubeconfig"; // Define this path
    private static string configsuffix = "config_suffix"; // Define this suffix
    private static DirectoryInfo kubeconfigDir = new DirectoryInfo("path_to_kubeconfig_directory"); // Define this path

    public static void SearchFiles(string target, string context)
    {
        _target = target;
        _context = context;
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
            if (file.Contains(configsuffix))
            {
                if (!file.Contains("bk") && !file.Contains(".back"))
                {
                    ConfigFound.Add(file);
                }
            }
        }
    }

    public static void Makebackup()
    {
        // date format date + time
        string result = DateTime.Now.ToString("dd-MM-yyyy_HH-mm-ss");

        // copy files .kube to .kube/bk and append _date + time
        foreach (var file in ConfigFound)
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
        // If exist file .kube/config
        bool exist = File.Exists(kubeconfigDir.FullName);

        // delete file .kube/config
        if (exist)
        {
            try
            {
                File.Delete(kubeconfigDir.FullName);
            }
            catch (Exception)
            {
                Console.WriteLine("Error on Create backup ⚠️");
            }
        }
    }

    public static void SwitcherConfig()
    {
        bool existTarget = File.Exists(kubeconfigDir.FullName + "_" + _target);

        if (existTarget)
        {
            Console.WriteLine("File exist");
            string targetfile = configsuffix + _target;
            string source = Path.Combine(kubeconfig, targetfile);
            string destination = kubeconfigDir.FullName;
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
