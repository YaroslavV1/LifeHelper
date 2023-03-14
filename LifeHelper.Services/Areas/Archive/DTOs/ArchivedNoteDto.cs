namespace LifeHelper.Services.Areas.Archive.DTOs;

public class ArchivedNoteDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    
    public ICollection<ArchivedSubNoteDto> ArchivedSubNotes { get; set; }
}