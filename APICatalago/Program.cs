using APICatalago.Context;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using APICatalogo.Extensions;
using APICatalago.Filters;
using APICatalogo.Logging;
using APICatalago.Repositories.Specific;
using APICatalago.Repositories.Specific.Interface;
using APICatalago.Repositories.Generic.Interface;
using APICatalago.Repositories.Generic;
using APICatalago.Repositories.hybrid.Interfaces;
using APICatalago.Repositories.hybrid;
using APICatalago.Repositories.UnitOfWork;
using APICatalago.DTOs.Mappings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(ApiExceptionFilter));
})
.AddJsonOptions(
    options =>
    options.JsonSerializerOptions
    .ReferenceHandler = ReferenceHandler.IgnoreCycles
);

//Utilizando o acesso dos arquivos de configuração
var valor = builder.Configuration["Secao1:ChaveS1"];
string? DbConnection = builder.Configuration.GetConnectionString("DefaultConnection");


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Injetando dependências no conteiner DI nativo
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(DbConnection));
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<IProdutosRepository, ProdutosRepository>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IProdutoHibridoRepository, ProdutoHibridoRepository>();
builder.Services.AddScoped<ICategoriaHibridoRepository, CategoriaHibridoRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddAutoMapper(typeof(ProdutoDTOMappingProfile));

// O tipo de filtro inserido no conteiner DI, é utilizado como um atributo - ele está em TesteController.
builder.Services.AddScoped<ApiLoggingFilter>();
builder.Logging.AddProvider(new CustomLoggerProvider(new CustomLoggerProviderConfiguration
{
    LogLevel = LogLevel.Information
}));

// esses parâmetros desabilitam a inferência do FromService sem a necessidade do atributo, ou habilitam.
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.DisableImplicitFromServicesParameters = false;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.EnvironmentName.Contains("Development"))
{
    app.UseSwagger();
    app.UseSwaggerUI();

    // Middleware criado
    app.ConfigureExceptionHandler();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();