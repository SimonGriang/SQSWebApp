using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using WebApp.ViewModelHandler;
using WebApp.Data;
using WebApp.DBSeeding;
using WebApp.Services;
using DeepL;

var builder = WebApplication.CreateBuilder(args);

var authKey = "f2981bee-344a-4a1f-b65f-877950fa3855:fx";

builder.Services.AddTransient<ITranslatorWrapper>(sp => new TranslatorWrapper(authKey));

builder.Services.AddTransient<ITranslationService, TranslationService>();

builder.Services.AddTransient<ITranslationRepository, TranslationRepository>();

builder.Services.AddTransient<ILanguageRepository, LanguageRepository>();

builder.Services.AddTransient<ICreateTranslationViewModelHandler, CreateTranslationViewModelHandler>();

builder.Services.AddDbContext<WebAppContext>(options =>
    options.UseNpgsql("Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=mypassword"));

builder.Services.AddControllersWithViews();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    LanguageSeeding.Initialize(services);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
