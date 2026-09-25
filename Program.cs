using TodoApi.Models;
using TodoApi.Services;
using static TodoApi.Services.GuidService;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<TodoService>();
builder.Services.AddScoped<IScopedService , GuidService>();
builder.Services.AddTransient<ITransientService, GuidService>();
builder.Services.AddSingleton<ISingletonService, GuidService>();
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
