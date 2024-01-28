using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Vues.Common;
public class ApiVersionFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var apiVersion = context.DocumentName[1..];
        RemoveApiVersionQueryParam(operation);
        SetupApiVersionHeaderParam(operation, apiVersion);
    }

    private void SetupApiVersionHeaderParam(OpenApiOperation operation, string apiVersion)
    {
        var versionParameter = operation.Parameters
                .FirstOrDefault(p => p.Name == "x-api-version" && p.In == ParameterLocation.Header);

        if (versionParameter is not null)
        {
            versionParameter.Schema.Default = new OpenApiString(apiVersion);
            versionParameter.Description = "Версия API";
            return;
        }

        operation.Parameters.Add(new OpenApiParameter(){
            Name = "x-api-version",
            Required = false,
            Schema = new OpenApiSchema { Type = "String",  Default = new OpenApiString(apiVersion) },
            In = ParameterLocation.Header,
            Description = "Версия API",
        });
    }

    private void RemoveApiVersionQueryParam(OpenApiOperation operation)
    {
        var versionParameter = operation.Parameters.FirstOrDefault(p => p.Name == "api-version" && p.In == ParameterLocation.Query);  
  
            if (versionParameter != null)  
                operation.Parameters.Remove(versionParameter);  
    }
}