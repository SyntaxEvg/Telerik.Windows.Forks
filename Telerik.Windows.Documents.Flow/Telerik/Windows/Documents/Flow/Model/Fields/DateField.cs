using System;
using Telerik.Windows.Documents.Core.Sevices;
using Telerik.Windows.Documents.Flow.Model.Fields.Formatting;

namespace Telerik.Windows.Documents.Flow.Model.Fields
{
	public class DateField : Field
	{
		internal DateField(RadFlowDocument document)
			: base(document)
		{
		}

		internal override FieldResult GetFieldResult(FieldUpdateContext context)
		{
			DateTime dateTime = DateTimeService.GetDateTime();
			string result = DateTimeFormatter.FormatDate(dateTime, base.DateTimeFormatting);
			return new FieldResult(result);
		}
	}
}
