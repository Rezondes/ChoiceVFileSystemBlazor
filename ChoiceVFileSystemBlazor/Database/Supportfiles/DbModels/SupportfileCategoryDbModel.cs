namespace ChoiceVFileSystemBlazor.Database.Supportfiles.DbModels;

public class SupportfileCategoryDbModel
{
    public SupportfileCategoryDbModel() { }

    public SupportfileCategoryDbModel(string name, int nummer)
    {
        Name = name;
        Nummer = nummer;
    }

    public Ulid Id { get; set; } = Ulid.NewUlid();
    
    public string Name { get; set; }
    
    public int Nummer { get; set; }
    
    // Navigation Properties
    public List<SupportfileDbModel> Supportfiles { get; set; }
}