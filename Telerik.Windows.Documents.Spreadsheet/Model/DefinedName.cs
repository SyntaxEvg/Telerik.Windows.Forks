using System;
using System.Collections.Generic;
using System.Text;
using Telerik.Windows.Documents.Spreadsheet.Copying;
using Telerik.Windows.Documents.Spreadsheet.Expressions;
using Telerik.Windows.Documents.Spreadsheet.Formatting;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model
{
	public class DefinedName : ISpreadsheetName, ICopyable<ISpreadsheetName>
	{
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		public SpreadsheetNameCollectionScope Scope
		{
			get
			{
				return this.scope;
			}
		}

		public string Comment
		{
			get
			{
				return this.comment;
			}
		}

		public string RefersTo
		{
			get
			{
				return this.formulaCellValue.GetValueAsString(CellValueFormat.GeneralFormat);
			}
		}

		public string Value
		{
			get
			{
				return this.GetDefinedNameValueAsString(int.MaxValue);
			}
		}

		public bool IsVisible
		{
			get
			{
				return this.isVisible;
			}
		}

		internal CellReferenceRangeExpression CellReferenceRangeExpression
		{
			get
			{
				return this.GetCellReferenceRangeExpression(this.formulaCellValue.Value);
			}
		}

		internal FormulaCellValue FormulaCellValue
		{
			get
			{
				return this.formulaCellValue;
			}
		}

		DefinedName(string name, SpreadsheetNameCollectionScope scope, FormulaCellValue cellValue, string comment, bool isVisible)
		{
			Guard.ThrowExceptionIfNullOrEmpty(name, "name");
			Guard.ThrowExceptionIfNull<SpreadsheetNameCollectionScope>(scope, "scope");
			Guard.ThrowExceptionIfNull<FormulaCellValue>(cellValue, "cellValue");
			this.name = name;
			this.scope = scope;
			this.formulaCellValue = cellValue;
			this.comment = comment;
			this.isVisible = isVisible;
		}

		CellReferenceRangeExpression GetCellReferenceRangeExpression(RadExpression expression)
		{
			CellReferenceRangeExpression cellReferenceRangeExpression = expression.GetValueAsConstantOrCellReference() as CellReferenceRangeExpression;
			if (cellReferenceRangeExpression != null && cellReferenceRangeExpression.IsValid)
			{
				return cellReferenceRangeExpression;
			}
			return null;
		}

		internal static DefinedName Create(string name, SpreadsheetNameCollectionScope scope, FormulaCellValue formulaCellValue, string comment, bool isVisible)
		{
			Guard.ThrowExceptionIfNullOrEmpty(name, "name");
			Guard.ThrowExceptionIfNull<SpreadsheetNameCollectionScope>(scope, "scope");
			Guard.ThrowExceptionIfNull<FormulaCellValue>(formulaCellValue, "formulaCellValue");
			if (DefinedName.IsNameValid(name))
			{
				return new DefinedName(name, scope, formulaCellValue, comment, isVisible);
			}
			return null;
		}

		internal string GetDefinedNameValueAsString(int maxStringLength)
		{
			RadExpression value = this.formulaCellValue.Value.GetValue();
			ArrayExpression arrayExpression = value as ArrayExpression;
			if (arrayExpression != null)
			{
				if (this.formulaCellValue.Value is CellReferenceRangeExpression)
				{
					return this.GetArrayExpressionString(arrayExpression, maxStringLength);
				}
				return DefinedName.ArrayString;
			}
			else
			{
				CellReferenceRangeExpression cellReferenceRangeExpression = value as CellReferenceRangeExpression;
				if (cellReferenceRangeExpression != null)
				{
					return this.GetCellReferenceExpressionString(cellReferenceRangeExpression, maxStringLength);
				}
				if (value is EmptyExpression)
				{
					return string.Empty;
				}
				return this.formulaCellValue.GetResultValueAsString(CellValueFormat.GeneralFormat);
			}
		}

		string GetCellReferenceExpressionString(CellReferenceRangeExpression expression, int maxStringValueLength)
		{
			RadExpression value = expression.GetValue();
			ArrayExpression arrayExpression = value as ArrayExpression;
			if (arrayExpression != null)
			{
				return this.GetArrayExpressionString(arrayExpression, maxStringValueLength);
			}
			return value.ToString(FormatHelper.DefaultSpreadsheetCulture);
		}

		string GetArrayExpressionString(ArrayExpression arrayExpression, int maxStringValueLength)
		{
			Guard.ThrowExceptionIfNull<ArrayExpression>(arrayExpression, "arrayExpression");
			if (arrayExpression.ColumnCount != 1 || arrayExpression.RowCount != 1)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("{");
				for (int i = 0; i < arrayExpression.RowCount; i++)
				{
					for (int j = 0; j < arrayExpression.ColumnCount; j++)
					{
						stringBuilder.Append("\"");
						stringBuilder.Append(arrayExpression[i, j].ToString(FormatHelper.DefaultSpreadsheetCulture));
						stringBuilder.Append("\"");
						if (j != arrayExpression.ColumnCount - 1)
						{
							stringBuilder.Append(FormatHelper.DefaultSpreadsheetCulture.ArrayListSeparator);
						}
					}
					if (i != arrayExpression.RowCount - 1)
					{
						stringBuilder.Append(FormatHelper.DefaultSpreadsheetCulture.ArrayRowSeparator);
					}
					if (stringBuilder.Length >= maxStringValueLength)
					{
						stringBuilder.Append("...");
						return stringBuilder.ToString();
					}
				}
				stringBuilder.Append("}");
				return stringBuilder.ToString();
			}
			if (arrayExpression[0, 0] is EmptyExpression)
			{
				return string.Empty;
			}
			ArrayExpression arrayExpression2 = arrayExpression[0, 0] as ArrayExpression;
			if (arrayExpression2 != null)
			{
				return this.GetArrayExpressionString(arrayExpression2, maxStringValueLength);
			}
			return this.formulaCellValue.GetResultValueAsString(CellValueFormat.GeneralFormat);
		}

		internal static bool IsNameValid(string name)
		{
			Guard.ThrowExceptionIfNullOrEmpty(name, "value");
			char c = name[0];
			if (!char.IsLetter(c) && c != '_' && c != '\\')
			{
				return false;
			}
			if (NameConverter.IsValidA1CellName(name))
			{
				return false;
			}
			HashSet<char> hashSet = new HashSet<char> { '%', '$', '!', '[', ']', '#', '<', '>', '=' };
			for (int i = 0; i < name.Length; i++)
			{
				if (hashSet.Contains(name[i]) || char.IsWhiteSpace(name[i]))
				{
					return false;
				}
			}
			double num;
			return !FormatHelper.DefaultSpreadsheetCulture.TryParseDouble(name, out num) && !DefinedName.AreNamesEqual(name, "R") && !DefinedName.AreNamesEqual(name, "C") && !DefinedName.AreNamesEqual(name, "TRUE") && !DefinedName.AreNamesEqual(name, "FALSE") && !DefinedName.AreNamesEqual(name, DefinedName.PrintAreaDefinedName) && !DefinedName.AreNamesEqual(name, DefinedName.FilterDefinedName);
		}

		internal static bool AreNamesEqual(string first, string second)
		{
			return string.Equals(first, second, StringComparison.CurrentCultureIgnoreCase);
		}

		ISpreadsheetName ICopyable<ISpreadsheetName>.Copy(CopyContext context)
		{
			context.SpreadsheetNameExistsInTargetWorkbook = context.TargetWorksheet.Workbook.Names.Contains(this.Name);
			SpreadsheetNameCollectionScope spreadsheetNameCollectionScope = ((ICopyable<SpreadsheetNameCollectionScope>)this.Scope).Copy(context);
			FormulaCellValue cellValue = ((ICopyable<FormulaCellValue>)this.FormulaCellValue).Copy(context);
			return new DefinedName(this.Name, spreadsheetNameCollectionScope, cellValue, this.Comment, this.IsVisible);
		}

		internal static readonly string PrintAreaDefinedName = "_xlnm.Print_Area";

		internal static readonly string FilterDefinedName = "_xlnm._FilterDatabase";

		static readonly string ArrayString = "{...}";

		readonly string name;

		readonly SpreadsheetNameCollectionScope scope;

		readonly FormulaCellValue formulaCellValue;

		readonly string comment;

		readonly bool isVisible;
	}
}
