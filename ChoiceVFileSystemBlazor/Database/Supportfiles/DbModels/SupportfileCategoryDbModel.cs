namespace ChoiceVFileSystemBlazor.Database.Supportfiles.DbModels;

public class SupportfileCategoryDbModel
{
    public SupportfileCategoryDbModel() { }

    public SupportfileCategoryDbModel(Ulid supportfileId, string name, int nummer)
    {
        SupportfileId = supportfileId;
        Name = name;
        Nummer = nummer;
    }

    public Ulid Id { get; set; } = Ulid.NewUlid();
    
    public Ulid SupportfileId { get; set; }
    
    public string Name { get; set; }
    
    public int Nummer { get; set; }
    
    // Navigation Properties
    public SupportfileDbModel Supportfile { get; set; }
}