using CoreApp.Module;
using CoreApp.Repositories;
using CoreApp.Services;
using Infrastructure.Memory;
using WebApi.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddContactsModule(builder.Configuration);

builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.AddSingleton<IPersonRepositoryAsync, MemoryPersonRepository>();
builder.Services.AddSingleton<ICompanyRepositoryAsync, MemoryCompanyRepository>();
builder.Services.AddSingleton<IOrganizationRepositoryAsync, MemoryOrganizationRepository>();
builder.Services.AddSingleton<IContactRepositoryAsync, MemoryContactRepository>();

builder.Services.AddSingleton<IContactUnitOfWork, MemoryContactUnitOfWork>();

builder.Services.AddSingleton<IPersonService, PersonService>();

builder.Services.AddExceptionHandler<ProblemDetailsExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

//app.UseHttpsRedirection();
app.UseAuthorization();
app.UseExceptionHandler();
app.MapControllers();

app.Run();