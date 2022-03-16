using API_WEB_Superheroes.Data;
using API_WEB_Superheroes.Repository.IConfiguration;
using API_WEB_Superheroes.SuperHeroe_Mapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

//DbContext
builder.Services.AddDbContext<ApplicationDbContext>(Options =>
{
    Options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

//Scope with the UnitOfWork
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


//builder.Services.AddAuthentication()
//Agregar dependencia del token
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });


//AutoMapper
builder.Services.AddAutoMapper(typeof(SuperHeroeMappers));


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("ApiSuperHeroe", new OpenApiInfo
    {
        Title = "API SuperHeroe",
        Version = "v1",
        Description = "API de SuperHeroes"
    });

    option.SwaggerDoc("ApiUsuario", new OpenApiInfo
    {
        Title = "API Usuario [Registrarse/Acceder]",
        Version = "v1",
        Description = "API de Usuarios"
    });



    // Hacer que los comentarios sirvan como detalles en la documentacion del API en Swagger
    //Comentarios XML
    var archivoXmlComentarios = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var rutaApiComentarios = Path.Combine(AppContext.BaseDirectory, archivoXmlComentarios);
    option.IncludeXmlComments(rutaApiComentarios);


    //AUTENTICACION EN LA DOCUMENTACION
    //ESTO ES PARA PODER USAR EL TOKEN DEL USUARIO Y VALIDARLO PARA INGRESAR A METODOS NO AUTORIZADOS
    //Primero definir el esquema de seguridad
    option.AddSecurityDefinition("Bearer",
        new OpenApiSecurityScheme
        {
            Description = "Autenticación JWT (Bearer)",
            Type = SecuritySchemeType.Http,
            Scheme = "Bearer"
        });


    option.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                      {
                            Id = "Bearer",
                            Type = ReferenceType.SecurityScheme
                        }
                        }, new List<string>()
                    }
                });

});

// DAR SOPORTE PARA CORS
builder.Services.AddCors();

var app = builder.Build(); 

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/ApiSuperHeroe/swagger.json", "API SuperHeroe");
        options.SwaggerEndpoint("/swagger/ApiUsuario/swagger.json", "API Usuario");

        //options.RoutePrefix = "";
    });
}

app.UseRouting();

app.UseHttpsRedirection();


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

//CORS
app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
