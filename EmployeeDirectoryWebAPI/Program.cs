
using Domain.Providers;
using EmployeeDirectoryWebAPI.Interfaces;
using EmployeeDirectoryWebAPI.Providers;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
   // options.CustomSchemaIds(type => type.ToString());

});

builder.Services.AddScoped<Data.Interfaces.IDepartmentRepository, Data.Repository.DepartmentRepository>();
builder.Services.AddScoped<Data.Interfaces.IProjectRepository, Data.Repository.ProjectRepository>();
builder.Services.AddScoped<Data.Interfaces.IRoleDetailRepository, Data.Repository.RoleDetailsRepository>();
builder.Services.AddScoped<Data.Interfaces.IRoleRepository, Data.Repository.RoleRepository>();
builder.Services.AddScoped<Data.Interfaces.ILocationRepository, Data.Repository.LocationRepository>();
builder.Services.AddScoped<Data.Interfaces.IEmployeeRepository, Data.Repository.EmployeeRepository>();
builder.Services.AddScoped<IRoleProvider,RoleProvider >();
builder.Services.AddScoped<IValidations, Validations>();
builder.Services.AddScoped<IEmployeeProvider, EmployeeProvider >();

builder.Services.AddDbContext<Data.AshwithEmployeeDirectoryContext>(Options =>
{
    var str = builder.Configuration.GetConnectionString("EmployeeDirectory");
    Options.UseSqlServer(str);
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
