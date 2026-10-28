using BookNest.Client.Models.Categories;

namespace BookNest.Client.Services.Categories
{

    /// <summary>
        /// Provides methods for communicating with the Categories API.
    /// </summary>
    public class CategoriesApiService
    {
        private readonly HttpClient _httpClient;

        public CategoriesApiService(IHttpClientFactory httpClientFactory)
        {
            // Creates an HTTP client using the named client configuration.
            _httpClient = httpClientFactory.CreateClient("BookNestApi");
        }





        // Retrieves all categories from the API.
        public async Task<List<CategoryResponse>> GetAllAsync()
        {
            // Sends a GET request to the Categories endpoint
            // and deserializes the JSON response into a list of CategoryResponse objects.
            var categories = await _httpClient
            .GetFromJsonAsync<List<CategoryResponse>>(
                "api/Categories");

            // Returns the retrieved categories, or an empty list if the response is null.
            return categories ?? new List<CategoryResponse>();
        }



    }
}
