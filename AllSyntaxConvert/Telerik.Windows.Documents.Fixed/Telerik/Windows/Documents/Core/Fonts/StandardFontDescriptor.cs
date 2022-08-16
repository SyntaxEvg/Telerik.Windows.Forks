using System;

namespace Telerik.Windows.Documents.Core.Fonts
{
	class StandardFontDescriptor
	{
		public StandardFontDescriptor(double ascent, double descent)
		{
			this.ascent = ascent;
			this.descent = descent;
		}

		public double Ascent
		{
			get
			{
				return this.ascent;
			}
		}

		public double Descent
		{
			get
			{
				return this.descent;
			}
		}

		readonly double ascent;

		readonly double descent;
	}
}
