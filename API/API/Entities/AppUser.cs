using Microsoft.AspNetCore.Identity;

namespace API.Entities;

public class AppUser : IdentityUser<int>
{
    public string Name { get; set; }
    public DateOnly RegistrationDate { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public Gender? Gender { get; set; }
    public int? BodyWeight { get; set; }
    public int? BodyHeight { get; set; }
    public float? PhysicalActivityCoefficient { get; set; }

    public ICollection<Address> Addresses { get; set; }
    public ICollection<UserDish> UserDishes { get; set; }
    public ICollection<Order> Orders { get; set; }
    public ICollection<Message> MessagesSent { get; set; }
    public ICollection<Message> MessagesReceived { get; set; }
    public ICollection<AppUserRole> AppUserRoles { get; set; }
}

public enum Gender
{
    Male = 1,
    Female,
    Other
}