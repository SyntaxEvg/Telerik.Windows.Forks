using System;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions
{
	public abstract class ConstantExpression<T> : ConstantExpression
	{
		public T Value
		{
			get
			{
				return this.value;
			}
		}

		protected ConstantExpression(T value)
		{
			this.value = value;
		}

		internal override bool SimilarEquals(object obj)
		{
			ConstantExpression<T> constantExpression = obj as ConstantExpression<T>;
			return constantExpression != null && TelerikHelper.EqualsOfT<T>(this.Value, constantExpression.Value);
		}

		internal override string GetValueAsString(SpreadsheetCultureHelper cultureInfo)
		{
			T t = this.Value;
			return t.ToString();
		}

		internal override string ToString(SpreadsheetCultureHelper cultureInfo)
		{
			return this.GetValueAsString(cultureInfo);
		}

		readonly T value;
	}
}
