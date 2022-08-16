using System;
using Telerik.Documents.SpreadsheetStreaming.Utilities;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Model
{
	class NumberFormat
	{
		internal NumberFormat()
		{
		}

		internal NumberFormat(string numberFormatString)
		{
			this.NumberFormatString = numberFormatString;
		}

		public string NumberFormatString { get; set; }

		internal int? NumberFormatId { get; set; }

		public override bool Equals(object obj)
		{
			NumberFormat numberFormat = obj as NumberFormat;
			return numberFormat != null && ObjectExtensions.EqualsOfT<string>(this.NumberFormatString, numberFormat.NumberFormatString);
		}

		public override int GetHashCode()
		{
			return this.NumberFormatString.GetHashCodeOrZero();
		}
	}
}
