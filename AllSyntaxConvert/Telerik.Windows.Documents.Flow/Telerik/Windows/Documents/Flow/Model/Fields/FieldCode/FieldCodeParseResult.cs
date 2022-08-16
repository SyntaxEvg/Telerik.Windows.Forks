using System;

namespace Telerik.Windows.Documents.Flow.Model.Fields.FieldCode
{
	class FieldCodeParseResult
	{
		public FieldCodeParseResult(Field field, FieldParameters fieldParameters)
		{
			this.Field = field;
			this.FieldParameters = fieldParameters;
		}

		public Field Field { get; set; }

		public FieldParameters FieldParameters { get; set; }
	}
}
