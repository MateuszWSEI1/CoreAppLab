using CoreApp.Module;
using Infrastructure;
using Infrastructure.EntityFramework.Context;
using Microsoft.EntityFrameworkCore;
using WebApi.Exceptions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddContactsEfModule(builder.Configuration);
builder.Services.AddContactsModule(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddExceptionHandler<ProblemDetailsExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

app.UseExceptionHandler();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ContactsDbContext>();
    db.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();