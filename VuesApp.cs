using System.Reflection;
using Asp.Versioning;
using Asp.Versioning.Conventions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;

namespace Vues.Common;

public class VuesApp
{
    private bool _useSwagger;

    private int _defaultMajorApiVersion = 1;
    private int _defaultMinorApiVersion = 0;

    private readonly Assembly _apiAssembly;
    private readonly WebApplicationBuilder _builder;
    private readonly IServiceCollection _services;
    private readonly VuesApiDescription _apiDescription;

    public VuesApp(string[] args, VuesApiDescription apiDescription)
    {
        _builder = WebApplication.CreateBuilder(args);
        _services = _builder.Services;
        _apiAssembly = Assembly.GetCallingAssembly();
        _apiDescription = apiDescription;
    }

    /// <summary>
    /// Приложение будет использовать Swagger
    /// </summary>
    /// <returns>VuesApp</returns>
    public VuesApp UseSwagger()
    {
        _useSwagger = true;
        return this;
    }



    private void AddApiVersioning()
    {
        _services.AddApiVersioning(opt =>
        {
            opt.DefaultApiVersion = new ApiVersion(_defaultMajorApiVersion, _defaultMinorApiVersion);
            opt.AssumeDefaultVersionWhenUnspecified = true;
            opt.ReportApiVersions = true;
            opt.ApiVersionReader = ApiVersionReader.Combine(new HeaderApiVersionReader("x-api-version"), new QueryStringApiVersionReader("api-version"));
        }).AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VV";
        });
    }


    private void AddSwagger(IConfiguration configuration, ApiVersion[] apiVersions)
    {
        if (!_useSwagger)
            return;

        _services.AddSwaggerGen(c=>
        {
            c.OperationFilter<ApiVersionFilter>();

            foreach (var apiVersion in apiVersions)
            {
                var apiVersionName = $"v{apiVersion.MajorVersion}.{apiVersion.MinorVersion}";
                var defaultApiVersionName = $"v{_defaultMajorApiVersion}.{_defaultMinorApiVersion}";
                var defaultApiDescription = $"<br/><br/><b>Актуальная версия API - {defaultApiVersionName} &nbsp; <a href='../swagger/index.html?urls.primaryName={defaultApiVersionName}'>[ссылка]</a></b>";
                c.SwaggerDoc(apiVersionName, new OpenApiInfo()
                {
                    Title = configuration["Swagger:Title"] ?? "Название API",
                    Version = apiVersionName,
                    Description = (configuration["Swagger:Description"] ?? "Описание API не заполнено") + defaultApiDescription,
                });
            }
        });
    }

    public WebApplication BuildApi(){
        return BuildApi((s,c) => {});
    }

    public WebApplication BuildApi(Action<IServiceCollection, IConfiguration> lambda)
    {
        lambda(_services, _builder.Configuration);

        AddApiVersioning();
        

        if(_useSwagger)
        {
            _services.AddEndpointsApiExplorer();
            AddSwagger(_builder.Configuration, _apiDescription.ApiVersions);
        }

        _services.AddValidatorsFromAssembly(_apiAssembly);


        var app = _builder.Build();
        app.UseMiddleware<ErrorHandlerMiddleware>();
       

        if(_useSwagger)
        {
            app.Use(async (context, next) =>
            {
                var isFrontPage = context.Request.Path.Value!.EndsWith("swagger/index.html") && !context.Request.QueryString.HasValue;
                if (isFrontPage)
                {
                    context.Response.Redirect($"/swagger/index.html?urls.primaryName=v{_defaultMajorApiVersion}.{_defaultMinorApiVersion}");
                    return;
                }

                await next();
            });
        }


        var apiVersionSet = app.NewApiVersionSet()
                                 .HasApiVersions(_apiDescription.ApiVersions)
                                 .ReportApiVersions()
                                 .Build();

        app.UseRouting();


        app.RegisterEndpoints(_apiAssembly, _apiDescription, apiVersionSet);


        if (_useSwagger)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                var descriptions = app.DescribeApiVersions();
                foreach (var desc in descriptions)
                {
                    var url = $"/swagger/{desc.GroupName}/swagger.json";
                    var name = desc.GroupName.ToLowerInvariant();
                    options.SwaggerEndpoint(url, $"{name}");
                }
                options.EnableTryItOutByDefault();

                // https://github.com/domaindrivendev/Swashbuckle.AspNetCore/issues/2516
                options.ConfigObject.AdditionalItems["queryConfigEnabled"] = true;

            });
        }
        return app;
    }
}
