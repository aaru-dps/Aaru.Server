using System.Diagnostics;
using Aaru.CommonTypes.Interop;
using Aaru.Server;
using Aaru.Server.Components;
using Aaru.Server.Components.Account;
using Blazorise;
using Blazorise.Bootstrap5;
using Blazorise.Icons.FontAwesome;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DbContext = Aaru.Server.Database.DbContext;
using Version = System.Version;

Console.Clear();

// ReSharper disable once StringLiteralTypo
Console.Write("""
              [32m                             .                ,,
              [32m                          ;,.                  '0d.
              [32m                        oc                       oWd                      [31m  ▄▄▄      ▄▄▄       ██▀███   █    ██
              [0m[32m                      ;X.                         'WN'                    [31m ▒████▄   ▒████▄    ▓██ ▒ ██▒ ██  ▓██▒
              [0m[32m                     oMo                           cMM:                   [31m ▒██  ▀█▄ ▒██  ▀█▄  ▓██ ░▄█ ▒▓██  ▒██░
              [0m[32m                    ;MM.                           .MMM;                  [31m ░██▄▄▄▄██░██▄▄▄▄██ ▒██▀▀█▄  ▓▓█  ░██░
              [0m[32m                    NMM                             WMMW                  [31m  ▓█   ▓██▒▓█   ▓██▒░██▓ ▒██▒▒▒█████▓
              [0m[32m                   'MMM                             MMMM;                 [31m  ▒▒   ▓▒█░▒▒   ▓▒█░░ ▒▓ ░▒▓░░▒▓▒ ▒ ▒
              [0m[32m                   ,MMM:                           dMMMM:                 [31m   ▒   ▒▒ ░ ▒   ▒▒ ░  ░▒ ░ ▒░░░▒░ ░ ░
              [0m[32m                   .MMMW.                         :MMMMM.                 [31m   ░   ▒    ░   ▒     ░░   ░  ░░░ ░ ░
              [0m[32m                    XMMMW:    .:xKNMMMMMMN0d,    lMMMMMd                  [31m       ░  ░     ░  ░   ░        ░
              [0m[32m                    :MMMMMK; cWMNkl:;;;:lxKMXc .0MMMMMO[0m
              [32m                   ..KMMMMMMNo,.             ,OMMMMMMW:,.                 [37;1m          Aaru Website[0m
              [32m            .;d0NMMMMMMMMMMMMMMW0d:'    .;lOWMMMMMMMMMMMMMXkl.            [37;1m          Version [0m[33m{0}[37;1m-[0m[31m{1}[0m
              [32m          :KMMMMMMMMMMMMMMMMMMMMMMMMc  WMMMMMMMMMMMMMMMMMMMMMMWk'[0m
              [32m        ;NMMMMWX0kkkkO0XMMMMMMMMMMM0'  dNMMMMMMMMMMW0xl:;,;:oOWMMX;       [37;1m          Running under [35;1m{2}[37;1m, [35m{3}-bit[37;1m in [35m{4}-bit[37;1m mode.[0m
              [32m       xMMWk:.           .c0MMMMMW'      OMMMMMM0c'..          .oNMO      [37;1m          Using [33;1m{5}[37;1m version [31;1m{6}[0m
              [32m      OMNc            .MNc   oWMMk       'WMMNl. .MMK             ;KX.[0m
              [32m     xMO               WMN     ;  .,    ,  ':    ,MMx               lK[0m
              [32m    ,Md                cMMl     .XMMMWWMMMO      XMW.                :[0m
              [32m    Ok                  xMMl     XMMMMMMMMc     0MW,[0m
              [32m    0                    oMM0'   lMMMMMMMM.   :NMN'[0m
              [32m    .                     .0MMKl ;MMMMMMMM  oNMWd[0m
              [32m                            .dNW cMMMMMMMM, XKl[0m
              [32m                                 0MMMMMMMMK[0m
              [32m                                ;MMMMMMMMMMO                              [37;1m          Proudly presented to you by:[0m
              [32m                               'WMMMMKxMMMMM0                             [34;1m          Natalia Portillo[0m
              [32m                              oMMMMNc  :WMMMMN:[0m
              [32m                           .dWMMM0;      dWMMMMXl.                        [37;1m          Thanks to all contributors, collaborators, translators, donators and friends.[0m
              [32m               .......,cd0WMMNk:           c0MMMMMWKkolc:clodc'[0m
              [32m                 .';loddol:'.                 ':oxkkOkkxoc,.[0m
              [0m

              """,
              Aaru.CommonTypes.Interop.Version.GetVersion(),
#if DEBUG
              "DEBUG",
#else
              "RELEASE",
#endif
              DetectOS.GetPlatformName(DetectOS.GetRealPlatformID()),
              Environment.Is64BitOperatingSystem ? 64 : 32,
              Environment.Is64BitProcess ? 64 : 32,
              DetectOS.IsMono ? "Mono" : ".NET Core",
              DetectOS.IsMono
                  ? Aaru.CommonTypes.Interop.Version.GetMonoVersion()
                  : Aaru.CommonTypes.Interop.Version.GetNetCoreVersion());

