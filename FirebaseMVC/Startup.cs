using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AlStudente.Auth;
using AlStudente.Repositories;

namespace AlStudente
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
            services.AddHttpClient();
            services.AddTransient<IFirebaseAuthService, FirebaseAuthService>();
            services.AddTransient<IUserProfileRepository, UserProfileRepository>();
            services.AddTransient<ITeacherRepository, TeacherRepository>();
            services.AddTransient<IStudentRepository, StudentRepository>();
            services.AddTransient<IInstrumentRepository, InstrumentRepository>();
            services.AddTransient<ILessonDayRepository, LessonDayRepository>();
            services.AddTransient<ILessonTimeRepository, LessonTimeRepository>();
            services.AddTransient<ILevelRepository, LevelRepository>();
            services.AddTransient<ITeacherNoteRepository, TeacherNoteRepository>();
            

            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => options.Cookie.SameSite = SameSiteMode.Strict);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
