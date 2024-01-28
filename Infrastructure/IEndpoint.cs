using Asp.Versioning.Builder;

namespace Vues.Common;

public interface IEndpoint
{
    public void DefineEndpoint(WebApplication app, ApiVersionSet apiVersionSet);
}
