﻿using System;
using System.Reflection;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using NSwag.AspNet.Owin;
using Owin;

namespace WebApi
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            try
            {
                LogHelper.CreateLog($"Application {AppDomain.CurrentDomain.FriendlyName} start.");

                var config = new HttpConfiguration();

                app.UseSwaggerUi3(typeof(Startup).Assembly, settings =>
                {
                    // configure settings here
                    // settings.GeneratorSettings.*: Generator settings and extension points
                    // settings.*: Routing and UI settings
                    //settings.MiddlewareBasePath = "/swagger";
                    settings.GeneratorSettings.DefaultUrlTemplate = "api/{controller}/{action}/{id}";
                });

                Register(config);

                ConfigureOAuthTokenGeneration(app);

                app.UseWebApi(config);
                config.EnsureInitialized();
            }
            catch (Exception ex)
            {
                LogHelper.CreateLog(ex);
            }
        }

        public void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            InitAutofac();

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Filters.Add(new AuthorizeAttribute());
            //config.MessageHandlers.Add(new CustomMessageHandler());
        }

        private void InitAutofac()
        {
            var builder = new ContainerBuilder();

            // Get your HttpConfiguration.
            var config = GlobalConfiguration.Configuration;

            // Register your Web API controllers.
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            // OPTIONAL: Register the Autofac filter provider.
            builder.RegisterWebApiFilterProvider(config);

            // OPTIONAL: Register the Autofac model binder provider.
            builder.RegisterWebApiModelBinderProvider();

            // Set the dependency resolver to be Autofac.
            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}