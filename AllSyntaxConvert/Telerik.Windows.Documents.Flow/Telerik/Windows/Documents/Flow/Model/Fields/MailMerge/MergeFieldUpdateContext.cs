using System;

namespace Telerik.Windows.Documents.Flow.Model.Fields.MailMerge
{
	class MergeFieldUpdateContext : FieldUpdateContext
	{
		internal MergeFieldUpdateContext(FieldInfo fieldInfo, object currentRecord)
			: base(fieldInfo)
		{
			this.CurrentRecord = currentRecord;
		}

		internal object CurrentRecord { get; set; }
	}
}
