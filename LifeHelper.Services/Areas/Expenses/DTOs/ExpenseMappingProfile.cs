using AutoMapper;
using LifeHelper.Infrastructure.Entities;

namespace LifeHelper.Services.Areas.Expenses.DTOs;

public class ExpenseMappingProfile : Profile
{
    public ExpenseMappingProfile()
    {
        CreateMap<Expense, ExpenseDto>();
        CreateMap<ExpenseInputDto, Expense>();
    }
}