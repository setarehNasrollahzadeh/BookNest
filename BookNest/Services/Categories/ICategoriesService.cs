using BookNest.DTOs.Categories;
using BookNest.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookNest.Services.Categories
{
    public interface ICategoriesService
    {
        //Returns all categories in the database
        Task<IReadOnlyList<Category>> GetAllCategories();


        //Returns a category by ID in the database
        Task<Category?> GetCategory(int id);


        // Creates a new category and returns the created entity
        Task<Category> CreateCategoryAsync (Category category);


        // Updates an existing category and returns the updated entity
        Task<Category?> UpdateCategoryAsync(int id, string title);


        // Deletes an existing category, returns true if category was deleted or false if it was not found
        Task<bool> DeleteCategoryAsync(int id);


    }
}
