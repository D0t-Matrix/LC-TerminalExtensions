using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Matrix.TerminalExtensions;

internal static class Utils
{
    /// <summary>
    /// Uses reflection to get the field value from an object.
    /// </summary>
    ///
    /// <param name="instance">The instance object.</param>
    /// <param name="fieldName">The field's name which is to be fetched.</param>
    ///
    /// <returns>The field value from the object.</returns>
    internal static T GetInstancedStructField<T>(object instance, string fieldName) where T : struct
    {
        const BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static;
        FieldInfo field = instance.GetType().GetField(fieldName, bindFlags);
        return field.GetValue(instance) is T result ? result : default;
    }

    /// <summary>
    /// Uses reflection to get the field value from an object.
    /// </summary>
    ///
    /// <param name="instance">The instance object.</param>
    /// <param name="fieldName">The field's name which is to be fetched.</param>
    ///
    /// <returns>The field value from the object.</returns>
    internal static T? GetInstancedClassField<T>(object instance, string fieldName) where T : class
    {
        const BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static;
        FieldInfo field = instance.GetType().GetField(fieldName, bindFlags);
        return field.GetValue(instance) is T result ? result : default;
    }



    /// <summary>
    /// Use reflection to get an invocable method from an object.
    /// </summary>
    /// 
    /// <param name="type">The instance type.</param>
    /// <param name="instance">The instance object.</param>
    /// <param name="methodName">The method's name which is to be fetched.</param>
    /// 
    /// <returns>The method as a delegate.</returns>
    internal static T GetInstanceMethod<T>(Type type, object instance, string methodName)
    {
        const BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static;
        MethodInfo method = type.GetMethod(methodName, bindFlags);
        return (T)(object)Delegate.CreateDelegate(typeof(T), instance, method);
    }
}
