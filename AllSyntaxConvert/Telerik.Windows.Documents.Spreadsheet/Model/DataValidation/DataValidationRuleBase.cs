using System;
using Telerik.Windows.Documents.Spreadsheet.Core.DataStructures;
using Telerik.Windows.Documents.Spreadsheet.PropertySystem;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model.DataValidation
{
	public abstract class DataValidationRuleBase : IDataValidationRule
	{
		public bool ShowInputMessage
		{
			get
			{
				return this.showInputMessage;
			}
		}

		public string InputMessageTitle
		{
			get
			{
				return this.inputMessageTitle;
			}
		}

		public string InputMessageContent
		{
			get
			{
				return this.inputMessageContent;
			}
		}

		public bool ShowErrorMessage
		{
			get
			{
				return this.showErrorMessage;
			}
		}

		public ErrorStyle ErrorStyle
		{
			get
			{
				return this.errorStyle;
			}
		}

		public string ErrorAlertTitle
		{
			get
			{
				return this.errorAlertTitle;
			}
		}

		public string ErrorAlertContent
		{
			get
			{
				return this.errorMessageContent;
			}
		}

		protected DataValidationRuleBase(DataValidationRuleContextBase context)
		{
			Guard.ThrowExceptionIfNull<DataValidationRuleContextBase>(context, "context");
			this.showInputMessage = context.ShowInputMessage;
			this.inputMessageTitle = context.InputMessageTitle;
			this.inputMessageContent = context.InputMessageContent;
			this.showErrorMessage = context.ShowErrorMessage;
			this.errorStyle = context.ErrorStyle;
			this.errorAlertTitle = context.ErrorAlertTitle;
			this.errorMessageContent = context.ErrorAlertContent;
		}

		public bool Evaluate(Worksheet worksheet, int rowIndex, int columnIndex, ICellValue cellValue)
		{
			Guard.ThrowExceptionIfNull<Worksheet>(worksheet, "worksheet");
			Guard.ThrowExceptionIfNull<ICellValue>(cellValue, "cellValue");
			ICompressedList<ICellValue> propertyValueCollection = worksheet.Cells.PropertyBag.GetPropertyValueCollection<ICellValue>(CellPropertyDefinitions.ValueProperty);
			long index = WorksheetPropertyBagBase.ConvertCellIndexToLong(rowIndex, columnIndex);
			ICellValue value = propertyValueCollection.GetValue(index);
			propertyValueCollection.SetValue(index, cellValue);
			bool result = false;
			try
			{
				result = this.EvaluateOverride(worksheet, rowIndex, columnIndex, cellValue);
			}
			finally
			{
				propertyValueCollection.SetValue(index, value);
			}
			return result;
		}

		protected abstract bool EvaluateOverride(Worksheet worksheet, int rowIndex, int columnIndex, ICellValue cellValue);

		public override bool Equals(object obj)
		{
			DataValidationRuleBase dataValidationRuleBase = obj as DataValidationRuleBase;
			return dataValidationRuleBase != null && (TelerikHelper.EqualsOfT<bool>(this.ShowInputMessage, dataValidationRuleBase.ShowInputMessage) && TelerikHelper.EqualsOfT<string>(this.InputMessageTitle, dataValidationRuleBase.InputMessageTitle) && TelerikHelper.EqualsOfT<string>(this.InputMessageContent, dataValidationRuleBase.InputMessageContent) && TelerikHelper.EqualsOfT<bool>(this.ShowErrorMessage, dataValidationRuleBase.ShowErrorMessage) && TelerikHelper.EqualsOfT<ErrorStyle>(this.ErrorStyle, dataValidationRuleBase.ErrorStyle) && TelerikHelper.EqualsOfT<string>(this.ErrorAlertTitle, dataValidationRuleBase.ErrorAlertTitle) && TelerikHelper.EqualsOfT<string>(this.ErrorAlertContent, dataValidationRuleBase.ErrorAlertContent)) && TelerikHelper.EqualsOfT<Type>(base.GetType(), dataValidationRuleBase.GetType());
		}

		public override int GetHashCode()
		{
			return TelerikHelper.CombineHashCodes(TelerikHelper.CombineHashCodes(this.ShowInputMessage.GetHashCodeOrZero(), this.InputMessageTitle.GetHashCodeOrZero(), this.InputMessageContent.GetHashCodeOrZero(), base.GetType().GetHashCodeOrZero()), TelerikHelper.CombineHashCodes(this.ShowErrorMessage.GetHashCodeOrZero(), this.ErrorStyle.GetHashCodeOrZero(), this.ErrorAlertTitle.GetHashCodeOrZero(), this.ErrorAlertContent.GetHashCodeOrZero()));
		}

		readonly bool showInputMessage;

		readonly string inputMessageTitle;

		readonly string inputMessageContent;

		readonly bool showErrorMessage;

		readonly ErrorStyle errorStyle;

		readonly string errorAlertTitle;

		readonly string errorMessageContent;
	}
}
