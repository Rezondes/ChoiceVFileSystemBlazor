namespace ChoiceVFileSystemBlazor.Services.Vikunja.Models;

public class VikunjaFile
{
    public VikunjaFile(){}
    
    public VikunjaFile(string created, int id, byte[] mime, string name, long size)
    {
        Created = created;
        Id = id;
        Mime = mime;
        Name = name;
        Size = size;
    }

    public string Created { get; set; }
    public int Id { get; set; }
    public byte[] Mime { get; set; }
    public string Name { get; set; }
    public long Size { get; set; }
}