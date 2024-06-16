using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
    app.UseSwagger();
    app.UseSwaggerUI();


app.UseHttpsRedirection();
string fileUpload = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "video");
if (!Directory.Exists(fileUpload))
{ Directory.CreateDirectory(fileUpload); }
var provider= new FileExtensionContentTypeProvider();
provider.Mappings.Add(".idvpack", "application/octet-stream");
app.UseStaticFiles(new StaticFileOptions { FileProvider=new PhysicalFileProvider(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"video"))});

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(fileUpload),
    RequestPath = "/video",
    ContentTypeProvider= provider

});

app.UseAuthorization();

app.MapControllers();

app.Run();
