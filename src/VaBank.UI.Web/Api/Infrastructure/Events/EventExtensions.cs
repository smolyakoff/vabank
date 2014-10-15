using System;
using System.Linq;
using VaBank.Common.Events;

namespace VaBank.UI.Web.Api.Infrastructure.Events
{
    public static class EventExtensions
    {
        public static bool CanHandle<TEvent>(this Type handlerType) where TEvent : IEvent
        {
            if (!IsHandler(handlerType))
                throw new InvalidCastException();
            return handlerType.GenericTypeArguments.First().IsAssignableFrom<TEvent>();
        }

        public static bool IsHandler(this Type type)
        {
            return typeof(IHandler<>).IsAssignableFrom(handlerType);
        }
    }
}