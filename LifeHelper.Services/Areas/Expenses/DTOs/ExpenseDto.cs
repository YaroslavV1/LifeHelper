namespace LifeHelper.Services.Areas.Expenses.DTOs;

public class ExpenseDto
{
    public int Id { get; set; }
    public decimal SpentMoney { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedDate { get; set; }
}