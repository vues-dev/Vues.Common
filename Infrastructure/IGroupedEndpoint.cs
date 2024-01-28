using Asp.Versioning.Builder;

namespace Vues.Common;

public interface IGroupedEndpoint
{
    public string ApiGroup { get; }
    public void DefineEndpoint(RouteGroupBuilder app, ApiVersionSet apiVersionSet);
}
