using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Proteccion.TableroControl.Datos.DAO;
using Proteccion.TableroControl.Datos.DataContext;
using Proteccion.TableroControl.Dominio.Entidades;
using Proteccion.TableroControl.Dominio.Interfaces;
using Proteccion.TableroControl.Presentacion.Helpers;
using Proteccion.TableroControl.Presentacion.Hubs;
using Proteccion.TableroControl.Proxy.BL;
using Serilog;
using System;

namespace Proteccion.TableroControl.Presentacion
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
                options.Secure = CookieSecurePolicy.SameAsRequest;
            });

            services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApp(Configuration.GetSection("AzureConfig"));

            services.AddControllersWithViews(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            });
            services.AddRazorPages()
                 .AddMicrosoftIdentityUI();


            services.AddDbContext<TableroControlContext>(context =>
            {
                context.UseSqlServer(Configuration.GetConnectionString("TableroControl"));
            });

            // Envio de correo
            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
            services.Configure<AzureConfig>(Configuration.GetSection("AzureConfig"));
           
            services.AddTransient<IEmail, EnvioCorreoHelper>();
            services.AddTransient<IAzure, ApiHelper>();
            services.AddTransient<IParametroDatos, ParametroDatos>();
            services.AddTransient<IParametroProxy, ParametroProxy>();
            services.AddTransient<IOrigenDatos, OrigenDatos>();
            services.AddTransient<IOrigenDatosProxy, OrigenDatosProxy>();
            services.AddTransient<IComunDatos, ComunDatos>();
            services.AddTransient<IComunProxy, ComunProxy>();
            services.AddTransient<ITopicoDatos, TopicoDatos>();
            services.AddTransient<ITopicoProxy, TopicoProxy>();
            services.AddTransient<IEstadisticaDatos, EstadisticaDatos>();
            services.AddTransient<IEstadisticaProxy, EstadisticaProxy>();
            services.AddTransient<ILogEjecucionDatos, LogEjecucionDatos>();
            services.AddTransient<ILogEjecucionProxy, LogEjecucionProxy>();
            services.AddTransient<IValidacionDatos, ValidacionDatos>();
            services.AddTransient<IValidacionProxy, ValidacionProxy>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(60);
            });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IManagerSession, ManagerSession>();
            services.AddSignalR();




        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddSerilog();



            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(Configuration["UrlError"].ToString());
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy(); 
            app.UseAuthentication();
            app.UseSession();

            app.UseSignalR(routes =>
            {
                routes.MapHub<NotificacionHub>(Configuration["UrlHub"].ToString());
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Tablero}/{id?}");
            });
        }
    }
}
