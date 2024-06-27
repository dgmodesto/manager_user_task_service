namespace Sdk.Api.Swagger;

using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Interfaces;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

public class ConfigureSwaggerGenOptions : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiOptions _apiOptions;
    private readonly IApiVersionDescriptionProvider _apiVersionDescriptionProvider;

    public ConfigureSwaggerGenOptions(IApiVersionDescriptionProvider apiVersionDescriptionProvider,
        IApiOptions apiOptions)
    {
        _apiVersionDescriptionProvider = apiVersionDescriptionProvider;
        _apiOptions = apiOptions;
    }

    public void Configure(SwaggerGenOptions options)
    {
        foreach (var description in _apiVersionDescriptionProvider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(description.GroupName, CreateOpenApiInfo(description, _apiOptions.Title));
            var xmlFilename = $"ManagerUserTaskApi.Api.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        }
    }

    private static OpenApiInfo CreateOpenApiInfo(ApiVersionDescription description, string title)
    {
        var info = new OpenApiInfo
        {
            Title = title,
            Version = description.ApiVersion.ToString(),
            Description = "Projeto desenvolvido para demonstrar como documentar a sua API utilizando o Redoc",
            Contact = new Microsoft.OpenApi.Models.OpenApiContact()
            {
                Name = "Douglas Modesto",
                Email = "douglasgomesmodesto@gmail.com",
                Url = new Uri("https://www.linkedin.com/in/douglasmodesto/")

            },
            Extensions = new Dictionary<string, IOpenApiExtension>
            {
                {"x-logo", new OpenApiObject
                    {
                    {"url", new OpenApiString("https://avatars.githubusercontent.com/u/29544464?v=4")},
                    { "altText", new OpenApiString("News")}
                    }
                }
            }
        };


        if (description.IsDeprecated)
            info.Description +=
                " This API version has been deprecated. Please use one of the new APIs available from the explorer.";

        return info;
    }
}