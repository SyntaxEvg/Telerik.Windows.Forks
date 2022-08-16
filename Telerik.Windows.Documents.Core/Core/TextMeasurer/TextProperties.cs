using System;
using System.Text.RegularExpressions;
using System.Windows;

namespace Telerik.Windows.Documents.Core.TextMeasurer
{
	public class TextProperties
	{
		public TextProperties(string text, double size = 100.0, SubStringPosition subStringPosition = SubStringPosition.None)
		{
			this.size = size;
			this.subStringPosition = subStringPosition;
			this.flowDirection = (TextProperties.rtlRegex.IsMatch(text) ? FlowDirection.RightToLeft : FlowDirection.LeftToRight);
			if (this.flowDirection == FlowDirection.RightToLeft)
			{
				switch (subStringPosition)
				{
				case SubStringPosition.Start:
					text += '\u200d';
					break;
				case SubStringPosition.Middle:
					text = '\u200d' + text + '\u200d';
					break;
				case SubStringPosition.End:
					text = '\u200d' + text;
					break;
				}
			}
			this.text = text;
		}

		public SubStringPosition SubStringPosition
		{
			get
			{
				return this.subStringPosition;
			}
		}

		public double Size
		{
			get
			{
				return this.size;
			}
		}

		public string Text
		{
			get
			{
				return this.text;
			}
		}

		internal FlowDirection FlowDirection
		{
			get
			{
				return this.flowDirection;
			}
		}

		const string IsRtlLanguagePattern = "[\\p{IsArabic}\\p{IsHebrew}\\p{IsDevanagari}]";

		static readonly Regex rtlRegex = new Regex("[\\p{IsArabic}\\p{IsHebrew}\\p{IsDevanagari}]");

		readonly string text;

		readonly double size;

		readonly SubStringPosition subStringPosition;

		readonly FlowDirection flowDirection;
	}
}
