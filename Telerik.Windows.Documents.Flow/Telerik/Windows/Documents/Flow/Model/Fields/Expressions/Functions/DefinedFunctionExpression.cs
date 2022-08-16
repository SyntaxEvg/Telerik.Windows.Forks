using System;

namespace Telerik.Windows.Documents.Flow.Model.Fields.Expressions.Functions
{
	class DefinedFunctionExpression : SingleArgumentFunctionExpression
	{
		public DefinedFunctionExpression(Expression[] operand)
			: base(operand)
		{
		}

		public override ExpressionResult GetResult()
		{
			double num = (double)((base.Argument.GetResult().Error == null) ? 1 : 0);
			if (num != 0.0 && base.Argument is BookmarkExpression)
			{
				BookmarkExpression bookmarkExpression = (BookmarkExpression)base.Argument;
				num = (double)((bookmarkExpression.Document.GetBookmarkByName(bookmarkExpression.BookmarkName) != null) ? 1 : 0);
			}
			return new ExpressionResult(num);
		}
	}
}
