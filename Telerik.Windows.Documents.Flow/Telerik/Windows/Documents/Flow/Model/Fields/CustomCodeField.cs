using System;

namespace Telerik.Windows.Documents.Flow.Model.Fields
{
	public class CustomCodeField : Field
	{
		public CustomCodeField(RadFlowDocument document)
			: base(document)
		{
		}

		internal override FieldResult GetFieldResult(FieldUpdateContext context)
		{
			return null;
		}
	}
}
