using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using SharedUtils.Api.RequestViewModels;
using System.Threading.Tasks;

namespace openpost.Requirements
{
    public class IsUserAuthorizedAccessHandler : AuthorizationHandler<AuthorizedUserRequirement, AuthRequest>
    {
        private readonly Data.ApplicationDbContext _dbContext;

        public IsUserAuthorizedAccessHandler(Data.ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, AuthorizedUserRequirement requirement, AuthRequest resource)
        {
            if (!(await _dbContext.Authors.AnyAsync(a => a.Id == resource.Id &&
                                                      a.TokenId == resource.Token &&
                                                      a.SourcePlatformId == resource.SourcePlatform)))
            {
                //Failsafe, we check AuthRequest is PostCommentViewModel, if not, we cannot check if page allows Anonymous comments.
                if (!(resource is PostCommentViewModel)) return;

                var postCommentViewModel = (resource as PostCommentViewModel);
                //Then we check if page with specified public identifier allowing anonymous comments on this platform exists.
                if (!(await _dbContext.Pages.AnyAsync(p => p.SourcePlatformId == resource.SourcePlatform &&
                                                         p.PublicIdentifier == postCommentViewModel.PageIdentifier &&
                                                         p.AllowAnonymousComments))) return;
            }

            context.Succeed(requirement);
        }
    }
}
