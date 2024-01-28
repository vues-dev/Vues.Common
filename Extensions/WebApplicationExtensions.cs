using System.Reflection;
using Asp.Versioning.Builder;

namespace Vues.Common;

public static class WebApplicationExtensions
{
    public static void RegisterEndpoints(this WebApplication app, Assembly assembly, VuesApiDescription apiDescription, ApiVersionSet apiVersionSet)
    {
        foreach (Type mytype in assembly.GetTypes()
             .Where(mytype => mytype.GetInterfaces().Contains(typeof(IEndpoint))))
        {
            var endpoint = (IEndpoint?)Activator.CreateInstance(mytype);

            endpoint?.DefineEndpoint(app, apiVersionSet);
        }


        var paths = apiDescription.GetType()
                            .GetFields(BindingFlags.Public | BindingFlags.Static)
                            .Where(f => f.FieldType == typeof(string)
                                    && f.IsLiteral
                                    && !f.IsInitOnly)
                            .Select(f => (string)f.GetValue(null)!)
                            .ToDictionary(f => f, app.MapGroup);


        foreach (Type mytype in assembly.GetTypes()
             .Where(mytype => mytype.GetInterfaces().Contains(typeof(IGroupedEndpoint))))
        {
            var endpoint = (IGroupedEndpoint?)Activator.CreateInstance(mytype);

            endpoint?.DefineEndpoint(paths[endpoint.ApiGroup], apiVersionSet);
        }
    }
}
