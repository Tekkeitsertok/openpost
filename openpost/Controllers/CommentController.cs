using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using SharedUtils.Api.RequestViewModels;
using System.Globalization;
using SharedUtils.Api.ResponseViewModels;
using SharedUtils;
using System;

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
        [HttpPost]
        public async Task<IActionResult> Show(ShowCommentsViewModel model)
        {
            if(string.IsNullOrEmpty(model.Language))
            {
                return Content("Language not specified.");
            }
            if(string.IsNullOrEmpty(model.SourcePlatform))
            {
                return Content("SourcePlatform not specified.");
            }
            if (string.IsNullOrEmpty(model.Page))
            {
                return Content("Page not specified.");
            }
            //Set default locale for UI
            CultureInfo.CurrentUICulture = new CultureInfo(model.Language);
            CultureInfo.CurrentCulture = new CultureInfo(model.Language);

            var selectedPage = await _dbContext.Pages.SingleOrDefaultAsync(p => p.PublicIdentifier == model.Page && p.SourcePlatformId == model.SourcePlatform);
            if (selectedPage != null)
            {
                ViewBag.CommentsCount = selectedPage.CommentsCount;
                ViewBag.AllowAnonymousComments = selectedPage.AllowAnonymousComments;
            }
            else
            {
                ViewBag.CommentsCount = 0;
                ViewBag.AllowAnonymousComments = false;
            }
            var AuthorizedUserReq = new Requirements.AuthorizedUserRequirement();
            await _authorizationService.AuthorizeAsync(null, model, AuthorizedUserReq);
            ViewBag.IsGuest = AuthorizedUserReq.IsGuest;
            if (!ViewBag.IsGuest)
            {
                //It means we passed the auth, so ID & Token are valid
                ViewBag.AuthorId = model.Id;
            } 
            ViewBag.SourcePlatformId = model.SourcePlatform;
            ViewBag.PublicIdentifier = model.Page;
            ViewBag.LangCode = model.Language;
            ViewBag.IsForum = model.IsForum;
            return View(new PostCommentViewModel()
            {
                Id = model.Id,
                Token = model.Token,
                AuthenticatedMode = !ViewBag.IsGuest,
                SourcePlatform = model.SourcePlatform,
                PageIdentifier = model.Page,
            });
        }

        [Route("comment/post")]
        [HttpPost]
        public async Task<ResponseAddCommentViewModel> Post(PostCommentViewModel model)
        {
            if(model.AuthenticatedMode && (string.IsNullOrEmpty(model.Id) || string.IsNullOrEmpty(model.Token)))
                return new ResponseAddCommentViewModel(ResponseAddCommentViewModel.OperationResult.Invalid);
            if (string.IsNullOrEmpty(model.SourcePlatform) || string.IsNullOrWhiteSpace(model.PageIdentifier) 
                || string.IsNullOrWhiteSpace(model.Content))
                return new ResponseAddCommentViewModel(ResponseAddCommentViewModel.OperationResult.Invalid);

            if (!(await _authorizationService.AuthorizeAsync(null, model, new Requirements.AuthorizedUserRequirement())))
            {
                return new ResponseAddCommentViewModel(ResponseAddCommentViewModel.OperationResult.AuthorizationFailed);
            }

            bool IsNewPage = false;

            var page = await _dbContext.Pages.SingleOrDefaultAsync(p => p.PublicIdentifier == model.PageIdentifier && p.SourcePlatformId == model.SourcePlatform);
            if (page == null)
            {
                page = new Models.Page()
                {
                    Id = ShortGuid.NewGuid().Value,
                    SourcePlatformId = model.SourcePlatform,
                    PublicIdentifier = model.PageIdentifier
                };
                IsNewPage = true;
            }

            var parentComment = await _dbContext.Comments.SingleOrDefaultAsync(c => c.Id == model.Parent && c.PageId == page.Id);

            bool added = true;
            ResponseAddCommentViewModel commentResponse = null;
            Models.Comment comment = null;
            if (!string.IsNullOrWhiteSpace(model.Current) && model.Current.Length == 22)
            {
                //Editing comment is only available in authenticated mode.
                if(model.AuthenticatedMode)
                {
                    comment = await _dbContext.Comments.SingleOrDefaultAsync(c => c.Id == model.Current && c.AuthorId == model.Id);
                }   
                if (comment == null) return new ResponseAddCommentViewModel(ResponseAddCommentViewModel.OperationResult.AuthorizationFailed);
                commentResponse = new ResponseAddCommentViewModel(ResponseAddCommentViewModel.OperationResult.CommentEdited);
                comment.Content = model.Content;
                added = false;
            }
            else
            {
                commentResponse = new ResponseAddCommentViewModel(ResponseAddCommentViewModel.OperationResult.CommentAdded);
                comment = new Models.Comment()
                {
                    Title = model.Title,
                    Content = model.Content,
                    PostDate = DateTime.UtcNow,
                    PageId = page.Id,
                };
            }

            if (!model.AuthenticatedMode)
            {
                Models.Author AnonymousAuthor = new Models.Author()
                {
                    Id = ShortGuid.NewGuid().Value,
                    SourcePlatformId = model.SourcePlatform,
                    DisplayName = model.Pseudonym ?? "Anonymous",
                    Email = model.Email,
                    IsAnonymous = true,
                    WebSite = model.Website,
                };
                //set comment's author to newly created Anonymous Author
                comment.AuthorId = AnonymousAuthor.Id;
                //Fill response data
                commentResponse.Author = AnonymousAuthor.DisplayName;
                commentResponse.ProfileUrl = AnonymousAuthor.WebSite;
                //Add anonymous user to database.
                await _dbContext.Authors.AddAsync(AnonymousAuthor);
            } else
            {
                var author = await _dbContext.Authors.SingleOrDefaultAsync(a => a.Id == model.Id);
                //Set AuthorId
                comment.AuthorId = model.Id;
                //Fill Response data
                commentResponse.Author = author.DisplayName;
                commentResponse.AvatarUrl = author.AvatarUrl;
                //todo : profile url.
            }
            
            commentResponse.Title = model.Title;
            commentResponse.Content = model.Content;
            
            if (parentComment != null)
            {
                comment.Depth = (byte)(parentComment.Depth + 1);
                comment.ParentId = parentComment.Id;
                commentResponse.ParentCommentId = parentComment.Id;
                commentResponse.Depth = comment.Depth;
            }
            else
            {
                commentResponse.Depth = 1;
            }

            if (added)
            {
                //Add comment
                await _dbContext.Comments.AddAsync(comment);
                //Update page count
                page.CommentsCount++;
                //Insert/Update page in dbContext.
                if (!IsNewPage)
                {
                    _dbContext.Pages.Update(page);
                }
                else
                {
                    await _dbContext.Pages.AddAsync(page);
                }
            }
            else
            {
                //Update comment
                _dbContext.Comments.Update(comment);
            }

            //Save changes
            await _dbContext.SaveChangesAsync();

            return commentResponse;
        }

        [Route("comment/delete")]
        [HttpPost]
        public async Task<ResponseDeleteCommentViewModel> Delete(DeleteCommentViewModel model)
        {
            if (!model.AuthenticatedMode || string.IsNullOrEmpty(model.Id) || string.IsNullOrEmpty(model.Token) || string.IsNullOrEmpty(model.SourcePlatform) ||
                string.IsNullOrWhiteSpace(model.CommentId))
                return new ResponseDeleteCommentViewModel(ResponseDeleteCommentViewModel.OperationResult.Invalid);

            if (!(await _authorizationService.AuthorizeAsync(null, model, new Requirements.AuthorizedUserRequirement())))
            {
                return new ResponseDeleteCommentViewModel(ResponseDeleteCommentViewModel.OperationResult.AuthorizationFailed);
            }

            var comment = await _dbContext.Comments.Include(c => c.Page).SingleOrDefaultAsync(c => c.Id == model.CommentId && c.AuthorId == model.Id);
            if (comment == null) return new ResponseDeleteCommentViewModel(ResponseDeleteCommentViewModel.OperationResult.AuthorizationFailed);

            comment.Content = null;

            comment.Page.CommentsCount--;
            //Update comment
            _dbContext.Comments.Update(comment);
            //Update Page
            _dbContext.Pages.Update(comment.Page);
            //Save changes
            await _dbContext.SaveChangesAsync();

            return new ResponseDeleteCommentViewModel(ResponseDeleteCommentViewModel.OperationResult.CommentDeleted);
        }
    }
}
