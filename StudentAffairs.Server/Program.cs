var builder = WebApplication.CreateBuilder(args);

// Add MudBlazor services
builder.Services.AddMudServices();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddAutoMapper(typeof(MappingProfiles)); 

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddControllers();
builder.Services.AddLocalization();

builder.Services.AddTransient<IValidator<StudentToCreateDto>, StudentValidation>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});


var app = builder.Build();

#region DataBase Migration
using var scope = app.Services.CreateScope();

var service = scope.ServiceProvider;

var _dbContect = service.GetRequiredService<ApplicationDbContext>();

var loggerFactory = service.GetRequiredService<ILoggerFactory>();

try
{
    await _dbContect.Database.MigrateAsync();
    await ApplicationContextSeed.SeedDataAsync(_dbContect);
}
catch (Exception ex)
{
    var logger = loggerFactory.CreateLogger<Program>();
    logger.LogError(ex, "An error occurred while migrating the database.");
}
#endregion


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRequestLocalization(LocalizationHelper.GetLocalizationOptions(builder.Configuration));

app.UseAntiforgery();

app.MapControllers();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

await app.RunAsync();
