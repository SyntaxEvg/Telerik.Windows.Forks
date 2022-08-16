using System;

namespace Telerik.Windows.Documents.Flow.Model.Styles.Core
{
	abstract class StylePropertyBase<TValue> : IStyleProperty<TValue>, IStyleProperty
	{
		public StylePropertyBase(StylePropertyDefinition<TValue> propertyDefinition, DocumentElementPropertiesBase propertyContainer)
		{
			this.propertyDefinition = propertyDefinition;
			this.propertyContainer = propertyContainer;
		}

		public IStylePropertyDefinition PropertyDefinition
		{
			get
			{
				return this.propertyDefinition;
			}
		}

		public DocumentElementPropertiesBase PropertyContainer
		{
			get
			{
				return this.propertyContainer;
			}
		}

		public TValue DefaultValue
		{
			get
			{
				return this.propertyDefinition.DefaultValue;
			}
		}

		public virtual bool HasLocalValue
		{
			get
			{
				return this.LocalValue != null;
			}
		}

		public TValue LocalValue
		{
			get
			{
				return this.localValue;
			}
			set
			{
				if (!this.PropertyDefinition.Validation.IsValid(value))
				{
					throw new ArgumentException("Invalid value");
				}
				this.localValue = value;
			}
		}

		protected StylePropertyDefinition<TValue> PropertyDefinitionOfT
		{
			get
			{
				return this.propertyDefinition;
			}
		}

		public virtual void ClearValue()
		{
			this.localValue = default(TValue);
		}

		public abstract TValue GetActualValue();

		public object GetLocalValueAsObject()
		{
			return this.LocalValue;
		}

		public object GetActualValueAsObject()
		{
			return this.GetActualValue();
		}

		public void SetValueAsObject(object value)
		{
			this.LocalValue = (TValue)((object)value);
		}

		readonly StylePropertyDefinition<TValue> propertyDefinition;

		readonly DocumentElementPropertiesBase propertyContainer;

		TValue localValue;
	}
}
