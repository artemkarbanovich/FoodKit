﻿using System.ComponentModel.DataAnnotations;

namespace API.DTOs.User;

public class OrderDto
{
    [Required] 
    public int AddressId { get; set; }

    [Required]
    public DateTime OrderDate { get; set; }

    [Required] 
    public DateTime DeliveryDate { get; set; }

    [Required] 
    public decimal TotalPrice { get; set; }

    [Required]
    public string Status { get; set; }

    [Required] 
    public List<OrderDishParameterDto> OrderDishParameters { get; set; }
}