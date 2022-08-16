using System;
using System.Windows.Media;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.Model
{
	class RtfColor
	{
		public RtfColor(Color color, bool isAutomatic = false)
		{
			this.color = color;
			this.isAutomatic = isAutomatic;
		}

		public Color Color
		{
			get
			{
				return this.color;
			}
		}

		public bool IsAutomatic
		{
			get
			{
				return this.isAutomatic;
			}
		}

		readonly Color color;

		readonly bool isAutomatic;
	}
}
