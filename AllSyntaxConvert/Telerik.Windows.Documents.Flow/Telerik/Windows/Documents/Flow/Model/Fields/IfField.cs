using System;
using Telerik.Windows.Documents.Flow.Model.Editing;
using Telerik.Windows.Documents.Flow.Model.Fields.FieldCode;

namespace Telerik.Windows.Documents.Flow.Model.Fields
{
	public class IfField : ComparisonFieldBase
	{
		internal IfField(RadFlowDocument document)
			: base(document)
		{
		}

		internal override FieldResult GetFieldResult(FieldUpdateContext context)
		{
			FieldParameters parameters = context.Parameters;
			RadFlowDocumentEditor editor = context.Editor;
			ComparisonFieldResult comparisonFieldResult = ComparisonFieldBase.Compare(parameters.Comparison, editor.Document);
			string result = comparisonFieldResult.Result;
			if (!comparisonFieldResult.IsError)
			{
				if (comparisonFieldResult.CompareValue)
				{
					result = parameters.FirstArgument.Value;
				}
				else
				{
					result = parameters.SecondArgument.Value;
				}
			}
			return new FieldResult(result, comparisonFieldResult.IsError);
		}
	}
}
