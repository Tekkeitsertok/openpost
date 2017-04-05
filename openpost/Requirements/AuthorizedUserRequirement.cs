using Microsoft.AspNetCore.Authorization;

namespace openpost.Requirements
{
    public class AuthorizedUserRequirement : IAuthorizationRequirement
    {
        public bool IsGuest { get; set; } = false;
    }
}
