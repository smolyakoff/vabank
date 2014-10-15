using System;
using System.Linq;
using VaBank.Common.Events;

namespace VaBank.UI.Web.Events
{
    public static class EventExtensions
    {
        public static bool CanHandle<TEvent>(this Type handlerType) where TEvent : IEvent
        {
            return
                handlerType.GetInterfaces()
                    .Any(
                        x =>
                            x.IsGenericType && x.GetGenericTypeDefinition() == typeof (IHandler<>) &&
                            x.GetGenericArguments().Contains(typeof (TEvent)));
        }

        public static bool IsHandler(this Type type)
        {
            return type.GetInterfaces().Any(x => x.IsGenericType
                                                 && x.GetGenericTypeDefinition() == typeof (IHandler<>));
        }
    }
}