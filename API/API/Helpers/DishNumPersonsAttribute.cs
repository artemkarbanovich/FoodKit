using System.ComponentModel.DataAnnotations;

namespace API.Helpers;

public class DishNumPersonsAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        if (value is int)
        {
            if ((int)value == 1 || (int)value == 2 || (int)value == 4)
                return true;
        }
        return false;
    }
}