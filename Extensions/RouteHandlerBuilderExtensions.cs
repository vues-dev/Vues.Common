namespace Vues.Common;

public static class RouteHandlerBuilderExtensions
{
    public static RouteHandlerBuilder ValidateRequest(this RouteHandlerBuilder handlerBuilder)
    {
        return handlerBuilder.AddEndpointFilter<ValidationFilter>();
    }
}
