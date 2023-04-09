using LifeHelper.Infrastructure.Enums;

namespace LifeHelper.Infrastructure.Entities;

public class User
{
    public int Id { get; set; }
    public string Nickname { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    
    public UserMoney UserMoney { get; set; }
    public ICollection<Role> Roles { get; set; }
    public ICollection<Note> Notes { get; set; }
    public ICollection<Category> Categories { get; set; }

    public User()
    {
        UserMoney = new UserMoney
        {
            UserId = Id,
            Money = decimal.Zero
        };
        
        Categories = ((CategoryTitle[])Enum.GetValues(typeof(CategoryTitle)))
            .Select(categoryTitle => new Category
            {
                UserId = Id,
                Title = categoryTitle.ToString().Normalize(),
                MoneyLimit = decimal.Zero
            }).ToList();
    }
}