using System;
using System.Globalization;
using Telerik.Windows.Documents.Globalization;

namespace Telerik.Windows.Documents.Fixed.Utilities
{
	static class PdfHelper
	{
		public static OpenXmlCultureInfo CultureInfo
		{
			get
			{
				return PdfHelper.cultureInfo;
			}
		}

		static readonly OpenXmlCultureInfo cultureInfo = new OpenXmlCultureInfo(System.Globalization.CultureInfo.InvariantCulture);
	}
}
