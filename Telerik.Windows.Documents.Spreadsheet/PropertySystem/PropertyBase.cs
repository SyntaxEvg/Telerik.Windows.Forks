using System;
using Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.PropertySystem
{
	abstract class PropertyBase<TValue, TContext> : IProperty<TValue>, IProperty where TContext : WorksheetCommandContextBase
	{
		IPropertyDefinition IProperty.PropertyDefinition
		{
			get
			{
				return this.propertyDefinition;
			}
		}

		protected IPropertyDefinition<TValue> PropertyDefinition
		{
			get
			{
				return this.propertyDefinition;
			}
		}

		public Worksheet Worksheet
		{
			get
			{
				return this.worksheet;
			}
		}

		public PropertyBase(Worksheet worksheet, IPropertyDefinition<TValue> propertyDefinition)
		{
			Guard.ThrowExceptionIfNull<Worksheet>(worksheet, "worksheet");
			Guard.ThrowExceptionIfNull<IPropertyDefinition<TValue>>(propertyDefinition, "propertyDefinition");
			this.worksheet = worksheet;
			this.propertyDefinition = propertyDefinition;
			this.setPropertyCommand = this.CreateSetPropertyCommand();
		}

		protected abstract UndoableWorksheetCommandBase<TContext> CreateSetPropertyCommand();

		protected abstract TContext CreateSetPropertyCommandContext(Worksheet worksheet, IPropertyDefinition<TValue> property, CellRange cellRange, TValue value);

		protected abstract TContext CreateClearPropertyCommandContext(Worksheet worksheet, IPropertyDefinition<TValue> property, CellRange cellRange);

		public TValue GetDefaultValue()
		{
			return this.propertyDefinition.DefaultValue;
		}

		public abstract RangePropertyValue<TValue> GetValue(CellRange cellRange);

		public void SetValue(CellRange cellRange, TValue value)
		{
			if (this.ValidateCellRange(cellRange))
			{
				TContext context = this.CreateSetPropertyCommandContext(this.Worksheet, this.propertyDefinition, cellRange, value);
				this.Worksheet.ExecuteCommand<TContext>(this.setPropertyCommand, context);
			}
		}

		public void ClearValue(CellRange cellRange)
		{
			if (this.ValidateCellRange(cellRange))
			{
				TContext context = this.CreateClearPropertyCommandContext(this.Worksheet, this.propertyDefinition, cellRange);
				this.Worksheet.ExecuteCommand<TContext>(this.setPropertyCommand, context);
			}
		}

		public void SetDefaultValue(CellRange cellRange)
		{
			if (this.ValidateCellRange(cellRange))
			{
				this.SetValue(cellRange, this.GetDefaultValue());
			}
		}

		protected abstract bool ValidateCellRange(CellRange cellRange);

		readonly Worksheet worksheet;

		readonly IPropertyDefinition<TValue> propertyDefinition;

		readonly UndoableWorksheetCommandBase<TContext> setPropertyCommand;
	}
}
