using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Contexts;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Export
{
	class DifferentialFormatsExportContext
	{
		public DifferentialFormatsExportContext()
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

		public int RegisterFormatAndGetId(ThemableColor color)
		{
			if (this.formats.Contains(color))
			{
				return this.formats.IndexOf(color);
			}
			this.formats.Add(color);
			return this.formats.Count - 1;
		}

		public int RegisterFormatAndGetId(IFill fill)
		{
			if (this.formats.Contains(fill))
			{
				return this.formats.IndexOf(fill);
			}
			this.formats.Add(fill);
			return this.formats.Count - 1;
		}

		public int GetFormatId(ThemableColor color)
		{
			return this.formats.IndexOf(color);
		}

		public int GetFormatId(IFill fill)
		{
			return this.formats.IndexOf(fill);
		}

		public IEnumerable<DifferentialFormatInfo> GetRegisteredDifferentialFormats()
		{
			for (int i = 0; i < this.formats.Count; i++)
			{
				ThemableColor color = this.formats[i] as ThemableColor;
				if (color != null)
				{
					yield return new DifferentialFormatInfo
					{
						FontInfo = new FontInfo?(new FontInfo
						{
							ForeColor = color
						})
					};
				}
				IFill fill = this.formats[i] as IFill;
				if (fill != null)
				{
					DifferentialFormatInfo info = new DifferentialFormatInfo();
					if (fill == PatternFill.Default)
					{
						info.Fill = NoneFill.Instance;
					}
					else
					{
						info.Fill = fill;
					}
					yield return info;
				}
			}
			yield break;
		}

		readonly List<object> formats;
	}
}
