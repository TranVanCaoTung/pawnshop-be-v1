using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using System.Security.Claims;

namespace PawnShopBE.Helpers
{
    public class RemovePrefixFromAuthorizationHeaderFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var authAttributes = context.MethodInfo.GetCustomAttributes<AuthorizeAttribute>();
            if (authAttributes.Any())
            {
                var authHeaderParameter = operation.Parameters.SingleOrDefault(p => p.In == ParameterLocation.Header && p.Name == "Authorization");
                if (authHeaderParameter != null)
                {
                    authHeaderParameter.Description = authHeaderParameter.Description.Replace("Bearer ", "");
                }
            }
        }

    }
}
