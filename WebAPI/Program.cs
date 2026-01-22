using core.application;
using infrastructure.persistence;
using infrastructure.shared;
using WebAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);

//services extensions
builder.Services.AddApplicationLayer();
builder.Services.AddPersistenceInfrastructure(builder.Configuration);
builder.Services.AddSharedInfrastructure(builder.Configuration);
builder.Services.AddApiVersioningExtension();

//API
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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
