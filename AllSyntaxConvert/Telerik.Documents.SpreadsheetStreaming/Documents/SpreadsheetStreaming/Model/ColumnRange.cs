using System;
using Telerik.Documents.SpreadsheetStreaming.Utilities;

namespace Telerik.Documents.SpreadsheetStreaming.Model
{
	class ColumnRange
	{
		public ColumnRange(int columnNumber)
		{
			this.FirstColumnNumber = columnNumber;
			this.LastColumnNumber = columnNumber;
		}

		public int FirstColumnNumber { get; set; }

		public int LastColumnNumber { get; set; }

		public bool? CustomWidth { get; set; }

		public double? Width { get; set; }

		public int? OutlineLevel { get; set; }

		public bool? Hidden { get; set; }

		public bool HasEqualColumnProperties(ColumnRange other)
		{
			return ObjectExtensions.EqualsOfT<bool?>(this.CustomWidth, other.CustomWidth) && ObjectExtensions.EqualsOfT<double?>(this.Width, other.Width) && ObjectExtensions.EqualsOfT<int?>(this.OutlineLevel, other.OutlineLevel) && ObjectExtensions.EqualsOfT<bool?>(this.Hidden, other.Hidden);
		}

		public void MergeWith(ColumnRange other)
		{
			this.FirstColumnNumber = System.Math.Min(this.FirstColumnNumber, other.FirstColumnNumber);
			this.LastColumnNumber = Math.Max(this.LastColumnNumber, other.LastColumnNumber);
		}
	}
}
