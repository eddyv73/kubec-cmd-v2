namespace kubec_cmd;

public class Args {
    public string? target { get; set; }
    public string? context { get; set; }
    public bool interactive { get; set; }
}

public class ArgsController
{
    public Args ArgsControl (string[] args)
    {
        var argsList = new Args();

        // Check for interactive mode first
        if (args.Contains("-i") || args.Contains("--interactive"))
        {
            argsList.interactive = true;
            return argsList;
        }

        // Show help if no args or just --help
        if (args.Length == 0 || args.Contains("--help") || args.Contains("-h"))
        {
            DirHelper.PrintInstructions();
            return argsList;
        }

        // Handle --list
        if (args.Contains("--list"))
        {
            var files = KubeConfigList.ListFilesInPath();
            foreach (var file in files)
            {
                Console.WriteLine(file);
            }
            return argsList;
        }

        // Handle --clean
        if (args.Contains("--clean"))
        {
            FilesManager.CleanBackups();
            return argsList;
        }

        // Handle -t target
        int targetIndex = Array.IndexOf(args, "-t");
        if (targetIndex != -1 && args.Length > targetIndex + 1)
        {
            string targetFile = args[targetIndex + 1];
            argsList.target = targetFile;
            Console.WriteLine("Target found \u279C " + targetFile);
            var files = KubeConfigList.ListFilesInPath();
            FilesManager.SearchFiles(argsList.target, argsList.context, files);
        }
        else
        {
            Console.WriteLine("No target file found");
            DirHelper.PrintInstructions();
        }

        return argsList;
    }
}