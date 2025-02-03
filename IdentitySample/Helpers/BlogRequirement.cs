using IdentitySample.Models.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace IdentitySample.Helpers
{
    public class BlogRequirement : IAuthorizationRequirement
    {
    }

    public class IsBlogForUserAuthorizationHandler : AuthorizationHandler<BlogRequirement, BlogDTO>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, BlogRequirement requirement, BlogDTO resource)
        {
            if (context.User.Identity?.Name == resource.UserName)
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
