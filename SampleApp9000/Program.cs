using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi;
using SampleApp9000.Extensions;
using Swashbuckle.Rebase.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc
    (
        "sampleapp",
        new OpenApiInfo
        {
            Title = "Sample App 9000",
            Version = "1.0.0",
            Description = "Provides a simple example of the tool"
        }
    );
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.UseSwagger
(
    options =>
    {
        options.PreSerializeFilters.Add((swagger, httpReq) =>
        {
            if (!string.IsNullOrWhiteSpace(httpReq?.Host.Value))
            {
                swagger.Servers ??= new List<OpenApiServer>();
                swagger.Servers.Add
                (
                    new OpenApiServer { Url = $"{httpReq.Scheme}://{httpReq.Host.Value}{{basePath}}" }
                        .WithVariable("basePath", new OpenApiServerVariable { Default = "/test" })
                );
            }
        });

        options.RemoveRoot("/test");
    }
);

// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
// specifying the Swagger JSON endpoint.
app.UseSwaggerUI(x =>
{
    x.DocumentTitle = "SampleApp9000";
    x.SwaggerEndpoint("/swagger/sampleapp/swagger.json", "Sample App 9000");
    x.RoutePrefix = string.Empty;
});

app.MapControllers();

app.Run();
