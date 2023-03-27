namespace LifeHelper.Infrastructure.Entities;

public class Category
{
    public int Id { get; set; }
    public string Title { get; set; }
    public decimal MoneyLimit { get; set; }
    
    public int UserId { get; set; }
    public User User { get; set; }
}