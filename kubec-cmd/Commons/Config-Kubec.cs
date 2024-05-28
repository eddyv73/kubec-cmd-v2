namespace kubec_cmd.Commons;

public class Config_Kubec
{
    public string CurrentConfig { get; set; }
    public string backupDirectory { get; set; }
    ListFiles[] files { get; set; }
    
}


public class ListFiles
{
    public string fileName { get; set; }
    public string Dir { get; set; }
}