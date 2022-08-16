using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Flow.Model.Fields.Expressions.Operators
{
	static class BinaryOperatorExpressionFactory
	{
		static BinaryOperatorExpressionFactory()
		{
			BinaryOperatorExpressionFactory.operators.Add(ExpressionTokenType.Plus, (Expression arg1, Expression arg2) => new AdditionExpression(arg1, arg2));
			BinaryOperatorExpressionFactory.operators.Add(ExpressionTokenType.Minus, (Expression arg1, Expression arg2) => new SubtractionExpression(arg1, arg2));
			BinaryOperatorExpressionFactory.operators.Add(ExpressionTokenType.Multiply, (Expression arg1, Expression arg2) => new MultiplicationExpression(arg1, arg2));
			BinaryOperatorExpressionFactory.operators.Add(ExpressionTokenType.Divide, (Expression arg1, Expression arg2) => new DivisionExpression(arg1, arg2));
			BinaryOperatorExpressionFactory.operators.Add(ExpressionTokenType.Power, (Expression arg1, Expression arg2) => new PowerExpression(arg1, arg2));
			BinaryOperatorExpressionFactory.operators.Add(ExpressionTokenType.Equal, (Expression arg1, Expression arg2) => new EqualExpression(arg1, arg2));
			BinaryOperatorExpressionFactory.operators.Add(ExpressionTokenType.NotEqual, (Expression arg1, Expression arg2) => new NotEqualExpression(arg1, arg2));
			BinaryOperatorExpressionFactory.operators.Add(ExpressionTokenType.LessThan, (Expression arg1, Expression arg2) => new LessThanExpression(arg1, arg2));
			BinaryOperatorExpressionFactory.operators.Add(ExpressionTokenType.LessThanOrEqualTo, (Expression arg1, Expression arg2) => new LessThanOrEqualToExpression(arg1, arg2));
			BinaryOperatorExpressionFactory.operators.Add(ExpressionTokenType.GreaterThan, (Expression arg1, Expression arg2) => new GreaterThanExpression(arg1, arg2));
			BinaryOperatorExpressionFactory.operators.Add(ExpressionTokenType.GreaterThanOrEqualTo, (Expression arg1, Expression arg2) => new GreaterThanOrEqualToExpression(arg1, arg2));
		}

		public static bool TryCreateExpression(ExpressionTokenType type, Func<Expression[]> getArgsFunc, out BinaryOperatorExpression result)
		{
			result = null;
			Func<Expression, Expression, BinaryOperatorExpression> func = null;
			if (BinaryOperatorExpressionFactory.operators.TryGetValue(type, out func))
			{
				Expression[] array = getArgsFunc();
				result = func(array[0], array[1]);
				return true;
			}
			return false;
		}

		static readonly Dictionary<ExpressionTokenType, Func<Expression, Expression, BinaryOperatorExpression>> operators = new Dictionary<ExpressionTokenType, Func<Expression, Expression, BinaryOperatorExpression>>();
	}
}
