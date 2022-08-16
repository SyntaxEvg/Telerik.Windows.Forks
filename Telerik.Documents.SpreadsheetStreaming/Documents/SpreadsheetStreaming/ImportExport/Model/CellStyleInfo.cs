using System;
using Telerik.Documents.SpreadsheetStreaming.Utilities;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Model
{
	class CellStyleInfo
	{
		internal CellStyleInfo()
		{
		}

		internal CellStyleInfo(string name, int formattingRecordId, int? builtInId = null)
		{
			this.Name = name;
			this.FormattingRecordId = formattingRecordId;
			this.BuiltInId = builtInId;
		}

		public string Name { get; internal set; }

		public int FormattingRecordId { get; internal set; }

		public int? BuiltInId { get; internal set; }

		public override bool Equals(object obj)
		{
			CellStyleInfo cellStyleInfo = obj as CellStyleInfo;
			return cellStyleInfo != null && (ObjectExtensions.EqualsOfT<string>(this.Name, cellStyleInfo.Name) && ObjectExtensions.EqualsOfT<int>(this.FormattingRecordId, cellStyleInfo.FormattingRecordId)) && ObjectExtensions.EqualsOfT<int?>(this.BuiltInId, cellStyleInfo.BuiltInId);
		}

		public override int GetHashCode()
		{
			return ObjectExtensions.CombineHashCodes(this.Name.GetHashCodeOrZero(), this.FormattingRecordId.GetHashCodeOrZero(), this.BuiltInId.GetHashCodeOrZero());
		}
	}
}
