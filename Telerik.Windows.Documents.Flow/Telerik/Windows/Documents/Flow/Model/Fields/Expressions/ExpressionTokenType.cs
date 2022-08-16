using System;

namespace Telerik.Windows.Documents.Flow.Model.Fields.Expressions
{
	enum ExpressionTokenType
	{
		Number,
		Plus,
		Minus,
		Multiply,
		Divide,
		Power,
		Percent,
		Equal,
		LessThan,
		LessThanOrEqualTo,
		GreaterThan,
		GreaterThanOrEqualTo,
		NotEqual,
		ListSeparator,
		LeftParenthesis,
		RightParenthesis,
		Space,
		FunctionStart,
		Function,
		UnaryMinus,
		True,
		False,
		Bookmark
	}
}
