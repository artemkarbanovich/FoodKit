using API.DTOs.Admin;
using API.Entities;
using API.Extensions;
using API.Helpers.Paginator;
using API.Helpers.QueryParams;
using API.Interfaces.Data;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize(Policy = "AdminPolicy")]
public class IngredientController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper; 

    public IngredientController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    

    [HttpPost("add-ingredient")]
    public async Task<ActionResult<int>> AddIngredient(IngredientDto ingredientDto, [FromQuery] bool addAnyway = false)
    {
        if (await _unitOfWork.IngredientRepository.AnyIngredientsByLevenshteinDistanceAsync(2, ingredientDto.Name)
            && !addAnyway)
                return BadRequest("Похоже, что у вас уже есть такой ингредиент");

        var ingredient = new Ingredient();
        _mapper.Map(ingredientDto, ingredient);

        await _unitOfWork.IngredientRepository.AddIngredientAsync(ingredient);

        if (await _unitOfWork.CompleteAsync())
            return ingredient.Id;

        return BadRequest("Не удалось добавить ингредиент");
    }

    [HttpGet("get-ingredients")]
    public async Task<ActionResult<PagedList<IngredientDto>>> GetIngredients([FromQuery] IngredientParam ingredientParam)
    {
        var ingredients = await _unitOfWork.IngredientRepository.GetIngredientsAsync(ingredientParam);

        if (ingredients == null)
            return BadRequest("Не удалось получить ингредиенты");

        Response.AddPaginationHeader(ingredients.CurrentPage, ingredients.PageSize, ingredients.TotalCount);

        return Ok(ingredients);
    }

    [HttpDelete("delete-ingredient/{ingredientId}")]
    public async Task<ActionResult> DeleteIngredient(int ingredientId)
    {
        var ingredient = await _unitOfWork.IngredientRepository.GetIngredientByIdAsync(ingredientId);

        if(ingredient == null)
            return BadRequest("Ошибка удаления ингредиента");
        
        _unitOfWork.IngredientRepository.DeleteIngredient(ingredient);

        if (await _unitOfWork.CompleteAsync())
            return Ok();

        return BadRequest("Ошибка удаления ингредиента");
    }
}