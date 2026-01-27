using core.application;
using infrastructure.identity;
using infrastructure.identity.Models;
using infrastructure.identity.Seeds;
using infrastructure.persistence;
using infrastructure.shared;
using Microsoft.AspNetCore.Identity;
using WebAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);

//
// Services Extensions
//

builder.Services.AddApplicationLayer();
builder.Services.AddPersistenceInfrastructure(builder.Configuration);
builder.Services.AddSharedInfrastructure(builder.Configuration);
builder.Services.AddApiVersioningExtension();
builder.Services.AddIdentityInfrastructure(builder.Configuration);

//
// API
//

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//
// Identity
//

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        await DefaultRoles.SeedAsync(userManager, roleManager);
        await DefaultAdminUser.SeedAsync(userManager, roleManager);
        await DefaultBasicUser.SeedAsync(userManager, roleManager);
    }
    catch (Exception ex)
    {
        throw;
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

//custom error handling middleware
app.UseErrorHandlingMiddleware();

app.MapControllers();

app.Run();
