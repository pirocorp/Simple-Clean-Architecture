namespace CleanArchitecture.Application.Common.Mappings;

using AutoMapper;

using System.Reflection;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        this.ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());
    }

    private void ApplyMappingsFromAssembly(Assembly assembly)
    {
        var mapFromType = typeof(IMapFrom<>);
        const string mappingMethodName = nameof(IMapFrom<object>.Mapping);

        bool HasInterface(Type t) 
            => t.IsGenericType && t.GetGenericTypeDefinition() == mapFromType;
        
        var types = assembly
            .GetExportedTypes()
            .Where(t => t.GetInterfaces().Any(HasInterface))
            .ToList();
        
        var argumentTypes = new Type[] { typeof(Profile) };

        foreach (var type in types)
        {
            var instance = Activator.CreateInstance(type);
            
            var methodInfo = type.GetMethod(mappingMethodName);

            if (methodInfo != null)
            {
                methodInfo.Invoke(instance, new object[] { this });
            }
            else
            {
                var interfaces = type
                    .GetInterfaces()
                    .Where(HasInterface)
                    .ToList();

                if (interfaces.Count <= 0)
                {
                    continue;
                }

                var methods = interfaces
                    .Select(@interface => @interface.GetMethod(mappingMethodName, argumentTypes))
                    .ToList();

                foreach (var interfaceMethodInfo in methods)
                {
                    interfaceMethodInfo?.Invoke(instance, new object[] { this });
                }
            }
        }
    }
}
