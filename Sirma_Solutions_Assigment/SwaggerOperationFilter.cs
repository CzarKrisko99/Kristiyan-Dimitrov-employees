using Microsoft.OpenApi.Models;
using Sirma_Solutions_Assigment.BussinessLogic;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Sirma_Solutions_Assigment.API
{
    public class SwaggerOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var fileUploadMime = Constants.CONTENT_TYPE_FORM_DATA;
            if (operation.RequestBody == null || !operation.RequestBody.Content.Any(x => x.Key.Equals(fileUploadMime, StringComparison.InvariantCultureIgnoreCase)))
                return;

            var fileParams = context.MethodInfo.GetParameters().Where(p => p.ParameterType == typeof(IFormFile));
            operation.RequestBody.Content[fileUploadMime].Schema.Properties =
                fileParams.ToDictionary(k => k.Name, v => new OpenApiSchema()
                {
                    Type = "string",
                    Format = "binary"
                });
        }
    }
}
