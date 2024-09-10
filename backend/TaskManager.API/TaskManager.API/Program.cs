using FluentValidation.AspNetCore;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using TaskManager.API.Middlewares;
using TaskManager.API.Validators;
using TaskManager.Application.Cryptograph.Contracts;
using TaskManager.Application.Repositories.Contracts;
using TaskManager.Application.Services;
using TaskManager.Domain.Entities;
using TaskManager.Infrastructure.Cryptograph;
using TaskManager.Infrastructure.Persistence.Repositories;
using TaskManager.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:SecretKey"]);

if (key is null)
{
    throw new ArgumentNullException(nameof(key));
}

// Add services to the container.
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});


builder.Services.AddControllers();

// Configura o CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<TaskManagerDbContext>(options =>
       options.UseNpgsql(builder.Configuration.GetConnectionString("DbConnection")));

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IHashGenerator, HashGenerator>();
builder.Services.AddScoped<IHashCompare, HashCompare>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICollaboratorRepository, CollaboratorRepository>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<ITimeTrackerRepository, TimeTrackerRepository>();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<UserRegisterRequestValidator>();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Version = "v1",
        Title = "Task Manager API",
        Description = "API para gerenciar tarefas e usuários.",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Gabriel Cedraz",
            Email = "cedrazdev@gmail.com",
        }
    });

    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Insira o token JWT no campo 'Authorization'. Exemplo: 'Bearer {token}'",
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement{
    {
        new OpenApiSecurityScheme{
            Reference = new OpenApiReference{
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"}
            },
            new string[] {}
        }
    });

    options.EnableAnnotations();
});

var app = builder.Build();

// Seeding após a criação do host
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<TaskManagerDbContext>();
        var hasher = services.GetRequiredService<IHashGenerator>();

        SeedData(context, hasher);  // Chama o seed para criar os usuários
    }
    catch (Exception ex)
    {
        Console.WriteLine("Ocorreu um erro ao realizar o seed no banco de dados: " + ex.Message);
    }
}

app.UseCors("AllowAllOrigins");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
app.MapControllers();
app.Run();

// Método para realizar o seed
async Task SeedData(TaskManagerDbContext context, IHashGenerator hasher)
{
    if (!context.Users.Any())
    {
        var hashedPassword = await hasher.HashAsync("senha123");
        var users = new List<User>
        {
            new("user1", hashedPassword),
            new("user2", hashedPassword),
            new("user3", hashedPassword)
        };

        context.Users.AddRange(users);
        context.SaveChanges();
    }
}
