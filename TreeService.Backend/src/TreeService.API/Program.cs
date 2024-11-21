using TreeService.API;
using TreeService.API.Extensions;
using TreeService.API.Middlewares;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDependencies(builder.Configuration);
builder.Services.AddCors();
var app = builder.Build();
app.UseExceptionMiddleware();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
await app.Services.RunMigrations();
/// test
app.UseCors(c => c.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
