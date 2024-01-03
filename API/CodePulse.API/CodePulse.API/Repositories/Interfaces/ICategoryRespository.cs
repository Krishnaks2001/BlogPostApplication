using CodePulse.API.Models.Domain;

namespace CodePulse.API.Repositories.Interfaces
{
    public interface ICategoryRespository
    {
        Task<Category> CreateAsync(Category category);

        Task<IEnumerable<Category>> GetAllCategoriesAsync();

        Task<Category?> GetById(Guid id);

        Task<Category?> UpdateAsync(Category category);

        Task<Category?> DeleteAsync(Guid id);
    }
}
