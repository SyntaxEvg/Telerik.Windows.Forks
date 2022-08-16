using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;

namespace Telerik.Windows.Documents.Flow.Model.Fields.MailMerge
{
	static class PropertyValueExtractor
	{
		internal static object GetValue(object source, string propertyName)
		{
			if (source == null)
			{
				return null;
			}
			DynamicObject dynamicObject = source as DynamicObject;
			if (dynamicObject != null)
			{
				object result = null;
				SimpleGetMemberBinder binder = new SimpleGetMemberBinder(propertyName);
				dynamicObject.TryGetMember(binder, out result);
				return result;
			}
			ExpandoObject expandoObject = source as ExpandoObject;
			if (expandoObject != null)
			{
				object result2 = null;
				((IDictionary<string, object>)expandoObject).TryGetValue(propertyName, out result2);
				return result2;
			}
			PropertyInfo property = source.GetType().GetProperty(propertyName);
			if (property != null)
			{
				return property.GetValue(source, null);
			}
			return null;
		}
	}
}
