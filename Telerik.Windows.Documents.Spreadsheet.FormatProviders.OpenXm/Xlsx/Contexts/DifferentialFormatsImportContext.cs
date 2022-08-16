using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Contexts
{
	class DifferentialFormatsImportContext
	{
		public DifferentialFormatsImportContext()
		{
			this.formats = new List<object>();
		}

		public int Count
		{
			get
			{
				return this.formats.Count;
			}
		}

		public void RegisterFill(IFill fill)
		{
			this.formats.Add(fill);
		}

		public void RegisterFont(FontInfo info)
		{
			this.formats.Add(info);
		}

		public IFill GetFillById(int index)
		{
			IFill fill = this.formats[index] as IFill;
			if (fill != null && fill is NoneFill)
			{
				return PatternFill.Default;
			}
			return fill;
		}

		public ThemableColor GetColorById(int index)
		{
			FontInfo? fontInfo = this.formats[index] as FontInfo?;
			if (fontInfo != null)
			{
				return fontInfo.Value.ForeColor;
			}
			return null;
		}

		readonly List<object> formats;
	}
}
