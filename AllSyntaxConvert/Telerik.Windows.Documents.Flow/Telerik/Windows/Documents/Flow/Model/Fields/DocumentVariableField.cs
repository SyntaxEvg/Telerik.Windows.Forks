using System;
using Telerik.Windows.Documents.Flow.Model.Fields.FieldCode;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.Model.Fields
{
	public class DocumentVariableField : Field
	{
		internal DocumentVariableField(RadFlowDocument document)
			: base(document)
		{
		}

		public string Name { get; set; }

		internal override void LoadParametersOverride(FieldParameters parameters)
		{
			if (parameters.FirstArgument != null)
			{
				this.Name = parameters.FirstArgument.Value;
			}
		}

		internal override FieldResult GetFieldResult(FieldUpdateContext context)
		{
			Guard.ThrowExceptionIfNull<FieldUpdateContext>(context, "context");
			string result;
			bool isError;
			if (string.IsNullOrEmpty(this.Name))
			{
				result = DocumentVariableField.nameNotDefinedError;
				isError = true;
			}
			else if (context.Document.DocumentVariables.TryGetValue(this.Name, out result))
			{
				isError = false;
			}
			else
			{
				result = DocumentVariableField.missingDocumentVariableError;
				isError = true;
			}
			return new FieldResult(result, isError);
		}

		static readonly string nameNotDefinedError = "Error! Document Variable not defined.";

		static readonly string missingDocumentVariableError = "Error! No document variable supplied.";
	}
}
