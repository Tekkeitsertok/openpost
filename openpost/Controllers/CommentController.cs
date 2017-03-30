using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using SharedUtils.Api.RequestViewModels;
using System.Globalization;

namespace openpost.Controllers
{
    public class CommentController : Controller
    {
        private readonly Data.ApplicationDbContext _dbContext;
        private readonly IAuthorizationService _authorizationService;

        public CommentController(Data.ApplicationDbContext dbContext,
            IAuthorizationService authorizationService)
        {
            _dbContext = dbContext;
            _authorizationService = authorizationService;
        }

        [Route("comment/show")]
        public async Task<IActionResult> Show(ShowCommentsViewModel model)
        {
            //Set default locale for UI
            CultureInfo.CurrentUICulture = new CultureInfo(model.Language);
            CultureInfo.CurrentCulture = new CultureInfo(model.Language);

            var selectedPage = await _dbContext.Pages.SingleOrDefaultAsync(p => p.PublicIdentifier == model.Page && p.SourcePlatformId == model.SourcePlatform);
            if (selectedPage != null)
            {
                ViewBag.CommentsCount = selectedPage.CommentsCount;
            }
            else ViewBag.CommentsCount = 0;
            ViewBag.IsGuest = !(await _authorizationService.AuthorizeAsync(null, model, new Requirements.AuthorizedUserRequirement()));
            if (!ViewBag.IsGuest)
            {
                //It means we passed the auth, so ID & Token are valid
                ViewBag.AuthorId = model.Id;
            }
            ViewBag.SourcePlatformId = model.SourcePlatform;
            ViewBag.PublicIdentifier = model.Page;
            ViewBag.LangCode = model.Language;
            return View(new PostCommentViewModel()
            {
                Id = model.Id,
                Token = model.Token,
                SourcePlatform = model.SourcePlatform,
                PageIdentifier = model.Page,
            });
        }
    }
}
