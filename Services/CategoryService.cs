using System;
using CodingCourses.Common.Entities;
using CodingCourses.DTOs;
using CodingCourses.Data;
namespace CodingCourses.Services;

public class CategoryService : ICategoryService
{
    private readonly AppDbContext _context;

    public CategoryService(AppDbContext context)
    {
        _context = context;
    }

    public IEnumerable<CategoryResponseDto> GetAllCategories()
    {
        return _context.Categories
            .Select(c => new CategoryResponseDto 
            { 
                Id = c.Id, 
                CategoryName = c.CategoryName 
            }).ToList();
    }
    public CategoryResponseDto? GetCategoryById(int id)
    {
        var category = _context.Categories.Find(id);
        if (category == null) return null;

        return new CategoryResponseDto 
        { 
            Id = category.Id, 
            CategoryName =  category.CategoryName 
        };
    }
    public CategoryResponseDto CreateCategory(CategoryCreateDto dto)
    {
        var category = new Category
        {
            CategoryName = dto.CategoryName
        };

        _context.Categories.Add(category);
        _context.SaveChanges();

        return new CategoryResponseDto 
        { 
            Id = category.Id, 
            CategoryName = category.CategoryName 
        };
    }
    public bool DeleteCategory(int id)
    {
        var category = _context.Categories.Find(id);
        if (category == null) return false;

        _context.Categories.Remove(category);
        _context.SaveChanges();
        return true;
    }
}
