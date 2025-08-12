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
            // Solo mostrar archivos de configuración principales, no backups ni zips
            bool isConfig = file.Name.Contains(Program.GlobalVariables.configsuffix);
            bool isBackup = file.Name.Contains(".") && HasDateSuffix(file.Name);
            bool isZip = file.Extension.Equals(".zip", StringComparison.OrdinalIgnoreCase);
            bool isBkOrBack = file.Name.Contains("bk") || file.Name.Contains(".back");

            if (isConfig && !isBackup && !isZip && !isBkOrBack)
            {
                configFound.Add(file.Name);
            }
        }
        return configFound;
    }

    /// <summary>
    /// Detecta si el nombre de archivo tiene un sufijo de fecha típico de backup.
    /// Ejemplo: config_cashify.19-06-2023_18-22-07
    /// </summary>
    private static bool HasDateSuffix(string fileName)
    {
        // Busca patrón de fecha: .dd-MM-yyyy_HH-mm-ss
        var parts = fileName.Split('.');
        if (parts.Length < 2) return false;
        var last = parts[parts.Length - 1];
        DateTime dt;
        return DateTime.TryParseExact(last, "dd-MM-yyyy_HH-mm-ss", null, System.Globalization.DateTimeStyles.None, out dt);
    }
}