using System;
using Telerik.Windows.Documents.Flow.Model.Editing;
using Telerik.Windows.Documents.Flow.Model.Fields.FieldCode;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.Model.Fields
{
	class FieldUpdateContext
	{
		public FieldUpdateContext(FieldInfo fieldInfo)
		{
			Guard.ThrowExceptionIfNull<FieldInfo>(fieldInfo, "fieldInfo");
			this.FieldInfo = fieldInfo;
		}

		public FieldParameters Parameters { get; internal set; }

		public FieldInfo FieldInfo { get; set; }

		public RadFlowDocumentEditor Editor
		{
			get
			{
				if (this.editor == null)
				{
					this.editor = new RadFlowDocumentEditor(this.FieldInfo.Document);
				}
				return this.editor;
			}
		}

		public RadFlowDocument Document
		{
			get
			{
				return this.FieldInfo.Document;
			}
		}

		RadFlowDocumentEditor editor;
	}
}
