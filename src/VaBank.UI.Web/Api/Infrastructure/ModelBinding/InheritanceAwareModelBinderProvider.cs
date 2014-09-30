using System;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace VaBank.UI.Web.Api.Infrastructure.ModelBinding
{
    public class InheritanceAwareModelBinderProvider : ModelBinderProvider
    {
        private readonly Func<IModelBinder> _modelBinderFactory;
        private readonly Type _modelType;

        public InheritanceAwareModelBinderProvider(Type modelType, IModelBinder modelBinder)
        {
            if (modelType == null)
            {
                throw new ArgumentNullException("modelType");
            }
            if (modelBinder == null)
            {
                throw new ArgumentNullException("modelBinder");
            }

            _modelType = modelType;
            _modelBinderFactory = () => modelBinder;
        }

        public override IModelBinder GetBinder(HttpConfiguration configuration, Type modelType)
        {
            return _modelType.IsAssignableFrom(modelType) ? _modelBinderFactory() : null;
        }
    }
}