using System;

namespace Telerik.Windows.Documents.Flow.Model.Fields
{
	public sealed class ParseErrorField : Field
	{
		internal ParseErrorField(RadFlowDocument document, string errorMessage)
			: base(document)
		{
			this.ErrorMessage = errorMessage;
		}

		public string ErrorMessage { get; set; }

		internal override FieldResult GetFieldResult(FieldUpdateContext context)
		{
			return null;
		}
	}
}
