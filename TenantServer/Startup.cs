using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Common.MailKit;
using Entities.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using TenantServer.Extensions;

namespace TenantServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/configs/nlog.config"));
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureCors();
            services.ConfigureSwagger();
            services.ConfigureRedis(Configuration);
            services.ConfigureLoggerService();
            services.ConfigureSqlServerService(Configuration);
            services.ConfigureRepositoryWrapper();
            var mailConfig= JsonConfigurationReader.GetAppSettings<MailKitConfig>("mailkit", Path.Combine("Configs", "mailserver.json"));
            services.ConfigureMail(mailConfig);
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors("CorsPolicy");
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //??????????????????Swagger????JSON??????
            app.UseSwagger();
            //????????????????swagger-ui??????Swagger JSON??????
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "???????????????????????????? V1");
                c.RoutePrefix = string.Empty; //??????????Swagger
            });
        }
    }
}
