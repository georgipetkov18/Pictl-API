using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PictlData;
using PictlData.Middlewares;
using PictlData.Repositories;
using PictlData.Services;

namespace PictlAPI
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
            services.AddControllers();
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(this.Configuration.GetConnectionString("DefaultConnection")));

            services.Configure<AppSettings>(this.Configuration.GetSection("AppSettings"));

            services.AddTransient<IRepository, Repository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPhotosService, PhotosService>();
            services.AddScoped<IAlbumsService, AlbumsService>();
            services.AddScoped<ICategoriesService, CategoriesService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IRepository repo)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                repo.MigrateDatabase();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            //app.UseAuthorization();

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());   // TODO: Probably shouldn't allow everything

            app.UseMiddleware<JwtMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
