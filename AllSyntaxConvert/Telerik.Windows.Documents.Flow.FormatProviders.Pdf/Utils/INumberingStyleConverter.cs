using System;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Pdf.Utils
{
	public interface INumberingStyleConverter
	{
		string ConvertNumberToText(int number);
	}
}
