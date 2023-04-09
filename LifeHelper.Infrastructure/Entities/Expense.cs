namespace LifeHelper.Infrastructure.Entities;

public class Expense
{
    public int Id { get; set; }
    public decimal SpentMoney { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    
    public int CategoryId { get; set; }
    public Category Category { get; set; }
}