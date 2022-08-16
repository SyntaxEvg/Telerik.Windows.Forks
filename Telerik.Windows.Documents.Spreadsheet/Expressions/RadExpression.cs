using System;
using System.Collections.Generic;
using System.Globalization;
using Telerik.Windows.Documents.Spreadsheet.Expressions.Parser;
using Telerik.Windows.Documents.Spreadsheet.Formatting;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions
{
	public abstract class RadExpression
	{
		public RadExpression GetValue()
		{
			if (!this.Evaluate())
			{
				return ErrorExpressions.CyclicReference;
			}
			return this.value;
		}

		internal bool Evaluate()
		{
			if (this.state == ExpressionState.Invalid)
			{
				this.state = ExpressionState.Computing;
				this.value = this.GetValueOverride();
				this.state = ExpressionState.Valid;
			}
			else if (this.state == ExpressionState.Computing)
			{
				return false;
			}
			return true;
		}

		public string GetValueAsString()
		{
			return this.GetValueAsString(FormatHelper.DefaultSpreadsheetCulture);
		}

		public string GetValueAsString(CultureInfo cultureInfo)
		{
			return this.GetValueAsString(new SpreadsheetCultureHelper(cultureInfo));
		}

		public override string ToString()
		{
			return this.ToString(FormatHelper.DefaultSpreadsheetCulture);
		}

		public string ToString(CultureInfo cultureInfo)
		{
			return this.ToString(new SpreadsheetCultureHelper(cultureInfo));
		}

		protected abstract RadExpression GetValueOverride();

		protected internal void InvalidateValue()
		{
			if (this.state != ExpressionState.Invalid)
			{
				this.state = ExpressionState.Invalid;
				this.OnValueInvalidated();
			}
		}

		protected void AttachToChildrenEvent(IEnumerable<RadExpression> childExpressions)
		{
			foreach (RadExpression childExpression in childExpressions)
			{
				this.AttachToChildEvent(childExpression);
			}
		}

		protected void AttachToChildEvent(RadExpression childExpression)
		{
			Guard.ThrowExceptionIfNull<RadExpression>(childExpression, "childExpression");
			if (!(childExpression is ConstantExpression))
			{
				childExpression.ValueInvalidated += this.ChildExpression_ValueInvalidated;
			}
		}

		internal static ParseResult TryParse(string value, Worksheet worksheet, CellIndex cellIndex, out RadExpression result)
		{
			return RadExpression.TryParse(value, worksheet, cellIndex.RowIndex, cellIndex.ColumnIndex, out result);
		}

		internal static ParseResult TryParse(string value, Worksheet worksheet, int rowIndex, int columnIndex, out RadExpression result)
		{
			InputStringCollection inputStringCollection;
			return RadExpression.TryParse(value, worksheet, rowIndex, columnIndex, out result, out inputStringCollection, false);
		}

		internal static ParseResult TryParse(string value, Worksheet worksheet, int rowIndex, int columnIndex, out RadExpression result, out InputStringCollection stringExpressionCollection, bool appendWorksheetName = false)
		{
			return RadExpression.TryParse(value, worksheet, rowIndex, columnIndex, FormatHelper.DefaultSpreadsheetCulture, out result, out stringExpressionCollection, appendWorksheetName);
		}

		internal static ParseResult TryParse(string value, Worksheet worksheet, int rowIndex, int columnIndex, SpreadsheetCultureHelper spreadsheetCultureInfo, out RadExpression result, out InputStringCollection stringExpressionCollection, bool appendWorksheetName = false)
		{
			Guard.ThrowExceptionIfNullOrEmpty(value, "value");
			Guard.ThrowExceptionIfNull<Worksheet>(worksheet, "worksheet");
			Guard.ThrowExceptionIfInvalidRowIndex(rowIndex);
			Guard.ThrowExceptionIfInvalidColumnIndex(columnIndex);
			Guard.ThrowExceptionIfNull<SpreadsheetCultureHelper>(spreadsheetCultureInfo, "spreadsheetCultureInfo");
			stringExpressionCollection = null;
			result = null;
			if (!RadExpression.StartsLikeExpression(value))
			{
				return ParseResult.Unsuccessful;
			}
			string input = SpreadsheetCultureHelper.ClearFormulaValue(value);
			if (!string.IsNullOrEmpty(input))
			{
				return ExpressionParser.ExpressionParserInstance.Parse(input, worksheet, rowIndex, columnIndex, spreadsheetCultureInfo, out stringExpressionCollection, out result, appendWorksheetName);
			}
			return ParseResult.Error;
		}

		internal static bool StartsLikeExpression(string value)
		{
			return !string.IsNullOrEmpty(value) && value.Length > 1 && SpreadsheetCultureHelper.IsCharEqualTo(value[0], new string[] { "=", "+", "-" });
		}

		internal virtual string GetValueAsString(SpreadsheetCultureHelper cultureInfo)
		{
			return this.GetValue().ToString(cultureInfo);
		}

		internal abstract string ToString(SpreadsheetCultureHelper cultureInfo);

		internal abstract RadExpression CloneAndTranslate(ExpressionCloneAndTranslateContext context);

		internal abstract void Translate(ExpressionTranslateContext context);

		void ChildExpression_ValueInvalidated(object sender, EventArgs e)
		{
			this.InvalidateValue();
		}

		internal virtual bool SimilarEquals(object obj)
		{
			RadExpression radExpression = obj as RadExpression;
			return radExpression != null && ((this.value == null && radExpression.value == null) || ((this.value == null || radExpression.value != null) && (this.value != null || radExpression.value == null) && this.value.Equals(radExpression.value)));
		}

		public event EventHandler ValueInvalidated;

		protected virtual void OnValueInvalidated()
		{
			if (this.ValueInvalidated != null)
			{
				this.ValueInvalidated(this, EventArgs.Empty);
			}
		}

		ExpressionState state;

		RadExpression value;
	}
}
