using System;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Contexts
{
	struct StyleInfo
	{
		public string Name { get; set; }

		public int? BuiltInId { get; set; }

		public int FormattingRecordId { get; set; }

		public static bool operator ==(StyleInfo first, StyleInfo second)
		{
			return first.Equals(second);
		}

		public static bool operator !=(StyleInfo first, StyleInfo second)
		{
			return !first.Equals(second);
		}

		public override bool Equals(object obj)
		{
			if (obj == null || !(obj is StyleInfo))
			{
				return false;
			}
			StyleInfo styleInfo = (StyleInfo)obj;
			return TelerikHelper.EqualsOfT<string>(this.Name, styleInfo.Name) && TelerikHelper.EqualsOfT<int?>(this.BuiltInId, styleInfo.BuiltInId) && TelerikHelper.EqualsOfT<int>(this.FormattingRecordId, styleInfo.FormattingRecordId);
		}

		public override int GetHashCode()
		{
			return TelerikHelper.CombineHashCodes(this.Name.GetHashCodeOrZero(), this.BuiltInId.GetHashCodeOrZero(), this.FormattingRecordId.GetHashCodeOrZero());
		}

		public override string ToString()
		{
			return string.Format("Name: {0}; BuiltinId: {1}; FormattingRecordId: {2}", this.Name, this.BuiltInId, this.FormattingRecordId);
		}
	}
}
