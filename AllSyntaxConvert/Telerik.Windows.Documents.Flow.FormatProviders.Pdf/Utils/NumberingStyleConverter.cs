using System;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Pdf.Utils
{
	class NumberingStyleConverter : INumberingStyleConverter
	{
		public NumberingStyleConverter(Func<int, string> converter)
		{
			Guard.ThrowExceptionIfNull<Func<int, string>>(converter, "converter");
			this.converter = converter;
		}

		public string ConvertNumberToText(int number)
		{
			return this.converter(number);
		}

		readonly Func<int, string> converter;
	}
}
