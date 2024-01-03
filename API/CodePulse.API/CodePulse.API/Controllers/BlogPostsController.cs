using Azure.Core;
using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Models.Dto;
using CodePulse.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostsController : ControllerBase
    {
        private readonly IBlogPostRepository _blogPostRespository;
        private readonly ICategoryRespository _categoryRespository;

        public BlogPostsController(IBlogPostRepository blogPostRespository,ICategoryRespository categoryRespository)
        {
            this._blogPostRespository = blogPostRespository;
            this._categoryRespository = categoryRespository;
        }
        [HttpPost]
        [Authorize(Roles = "Writer")]

        public async Task<IActionResult> CreateBlogPost([FromBody] BlogPostRequest request)
        {
            var blogPost = new BlogPost
            {
                Title = request.Title,
                ShortDescription = request.ShortDescription,
                Content = request.Content,
                UrlHandle = request.UrlHandle,
                FeaturedImageUrl = request.FeaturedImageUrl,
                PublishedDate = request.PublishedDate,
                Author = request.Author,
                IsVisible = request.IsVisible,
                Categories = new List<Category>()

            };
            foreach(var category in request.Categories){
                var categoryObtained = await _categoryRespository.GetById(category);
                if(categoryObtained is not null)
                {
                    blogPost.Categories.Add(categoryObtained);
                }
                
            }
             blogPost = await _blogPostRespository.CreateAsync(blogPost);

            var blogPostResult = new BlogPostDto
            {
                Id = blogPost.Id,
                Title = blogPost.Title,
                ShortDescription = blogPost.ShortDescription,
                Content = blogPost.Content,
                UrlHandle = blogPost.UrlHandle,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                PublishedDate = blogPost.PublishedDate,
                Author = blogPost.Author,
                IsVisible = blogPost.IsVisible,
                Categories = blogPost.Categories.Select(x => new CategoryDto { Id = x.Id, Name = x.Name, UrlHandle = x.UrlHandle, }).ToList(),
            };
            return Ok(blogPostResult);

        }
        //Get all blog posts
        [HttpGet]
        public async Task<IActionResult> GetAllBlogPosts()
        {
            var blogPosts = await _blogPostRespository.GetAllAsync();

            var result = new List<BlogPostDto>();
            foreach (var blogPost in blogPosts)
            {
                result.Add(new BlogPostDto
                {
                    Id = blogPost.Id,

                    Title = blogPost.Title,
                    ShortDescription = blogPost.ShortDescription,
                    Content = blogPost.Content,
                    UrlHandle = blogPost.UrlHandle,
                    FeaturedImageUrl = blogPost.FeaturedImageUrl,
                    PublishedDate = blogPost.PublishedDate,
                    Author = blogPost.Author,
                    IsVisible = blogPost.IsVisible,
                    Categories = blogPost.Categories.Select(x => new CategoryDto { Id = x.Id, Name = x.Name, UrlHandle = x.UrlHandle, }).ToList(),

                });
            }
            return Ok(result);
        }

        // get one blog post

        [HttpGet]
        [Route("{id:Guid}")]

        public async Task<IActionResult> GetBlogPost([FromRoute] Guid id)
        {
            var response = await _blogPostRespository.GetAsync(id);

            var result = new BlogPostDto
            {
                Id = response.Id,
                Title = response.Title,
                ShortDescription = response.ShortDescription,
                Content = response.Content,
                UrlHandle = response.UrlHandle,
                FeaturedImageUrl = response.FeaturedImageUrl,
                PublishedDate = response.PublishedDate,
                Author = response.Author,
                IsVisible = response.IsVisible,
                Categories = new List<CategoryDto>()
            };
            if (response.Categories == null)
            {
                return NotFound();
            }
                foreach (var category in response.Categories)
                {
                    result.Categories.Add(new CategoryDto
                    {
                        Id = category.Id,
                        Name = category.Name,
                        UrlHandle = category.UrlHandle
                    });
                }
            
            return Ok(result);
        }

        [HttpGet]
        [Route("{urlHandle}")]
        public async Task<IActionResult> GetBlogPostByUrl([FromRoute] string urlHandle)

        {
            var response = await _blogPostRespository.GetByUrlAsync(urlHandle);

            var result = new BlogPostDto
            {
                Id = response.Id,
                Title = response.Title,
                ShortDescription = response.ShortDescription,
                Content = response.Content,
                UrlHandle = response.UrlHandle,
                FeaturedImageUrl = response.FeaturedImageUrl,
                PublishedDate = response.PublishedDate,
                Author = response.Author,
                IsVisible = response.IsVisible,
                Categories = new List<CategoryDto>()
            };
            if (response.Categories == null)
            {
                return NotFound();
            }
            foreach (var category in response.Categories)
            {
                result.Categories.Add(new CategoryDto
                {
                    Id = category.Id,
                    Name = category.Name,
                    UrlHandle = category.UrlHandle
                });
            }

            return Ok(result);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]

        public async Task<IActionResult> UpdateBlogPost([FromRoute] Guid id, UpdateBlogPost blogpost)
        {
            var resposne = new BlogPost
            {
                Id = id,
                Title = blogpost.Title,
                ShortDescription = blogpost.ShortDescription,
                Content = blogpost.Content,
                UrlHandle = blogpost.UrlHandle,
                Author = blogpost.Author,
                IsVisible = blogpost.IsVisible,
                FeaturedImageUrl = blogpost.FeaturedImageUrl,
                PublishedDate = blogpost.PublishedDate,
                Categories = new List<Category>()
            };
            foreach(var categoryGuid in blogpost.Categories){

                var existedBlogPost = await _categoryRespository.GetById(categoryGuid);

                if(existedBlogPost != null)
                {
                    resposne.Categories.Add(existedBlogPost);
                }
            }

            var updateBlogPost = await _blogPostRespository.UpdateAsync(resposne);
            if(updateBlogPost == null)
            {
                return NotFound();
            }
            var blogPostResult = new BlogPostDto
            {
                Id = updateBlogPost.Id,
                Title = updateBlogPost.Title,
                ShortDescription = updateBlogPost.ShortDescription,
                Content = updateBlogPost.Content,
                UrlHandle = updateBlogPost.UrlHandle,
                Author = updateBlogPost.Author,
                IsVisible = updateBlogPost.IsVisible,
                FeaturedImageUrl = updateBlogPost.FeaturedImageUrl,
                PublishedDate = updateBlogPost.PublishedDate,
                Categories = updateBlogPost.Categories.Select(x => new CategoryDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle,
                }).ToList()
            };
            return Ok(blogPostResult);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]


        public async Task<IActionResult> DeleteBlogPost(Guid id)
        {
            var blogPost = await _blogPostRespository.DeleteAsync(id);
            if(blogPost == null)
            {
                return NotFound();
            }
            var blogPostResult = new BlogPostDto
            {
                Id = blogPost.Id,
                Title = blogPost.Title,
                ShortDescription = blogPost.ShortDescription,
                Content = blogPost.Content,
                UrlHandle = blogPost.UrlHandle,
                Author = blogPost.Author,
                IsVisible = blogPost.IsVisible,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                PublishedDate = blogPost.PublishedDate,
                Categories = blogPost.Categories.Select(x => new CategoryDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle,
                }).ToList()
            };
            return Ok(blogPostResult);
        }
    }
}
