using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using kubec_cmd;

public class KubeConfigList
{
    // Regex to match backup files with date pattern like _19-12-2025_22-02-25
    private static readonly Regex BackupDatePattern = new Regex(@"_\d{2}-\d{2}-\d{4}_\d{2}-\d{2}-\d{2}$");

    public static List<string> ListFilesInPath()
    {
        var userfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        string kubeconfigPath = Path.Join(userfile, ".kube");
        var configFound = new List<string>();
        var fileManager = new DirectoryInfo(kubeconfigPath);
        FileInfo[] files = fileManager.GetFiles("*", SearchOption.AllDirectories);

        foreach (var file in files)
        {
            if (file.Name.Contains(Program.GlobalVariables.configsuffix))
            {
                // Skip backup files (containing "bk", ".back", or date pattern)
                if (!file.Name.Contains("bk") &&
                    !file.Name.Contains(".back") &&
                    !BackupDatePattern.IsMatch(file.Name))
                {
                    configFound.Add(file.Name);
                }
            }
        }

        return configFound;
    }
}