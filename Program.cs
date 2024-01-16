var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://localhost:4242");

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    // Configure the Swagger endpoint to use the /api prefix
    c.RoutePrefix = "api";
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "UPS Log Monitor API");
});

app.UseRouting();


app.Map("/upsData", app =>
{
    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
    });
});

app.Map("/upsStatus", app =>
{
    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
    });
});

app.Run();
