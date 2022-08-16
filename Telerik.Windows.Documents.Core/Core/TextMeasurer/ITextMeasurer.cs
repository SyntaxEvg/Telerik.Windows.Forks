using System;
using Telerik.Windows.Documents.Core.Fonts;

namespace Telerik.Windows.Documents.Core.TextMeasurer
{
	public interface ITextMeasurer
	{
		TextMeasurementInfo MeasureText(TextProperties textProperties, FontProperties fontProperties);
	}
}
