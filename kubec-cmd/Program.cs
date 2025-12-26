using System.Text;

namespace kubec_cmd;

class Program
{
    public static class GlobalVariables
    {
        public const string configsuffix  = "config_";
        private const string _target = "";
        private const string _context = "";
    }


    static void Main(string[] args)
    {
        // Configure UTF-8 encoding for proper emoji display on all terminals
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        // Process arguments
        ArgsController _args = new ArgsController();
        var res = _args.ArgsControl(args);

        // Launch interactive mode if requested
        if (res.interactive)
        {
            var shell = new InteractiveShell();
            shell.Run();
        }
    }
}
