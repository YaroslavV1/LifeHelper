using LifeHelper.Infrastructure.Enums;

namespace LifeHelper.Infrastructure.Entities;

public class Role
{
    public int Id { get; set; }
    public RoleName RoleName { get; set; }
    public string NormalName { get; set; }
    
    public ICollection<User> Users { get; set; }
}