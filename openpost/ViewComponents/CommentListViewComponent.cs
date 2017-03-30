using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using openpost.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace openpost.ViewComponents
{
    public class CommentListViewComponent : ViewComponent
    {
        private readonly Data.ApplicationDbContext _dbContext;

        public CommentListViewComponent(Data.ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync(string source, string page, string parent = null, byte depth = 0)
        {
            if (depth > 2) return Content(string.Empty);

            var platformPage = await _dbContext.Pages.SingleOrDefaultAsync(p => p.SourcePlatformId == source && p.PublicIdentifier == page);

            if (platformPage == null) return Content(string.Empty);

            //ViewBag.SourcePlatformId = source;
            //ViewBag.PublicIdentifier = page;

            List<CommentViewModel> comments = await _dbContext.Comments.Include(c => c.Author).ThenInclude(a => a.SourcePlatform)
                .Where(c => c.ParentId == parent && c.PageId == platformPage.Id && c.Depth == depth).OrderBy(c => c.PostDate)
                .Select(c => new CommentViewModel()
                {
                    Id = c.Id,
                    Depth = c.Depth,
                    PostDate = c.PostDate,
                    Author = c.Author.DisplayName,
                    AuthorId = c.AuthorId,
                    AvatarUrl = c.Author.AvatarUrl,
                    ProfileUrl = $"{c.Author.SourcePlatform.ProviderApi ?? string.Empty}/{c.Author.PlatformId}",
                    Content = c.Content,
                }).ToListAsync();

            return View("List", comments);
        }
    }
}
