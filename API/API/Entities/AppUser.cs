using Microsoft.AspNetCore.Identity;

namespace API.Entities;

public class AppUser : IdentityUser<int>
{
    public string Name { get; set; }
    public DateOnly RegistrationDate { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public string Gender { get; set; }                      //male, female, not selected
    public int BodyWeight { get; set; }                     //kg
    public int BodyHeight { get; set; }                     //sm
    public int PhysicalActivityCoefficient { get; set; }

    public ICollection<Address> Addresses { get; set; }
    public ICollection<UserDish> UserDishes { get; set; }
    public ICollection<Order> Orders { get; set; }
}