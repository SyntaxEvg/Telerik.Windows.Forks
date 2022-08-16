using System;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions
{
	public class EmptyExpression : ConstantExpression<string>
	{
		internal EmptyExpression()
			: base(string.Empty)
		{
		}

		public static readonly EmptyExpression Empty = new EmptyExpression();
	}
}
