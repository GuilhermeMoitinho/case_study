using Microsoft.OpenApi.Models;
using System.Reflection;

namespace CaseLocaliza.Configurations;

internal static class SwaggerConfig
{
    internal static WebApplicationBuilder AddOpenApi(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer().AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1",
                new OpenApiInfo
                {
                    Title = "Case Localiza",
                    Version = "v1",
                    Description =
                        """
                        OpenAPI specification for testing and consuming Case localiza

                        ## Resources

                        * https://github.com/OAI/OpenAPI-Specification
                        \
                        _© Guilherme Moitinho
                        """
                });
        });

        return builder; 
    }
}
