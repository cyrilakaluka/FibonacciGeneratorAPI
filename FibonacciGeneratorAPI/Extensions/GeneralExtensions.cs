using System;

namespace FibonacciGeneratorAPI.Extensions
{
    public static class GeneralExtensions
    {
        public static bool IsConcreteTypeOf(this Type type, Type typeOf)
        {
            return typeOf.IsAssignableFrom(type) && !(type.IsInterface || type.IsAbstract);
        }
    }
}
