using System;
using Telerik.Windows.Documents.Core.Fonts.Type1.Type1Format;

namespace Telerik.Windows.Documents.Fixed.Model.Fonts
{
	class StandardFont : Type1Font
	{
		public StandardFont(string name, double ascent, double descent, double lineGap, bool isBold, bool isItalic, Type1FontSource fontSource)
			: base(name, fontSource)
		{
			base.Ascent.SetDefaultValueFunc(() => ascent / 1000.0);
			base.Descent.SetDefaultValueFunc(() => descent / 1000.0);
			this.lineGap = lineGap / 1000.0;
			this.isBold = isBold;
			base.IsItalic = isItalic;
		}

		internal override FontType Type
		{
			get
			{
				return FontType.Standard;
			}
		}

		internal override double LineGap
		{
			get
			{
				return this.lineGap;
			}
		}

		internal override bool IsBold
		{
			get
			{
				return this.isBold;
			}
		}

		readonly double lineGap;

		readonly bool isBold;
	}
}
