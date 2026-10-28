using BookNest.Data;
using BookNest.DTOs.Categories;
using BookNest.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookNest.Services.Categories
{
    public class CategoriesService : ICategoriesService
    {
        private readonly BookNestDbContext _context;

        public CategoriesService(BookNestDbContext context)
        {
            _context = context;
        }






        //Returns all categories in the database
        public async Task<IReadOnlyList<Category>> GetAllCategories()
        {
            return await _context.Categories
                .AsNoTracking()
                .ToListAsync();
        }






        //Returns a category by ID in the database
        public async Task<Category?> GetCategory(int id)
        {
            return await _context.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.CategoryId == id);
        }






        // Creates a new category and returns the created entity
        public async Task<Category> CreateCategoryAsync(Category category)
        {
            // Mark the category as added in the DbContext
            _context.Categories.Add(category);

            // Save the changes to the database.
            await _context.SaveChangesAsync();

            // Return the created category, including its generated ID.
            return category;

        }






        // Updates an existing category and returns the updated entity
        public async Task<Category?> UpdateCategoryAsync(int id, string title)
        {
            // Find the existing category in the database.
            var existingCategory = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == id);


            // Return null if the category does not exist.
            if (existingCategory == null)
            {
                return null;
            }


            // Update the category title.
            existingCategory.Title = title;


            // Save the changes to the database.
            await _context.SaveChangesAsync();


            // Return the updated category.
            return existingCategory;
        }





        // Deletes an existing category, returns true if category was deleted or false if it was not found
        public async Task<bool> DeleteCategoryAsync(int id) 
        {
            // Finds the existing category in the database.
            var existingCategory = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == id);


            // Returns false if the category does not exist
            if (existingCategory == null)
            {
                return false;
            }


            // Marks the category for deletion
            _context.Categories.Remove(existingCategory);


            // Saves the changes to the database
            await _context.SaveChangesAsync();


            // Returns true to show that the category was deleted.
            return true;

        }








    }
}