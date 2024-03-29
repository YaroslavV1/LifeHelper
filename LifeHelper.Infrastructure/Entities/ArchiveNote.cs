namespace LifeHelper.Infrastructure.Entities;

public class ArchiveNote
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    
    public int UserId { get; set; }
    public User User { get; set; }
    
    public ICollection<ArchiveSubNote> ArchiveSubNotes { get; set; }
}