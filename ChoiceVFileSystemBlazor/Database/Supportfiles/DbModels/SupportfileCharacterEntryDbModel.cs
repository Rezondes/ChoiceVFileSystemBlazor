namespace ChoiceVFileSystemBlazor.Database.Supportfiles.DbModels;

public class SupportfileCharacterEntryDbModel
{
    public SupportfileCharacterEntryDbModel() { }

    public SupportfileCharacterEntryDbModel(Ulid supportfileId, int accountId, int characterId, string characterFirstName, string characterLastName)
    {
        SupportfileId = supportfileId;
        AccountId = accountId;
        CharacterId = characterId;
        CharacterFirstName = characterFirstName;
        CharacterLastName = characterLastName;
    }
    
    public Ulid Id { get; set; } = Ulid.NewUlid();
    
    public Ulid SupportfileId { get; set; }
    public int AccountId { get; set; }
    public int CharacterId { get; set; }
    
    public string CharacterFirstName { get; set; }
    public string CharacterLastName { get; set; }

    public string GetFullDisplayString() => $"[{CharacterId}] {CharacterFirstName} {CharacterLastName}";
    
    // Navigation Properties
    public SupportfileDbModel Supportfile { get; set; }
}