Console.WriteLine("\e[31;1mBuilding web application...\e[0m");

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents().AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

builder.Services.AddAuthentication(static options =>
        {
            options.DefaultScheme       = IdentityConstants.ApplicationScheme;
            options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
        })
       .AddIdentityCookies();

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                          throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContextFactory<DbContext>(options => options
                                                          .UseMySql(connectionString,
                                                                    new MariaDbServerVersion(new Version(10, 4, 0)))
                                                          .UseLazyLoadingProxies());

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<IdentityUser>(static options =>
        {
            options.SignIn.RequireConfirmedAccount = true;
            options.User.RequireUniqueEmail        = true;
        })
       .AddEntityFrameworkStores<DbContext>()
       .AddSignInManager()
       .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<IdentityUser>, IdentityNoOpEmailSender>();

builder.Services.AddBlazorise(static options => { options.Immediate = true; })
       .AddBootstrap5Providers()
       .AddFontAwesomeIcons();

// Add services to the container.
builder.Services.AddControllers();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if(app.Environment.IsDevelopment())
    app.UseMigrationsEndPoint();
else
{
    app.UseExceptionHandler("/Error", true);

    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.MapStaticAssets();
app.UseAntiforgery();

app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();
app.MapControllers();

using(IServiceScope scope = app.Services.CreateScope())
{
    IServiceProvider services = scope.ServiceProvider;

    try
    {
        Stopwatch stopwatch = new();

        stopwatch.Start();
        Console.WriteLine("\e[31;1mUpdating database with Entity Framework...\e[0m");
        DbContext context = services.GetRequiredService<DbContext>();
        await context.Database.MigrateAsync();
        stopwatch.Stop();

        Console.WriteLine("\e[31;1mTook \e[32;1m{0} seconds\e[31;1m...\e[0m", stopwatch.Elapsed.TotalSeconds);

        stopwatch.Restart();
        Console.WriteLine("\e[31;1mSeeding Identity...\e[0m");
        await Seeder.SeedAsync(context, services);
        await context.Database.MigrateAsync();
        stopwatch.Stop();

        Console.WriteLine("\e[31;1mTook \e[32;1m{0} seconds\e[31;1m...\e[0m", stopwatch.Elapsed.TotalSeconds);
    }
    catch(Exception ex)
    {
        Console.WriteLine("\e[31;1mCould not open database...\e[0m");
#if DEBUG
        Console.WriteLine("\e[31;1mException: {0}\e[0m", ex);
#endif
        return;
    }
}

Console.WriteLine("\e[31;1mStarting web server...\e[0m");

await app.RunAsync();