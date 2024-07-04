using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using TaskManagement.Application.Queries.Tasks;
using TaskManagement.Application.Validators;
using TaskManagement.Application.Validators.Tasks;
using TaskManagement.Infrastructure.Persistence;
using TaskManagement.Infrastructure.UnitOfWorks;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(options =>
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var txt = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddMediatR(c => c.RegisterServicesFromAssemblyContaining<GetTasksQuery>());

builder.Services.AddValidatorsFromAssemblyContaining<AddTaskCommandValidator>();
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowOrigin", builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
    context.Database.Migrate();
}

app.UseCors("AllowOrigin");
app.Run();
