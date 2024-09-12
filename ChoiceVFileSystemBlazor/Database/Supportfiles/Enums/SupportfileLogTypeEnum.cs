namespace ChoiceVFileSystemBlazor.Database.Supportfiles.Enums;

public enum SupportfileLogTypeEnum
{
    AddFile = 1000,
    DeleteFile = 1001,
    RestoreFile = 1010,
    ModifyTitle = 1002,
    ModifyDescription = 1003,
    ModifyStatus = 1004,
    ModifyMinRank = 1005,
    
    AddEntry = 2000,
    RemoveEntry = 2001,
    ModifyEntry = 2002,
    RestoreEntry = 2003,
}