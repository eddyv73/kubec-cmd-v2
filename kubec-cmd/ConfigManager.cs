using kubec_cmd.Commons;
using YamlDotNet.Serialization;

namespace kubec_cmd;

public class ConfigManager
{
    private static string userfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
    private static string path = Path.Join(userfile,".kube",".kubec-cmd","kubec-config.yaml"); 
    
    public bool ExistConfig()
    {
        if (File.Exists(path))
        {
            return true;
        }
        return false;
    }
    
    public void ReadConfig()
    {
        // exist?
        if (File.Exists(path))
        {
            // read file
            var file = File.ReadAllText(path);
            Console.WriteLine(file);
        }
    }

    public bool CreateFileConfig()
    {
        try
        {
            //Create new yaml file using Config-kubec
            var config = new Config_Kubec
            {
                CurrentConfig = "config",
                backupDirectory = ".bk",
                files = new ListFiles[]
                {
                    new ListFiles
                    {
                        fileName = "config",
                        Dir = ".kube"
                    }
                }
            };
            var yaml = new SerializerBuilder().Build().Serialize(config);
            File.WriteAllText(path, yaml);
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }

    public bool UpdateFileList(List<string> files)
    {
        try
        {
            // read file
            var file = File.ReadAllText(path);
            var deserializer = new DeserializerBuilder().Build();
            var config = deserializer.Deserialize<Config_Kubec>(file);
            config.files = new ListFiles[files.Count];
            for (int i = 0; i < files.Count; i++)
            {
                config.files[i] = new ListFiles
                {
                    fileName = files[i],
                    Dir = ".kube"
                };
            }
            var yaml = new SerializerBuilder().Build().Serialize(config);
            File.WriteAllText(path, yaml);
            
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }
}