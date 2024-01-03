using CodePulse.API.Models.Domain;
using CodePulse.API.Models.Dto;

namespace CodePulse.API.Repositories.Interfaces
{
    public interface IBlogPostRepository
    {
        Task<BlogPost> CreateAsync(BlogPost blogPost);

        Task<IEnumerable<BlogPost>> GetAllAsync();

        Task<BlogPost?> GetAsync(Guid id );

        Task<BlogPost?> GetByUrlAsync( string urlHandle );

        Task<BlogPost?> UpdateAsync( BlogPost blogpost);

        Task<BlogPost?> DeleteAsync( Guid id );
    }
}
