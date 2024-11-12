namespace ChoiceVFileSystemBlazor.Services.Vikunja.Models;

public class VikunjaAddLabelToTaskModel
{
    public VikunjaAddLabelToTaskModel()
    {
    }

    public VikunjaAddLabelToTaskModel(string created, int labelId)
    {
        Created = created;
        LabelId = labelId;
    }

    public string Created { get; set; }
    public int LabelId { get; set; }
}