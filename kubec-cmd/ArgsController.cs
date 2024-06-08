using System.Text;

namespace kubec_cmd;

public class Args {
    public string target { get; set; }
    public string context { get; set; }
}

public class ArgsController
{
    public Args ArgsControl (string[] args)
    {
        Console.OutputEncoding = Encoding.Unicode;
        var argsList = new Args();
        var configManager = new ConfigManager();
        bool exist = configManager.ExistConfig();
        if (!exist)
        {
            configManager.CreateFileConfig();
        }
        if (args.Length == 1 && args.Contains("--clean"))
        {
            var backfiles = KubeConfigList.ListFilesBackup();
            Console.WriteLine("Total files deleted 🗑️ : " + backfiles.Count);
            
            return argsList;
        }
        if (args.Length == 1 && !args.Contains("--list"))
        {
            DirHelper.PrintInstructions();
            return argsList;
        }
        int targetIndex = Array.IndexOf(args, "-t");
        if (targetIndex != -1 && args.Length > targetIndex + 1)
        {
            string targetFile = args[targetIndex + 1];
            argsList.target = targetFile;
            // only for debug
            // Console.WriteLine("Argument 1 ↪︎ " + arguments[targetIndex]);
            // Console.WriteLine("Argument 2 ↪︎ " + arguments[targetIndex + 1]);
            Console.WriteLine("Target found ➥ " + targetFile);
            var files = KubeConfigList.ListFilesInPath(); 
            FilesManager.SearchFiles(argsList.target, argsList.context, files);
        } 
        else if (args.Contains("--list"))
        {
            var files = KubeConfigList.ListFilesInPath(); 
            foreach (var file in files)
            {
                Console.WriteLine(file);
            }

            if (files is not null && files.Count > 0)
            {
                bool update = configManager.UpdateFileList(files);
                if (update)
                    Console.WriteLine("Config file updated");
            }
        }
        else
        {
            Console.WriteLine("No target file found");
            DirHelper.PrintInstructions();
        }
        
        return argsList;
    }
}