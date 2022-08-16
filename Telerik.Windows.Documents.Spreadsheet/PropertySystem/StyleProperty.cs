using System;
using Telerik.Windows.Documents.Spreadsheet.Commands.WorkbookCommands;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.PropertySystem
{
	class StyleProperty<T> : StylePropertyBase
	{
		public bool IsValueSet
		{
			get
			{
				return this.isValueSet;
			}
		}

		public IPropertyDefinition<T> PropertyDefinition
		{
			get
			{
				return this.propertyDefinition;
			}
		}

		public override bool AffectsLayout
		{
			get
			{
				return this.propertyDefinition.AffectsLayout;
			}
		}

		internal StyleProperty(CellStyle cellStyle, IPropertyDefinition<T> propertyDefinition)
		{
			Guard.ThrowExceptionIfNull<CellStyle>(cellStyle, "cellStyle");
			Guard.ThrowExceptionIfNull<IPropertyDefinition<T>>(propertyDefinition, "propertyDefinition");
			this.cellStyle = cellStyle;
			this.propertyDefinition = propertyDefinition;
			this.value = propertyDefinition.DefaultValue;
		}

		public override object GetValueAsObject()
		{
			return this.GetValue();
		}

		public override void SetValue(object value)
		{
			this.SetValue((T)((object)value));
		}

		public T GetValue()
		{
			return this.value;
		}

		public void SetValue(T value)
		{
			if (!this.cellStyle.IsRemoved)
			{
				SetStylePropertyCommandContext context = new SetStylePropertyCommandContext(this.cellStyle.Workbook, this, this.value, value);
				this.cellStyle.Workbook.ExecuteCommand<SetStylePropertyCommandContext>(WorkbookCommands.SetStyleProperty, context);
				return;
			}
			this.SetValueInternal(value);
		}

		internal override void SetValueInternal(object value)
		{
			this.SetValueInternal((T)((object)value));
		}

		internal void SetValueInternal(T value)
		{
			this.isValueSet = true;
			if (!TelerikHelper.EqualsOfT<T>(this.value, value))
			{
				this.value = value;
				this.OnValueChanged();
			}
		}

		void OnValueChanged()
		{
			this.cellStyle.OnPropertyChanged(this.propertyDefinition);
		}

		readonly CellStyle cellStyle;

		readonly IPropertyDefinition<T> propertyDefinition;

		T value;

		bool isValueSet;
	}
}
