namespace LifeHelper.Infrastructure.Entities;

public class Note
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedDate { get; set; }
    
    public int UserId { get; set; }
    public User User { get; set; }
    
    public ICollection<SubNote> SubNotes { get; set; }
}