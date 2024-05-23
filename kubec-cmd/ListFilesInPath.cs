using System;
using System.Collections.Generic;
using System.IO;

class KubeConfigList
{
    public static List<string> ListFilesInPath()
    {
        string kubeconfigPath = kubeconfig.FullName;
        var configFound = new List<string>();
        var fileManager = new DirectoryInfo(kubeconfigPath);
        FileInfo[] files = fileManager.GetFiles("*", SearchOption.AllDirectories);

        foreach (var file in files)
        {
            if (file.Name.Contains(configsuffix))
            {
                if (!file.Name.Contains("bk") && !file.Name.Contains(".back") && file.Name.Contains(configsuffix))
                {
                    configFound.Add(file.Name);
                }
            }
        }

        return configFound;
    }

    private static DirectoryInfo kubeconfig = new DirectoryInfo("path_to_kubeconfig"); // Define this path
    private static string configsuffix = "config_suffix"; // Define this suffix
}