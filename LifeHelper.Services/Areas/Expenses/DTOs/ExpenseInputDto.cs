namespace LifeHelper.Services.Areas.Expenses.DTOs;

public class ExpenseInputDto
{
    public int CategoryId { get; set; }
    public decimal SpentMoney { get; set; }
    public string? Description { get; set; }
}