using BookNest.DTOs.Categories;
using BookNest.Models;
using BookNest.Services.Categories;
using Microsoft.AspNetCore.Mvc;

namespace BookNest.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoriesService _service;
        public CategoriesController(ICategoriesService service)
        {
            _service = service;
        }






        // GET: api/Categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryResponse>>> GetCategories()
        {
            // Retrieve all categories from the service.
            var categories = await _service.GetAllCategories();

            var response = categories.Select(c => new CategoryResponse
            {
                CategoryId = c.CategoryId,
                Title = c.Title
            });

            //Returns a 200 OK response with category or categories data
            return Ok(response);
        }






        // GET: api/Categories/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<CategoryResponse>> GetCategory(int id)
        {
            if (id <= 0)
            {
                // Returns a 400 Bad Request response if the category ID is less than or equal to zero.
                return BadRequest(new { Message = "Category ID must be greater than zero" });
            }

            // Retrieve the category from the service.
            var category = await _service.GetCategory(id);

            if (category == null)
            {
                // Returns a 404 Not Found response if the item doesn't exist
                return NotFound(new { Message = $"Category with ID {id} not found." });
            }


            // Maps the category entity to response DTO.
            var response = new CategoryResponse
            {
                CategoryId = category.CategoryId,
                Title = category.Title
            };

            //Returns a 200 OK response with the category data
            return Ok(response);
        }






        // POST api/Categories
        [HttpPost]
        public async Task<ActionResult<CategoryResponse>> CreateCategory([FromBody] CreateCategoryRequest request)
        {
            //Maps request data from user to database Category entity.
            var category = new Category
            {
                Title = request.Title
            };


            // Calls the service to create a new category in the database.
            var createdCategory = await _service.CreateCategoryAsync(category);


            // Maps createdCategory entity to response DTO.
            var response = new CategoryResponse
            {
                CategoryId = createdCategory.CategoryId,
                Title = createdCategory.Title
            };

            // Returns a 201 Created response and provides information about the created resource.
            return CreatedAtAction(
                nameof(GetCategory), 
                new { id = response.CategoryId }, 
                response);
        }






        // PUT api/Categories/{id}
        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateCategory(int id, [FromBody] UpdateCategoryRequest request)
        {
            if (id <= 0)
            {
                // Returns a 400 Bad Request response if the category ID is less than or equal to zero.
                return BadRequest(new { Message = "Category ID must be greater than zero" });
            }


            // Validate the category title.
            if (string.IsNullOrWhiteSpace(request.Title))
            {
                return BadRequest(new { Message = "Category title is required." });
            }


            // Update the category through the service.
            var updatedCategory = await _service.UpdateCategoryAsync(id,request.Title);


            
            if (updatedCategory == null)
            {
                // Returns a 404 Not Found response if the category doesn't exist
                return NotFound(new { Message = $"Category with ID {id} not found." });
            }


            // Returns a 204 No Content after a successful update
            return NoContent(); 
        }






        // DELETE api/Categories/{id}
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            if (id <= 0)
            {
                // Returns a 400 Bad Request response if the category ID is less than or equal to zero
                return BadRequest(new { Message = "Category ID must be greater than zero" });
            }


            // Deletes the category through the service
            var isDeleted = await _service.DeleteCategoryAsync(id);


            if (!isDeleted)
            {
                // Returns a 404 Not Found if the category does not exist
                return NotFound(new { Message = $"Category with ID {id} not found." });
            }


            // Return a 204 No Content after a successful deletion
            return NoContent();

        }







    }
}
