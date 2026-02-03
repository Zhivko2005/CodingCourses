using System;
using CodingCourses.DTOs;
namespace CodingCourses.Services;

public interface ICategoryService
{
    IEnumerable<CategoryResponseDto> GetAllCategories();
    CategoryResponseDto CreateCategory(CategoryCreateDto dto);
    CategoryResponseDto? GetCategoryById(int id);
    bool DeleteCategory(int id);
}
