﻿using Microsoft.AspNetCore.Identity;

namespace API.Entities;

public class AppRole : IdentityRole<int>
{
    public ICollection<AppUserRole> AppUserRoles { get; set; }
}