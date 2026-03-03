using Microsoft.EntityFrameworkCore;
using TopModel.Sample.Clients.Db;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddDbContext<TopModelSampleDbContext>(o =>
    o.UseNpgsql(b => b.MigrationsAssembly("TopModel.Sample.Api"))
);
var app = builder.Build();
app.MapControllers();
await app.RunAsync();
