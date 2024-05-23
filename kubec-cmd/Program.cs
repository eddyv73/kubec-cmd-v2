namespace kubec_cmd;

class Program
{
    const string configsuffix  = "config_";
    private const string _target = "";
    private const string _context = "";
    
    static void Main(string[] args)
    {
        ArgsController _args = new ArgsController();
        var res = _args.ArgsControl(args);
    }
}