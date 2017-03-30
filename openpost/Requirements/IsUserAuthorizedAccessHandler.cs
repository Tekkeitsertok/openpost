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
                                                      a.SourcePlatformId == resource.SourcePlatform))) return;

            context.Succeed(requirement);
        }
    }
}
