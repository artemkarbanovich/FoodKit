using API.DTOs.Admin;
using API.Entities;
using API.Interfaces;
using API.Interfaces.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize(Policy = "AdminPolicy")]
public class DishController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IImageService _imageService;

    public DishController(IUnitOfWork unitOfWork, IImageService imageService)
    {
        _unitOfWork = unitOfWork;
        _imageService = imageService;
    }


    [HttpPost("add-dish")]
    public async Task<ActionResult<int>> AddDish(DishAddDto dishAddDto)
    {
        var dishId = await _unitOfWork.DishRepository.AddDishAsync(dishAddDto);

        if (dishId == -1)
            return BadRequest("Ошибка добавления блюда");

        return dishId;
    }
    
    [HttpPost("add-dish-image")]
    public async Task<ActionResult> AddDishImage(IFormFile imageFile, [FromQuery] int dishId)
    {
        var result = await _imageService.AddImageAsync(imageFile);

        if (result.Error != null)
            return BadRequest("Ошибка добавления изображения");

        var image = new Image
        {
            DishId = dishId,
            Url = result.SecureUrl.AbsoluteUri,
            PublicId = result.PublicId
        };

        await _unitOfWork.DishRepository.AddImageAsync(image);

        if (await _unitOfWork.CompleteAsync())
            return Ok();

        return BadRequest("Ошибка добавления изображения");
    }

    [HttpDelete("delete-dish-image/{imageId}")]
    public async Task<ActionResult> DeleteDishImage(int imageId)
    {
        var image = await _unitOfWork.DishRepository.GetImageByIdAsync(imageId);

        if (image == null)
            return NotFound();
        else if (image.PublicId != null && (await _imageService.DeleteImageAsync(image.PublicId)).Error != null)
            return BadRequest("Ошибка удаления изображения");

        _unitOfWork.DishRepository.RemoveImage(image);

        if (await _unitOfWork.CompleteAsync())
            return Ok();

        return BadRequest("Ошибка удаления изображения");
    }
}