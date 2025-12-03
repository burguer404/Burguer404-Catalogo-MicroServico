using Catalogo.Infrastructure.ContextDb;
using Catalogo.Application.UseCases;
using Catalogo.Application.Gateways;
using Catalogo.Infrastructure.Repositories;
using Catalogo.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database
builder.Services.AddDbContext<CatalogoContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CatalogoConnection")));

// MongoDB
var mongoConnectionString = builder.Configuration.GetConnectionString("MongoConnection");
var mongoClient = new MongoClient(mongoConnectionString);
var mongoDatabase = mongoClient.GetDatabase("CatalogoDB");
builder.Services.AddSingleton<IMongoDatabase>(mongoDatabase);

// Repositories
builder.Services.AddScoped<CatalogoRepository>();
builder.Services.AddScoped<ImagemProdutoRepository>();

// Gateways
builder.Services.AddScoped<ICatalogoGateway, CatalogoGateway>();
builder.Services.AddScoped<IImagemProdutoGateway, ImagemProdutoGateway>();

// Use Cases
builder.Services.AddScoped<CriarProdutoUseCase>();
builder.Services.AddScoped<ListarProdutosUseCase>();
builder.Services.AddScoped<ObterProdutoPorIdUseCase>();
builder.Services.AddScoped<ObterCardapioUseCase>();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("CatalogoPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:5000") // Frontend
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Catálogo API v1");
    c.RoutePrefix = "swagger";
    c.DocumentTitle = "Catálogo API";
});

app.UseCors("CatalogoPolicy");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Migrations
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<CatalogoContext>();
    context.Database.Migrate();
}

app.Run();
