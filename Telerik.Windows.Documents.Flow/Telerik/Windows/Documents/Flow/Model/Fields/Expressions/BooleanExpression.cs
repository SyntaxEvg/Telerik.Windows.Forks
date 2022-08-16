using System;

namespace Telerik.Windows.Documents.Flow.Model.Fields.Expressions
{
	class BooleanExpression : Expression
	{
		BooleanExpression(bool value)
		{
			this.value = value;
		}

		public bool Value
		{
			get
			{
				return this.value;
			}
		}

		public override ExpressionResult GetResult()
		{
			return new ExpressionResult((double)(this.value ? 1 : 0));
		}

		public static readonly BooleanExpression True = new BooleanExpression(true);

		public static readonly BooleanExpression False = new BooleanExpression(false);

		readonly bool value;
	}
}
