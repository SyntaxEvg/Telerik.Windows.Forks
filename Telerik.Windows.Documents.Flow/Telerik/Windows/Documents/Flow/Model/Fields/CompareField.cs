using System;
using Telerik.Windows.Documents.Flow.Model.Editing;
using Telerik.Windows.Documents.Flow.Model.Fields.FieldCode;

namespace Telerik.Windows.Documents.Flow.Model.Fields
{
	public class CompareField : ComparisonFieldBase
	{
		internal CompareField(RadFlowDocument document)
			: base(document)
		{
		}

		internal override FieldResult GetFieldResult(FieldUpdateContext context)
		{
			FieldParameters parameters = context.Parameters;
			RadFlowDocumentEditor editor = context.Editor;
			return ComparisonFieldBase.Compare(parameters.Comparison, editor.Document);
		}
	}
}
