using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using SharedUtils.Api.RequestViewModels;
using System.Threading.Tasks;

namespace openpost.Requirements
{
    public class IsPlatformAuthorizedAccessHandler : AuthorizationHandler<AuthorizedPlatformRequirement, PlatformRequest>
    {
        private readonly Data.ApplicationDbContext _dbContext;

        public IsPlatformAuthorizedAccessHandler(Data.ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, AuthorizedPlatformRequirement requirement, PlatformRequest resource)
        {
            if (!(await _dbContext.Platforms.AnyAsync(p => p.Id == resource.SourcePlatform &&
                                                      p.ProviderAuthKey == resource.PlatformAuthKey &&
                                                      p.ProviderAuthPassword == resource.PlatformAuthPassword))) return;

            context.Succeed(requirement);
        }
    }
}
