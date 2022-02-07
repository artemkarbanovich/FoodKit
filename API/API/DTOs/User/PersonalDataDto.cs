using API.Entities;

namespace API.DTOs.User;

public class PersonalDataDto
{
    public string UserName { get; set; }
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public DateOnly RegistrationDate { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public Gender? Gender { get; set; }
    public int? BodyWeight { get; set; }
    public int? BodyHeight { get; set; }
    public float? PhysicalActivityCoefficient { get; set; }
}