using CoreEmptyProject1.Views.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace CoreEmptyProject1.Security
{
    public class CanEditOnlyOtherAdminRolesAndClaimHandler : AuthorizationHandler<ManageAdminRolesAndClaimsRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ManageAdminRolesAndClaimsRequirement requirement)
        {
            HttpContext httpContext = context.Resource switch
            {
                HttpContext hc => hc,                              // 1
                AuthorizationFilterContext afc => afc.HttpContext, // 2
                _ => null                                          // 3
            };

            Helper.Dump($"Resource type: {context.Resource?.GetType().FullName}");

            if (httpContext == null)
            {
                return Task.CompletedTask;
            }

            string loggedUserId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            string editedId = httpContext.Request.RouteValues["id"]?.ToString();

            Helper.Dump($"Logged: {loggedUserId}, Edited: {editedId}");
            Helper.Dump($"IsAdmin: {context.User.IsInRole("Admin")}");
            foreach (var c in context.User.Claims)
                Helper.Dump($"Claim: {c.Type} = {c.Value}");

            if (string.IsNullOrEmpty(loggedUserId) || string.IsNullOrEmpty(editedId))
            {
                return Task.CompletedTask;
            }

            if (context.User.IsInRole("Admin")
                && context.User.HasClaim(c => c.Type == "Edit Role" && c.Value == "true")
                && !string.Equals(editedId, loggedUserId, StringComparison.OrdinalIgnoreCase))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
