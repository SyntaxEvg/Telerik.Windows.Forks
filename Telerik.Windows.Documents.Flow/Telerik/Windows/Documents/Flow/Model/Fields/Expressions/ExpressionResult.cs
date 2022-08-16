using System;

namespace Telerik.Windows.Documents.Flow.Model.Fields.Expressions
{
	class ExpressionResult
	{
		public ExpressionResult(double value)
		{
			this.value = value;
			this.error = null;
		}

		public ExpressionResult(ExpressionException error)
		{
			this.error = error;
			this.value = 0.0;
		}

		public double Value
		{
			get
			{
				return this.value;
			}
		}

		public ExpressionException Error
		{
			get
			{
				return this.error;
			}
		}

		readonly double value;

		readonly ExpressionException error;
	}
}
