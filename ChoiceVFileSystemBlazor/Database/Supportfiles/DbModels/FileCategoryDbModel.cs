namespace ChoiceVFileSystemBlazor.Database.Supportfiles.DbModels;

public class FileCategoryDbModel
{
    public FileCategoryDbModel() { }

    public FileCategoryDbModel(string name, int number)
    {
        Name = name;
        Number = number;
    }

    public Ulid Id { get; set; } = Ulid.NewUlid();
    
    public string Name { get; set; }
    
    public int Number { get; set; }
    
    // Navigation Properties
    public List<FileDbModel> Supportfiles { get; set; }
}