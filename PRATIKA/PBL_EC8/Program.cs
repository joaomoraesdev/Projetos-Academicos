using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MongoDB.Driver;
using Microsoft.Extensions.Options;
using PBL_EC8.Bll;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    //options.ListenLocalhost(5014); // HTTP
    options.ListenLocalhost(7282, listenOptions => listenOptions.UseHttps()); // HTTPS
});

// Carregar configurações JWT do appsettings.json
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]);

// Configuração do JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

// Registrar o MongoClient
builder.Services.AddSingleton<IMongoClient, MongoClient>(sp =>
{
    //Configuração nova - aumento de tempo de requisão, devido a casos de internet lenta
    var settings = MongoClientSettings.FromConnectionString(builder.Configuration["MongoDBSettings:ConnectionString"]);
    settings.ConnectTimeout = TimeSpan.FromSeconds(60); // Ajusta para o tempo desejado
    settings.SocketTimeout = TimeSpan.FromSeconds(60);
    settings.UseTls = true; // Habilita SSL/TLS
    var client = new MongoClient(settings);
    return client;
});

// Registrar o UsuarioBll
builder.Services.AddScoped<UsuarioBll>(sp =>
{
    var mongoClient = sp.GetRequiredService<IMongoClient>();
    var databaseName = builder.Configuration["MongoDBSettings:DatabaseName"] ?? "db_pratika"; // Pega o nome do banco de dados
    var collectionName = "collection_usuarios";  // Nome da coleção
    return new UsuarioBll(mongoClient, databaseName, collectionName);
});

// Registrar o AnuncioBll
builder.Services.AddScoped<AnuncioBll>(sp =>
{
    var mongoClient = sp.GetRequiredService<IMongoClient>();
    var databaseName = builder.Configuration["MongoDBSettings:DatabaseName"] ?? "db_pratika"; // Pega o nome do banco de dados
    var collectionName = "collection_anuncios";  // Nome da coleção
    return new AnuncioBll(mongoClient, databaseName, collectionName);
});

// Registrar o ComunidadeBll
builder.Services.AddScoped<ComunidadeBll>(sp =>
{
    var mongoClient = sp.GetRequiredService<IMongoClient>();
    var databaseName = builder.Configuration["MongoDBSettings:DatabaseName"] ?? "db_pratika"; // Pega o nome do banco de dados
    var collectionName = "collection_posts";  // Nome da coleção
    return new ComunidadeBll(mongoClient, databaseName, collectionName);
});

// Registrar o CurtidaBll
builder.Services.AddScoped<CurtidaBll>(sp =>
{
    var mongoClient = sp.GetRequiredService<IMongoClient>();
    var databaseName = builder.Configuration["MongoDBSettings:DatabaseName"] ?? "db_pratika"; // Pega o nome do banco de dados
    var collectionName = "collection_curtidas";  // Nome da coleção
    return new CurtidaBll(mongoClient, databaseName, collectionName);
});

// Registrar o MarketplaceBll
builder.Services.AddScoped<MarketplaceBll>(sp =>
{
    var mongoClient = sp.GetRequiredService<IMongoClient>();
    var databaseName = builder.Configuration["MongoDBSettings:DatabaseName"] ?? "db_pratika";
    var collectionName = "collection_marketplace";
    return new MarketplaceBll(mongoClient, databaseName, collectionName);
});

// Registrar o RelevanciaBll
builder.Services.AddScoped<RelevanciaBll>(sp =>
{
    var mongoClient = sp.GetRequiredService<IMongoClient>();
    var databaseName = builder.Configuration["MongoDBSettings:DatabaseName"] ?? "db_pratika"; // Pega o nome do banco de dados
    var collectionName = "collection_relevancia";  // Nome da coleção
    return new RelevanciaBll(mongoClient, databaseName, collectionName);
});

// Registrar o JwtService no contêiner de dependências
builder.Services.AddSingleton<JwtService>();

// Registrar controllers com views
builder.Services.AddControllersWithViews();

// Adicionar a utilização de Session
builder.Services.AddSession();

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.UseAuthentication();

// Utilizar Session
app.UseSession();

// Configurar rotas padrão
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Menu}/{id?}");

app.Run();
