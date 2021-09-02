//-----------------------------------------------------------------------
// <copyright>
// Copyright © Corniel Nobel 2019-current
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

namespace Grenadiers
{
    /// <summary>Provides utility methods to apply the nil (null) object pattern.</summary>
    public static class Nil
    {
        /// <summary>Represents the <see cref="System.Net.IPAddress"/> nil object (<see cref="IPAddress.None"/>).</summary>
        public static readonly IPAddress IPAddress = IPAddress.None;

        /// <summary>Represents the <see cref="string"/> nil object (<see cref="string.Empty"/>).</summary>
        public static readonly string String = string.Empty;

        /// <summary>Represents the <see cref="System.Threading.Tasks.Task"/> nil object (<see cref="Task.CompletedTask"/>).</summary>
        public static readonly Task Task = Task.CompletedTask;

        /// <summary>Gets the nil object of a specific type.</summary>
        /// <typeparam name="T">
        /// The type to get the nil object for.
        /// </typeparam>
        public static T Object<T>() where T : class
            => (T)Instance(typeof(T));

        /// <summary>Gets the nil object of a specific type.</summary>
        public static object Object(Type type)
            => Guard.NotNull(type, nameof(type)).IsClass
            ? Instance(type)
            : throw new ArgumentException("Specified type is not a class.", nameof(type));

        /// <summary>Guards that the provided object is not null, by returning the nil object of needed.</summary>
        /// <typeparam name="T">
        /// the type of the object.
        /// </typeparam>
        /// <param name="obj">
        /// The object to guard for.
        /// </param>
        public static T OrNilObject<T>(this T obj) where T : class
            => obj ?? Object<T>();

        /// <summary>Guards that the provided object is not null, by returning the nil object of needed.</summary>
        /// <typeparam name="T">
        /// the type of the object.
        /// </typeparam>
        /// <param name="promise">
        /// The object to guard for.
        /// </param>
        public static async Task<T> OrNilObject<T>(this Task<T> promise, bool continueOnCapturedContext = false) where T : class
        {
            if (promise is null)
            {
                return Object<T>();
            }
            else
            {
                var result = await promise.ConfigureAwait(continueOnCapturedContext);
                return result.OrNilObject();
            }
        }

        /// <summary>Registers a factory to create a nil object for the specified type.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="factory"></param>
        public static void Register<T>(Func<T> factory) where T : class
            => factories[typeof(T)] = Guard.NotNull(factory, nameof(factory));

        /// <summary>Gets all registered types.</summary>
        public static IReadOnlyCollection<Type> Registered() => factories.Keys;

        private static object Instance(Type type)
            => Factory(type)
            ?? Fields(type)
            ?? Properties(type)
            ?? Methods(type)
            ?? throw NilObjectUnavailable.For(type);

        private static object Factory(Type type)
        => factories.TryGetValue(type, out var factory)
        ? factory()
        : default;

        private static object Fields(Type type)
           => FactoryNames
           .Select(name => Field(type, name))
           .FirstOrDefault(value => value is Type);

        private static object Methods(Type type)
           => FactoryNames
           .Select(name => Method(type, name))
           .FirstOrDefault(value => value is Type);

        private static object Properties(Type type)
            => FactoryNames
            .Select(name => Property(type, name))
            .FirstOrDefault(value => value is Type);

        private static object Field(Type type, string name)
        {
            if (type.GetField(name, FactoryBindings) is var field
                && Matches(type, field.FieldType))
            {
                var nil = field.GetValue(null);
                factories[type] = () => field.GetValue(null);
                return nil;
            }
            else { return default; }
        }

        private static object Method(Type type, string name)
        {
            if (type.GetMethod(name, FactoryBindings) is var method
                && Matches(type, method.ReturnType))
            {
                var nil = method.Invoke(null, Array.Empty<object>());
                factories[type] = () => method.Invoke(null, Array.Empty<object>());
                return nil;
            }
            else { return default; }
        }

        private static object Property(Type type, string name)
        {
            if (type.GetProperty(name, FactoryBindings) is var prop
                && Matches(type, prop.PropertyType))
            {
                var nil = prop.GetValue(null);
                factories[type] = () => prop.GetValue(null);
                return nil;
            }
            else { return default; }
        }

        private static bool Matches(Type available, Type requested) => requested.IsAssignableFrom(available);

        private static readonly Dictionary<Type, Func<object>> factories = new Dictionary<Type, Func<object>>()
        {
            { typeof(IPAddress), () => IPAddress },
            { typeof(string), () => String },
            { typeof(Task), () => Task },
        };

        private const BindingFlags FactoryBindings = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Static;
        private static readonly string[] FactoryNames = new[] { "Nil", "None", "Default", "Empty" };
    }
}
