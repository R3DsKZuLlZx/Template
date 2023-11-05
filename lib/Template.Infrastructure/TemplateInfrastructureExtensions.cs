using System.Reflection;
using Foundatio.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Quartz;
using Template.Application;
using Template.Application.Common.Interfaces;
using Template.Application.Customers;
using Template.Infrastructure.Database;
using Template.Infrastructure.Database.Repositories;
using Template.Infrastructure.FileStorage;
using Template.Infrastructure.Jobs;

namespace Template.Infrastructure;

public static class TemplateInfrastructureExtensions
{
    public static IServiceCollection AddTemplateInfrastructure(
        this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        serviceCollection.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString(nameof(ApplicationDbContext)));
        });

        serviceCollection.ConfigureSingletonOption<FileStorageOptions>(configuration, nameof(FileStorageOptions));

        serviceCollection.AddScoped<IFileRepository, FileRepository>(provider =>
        {
            var options = provider.GetRequiredService<FileStorageOptions>();

            IFileStorage fileStorage = options.StorageType.ToLowerInvariant() switch
            {
                FileStorageOptions.Folder => new FolderFileStorage(builder => builder.Folder(options.FolderPath)),
                FileStorageOptions.Azure => new AzureFileStorage(builder => builder
                    .ConnectionString(options.AzureStorageConnectionString)
                    .ContainerName(options.AzureStorageContainerName)),
                _ => throw new NotSupportedException(),
            };

            return new FileRepository(fileStorage);
        });

        serviceCollection.AddQuartz(options =>
        {
            options.UseMicrosoftDependencyInjectionJobFactory();
        });
        
        serviceCollection.AddQuartzHostedService(options =>
        {
            options.AwaitApplicationStarted = true;
            options.WaitForJobsToComplete = true;
        });

        serviceCollection.AddQuartzJobs(Assembly.GetExecutingAssembly());

        serviceCollection.AddHealthChecks().AddCustomHealthChecks(Assembly.GetExecutingAssembly());

        serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
        serviceCollection.AddScoped<ICustomerRepository, CustomerRepository>();
        
        return serviceCollection;
    }
    
    private static IServiceCollection AddQuartzJobs(
        this IServiceCollection serviceCollection, 
        Assembly assembly)
    {
        var types = assembly
            .GetTypes()
            .Where(x => x.BaseType is not null)
            .Where(x => x.BaseType!.IsGenericType)
            .Where(x => x.BaseType!.GetGenericTypeDefinition() == typeof(BaseJob<>))
            .ToArray();
     
        foreach (var type in types)
        {
            serviceCollection.ConfigureOptions(type);
        }
     
        return serviceCollection;
    }
     
    private static IHealthChecksBuilder AddCustomHealthChecks(
        this IHealthChecksBuilder builder, 
        Assembly assembly)
    {
        var types = assembly
            .GetTypes()
            .Where(x => !x.IsAbstract)
            .Where(x => !x.IsInterface)
            .Where(x => !x.IsGenericTypeDefinition) 
            .Where(x => x.IsAssignableTo(typeof(IHealthCheck)))
            .ToArray();
             
        foreach (var type in types)
        {
            var registration = new HealthCheckRegistration(
                type.Name,
                provider => (IHealthCheck)ActivatorUtilities.GetServiceOrCreateInstance(provider, type),
                null,
                null);
            
            builder.Add(registration);
        }
             
        return builder;
    }
}
