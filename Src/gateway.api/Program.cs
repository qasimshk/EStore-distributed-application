using MMLib.Ocelot.Provider.AppConfiguration;
using MMLib.SwaggerForOcelot.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Configuration.AddOcelotWithSwaggerSupport((opt) => opt.Folder = "Configuration");

    builder.Services.AddOcelot().AddAppConfiguration();

    builder.Services.AddSwaggerForOcelot(builder.Configuration);

    builder.Services.AddControllers();

    builder.Services.AddSwaggerGen();
}

var app = builder.Build();
{
    app.UseRouting();

    app.UseSwagger();

    app.MapControllers();

    app.UseStaticFiles();

    app.UseSwaggerForOcelotUI(opt => opt.DownstreamSwaggerHeaders =
    [
        new KeyValuePair<string, string>("Key", "Value")
    ]).UseOcelot().Wait();
}

app.Run();
