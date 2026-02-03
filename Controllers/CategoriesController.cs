using CodingCourses.DTOs;
using CodingCourses.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodingCourses.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_categoryService.GetAllCategories());
        }
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var category = _categoryService.GetCategoryById(id);
            if (category == null)
                return NotFound();

            return Ok(category);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")] 
        public IActionResult Create([FromBody] CategoryCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.CategoryName))
                return BadRequest("Category name cannot be empty.");

            var result = _categoryService.CreateCategory(dto);
            return Ok(result);
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var success = _categoryService.DeleteCategory(id);
            if (!success)
                return NotFound();

            return NoContent();
        }   
    }
}
