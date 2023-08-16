
using Serilog;
using Microsoft.EntityFrameworkCore;
using Kodigos.Infra.Data.Contexto;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using NetDevPack.Security.JwtExtensions;
using Microsoft.AspNetCore.Identity;
using NetDevPack.Security.Jwt.Core.Jwa;
using Kodigos.API.Utils;
using Kodigos.Dominio.ModelsData.Administration.Users;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(
    options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true
    );
builder.Configuration.AddJsonFile("serilog.json", true, true);
builder.Configuration.AddJsonFile("appsettings.local.json", true, true);

Log.Logger = new LoggerConfiguration().CreateBootstrapLogger();
builder.Host.UseSerilog((ctx, lc) => lc
       .WriteTo.Console()
       .ReadFrom.Configuration(ctx.Configuration));


ConfigurationManager Configuration = builder.Configuration;

string ConnectionString = Configuration.GetConnectionString("DefaultConnection");

Log.Information("Init SQL Server Connection");
builder.Services.AddDbContext<KodigosContext>(options =>
    options.UseSqlServer(ConnectionString, b => b.MigrationsAssembly("Kodigos.API")).EnableSensitiveDataLogging());
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
   var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
   options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

   options.SwaggerDoc("v1", new() { Title = "Payment On Time - Controle Financeiro Pessoal", Version = "v1" });
});

builder.Services.AddCors(opt =>
{
    opt.AddDefaultPolicy(pol =>
    {
        pol.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().WithExposedHeaders().SetPreflightMaxAge(TimeSpan.FromSeconds(2520));
    });
});

// Adding Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
// Adding Jwt Bearer
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.SetJwksOptions(new JwkOptions("http://localhost:5000/jwks", issuer: "localhost", TimeSpan.FromMinutes(20), audience: "System"));
});


// Add Identity Framework Core..
builder.Services.AddIdentityCore<UsersDTO>(
    options =>
    {
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
        options.Password.RequiredLength = 8;
        options.Password.RequireNonAlphanumeric = false;
    })

    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<KodigosContext>()
    .AddDefaultTokenProviders();

builder.Services.AddJwksManager(o =>
{
    o.Jws = Algorithm.Create(AlgorithmType.RSA, JwtType.Jws);
    o.Jwe = Algorithm.Create(AlgorithmType.RSA, JwtType.Jwe);
})
.PersistKeysToDatabaseStore<KodigosContext>();

builder.Services.AddMemoryCache();


var app = builder.Build();

Log.Information("Executing Migraitions Database");

using (var serviceScope = app.Services.GetService<IServiceScopeFactory>().CreateScope())
{
    var context = serviceScope.ServiceProvider.GetRequiredService<KodigosContext>();
    context.Database.Migrate();
}

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseJwksDiscovery(); 
app.UseCors();

app.MapControllers();

app.Run();

Log.Information("Builder FInish!");