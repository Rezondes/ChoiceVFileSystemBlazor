namespace ChoiceVFileSystemBlazor.Database.Supportfiles.DbModels;

public class FileCharacterEntryDbModel
{
    public FileCharacterEntryDbModel() { }

    public FileCharacterEntryDbModel(Ulid supportfileId, int accountId, int characterId, string discordId, string accountName, string characterFirstName, string characterLastName)
    {
        SupportfileId = supportfileId;
        AccountId = accountId;
        CharacterId = characterId;
        DiscordId = discordId;
        AccountName = accountName;
        CharacterFirstName = characterFirstName;
        CharacterLastName = characterLastName;
    }
    
    public Ulid Id { get; set; } = Ulid.NewUlid();
    
    public Ulid SupportfileId { get; set; }
    public int AccountId { get; set; }
    public int CharacterId { get; set; }
    public string DiscordId { get; set; }
    
    public string AccountName { get; set; }
    public string CharacterFirstName { get; set; }
    public string CharacterLastName { get; set; }

    public string GetFullDisplayString() => $"[{AccountId}] {AccountName} | {DiscordId} | [{CharacterId}] {CharacterFirstName} {CharacterLastName}";
    
    // Navigation Properties
    public FileDbModel File { get; set; }
}