using System;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Object = System.Object;

namespace Modules.DiContainer
{
    public static class DependencyUtils
    {
        private const BindingFlags Flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;

        public static void InjectDependencies(object injectableObject)
        {
            if (!HasDependencies(injectableObject))
            {
                return;
            }
            
            var dependencies = GetDependencies(injectableObject)
                .Select(Container.Resolve)
                .ToArray();
            
            injectableObject
                .GetType()
                .GetMethods(Flags)
                .Single(m => m.GetCustomAttribute<InjectAttribute>() != null)
                .Log(injectableObject, dependencies)
                .Invoke(injectableObject, dependencies); 
        }
        
        public static Type[] GetDependencies(Object injectableObject) =>
            injectableObject.GetType()
                .GetMethods(Flags)
                .Single(m => m.GetCustomAttribute<InjectAttribute>() != null)
                .GetParameters()
                .Select(p => p.ParameterType)
                .ToArray();

        public static bool HasDependencies(Object service) =>
            service.GetType()
                .GetMethods(Flags)
                .Where(p => p.GetCustomAttribute<InjectAttribute>() != null)
                .Any(p => p.GetParameters().Length > 0);

        private static MethodInfo Log(this MethodInfo method, object injectable, object[] dependency)
        {
            var dependencies = string.Join(", ", dependency.Select(d => d.GetType().Name));
            Debug.Log($"[{nameof(Container)}] Inject: {dependencies} to method {method.Name} of {injectable.GetType().Name}");
            return method;
        }
    }
}