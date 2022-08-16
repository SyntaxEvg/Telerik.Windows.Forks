using System;

namespace Telerik.Windows.Documents.Flow.Model.Fields.Expressions.Operators
{
	abstract class OperatorExpression : Expression
	{
		public abstract OperatorInfo OperatorInfo { get; }
	}
}
