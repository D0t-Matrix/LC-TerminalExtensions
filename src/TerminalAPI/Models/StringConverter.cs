using TerminalAPI.Attributes;
using System;
using System.Collections.Concurrent;
using System.Reflection;
using System.Diagnostics.CodeAnalysis;

namespace TerminalAPI.Models;

public static class StringConverter
{
    private static bool _initialized = false;

    public static ConcurrentDictionary<Type, StringConversionHandler> StringConverters { get; } = new();

    public static bool TryConvert(string value, Type type, [NotNullWhen(true)] out object? result)
    {
        if (!_initialized)
        {
            _initialized = true;
            RegisterFromType(typeof(DefaultStringConverters));
        }


        if (StringConverters.TryGetValue(type, out StringConversionHandler handler))
        {
            try
            {
                result = handler(value);
                return true;
            }
            catch (ArgumentException) { }
        }

        result = null;
        return false;
    }

    public static void RegisterFrom<T>(T instance, bool replaceExisting = true) where T : class
        => RegisterFromType(typeof(T), instance, replaceExisting);

    public static void RegisterFromType(Type type, object? instance = null, bool replaceExisting = true)
    {
        foreach(MethodInfo method in type.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic 
                                                    | BindingFlags.Public | BindingFlags.Static))
        {
            if (method.GetCustomAttribute<StringConverterAttribute>() is not null)
            {
                var parameters = method.GetParameters();
                if (parameters.Length == 1 && !(parameters[0].ParameterType != typeof(string)))
                {
                    var returnType = method.ReturnType;
                    object handler(string value) => method.Invoke(instance, new object[1]
                    {
                        value
                    });

                    if (replaceExisting || !StringConverters.ContainsKey(returnType))
                        StringConverters[returnType] = handler;
                }
            }
        }
    }
}
