using Energizet.CiCd.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSingleton(builder.Configuration.GetSection(nameof(CiCd)).Get<CiCd>()!);

var app = builder.Build();

app.MapControllers();

app.Run();