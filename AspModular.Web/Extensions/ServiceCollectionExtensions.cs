using Microsoft.AspNetCore.Hosting;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Autofac.Extensions.DependencyInjection;
using Autofac;
using AspModular.Infrastructure;
using AspModular.Data;
using AspModular.Data.Abstractions;
using AspModular.Data.Identity;

namespace AspModular.Web.Extensions
{
    public static class ServiceCollectionExtensions
    {
        private const string _moduleFolder = "Modules";

        public static IServiceCollection AddCustomizedMvc(this IServiceCollection services, IList<ModuleInfo> modules)
        {
            var mvcBuilder = services.AddMvc()
                .AddRazorOptions(o =>
                {
                    foreach (var module in modules)
                    {
                        var previous = o.CompilationCallback;
                        o.CompilationCallback = context =>
                        {
                            previous?.Invoke(context);
                            context.Compilation = context.Compilation.AddReferences(MetadataReference.CreateFromFile(module.Assembly.Location));
                        };
                    }
                })
                .AddViewLocalization()
                .AddDataAnnotationsLocalization();

            foreach (var module in modules)
            {
                // Register controller from modules
                mvcBuilder.AddApplicationPart(module.Assembly);

                // Register dependency in modules
                try
                {
                    var moduleInitializerType =
                   module.Assembly.GetTypes().FirstOrDefault(x => typeof(IModuleInitializer).IsAssignableFrom(x));
                    if ((moduleInitializerType != null) && (moduleInitializerType != typeof(IModuleInitializer)))
                    {
                        var moduleInitializer = (IModuleInitializer)Activator.CreateInstance(moduleInitializerType);
                        moduleInitializer.Init(services);
                    }
                }
                catch (ReflectionTypeLoadException ex)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (Exception exSub in ex.LoaderExceptions)
                    {
                        sb.AppendLine(exSub.Message);
                        var exFileNotFound = exSub as FileNotFoundException;
                        if (exFileNotFound != null)
                        {

                        }
                        sb.AppendLine();
                    }
                    string errorMessage = sb.ToString();
                }

            }

            return services;
        }

        public static IServiceCollection LoadInstalledModules(this IServiceCollection services, IHostingEnvironment hostingEnvironment)
        {
            var moduleRootFolder = new DirectoryInfo(Path.Combine(hostingEnvironment.ContentRootPath, _moduleFolder));
            var moduleFolders = moduleRootFolder.GetDirectories();

            foreach (var moduleFolder in moduleFolders)
            {
                var binFolder = new DirectoryInfo(Path.Combine(moduleFolder.FullName, "bin"));
                if (!binFolder.Exists)
                {
                    continue;
                }

                foreach (var file in binFolder.GetFileSystemInfos("*.dll", SearchOption.AllDirectories))
                {
                    Assembly assembly;
                    try
                    {
                        assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(file.FullName);
                    }
                    catch (FileLoadException)
                    {
                        // Get loaded assembly
                        assembly = Assembly.Load(new AssemblyName(Path.GetFileNameWithoutExtension(file.Name)));

                        if (assembly == null)
                        {
                            throw;
                        }
                    }

                    if (assembly.FullName.Contains(moduleFolder.Name))
                    {
                        GlobalConfiguration.Modules.Add(new ModuleInfo
                        {
                            Name = moduleFolder.Name,
                            Assembly = assembly,
                            Path = moduleFolder.FullName
                        });
                    }
                }
            }

            GlobalConfiguration.ContentRootPath = hostingEnvironment.ContentRootPath;
            GlobalConfiguration.WebRootPath = hostingEnvironment.WebRootPath;

            return services;
        }

        public static IServiceCollection AddCustomizedIdentity(this IServiceCollection services)
        {
            services.AddIdentity<User, Role>(configure => { configure.Cookies.ApplicationCookie.LoginPath = "/login"; })
                .AddEntityFrameworkStores<StorageContext, long>()
                .AddDefaultTokenProviders();
            return services;
        }

        public static IServiceCollection AddCustomizedDataStore(this IServiceCollection services, IConfigurationRoot configuration)
        {
            services.AddDbContext<StorageContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly("AspModular.Web")));

            return services;
        }

        public static IServiceProvider Build(this IServiceCollection services,
            IConfigurationRoot configuration, IHostingEnvironment hostingEnvironment)
        {
            var builder = new ContainerBuilder();
            builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>));
            builder.RegisterGeneric(typeof(RepositoryWithTypedId<,>)).As(typeof(IRepositoryWithTypedId<,>));

            foreach (var module in GlobalConfiguration.Modules)
            {
                builder.RegisterAssemblyTypes(module.Assembly).AsImplementedInterfaces();
            }

            builder.RegisterInstance(configuration);
            builder.RegisterInstance(hostingEnvironment);
            builder.Populate(services);
            var container = builder.Build();


            return container.Resolve<IServiceProvider>();
        }
    }
}
