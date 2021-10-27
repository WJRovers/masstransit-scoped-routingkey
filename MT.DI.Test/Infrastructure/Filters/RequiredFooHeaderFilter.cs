using System.Collections.Generic;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MT.DI.Test.Infrastructure.Filters
{
    public class RequiredFooHeaderFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "foo",
                In = ParameterLocation.Header,
                Description = "A value passed down",
                Required = true,
                Schema = new OpenApiSchema()
                {
                    Type = "string"
                }
            });
        }
    }
}