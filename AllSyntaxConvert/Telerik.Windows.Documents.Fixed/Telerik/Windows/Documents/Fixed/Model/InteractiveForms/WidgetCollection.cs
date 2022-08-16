using System;
using Telerik.Windows.Documents.Fixed.Model.Annotations;

namespace Telerik.Windows.Documents.Fixed.Model.InteractiveForms
{
	public class WidgetCollection<T> : WidgetCollectionBase<T> where T : Widget
	{
		internal WidgetCollection(IWidgetCreator<T> creator, Action<T> variableContentPropertiesInitializer)
			: base(creator)
		{
			this.initializeVariableContentProperties = variableContentPropertiesInitializer;
		}

		public T AddWidget()
		{
			T t = base.AddEmptyWidget();
			this.initializeVariableContentProperties(t);
			return t;
		}

		readonly Action<T> initializeVariableContentProperties;
	}
}
