namespace kubec_cmd;

public class Args {
    public string target { get; set; }
    public string context { get; set; }
}

public class ArgsController
{
    public Args ArgsControl (string[] args)
    {
        var argsList = new Args();
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
        } else if (args.Contains("--list"))
        {
            var files = KubeConfigList.ListFilesInPath(); 
            foreach (var file in files)
            {
                Console.WriteLine(file);
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