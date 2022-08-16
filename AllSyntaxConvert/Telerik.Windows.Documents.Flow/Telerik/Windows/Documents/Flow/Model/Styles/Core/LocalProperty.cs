using System;

namespace Telerik.Windows.Documents.Flow.Model.Styles.Core
{
	class LocalProperty<TValue> : StylePropertyBase<TValue>
	{
		public LocalProperty(StylePropertyDefinition<TValue> propertyDefinition, DocumentElementPropertiesBase propertyContainer)
			: base(propertyDefinition, propertyContainer)
		{
			base.LocalValue = base.PropertyDefinitionOfT.DefaultValue;
		}

		public override bool HasLocalValue
		{
			get
			{
				return true;
			}
		}

		public override TValue GetActualValue()
		{
			if (base.LocalValue == null)
			{
				return base.PropertyDefinitionOfT.DefaultValue;
			}
			return base.LocalValue;
		}

		public override void ClearValue()
		{
			base.LocalValue = base.PropertyDefinitionOfT.DefaultValue;
		}
	}
}
