using EnigmaVault.Web.Bff.Extensions;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
var environment = builder.Environment; 

builder.Services
    //Default
    .AddOpenApi()
    .AddAuthorization()
    //Custom
    .AddServices(configuration)
    .AddHttpClients(configuration)
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

app.Run();