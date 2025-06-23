using System.Reflection;
using Attributes;

namespace Attributes
{

    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class InjectableAttribute(ServiceLifetime lifetime = ServiceLifetime.Transient) : Attribute
    {
        public ServiceLifetime Lifetime { get; } = lifetime;
    }
}

namespace Extensions
{

    public static class ServiceCollectionExtensions
    {
        public static void AddInjectableServices(this IServiceCollection services, Assembly assembly)
        {
            var typesWithAttribute = assembly.GetTypes()
                .Where(t => t.GetCustomAttributes<InjectableAttribute>() != null);

            foreach (var type in typesWithAttribute)
            {
                var attribute = type.GetCustomAttribute<InjectableAttribute>();

                if(attribute == null)
                {
                    continue;
                }
                
                switch(attribute.Lifetime)
                {
                    case ServiceLifetime.Singleton:
                        services.AddSingleton(type);
                        break;
                    case ServiceLifetime.Transient:
                        services.AddTransient(type);
                        break;
                    case ServiceLifetime.Scoped:
                        services.AddScoped(type);
                        break;
                }
            }
        }
    }
}