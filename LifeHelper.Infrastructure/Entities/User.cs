namespace LifeHelper.Infrastructure.Entities;

public class User
{
    public int Id { get; set; }
    public string Nickname { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
   
    public ICollection<Role> Roles { get; set; }
    public ICollection<Note> Notes { get; set; }
}