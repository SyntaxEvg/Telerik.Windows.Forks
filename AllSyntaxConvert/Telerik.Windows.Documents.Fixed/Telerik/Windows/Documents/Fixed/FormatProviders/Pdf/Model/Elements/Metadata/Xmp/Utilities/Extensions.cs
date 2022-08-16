using System;
using System.Globalization;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Metadata.Xmp.Utilities
{
	static class Extensions
	{
		public static string ToXmpDate(this DateTime date)
		{
			return date.ToString("o", CultureInfo.InvariantCulture);
		}
	}
}
