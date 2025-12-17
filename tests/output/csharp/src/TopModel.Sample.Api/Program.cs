var app = WebApplication.CreateBuilder(args).Build();
app.MapControllers();
await app.RunAsync();
