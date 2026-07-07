using System.Reflection;
using EnigmaVault.Web.Bff.Extensions;

var builder = WebApplication.CreateBuilder(args);

Assembly? assembly = Assembly.GetExecutingAssembly();
IConfiguration configuration = builder.Configuration;
IWebHostEnvironment environment = builder.Environment; 

builder.Services
    //Default
    .AddOpenApi()
    .AddAuthorization()
    .AddHttpContextAccessor()
    //Custom
    .AddServices(configuration)
    .AddHttpClients(configuration)
    .AddDelegationsHandlers()
    .AddDistributedLock()
    .UseCors()
    .AddSharedCryptoKeyForDecryptCookie(configuration)
    .AddCookie(environment);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("AllowTrustedFrontend");
app.UseAuthentication(); 
app.UseAuthorization();  
app.MapEndpoints(assembly);

app.Run();