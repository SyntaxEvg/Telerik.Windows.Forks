using System;
using System.Windows;

namespace Telerik.Windows.Documents.Core.TextMeasurer
{
	public class TextMeasurementInfo
	{
		public static TextMeasurementInfo Empty
		{
			get
			{
				return new TextMeasurementInfo();
			}
		}

		public Size Size { get; set; }

		public double BaselineOffset { get; set; }
	}
}
