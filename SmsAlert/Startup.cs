using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SmsAlert.Sms;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Text;

namespace SmsAlert
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddOptions();

            //启用日志
            services.AddLogging(logging => {
                logging.AddConsole()
                    .AddDebug()
                    .AddConfiguration(Configuration.GetSection("Logging"));
            });

            //HttpClient注册为单实例
            services.AddSingleton<HttpClient>();

            //注册短信接口实现
            services.AddSingleton<ISms, CMSmsDefaultImpl>();
            //.NET Core的System.Text.Encoding不包含除UTF-8外的codepage，而短信接口需要使用gb2312编码，故需引入System.Text.Encoding.CodePages包并在注册
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            //短信接口配置
            services.Configure<SmsAlertConfig>(Configuration.GetSection("SmsAlertConfig"));

            //注册swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Title = "SmsAlert",
                    Version = "v1",
                    Description = "短信告警微服务，使用移动信息机发送短信进行告警",
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "SmsAlert V1");
            });
        }
    }
}
