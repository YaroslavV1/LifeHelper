namespace LifeHelper.Infrastructure.Entities;

public class SubtaskItem
{
    public int Id { get; set; }
    public string Title { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    
    public int TaskItemId { get; set; }
    public TaskItem TaskItem { get; set; }
}