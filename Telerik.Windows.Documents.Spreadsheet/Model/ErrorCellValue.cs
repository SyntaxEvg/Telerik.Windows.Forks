using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Formatting;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model
{
	public class ErrorCellValue : CellValueBase<string>
	{
		public override CellValueType ValueType
		{
			get
			{
				return CellValueType.Error;
			}
		}

		ErrorCellValue(string value)
			: base(value)
		{
		}

		internal static ErrorCellValue GetErrorCellValue(string key)
		{
			Guard.ThrowExceptionIfNullOrEmpty(key, "key");
			return ErrorCellValue.errorCellValues[key];
		}

		internal static bool TryGetErrorCellValue(string key, out ErrorCellValue value)
		{
			Guard.ThrowExceptionIfNullOrEmpty(key, "key");
			string key2 = key.ToUpper(FormatHelper.DefaultSpreadsheetCulture.CultureInfo);
			return ErrorCellValue.errorCellValues.TryGetValue(key2, out value);
		}

		static ErrorCellValue CreateErrorCellValue(string value)
		{
			Guard.ThrowExceptionIfNullOrEmpty(value, "value");
			ErrorCellValue errorCellValue = new ErrorCellValue(value);
			ErrorCellValue.errorCellValues[value] = errorCellValue;
			return errorCellValue;
		}

		static readonly Dictionary<string, ErrorCellValue> errorCellValues = new Dictionary<string, ErrorCellValue>();

		public static readonly ErrorCellValue NullErrorCellValue = ErrorCellValue.CreateErrorCellValue("#NULL!");

		public static readonly ErrorCellValue DivisionByZeroErrorCellValue = ErrorCellValue.CreateErrorCellValue("#DIV/0!");

		public static readonly ErrorCellValue ValueErrorCellValue = ErrorCellValue.CreateErrorCellValue("#VALUE!");

		public static readonly ErrorCellValue ReferenceErrorCellValue = ErrorCellValue.CreateErrorCellValue("#REF!");

		public static readonly ErrorCellValue NameErrorCellValue = ErrorCellValue.CreateErrorCellValue("#NAME?");

		public static readonly ErrorCellValue NumberErrorCellValue = ErrorCellValue.CreateErrorCellValue("#NUM!");

		public static readonly ErrorCellValue NotAvailableErrorCellValue = ErrorCellValue.CreateErrorCellValue("#N/A");
	}
}
