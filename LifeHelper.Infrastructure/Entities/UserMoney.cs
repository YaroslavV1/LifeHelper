namespace LifeHelper.Infrastructure.Entities;

public class UserMoney
{
    public int Id { get; set; }
    public decimal Money { get; set; }
    
    public int UserId { get; set; }
    public User User { get; set; }
}