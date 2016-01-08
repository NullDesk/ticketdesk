// TicketDesk - Attribution notice
// Original Code By:
//       zvolkov - http://www.zvolkov.com/clog/2012/04/01/an-improved-comma-separated-values-array-model-binder/
//    
// Contributor(s):
//
//      Stephen Redd (https://github.com/stephenredd)
//
// This file is distributed under the terms of the Microsoft Public 
// License (Ms-PL). See http://opensource.org/licenses/MS-PL
// for the complete terms of use. 
//
// For any distribution that contains code from this file, this notice of 
// attribution must remain intact, and a copy of the license must be 
// provided to the recipient.

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace TicketDesk.Web.Client
{
    public class CommaSeparatedModelBinder : DefaultModelBinder
    {
        private static readonly MethodInfo ToArrayMethod = typeof(Enumerable).GetMethod("ToArray");

        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            return BindCsv(bindingContext.ModelType, bindingContext.ModelName, bindingContext)
                    ?? base.BindModel(controllerContext, bindingContext);
        }

        protected override object GetPropertyValue(ControllerContext controllerContext, ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor, IModelBinder propertyBinder)
        {
            return BindCsv(propertyDescriptor.PropertyType, propertyDescriptor.Name, bindingContext)
                    ?? base.GetPropertyValue(controllerContext, bindingContext, propertyDescriptor, propertyBinder);
        }

        private object BindCsv(Type type, string name, ModelBindingContext bindingContext)
        {
            if (type.GetInterface(typeof(IEnumerable).Name) != null)
            {
                var actualValue = bindingContext.ValueProvider.GetValue(name);

                if (actualValue != null)
                {
                    var valueType = type.GetElementType() ?? type.GetGenericArguments().FirstOrDefault();

                    if (valueType != null && valueType.GetInterface(typeof(IConvertible).Name) != null)
                    {
                        var list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(valueType));

                        foreach (var splitValue in actualValue.AttemptedValue.Split(new[] { ',' }))
                        {
                            if (!String.IsNullOrWhiteSpace(splitValue))
                                list.Add(Convert.ChangeType(splitValue, valueType));
                        }

                        if (type.IsArray)
                            return ToArrayMethod.MakeGenericMethod(valueType).Invoke(this, new[] { list });
                        else
                            return list;
                    }
                }
            }

            return null;
        }
    }
}