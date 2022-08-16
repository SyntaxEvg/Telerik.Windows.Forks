using System;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Parser
{
	enum ExpressionTokenType
	{
		Number,
		Text,
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
		Ampersand,
		ListSeparator,
		Range,
		Intersection,
		Union,
		LeftParenthesis,
		RightParenthesis,
		Space,
		FunctionStart,
		Function,
		DefinedName,
		A1CellReference,
		A1CellReferenceRange,
		UnaryMinus,
		UnaryPlus,
		True,
		False,
		Array,
		Error,
		MissingValue
	}
}
