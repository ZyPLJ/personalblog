using Microsoft.EntityFrameworkCore;
using Personalblog.Model;
using Personalblog.Model.Entitys;
using Personalblog.Model.ViewModels;
using Personalblog.Model.ViewModels.Categories;
using Personalblog.Model.ViewModels.QueryFilters;
using Personalblog.Model.ViewModels.Tag;
using PersonalblogServices.Response;

namespace PersonalblogServices.Tag;

public class TagService:ITagService
{
    private readonly MyDbContext _myDbContext;

    public TagService(MyDbContext myDbContext)
    {
        _myDbContext = myDbContext;
    }
    public async Task<List<Personalblog.Model.Entitys.Tag>> GetAllTagAsync()
    {
        return await _myDbContext.Tags
            .ToListAsync();
    }

    public async Task<(List<PostTag>, PaginationMetadata)> GetAllTagPostAsync(QueryParameters param)
    {
        var postTags = await _myDbContext.PostTags
            .Where(p => p.TagId == param.TagId)
            .Include(p => p.Post)
            .ThenInclude(p => p.Categories)
            .Include(p => p.Post)
            .ThenInclude(p => p.Comments)
            .Include(p => p.Post)
            .ThenInclude(p => p.Tags)
            .Skip((param.Page - 1) * param.PageSize)
            .Take(param.PageSize)
            .ToListAsync();
    
        var totalCount = await _myDbContext.PostTags.CountAsync(p => p.TagId == param.TagId);
        var pageCount = (int)Math.Ceiling((double)totalCount / param.PageSize);
    
        var pagination = new PaginationMetadata()
        {
            PageNumber = param.Page,
            PageSize = param.PageSize,
            TotalItemCount = totalCount,
            PageCount = pageCount,
            HasPreviousPage = param.Page > 1,
            HasNextPage = param.Page < pageCount,
            IsFirstPage = param.Page == 1,
            IsLastPage = param.Page == pageCount,
            FirstItemOnPage = (param.Page - 1) * param.PageSize + 1,
            LastItemOnPage = Math.Min(param.Page * param.PageSize, totalCount)
        };
    
        return (postTags, pagination);
    }


    public async Task<Personalblog.Model.Entitys.Tag?> GetById(int tagId)
    {
        return await _myDbContext.Tags.FirstOrDefaultAsync(a => a.Id == tagId);
    }

    public async Task<ApiResponse> AddTagAsync(TagCreateDto dto)
    {
        var tagExists = await _myDbContext.Tags.AnyAsync(t => t.Name == dto.TagName);
        if (tagExists)
        {
            return new ApiResponse { Data = null, Message = $"{dto.TagName}标签已存在!", StatusCode = 409 };
        }

        var tag = new Personalblog.Model.Entitys.Tag { Name = dto.TagName };
        await _myDbContext.Tags.AddAsync(tag);
        await _myDbContext.SaveChangesAsync();

        return new ApiResponse { Data = null, Message = $"{dto.TagName}新增成功~" };
    }

    public async Task<ApiResponse> DelTagAsync(int id)
    {
        await using var transaction = await _myDbContext.Database.BeginTransactionAsync();
        try
        {
            var tag = await _myDbContext.Tags.FindAsync(id);
            if (tag == null)
            {
                return new ApiResponse { Message = "标签不存在！", StatusCode = 404 };
            }

            var postTagList = await GetPostTagsAsync(id);
            _myDbContext.PostTags.RemoveRange(postTagList);
            
            _myDbContext.Tags.Remove(tag);
            
            await _myDbContext.SaveChangesAsync();
            await transaction.CommitAsync();
            return new ApiResponse { Message = $"标签{tag.Name}删除成功,并删除其余下{postTagList.Count}篇文章！"};
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            Console.WriteLine(e);
            return new ApiResponse { Message = "删除失败！", StatusCode = 500 };
        }
    }

    public async Task<ApiResponse> UpdateTagAsync(TagUpdateDto dto)
    {
        var tag = await _myDbContext.Tags.FirstOrDefaultAsync(t => t.Id == dto.id);
        if (tag == null)
        {
            return new ApiResponse { Message = "标签不存在！", StatusCode = 404 };
        }
        var tagExists = await _myDbContext.Tags.AnyAsync(t => t.Name == dto.Name);
        if (tagExists)
        {
            return new ApiResponse { Data = null, Message = $"{dto.Name}标签名已存在!", StatusCode = 409 };
        }

        tag.Name = dto.Name;
        await _myDbContext.SaveChangesAsync();
        return new ApiResponse { Message = $"标签{dto.Name}修改成功！"};
    }

    public async Task<(List<TagDto>, PaginationMetadata)> GetPagedList(QueryParameters param)
    {
        var querySet = _myDbContext.Tags.AsQueryable();
        if (param.Search != null)
        {
            querySet = querySet.Where(t => t.Name.Contains(param.Search));
        }

        var data = await querySet
            .Select(t => new TagDto
            {
                Id = t.Id,
                Name = t.Name,
                PostCount = t.Posts!.Count
            })
            .Skip((param.Page - 1) * param.PageSize)
            .Take(param.PageSize)
            .ToListAsync();
        
        var pagination = new PaginationMetadata()
        {
            PageNumber = param.Page,
            PageSize = param.PageSize,
            TotalItemCount = await querySet.CountAsync()
        };
        return (data, pagination);
    }

    private async Task<List<PostTag>> GetPostTagsAsync(int tagId)
    {
        var postTag = await _myDbContext.PostTags.Where(p => p.TagId == tagId).ToListAsync();
        return postTag;
    }
}