using System;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Parser
{
	class ArrayExpressionToken : ExpressionToken
	{
		public ExpressionToken[,] Array
		{
			get
			{
				return this.array;
			}
		}

		public ArrayExpressionToken(ExpressionToken[,] array, string value)
			: base(ExpressionTokenType.Array, value)
		{
			Guard.ThrowExceptionIfNull<ExpressionToken[,]>(array, "array");
			this.array = array;
		}

		readonly ExpressionToken[,] array;
	}
}
