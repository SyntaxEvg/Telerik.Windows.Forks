using System;
using Telerik.Windows.Documents.Flow.Model.Fields.Expressions;

namespace Telerik.Windows.Documents.Flow.Model.Fields
{
	public class ExpressionField : Field
	{
		internal ExpressionField(RadFlowDocument document)
			: base(document)
		{
		}

		internal override FieldResult GetFieldResult(FieldUpdateContext context)
		{
			FieldResult result = null;
			if (context.Parameters.Expression != null)
			{
				Expression expression = ExpressionParser.Parse(context.Parameters.Expression, base.Document);
				ExpressionResult result2 = expression.GetResult();
				if (result2.Error == null)
				{
					result = new FieldResult(result2.Value.ToString());
				}
				else
				{
					result = new FieldResult("#ExpressionError", true);
				}
			}
			return result;
		}
	}
}
