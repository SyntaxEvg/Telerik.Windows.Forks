using System;

namespace Telerik.Windows.Documents.Flow.Model.Fields.Expressions
{
	class NumberExpression : Expression
	{
		public NumberExpression(double value)
		{
			this.value = value;
		}

		public double Value
		{
			get
			{
				return this.value;
			}
		}

		public override ExpressionResult GetResult()
		{
			return new ExpressionResult(this.Value);
		}

		readonly double value;
	}
}
