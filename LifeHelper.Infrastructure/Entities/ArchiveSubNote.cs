namespace LifeHelper.Infrastructure.Entities;

public class ArchiveSubNote
{
    public int Id { get; set; }
    public string Title { get; set; }
    
    public int ArchiveNoteId { get; set; }
    public ArchiveNote ArchiveNote { get; set; }
}