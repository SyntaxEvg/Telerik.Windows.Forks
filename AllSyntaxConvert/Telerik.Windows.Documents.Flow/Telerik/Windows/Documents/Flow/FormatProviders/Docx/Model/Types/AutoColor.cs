using System;
using System.Windows.Media;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Types
{
	class AutoColor
	{
		public AutoColor(bool isAutomatic)
		{
			this.IsAutomatic = isAutomatic;
		}

		public AutoColor(Color color, bool isAutomatic)
		{
			this.Color = color;
			this.IsAutomatic = isAutomatic;
		}

		public Color Color { get; set; }

		public bool IsAutomatic { get; set; }
	}
}
