using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Template.Application.Common.Behaviours;

namespace Template.Application;

public static class TemplateApplicationExtensions
{
    public static IServiceCollection AddTemplateApplication(
        this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        serviceCollection.AddMediatR(x => x.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        serviceCollection.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        
        serviceCollection.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
        serviceCollection.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        serviceCollection.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
        serviceCollection.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
        
        return serviceCollection;
    }
    
    public static IServiceCollection ConfigureSingletonOption<T>(
        this IServiceCollection serviceCollection,
        IConfiguration configuration,
        string section)
        where T : class, new()
    {
        return serviceCollection
            .AddOptions<T>()
            .Bind(configuration.GetSection(section))
            .ValidateDataAnnotations()
            .ValidateOnStart()
            .Services
            .AddSingleton(s => s.GetRequiredService<IOptions<T>>().Value);
    }
}
