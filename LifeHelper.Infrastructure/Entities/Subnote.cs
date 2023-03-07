namespace LifeHelper.Infrastructure.Entities;

public class Subnote
{
    public int Id { get; set; }
    public string Title { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    
    public int NoteId { get; set; }
    public Note Note { get; set; }
}