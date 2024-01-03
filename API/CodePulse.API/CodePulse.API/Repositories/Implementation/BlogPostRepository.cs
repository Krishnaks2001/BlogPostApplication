using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Models.Dto;
using CodePulse.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Repositories.Implementation
{
    public class BlogPostRepository: IBlogPostRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public BlogPostRepository(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<BlogPost> CreateAsync(BlogPost blogPost)
        {
            await _dbContext.BlogPosts.AddAsync(blogPost);
            await _dbContext.SaveChangesAsync();
            return blogPost;
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            return await _dbContext.BlogPosts.Include(x => x.Categories).ToListAsync();
        }

        public async Task<BlogPost?> GetAsync(Guid id) {
            return await _dbContext.BlogPosts.Include(y => y.Categories).FirstOrDefaultAsync(x => x.Id == id);
        }


        public async Task<BlogPost?> UpdateAsync( BlogPost blogpost)
        {
           var existingBlogPost = await _dbContext.BlogPosts.Include(x => x.Categories).FirstOrDefaultAsync(x => x.Id == blogpost.Id);

            if(existingBlogPost == null)
            {
                return null;
            }

            _dbContext.Entry(existingBlogPost).CurrentValues.SetValues(blogpost);
            existingBlogPost.Categories = blogpost.Categories;
            await _dbContext.SaveChangesAsync();
            return blogpost;
        }

        public async Task<BlogPost?> DeleteAsync(Guid id)
        {
            var existingBlogPost = await _dbContext.BlogPosts.FirstOrDefaultAsync(y => y.Id == id);
            if( existingBlogPost != null)
            {
                _dbContext.BlogPosts.Remove(existingBlogPost);
                await _dbContext.SaveChangesAsync();
                return existingBlogPost;
            }
            return null;
        }

        public async Task<BlogPost?> GetByUrlAsync(string urlHandle)
        {
            var existingBlogPost = await _dbContext.BlogPosts.Include(x => x.Categories).FirstOrDefaultAsync(x => x.UrlHandle == urlHandle);
            return existingBlogPost;
        }
    }
}
