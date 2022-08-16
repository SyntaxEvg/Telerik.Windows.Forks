using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.Model.Fields.Expressions.Operators
{
	static class OperatorInfos
	{
		static OperatorInfos()
		{
			OperatorInfos.UnaryMinus = OperatorInfos.RegisterOperatorInfo(ExpressionTokenType.UnaryMinus, new OperatorInfo("-", 5, OperatorAssociativity.Right));
			OperatorInfos.Percent = OperatorInfos.RegisterOperatorInfo(ExpressionTokenType.Percent, new OperatorInfo("%", 7, OperatorAssociativity.Left));
			OperatorInfos.Power = OperatorInfos.RegisterOperatorInfo(ExpressionTokenType.Power, new OperatorInfo("^", 10, OperatorAssociativity.Left));
			OperatorInfos.Multiply = OperatorInfos.RegisterOperatorInfo(ExpressionTokenType.Multiply, new OperatorInfo("*", 15, OperatorAssociativity.Left));
			OperatorInfos.Divide = OperatorInfos.RegisterOperatorInfo(ExpressionTokenType.Divide, new OperatorInfo("/", 15, OperatorAssociativity.Left));
			OperatorInfos.Plus = OperatorInfos.RegisterOperatorInfo(ExpressionTokenType.Plus, new OperatorInfo("+", 20, OperatorAssociativity.Left));
			OperatorInfos.Minus = OperatorInfos.RegisterOperatorInfo(ExpressionTokenType.Minus, new OperatorInfo("-", 20, OperatorAssociativity.Left));
			OperatorInfos.Equal = OperatorInfos.RegisterOperatorInfo(ExpressionTokenType.Equal, new OperatorInfo("=", 25, OperatorAssociativity.Left));
			OperatorInfos.LessThan = OperatorInfos.RegisterOperatorInfo(ExpressionTokenType.LessThan, new OperatorInfo("<", 25, OperatorAssociativity.Left));
			OperatorInfos.GreaterThan = OperatorInfos.RegisterOperatorInfo(ExpressionTokenType.GreaterThan, new OperatorInfo(">", 25, OperatorAssociativity.Left));
			OperatorInfos.LessThanOrEqualTo = OperatorInfos.RegisterOperatorInfo(ExpressionTokenType.LessThanOrEqualTo, new OperatorInfo("<=", 25, OperatorAssociativity.Left));
			OperatorInfos.GreaterThanOrEqualTo = OperatorInfos.RegisterOperatorInfo(ExpressionTokenType.GreaterThanOrEqualTo, new OperatorInfo(">=", 25, OperatorAssociativity.Left));
			OperatorInfos.NotEqual = OperatorInfos.RegisterOperatorInfo(ExpressionTokenType.NotEqual, new OperatorInfo("<>", 25, OperatorAssociativity.Left));
		}

		internal static OperatorInfo GetOperatorInfo(ExpressionTokenType tokenType)
		{
			OperatorInfo result;
			OperatorInfos.TokenTypeToOperatorInfo.TryGetValue(tokenType, out result);
			return result;
		}

		internal static bool IsOperator(ExpressionTokenType tokenType)
		{
			return OperatorInfos.GetOperatorInfo(tokenType) != null;
		}

		internal static OperatorAssociativity GetAssociativity(ExpressionTokenType tokenType)
		{
			return OperatorInfos.GetOperatorInfo(tokenType).Associativity;
		}

		internal static int ComparePrecedence(ExpressionTokenType tokenType1, ExpressionTokenType tokenType2)
		{
			return OperatorInfos.ComparePrecedence(OperatorInfos.GetOperatorInfo(tokenType1).Precedence, OperatorInfos.GetOperatorInfo(tokenType2).Precedence);
		}

		internal static int ComparePrecedence(int precedence1, int precedence2)
		{
			return precedence1 - precedence2;
		}

		static OperatorInfo RegisterOperatorInfo(ExpressionTokenType tokenType, OperatorInfo operatorInfo)
		{
			Guard.ThrowExceptionIfNull<OperatorInfo>(operatorInfo, "operatorInfo");
			OperatorInfos.TokenTypeToOperatorInfo.Add(tokenType, operatorInfo);
			return operatorInfo;
		}

		public static readonly OperatorInfo Range;

		public static readonly OperatorInfo Intersection;

		public static readonly OperatorInfo UnaryPlus;

		public static readonly OperatorInfo UnaryMinus;

		public static readonly OperatorInfo Percent;

		public static readonly OperatorInfo Power;

		public static readonly OperatorInfo Multiply;

		public static readonly OperatorInfo Divide;

		public static readonly OperatorInfo Plus;

		public static readonly OperatorInfo Minus;

		public static readonly OperatorInfo Ampersand;

		public static readonly OperatorInfo Equal;

		public static readonly OperatorInfo LessThan;

		public static readonly OperatorInfo GreaterThan;

		public static readonly OperatorInfo LessThanOrEqualTo;

		public static readonly OperatorInfo GreaterThanOrEqualTo;

		public static readonly OperatorInfo NotEqual;

		static readonly Dictionary<ExpressionTokenType, OperatorInfo> TokenTypeToOperatorInfo = new Dictionary<ExpressionTokenType, OperatorInfo>();
	}
}
