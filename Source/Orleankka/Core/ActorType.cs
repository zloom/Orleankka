using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Orleankka.Core
{
    using Utility;
    using Streams;

    class ActorType : IEquatable<ActorType>
    {
        static readonly Dictionary<string, ActorType> codes =
                    new Dictionary<string, ActorType>();

        static readonly Dictionary<Type, ActorType> types =
                    new Dictionary<Type, ActorType>();

        public static void Register(IEnumerable<Assembly> assemblies)
        {
            Register(assemblies.SelectMany(x => x.GetTypes()).ToArray());
        }

        static void Register(Type[] types)
        {
            var implementations = Implementations(types);
            var interfaces = Separated(Interfaces(types), implementations);

            Array.ForEach(interfaces, Register);
            Array.ForEach(implementations, Register);
        }

        static Type[] Interfaces(IEnumerable<Type> types)
        {
            return types.Where(x => x.IsInterface && typeof(IActor).IsAssignableFrom(x)).ToArray();
        }

        static Type[] Implementations(IEnumerable<Type> types)
        {
            return types.Where(x => x.IsClass && !x.IsAbstract && typeof(Actor).IsAssignableFrom(x)).ToArray();
        }

        static Type[] Separated(IEnumerable<Type> interfaces, IEnumerable<Type> implementations)
        {
            var implemented = new HashSet<Type>(implementations.SelectMany(x => x.GetInterfaces()));
            return interfaces.SkipWhile(i => implemented.Contains(i)).ToArray();
        }

        static void Register(Type actor)
        {
            var type = RegisterThis(actor);

            ActorInterface.Register(type);
            ActorPrototype.Register(type);
            ActorEndpointFactory.Register(type);

            Ref.Register(type);
            StreamSubscriptionMatcher.Register(type);
        }

        static ActorType RegisterThis(Type actor)
        {
            var type = ActorType.From(actor);
            var registered = codes.Find(type.Code);

            if (registered != null)
                throw new ArgumentException(
                    $"The type {actor} has been already registered " +
                    $"under the code {registered.Code}");

            codes.Add(type.Code, type);
            types.Add(actor, type);

            return type;
        }

        public static void Reset()
        {
            ResetThis();

            ActorInterface.Reset();
            ActorPrototype.Reset();
            ActorEndpointFactory.Reset();

            Ref.Reset();
            StreamSubscriptionMatcher.Reset();
        }

        static void ResetThis()
        {
            codes.Clear();
            types.Clear();
        }

        public readonly string Code;
        public readonly Type Interface;
        public readonly Type Implementation;

        ActorType(string code, Type @interface, Type implementation)
        {
            Code = code;
            Interface = @interface;
            Implementation = implementation;
        }

        public static ActorType Registered(Type type)
        {
            var result = types.Find(type);

            if (result == null)
                throw new InvalidOperationException(
                    $"Unable to map type '{type}' to the corresponding actor type. " +
                     "Make sure that you've registered the assembly containing this type");

            return result;
        }

        public static ActorType Registered(string code)
        {
            var result = codes.Find(code);

            if (result == null)
                throw new InvalidOperationException(
                    $"Unable to map code '{code}' to the corresponding actor type. " +
                     "Make sure that you've registered the assembly containing this type");

            return result;
        }

        public static ActorType From(Type type)
        {
            // figure out whether you deal with interface or concrete actor type
            // and get attributes accordingly, either from type or from it's interface
            // if it has separated interface of course; otherwise type <==> interface

            var @interface = InterfaceOf(type);
            var @implementation = ImplementationOf(type);
            var code = GetTypeCode(@interface);

            return new ActorType(code, @interface, @implementation);
        }

        static Type InterfaceOf(Type type)
        {
            return type;
        }

        static Type ImplementationOf(Type type)
        {
            return type;
        }

        static string GetTypeCode(Type type)
        {
            var customAttribute = type
                .GetCustomAttributes(typeof(ActorTypeCodeAttribute), false)
                .Cast<ActorTypeCodeAttribute>()
                .SingleOrDefault();

            return customAttribute != null
                    ? customAttribute.Code
                    : type.FullName;
        }

        public bool Equals(ActorType other)
        {
            return !ReferenceEquals(null, other) && (ReferenceEquals(this, other) 
                    || string.Equals(Code, other.Code));
        }

        public override bool Equals(object obj)
        {
            return !ReferenceEquals(null, obj) && (ReferenceEquals(this, obj) 
                    || obj.GetType() == GetType() && Equals((ActorType) obj));
        }

        public override int GetHashCode()
        {
            return Code.GetHashCode();
        }

        public static bool operator ==(ActorType left, ActorType right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ActorType left, ActorType right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return Code;
        }
    }

    static class ActorTypeActorSystemExtensions
    {
        internal static ActorRef ActorOf(this IActorSystem system, ActorType type, string id)
        {
            return system.ActorOf(ActorPath.From(type.Code, id));
        }
    }
}