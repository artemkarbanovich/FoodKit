using API.DTOs;
using API.DTOs.Admin;
using API.Extensions;
using API.Helpers.QueryParams;
using API.Interfaces;
using API.Interfaces.Data;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize(Policy = "AdminPolicy")]
public class DishController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IImageService _imageService;
    private readonly IMapper _mapper;

    public DishController(IUnitOfWork unitOfWork, IImageService imageService, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _imageService = imageService;
        _mapper = mapper;
    }


    [HttpPost("add-dish")]
    public async Task<ActionResult> AddDish([FromForm] DishAddDto dishAddDto)
    {
        if (!(await _unitOfWork.DishRepository.AddDishAsync(dishAddDto)))
            return BadRequest("Ошибка добавления блюда");

        return Ok();
    }

    [HttpPost("add-dish-images")]
    public async Task<ActionResult<List<ImageDto>>> AddDishImages([FromForm] List<IFormFile> imageFiles, [FromQuery] int dishId)
    {
        if (!(await _unitOfWork.DishRepository.AnyDishByIdAsync(dishId)))
            return NotFound();

        var imagesDto = await _unitOfWork.DishRepository.AddImagesAsync(imageFiles, dishId);

        if (imagesDto == null)
            return BadRequest("Ошибка добавления изображений");

        return imagesDto;
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

    [HttpGet("get-dishes-admin-list")]
    public async Task<ActionResult<List<DishAdminListDto>>> GetDishesAdminList([FromQuery] DishAdminListParam dishAdminListParam)
    {
        var dishesAdminList = await _unitOfWork.DishRepository.GetDishesAdminListAsync(dishAdminListParam);

        if (dishesAdminList == null)
            return BadRequest("Не удалось получить блюда");

        Response.AddPaginationHeader(dishesAdminList.CurrentPage, dishesAdminList.PageSize, dishesAdminList.TotalCount);

        return dishesAdminList;
    }

    [AllowAnonymous]
    [HttpGet("get-dish/{id}")]
    public async Task<ActionResult<DishDto>> GetDish(int id)
    {
        if (!(await _unitOfWork.DishRepository.AnyDishByIdAsync(id)))
            return NotFound();

        var dishDto = await _unitOfWork.DishRepository.GetFullDishByIdAsync(id);

        if (dishDto == null)
            return BadRequest("Ошибка получения блюда");

        return dishDto;
    }

    [HttpPut("update-dish")]
    public async Task<ActionResult> UpdateDish(DishUpdateDto dishUpdateDto)
    {
        if (!(await _unitOfWork.DishRepository.AnyDishByIdAsync(dishUpdateDto.Id)))
            return NotFound();

        var dish = await _unitOfWork.DishRepository.GetDishByIdAsync(dishUpdateDto.Id);

        if (dish == null)
            return BadRequest("Ошибка обновления блюда");

        _mapper.Map(dishUpdateDto, dish);

        _unitOfWork.DishRepository.UpdateDish(dish);

        if (await _unitOfWork.CompleteAsync())
            return NoContent();

        return BadRequest("Ошибка обновления блюда");
    }

    [HttpPut("update-dish-ingredients")]
    public async Task<ActionResult> UpdateDishIngredients(List<DishAddIngredientDto> ingredients, [FromQuery] int dishId)
    {
        if (!(await _unitOfWork.DishRepository.AnyDishByIdAsync(dishId)))
            return NotFound();

        if (await _unitOfWork.DishRepository.UpdateDishIngredientsAsync(ingredients, dishId))
            return Ok();

        return BadRequest("Ошибка обновления ингредиентов");
    }
}