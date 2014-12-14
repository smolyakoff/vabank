using System;
using System.Text.RegularExpressions;
using DotLiquid;
using Newtonsoft.Json.Linq;
using VaBank.Common.Validation;
using VaBank.Core.Common;

namespace VaBank.Core.Payments
{
    public class PaymentForm : Drop
    {
        private static readonly Regex PlaceholderRegex = new Regex(@"\{\{.+\}\}");

        private readonly JObject _container;

        public PaymentForm(JObject container)
        {
            Argument.NotNull(container, "container");
            _container = container;
        }

        public void MergeWith(JObject form)
        {
            if (form == null)
            {
                return;
            }
            _container.Merge(form);
        }

        public override object BeforeMethod(string method)
        {
            var token = _container.SelectToken(method);
            return token.ToString();
        }

        public T GetValue<T>(string key)
        {
            return _container[key].ToObject<T>();
        }

        public T RenderValueOrDefault<T>(string template)
        {
            if (string.IsNullOrEmpty(template))
            {
                return default(T);
            }
            try
            {
                string stringValue;
                if (PlaceholderRegex.IsMatch(template))
                {
                    var liquidTemplate = Template.Parse(template);
                    var hash = Hash.FromAnonymousObject(new
                    {
                        form = this
                    });
                    stringValue = liquidTemplate.Render(hash);
                }
                else
                {
                    stringValue = template;
                }
                return (T) Convert.ChangeType(stringValue, typeof (T));
            }
            catch (Exception ex)
            {
                throw new DomainException("Can't render value.", ex);
            }
        }

        public override string ToString()
        {
            return _container.ToString();
        }
    }
}
