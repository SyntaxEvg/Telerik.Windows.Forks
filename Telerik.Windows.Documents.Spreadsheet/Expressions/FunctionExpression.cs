using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Telerik.Windows.Documents.Spreadsheet.Core;
using Telerik.Windows.Documents.Spreadsheet.Expressions.Functions;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions
{
	public class FunctionExpression : RadExpression
	{
		internal FunctionBase Function
		{
			get
			{
				return this.function;
			}
		}

		internal bool IsValid
		{
			get
			{
				return this.function != null;
			}
		}

		internal FunctionExpression(string functionName, RadExpression[] arguments, Worksheet worksheet, int rowIndex, int columnIndex)
		{
			Guard.ThrowExceptionIfNullOrEmpty(functionName, "functionName");
			Guard.ThrowExceptionIfNull<RadExpression[]>(arguments, "args");
			this.worksheet = worksheet;
			this.rowIndex = rowIndex;
			this.columnIndex = columnIndex;
			this.functionName = functionName;
			this.arguments = arguments;
			this.function = FunctionManager.GetFunctionByName(this.functionName);
			if (this.IsValid && !this.function.IsArgumentNumberValid(arguments.Length))
			{
				throw new ExpressionException(SpreadsheetStrings.GeneralErrorMessage, "Spreadsheet_ErrorExpressions_GeneralErrorMessage", null);
			}
			this.functionExpressionToStrings = new Dictionary<CultureInfo, string>();
			base.AttachToChildrenEvent(this.arguments);
		}

		protected override RadExpression GetValueOverride()
		{
			RadExpression radExpression = ErrorExpressions.NameError;
			if (this.function != null)
			{
				FunctionEvaluationContext<RadExpression> context = new FunctionEvaluationContext<RadExpression>(this.arguments, this.worksheet, this.rowIndex, this.columnIndex);
				radExpression = this.function.Evaluate(context);
				CellReferenceRangeExpression cellReferenceRangeExpression = radExpression as CellReferenceRangeExpression;
				if (cellReferenceRangeExpression != null)
				{
					RadWeakEventListener<FunctionExpression, CellReferenceRangeExpression, EventArgs> radWeakEventListener = new RadWeakEventListener<FunctionExpression, CellReferenceRangeExpression, EventArgs>(this, cellReferenceRangeExpression);
					cellReferenceRangeExpression.ValueInvalidated += radWeakEventListener.OnEvent;
					radWeakEventListener.OnEventAction = new Action<FunctionExpression, object, EventArgs>(FunctionExpression.CellReferenceRangeExpressionValueInvalidatedWeakEventAction);
				}
			}
			return radExpression;
		}

		static void CellReferenceRangeExpressionValueInvalidatedWeakEventAction(FunctionExpression instance, object source, EventArgs e)
		{
			instance.InvalidateValue();
		}

		internal override RadExpression CloneAndTranslate(ExpressionCloneAndTranslateContext context)
		{
			RadExpression[] array = new RadExpression[this.arguments.Length];
			for (int i = 0; i < this.arguments.Length; i++)
			{
				array[i] = this.arguments[i].CloneAndTranslate(context);
			}
			return new FunctionExpression(this.functionName, array, context.Worksheet, context.RowIndex, context.ColumnIndex);
		}

		internal override void Translate(ExpressionTranslateContext context)
		{
			for (int i = 0; i < this.arguments.Length; i++)
			{
				this.arguments[i].Translate(context);
			}
		}

		internal override string ToString(SpreadsheetCultureHelper cultureInfo)
		{
			string text;
			if (!this.functionExpressionToStrings.TryGetValue(cultureInfo.CultureInfo, out text))
			{
				text = this.BuildExpressionToString(cultureInfo);
				this.functionExpressionToStrings[cultureInfo.CultureInfo] = text;
			}
			return text;
		}

		string BuildExpressionToString(SpreadsheetCultureHelper cultureInfo)
		{
			StringBuilder stringBuilder = new StringBuilder(this.functionName);
			stringBuilder.Append("(");
			for (int i = 0; i < this.arguments.Length; i++)
			{
				stringBuilder.Append(this.arguments[i].ToString(cultureInfo));
				if (i != this.arguments.Length - 1)
				{
					stringBuilder.Append(cultureInfo.ListSeparator);
				}
			}
			stringBuilder.Append(")");
			return stringBuilder.ToString();
		}

		internal override bool SimilarEquals(object obj)
		{
			FunctionExpression functionExpression = obj as FunctionExpression;
			if (functionExpression == null)
			{
				return false;
			}
			if (this.functionName != functionExpression.functionName || this.arguments.Length != functionExpression.arguments.Length)
			{
				return false;
			}
			for (int i = 0; i < this.arguments.Length; i++)
			{
				if (!this.arguments[i].SimilarEquals(functionExpression.arguments[i]))
				{
					return false;
				}
			}
			return true;
		}

		readonly string functionName;

		readonly FunctionBase function;

		readonly RadExpression[] arguments;

		readonly Worksheet worksheet;

		readonly int rowIndex;

		readonly int columnIndex;

		readonly Dictionary<CultureInfo, string> functionExpressionToStrings;
	}
}
