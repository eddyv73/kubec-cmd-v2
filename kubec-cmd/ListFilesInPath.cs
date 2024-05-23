using System;
using System.Collections.Generic;
using System.IO;
using kubec_cmd;

class KubeConfigList
{
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
                if (!file.Name.Contains("bk") && !file.Name.Contains(".back") && file.Name.Contains(Program.GlobalVariables.configsuffix))
                {
                    configFound.Add(file.Name);
                }
            }
        }

        return configFound;
    }
}