using Cel.GameOfLife.Infra;
using Cel.GameOfLife.Application;
using Cel.GameOfLife.API.Middlewares;
using Cel.GameOfLife.API.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy.AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials()
              .SetIsOriginAllowed(origin => true);
    });
});

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();

// configure services
builder.Services
    .AddInfraServices(builder.Configuration)
    .AddServices();

var app = builder.Build();
app.UseCors("CorsPolicy");

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment()) { }
app.UseSwagger();
app.UseSwaggerUI();


app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
