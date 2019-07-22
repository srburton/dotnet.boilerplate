using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

namespace App.Infra.Integration.RabbitMq.Extensions
{
    public static class TypeExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnumerable<Type> GetAssemblies(this Type type)
            => AppDomain.CurrentDomain
                        .GetAssemblies()
                        .SelectMany(a => a.GetTypes()
                        .Where(t => t.GetInterfaces().Contains(type)));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="interfalceType"></param>
        /// <param name="makeType"></param>
        /// <returns></returns>
        public static IEnumerable<Type> GetMakeGenericType(this Type interfalceType, Type makeType)
            => AppDomain.CurrentDomain
                        .GetAssemblies()
                        .SelectMany(a => a.GetTypes()
                        .Where(t => t.GetInterfaces().Contains(interfalceType.MakeGenericType(makeType))));

    }
}
