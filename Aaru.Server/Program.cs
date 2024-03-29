using Aaru.CommonTypes.Interop;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Prometheus;
using Version = Aaru.CommonTypes.Interop.Version;

namespace Aaru.Server;

public sealed class Program
{
    public static void Main(string[] args)
    {
        DateTime start;
        DateTime end;

        System.Console.Clear();

        System.Console.Write(
            "\u001b[32m                             .                ,,\n" +
            "\u001b[32m                          ;,.                  '0d.\n" +
            "\u001b[32m                        oc                       oWd                      \u001b[31m" +
            @"__/\\\\\\\\\\\\_____/\\\\\\\\\\\________/\\\\\\\\\_        " + "\n\u001b[0m" +
            "\u001b[32m                      ;X.                         'WN'                    \u001b[31m" +
            @" _\/\\\////////\\\__\/////\\\///______/\\\////////__       " + "\n\u001b[0m" +
            "\u001b[32m                     oMo                           cMM:                   \u001b[31m" +
            @"  _\/\\\______\//\\\_____\/\\\_______/\\\/___________      " + "\n\u001b[0m" +
            "\u001b[32m                    ;MM.                           .MMM;                  \u001b[31m" +
            @"   _\/\\\_______\/\\\_____\/\\\______/\\\_____________     " + "\n\u001b[0m" +
            "\u001b[32m                    NMM                             WMMW                  \u001b[31m" +
            @"    _\/\\\_______\/\\\_____\/\\\_____\/\\\_____________    " + "\n\u001b[0m" +
            "\u001b[32m                   'MMM                             MMMM;                 \u001b[31m" +
            @"     _\/\\\_______\/\\\_____\/\\\_____\//\\\____________   " + "\n\u001b[0m" +
            "\u001b[32m                   ,MMM:                           dMMMM:                 \u001b[31m" +
            @"      _\/\\\_______/\\\______\/\\\______\///\\\__________  " + "\n\u001b[0m" +
            "\u001b[32m                   .MMMW.                         :MMMMM.                 \u001b[31m" +
            @"       _\/\\\\\\\\\\\\/____/\\\\\\\\\\\____\////\\\\\\\\\_ " + "\n\u001b[0m" +
            "\u001b[32m                    XMMMW:    .:xKNMMMMMMN0d,    lMMMMMd                  \u001b[31m" +
            @"        _\////////////_____\///////////________\/////////__" + "\n\u001b[0m" +
            "\u001b[32m                    :MMMMMK; cWMNkl:;;;:lxKMXc .0MMMMMO\u001b[0m\n" +
            "\u001b[32m                   ..KMMMMMMNo,.             ,OMMMMMMW:,.                 \u001b[37;1m          Aaru Website\u001b[0m\n" +
            "\u001b[32m            .;d0NMMMMMMMMMMMMMMW0d:'    .;lOWMMMMMMMMMMMMMXkl.            \u001b[37;1m          Version \u001b[0m\u001b[33m{0}\u001b[37;1m-\u001b[0m\u001b[31m{1}\u001b[0m\n" +
            "\u001b[32m          :KMMMMMMMMMMMMMMMMMMMMMMMMc  WMMMMMMMMMMMMMMMMMMMMMMWk'\u001b[0m\n" +
            "\u001b[32m        ;NMMMMWX0kkkkO0XMMMMMMMMMMM0'  dNMMMMMMMMMMW0xl:;,;:oOWMMX;       \u001b[37;1m          Running under \u001b[35;1m{2}\u001b[37;1m, \u001b[35m{3}-bit\u001b[37;1m in \u001b[35m{4}-bit\u001b[37;1m mode.\u001b[0m\n" +
            "\u001b[32m       xMMWk:.           .c0MMMMMW'      OMMMMMM0c'..          .oNMO      \u001b[37;1m          Using \u001b[33;1m{5}\u001b[37;1m version \u001b[31;1m{6}\u001b[0m\n" +
            "\u001b[32m      OMNc            .MNc   oWMMk       'WMMNl. .MMK             ;KX.\u001b[0m\n" +
            "\u001b[32m     xMO               WMN     ;  .,    ,  ':    ,MMx               lK\u001b[0m\n" +
            "\u001b[32m    ,Md                cMMl     .XMMMWWMMMO      XMW.                :\u001b[0m\n" +
            "\u001b[32m    Ok                  xMMl     XMMMMMMMMc     0MW,\u001b[0m\n" +
            "\u001b[32m    0                    oMM0'   lMMMMMMMM.   :NMN'\u001b[0m\n" +
            "\u001b[32m    .                     .0MMKl ;MMMMMMMM  oNMWd\u001b[0m\n" +
            "\u001b[32m                            .dNW cMMMMMMMM, XKl\u001b[0m\n" +
            "\u001b[32m                                 0MMMMMMMMK\u001b[0m\n" +
            "\u001b[32m                                ;MMMMMMMMMMO                              \u001b[37;1m          Proudly presented to you by:\u001b[0m\n" +
            "\u001b[32m                               'WMMMMKxMMMMM0                             \u001b[34;1m          Natalia Portillo\u001b[0m\n" +
            "\u001b[32m                              oMMMMNc  :WMMMMN:\u001b[0m\n" +
            "\u001b[32m                           .dWMMM0;      dWMMMMXl.                        \u001b[37;1m          Thanks to all contributors, collaborators, translators, donators and friends.\u001b[0m\n" +
            "\u001b[32m               .......,cd0WMMNk:           c0MMMMMWKkolc:clodc'\u001b[0m\n" +
            "\u001b[32m                 .';loddol:'.                 ':oxkkOkkxoc,.\u001b[0m\n" +
            "\u001b[0m\n", Version.GetVersion(),
        #if DEBUG
            "DEBUG"
        #else
                          "RELEASE"
        #endif
          , DetectOS.GetPlatformName(DetectOS.GetRealPlatformID()),
            Environment.Is64BitOperatingSystem ? 64 : 32, Environment.Is64BitProcess ? 64 : 32,
            DetectOS.IsMono ? "Mono" : ".NET Core",
            DetectOS.IsMono ? Version.GetMonoVersion() : Version.GetNetCoreVersion());

        System.Console.WriteLine("\u001b[31;1mBuilding web application...\u001b[0m");

        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDbContext<AaruServerContext>(options => options.
                                                                    UseMySql(
                                                                        builder.Configuration.GetConnectionString(
                                                                            "DefaultConnection"),
                                                                        new
                                                                            MariaDbServerVersion(
                                                                                new System.Version(10, 4, 0))).
                                                                    UseLazyLoadingProxies());

        builder.Services.AddDefaultIdentity<IdentityUser>(options =>
        {
            options.SignIn.RequireConfirmedAccount = true;
            options.User.RequireUniqueEmail        = true;
        }).AddEntityFrameworkStores<AaruServerContext>();

        builder.Services.AddApplicationInsightsTelemetry();

        builder.Services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

        WebApplication app = builder.Build();

        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
        });

        app.UseHttpMetrics();

        if(builder.Environment.IsDevelopment())
            app.UseDeveloperExceptionPage();
        else
        {
            app.UseExceptionHandler("/Home/Error");

            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseDefaultFiles();
        app.UseStaticFiles();

        // Add other security headers
        app.UseMiddleware<SecurityHeadersMiddleware>();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute("areas",   "{area}/{controller=Home}/{action=Index}/{id?}");
            endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            endpoints.MapRazorPages();
        });

        app.Map("/metrics", metricsApp =>
        {
            metricsApp.UseMiddleware<BasicAuthMiddleware>("Aaru");

            // We already specified URL prefix in .Map() above, no need to specify it again here.
            metricsApp.UseMetricServer("");
        });

        using(IServiceScope scope = app.Services.CreateScope())
        {
            IServiceProvider services = scope.ServiceProvider;

            try
            {
                start = DateTime.Now;
                System.Console.WriteLine("\u001b[31;1mUpdating database with Entity Framework...\u001b[0m");
                AaruServerContext context = services.GetRequiredService<AaruServerContext>();
                context.Database.Migrate();
                end = DateTime.Now;

                System.Console.WriteLine("\u001b[31;1mTook \u001b[32;1m{0} seconds\u001b[31;1m...\u001b[0m",
                                         (end - start).TotalSeconds);

                start = DateTime.Now;
                System.Console.WriteLine("\u001b[31;1mSeeding Identity...\u001b[0m");
                Seeder.Seed(context, services);
                context.Database.Migrate();
                end = DateTime.Now;

                System.Console.WriteLine("\u001b[31;1mTook \u001b[32;1m{0} seconds\u001b[31;1m...\u001b[0m",
                                         (end - start).TotalSeconds);
            }
            catch(Exception ex)
            {
                System.Console.WriteLine("\u001b[31;1mCould not open database...\u001b[0m");
            #if DEBUG
                System.Console.WriteLine("\u001b[31;1mException: {0}\u001b[0m", ex.Message);
            #endif
                return;
            }
        }

        System.Console.WriteLine("\u001b[31;1mStarting web server...\u001b[0m");

        app.Run();
    }
}