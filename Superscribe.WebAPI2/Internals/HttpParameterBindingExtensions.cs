namespace Superscribe.WebApi2.Internals
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http.Controllers;
    using System.Web.Http.ModelBinding;
    using System.Web.Http.ValueProviders;

    internal static class HttpParameterBindingExtensions
    {
        public static bool WillReadUri(this HttpParameterBinding parameterBinding)
        {
            if (parameterBinding == null)
            {
                throw new ArgumentNullException("parameterBinding");
            }

            IValueProviderParameterBinding valueProviderParameterBinding = parameterBinding as IValueProviderParameterBinding;
            if (valueProviderParameterBinding != null)
            {
                IEnumerable<ValueProviderFactory> valueProviderFactories = valueProviderParameterBinding.ValueProviderFactories;
                if (valueProviderFactories.Any())
                {
                    return true;
                }
            }

            return false;
        }
    }
}
