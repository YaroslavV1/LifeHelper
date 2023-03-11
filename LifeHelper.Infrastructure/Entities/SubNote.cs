namespace LifeHelper.Infrastructure.Entities;

public class SubNote
{
    public int Id { get; set; }
    public string Title { get; set; }
    
    public int NoteId { get; set; }
    public Note Note { get; set; }
}