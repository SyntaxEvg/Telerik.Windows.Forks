using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Core.Fonts;
using Telerik.Windows.Documents.Fixed.Model.Common;
using Telerik.Windows.Documents.Fixed.Model.Fonts.Encoding;

namespace Telerik.Windows.Documents.Fixed.Model.Fonts
{
	abstract class CidFontBase : FontBase
	{
		public CidFontBase(string name, FontSource fontSource)
			: base(name, fontSource)
		{
			this.lang = new PdfProperty<string>();
			this.cidSet = new PdfProperty<byte[]>();
			this.widths = new PdfProperty<CidWidths<int>>();
			this.defaultWidth = new PdfProperty<int>();
			this.widthsVertical = new PdfProperty<CidWidths<CidWidthVertical>>();
			this.defaultWidthsVertical = new PdfProperty<CidWidthVertical>();
			this.cidToGidMapping = new PdfProperty<CidToGidMap>();
			this.cidSystemInfo = new PdfProperty<CidSystemInfo>(() => Telerik.Windows.Documents.Fixed.Model.Fonts.Encoding.CidSystemInfo.Default);
			this.encoding = new PdfProperty<CMapEncoding>();
		}

		internal override bool HasFontDescriptorProperties
		{
			get
			{
				return base.HasFontDescriptorProperties || this.Lang.HasValue || this.CidSet.HasValue;
			}
		}

		internal PdfProperty<string> Lang
		{
			get
			{
				return this.lang;
			}
		}

		internal PdfProperty<byte[]> CidSet
		{
			get
			{
				return this.cidSet;
			}
		}

		internal PdfProperty<CidWidths<int>> Widths
		{
			get
			{
				return this.widths;
			}
		}

		internal PdfProperty<int> DefaultWidth
		{
			get
			{
				return this.defaultWidth;
			}
		}

		internal PdfProperty<CidWidths<CidWidthVertical>> WidthsVertical
		{
			get
			{
				return this.widthsVertical;
			}
		}

		internal PdfProperty<CidWidthVertical> DefaultWidthsVertical
		{
			get
			{
				return this.defaultWidthsVertical;
			}
		}

		internal PdfProperty<CidToGidMap> CharIdToGlyphIdMapping
		{
			get
			{
				return this.cidToGidMapping;
			}
		}

		internal PdfProperty<CidSystemInfo> CidSystemInfo
		{
			get
			{
				return this.cidSystemInfo;
			}
		}

		internal PdfProperty<CMapEncoding> Encoding
		{
			get
			{
				return this.encoding;
			}
		}

		internal override FontSource ActualFontSource
		{
			get
			{
				return base.FontSource;
			}
		}

		internal override double GetWidth(int charCode)
		{
			double result;
			if (!this.TryGetWidthFromCache(charCode, out result))
			{
				if (this.DefaultWidth.HasValue)
				{
					result = (double)this.DefaultWidth.Value;
				}
				else
				{
					result = base.GetWidth(charCode);
				}
			}
			return result;
		}

		bool TryGetWidthFromCache(int charCode, out double width)
		{
			width = 0.0;
			bool result = false;
			if (this.Widths.HasValue)
			{
				this.EnsureWidthsCacheIsLoaded();
				result = this.charCodeToWidthCache.TryGetValue(charCode, out width);
			}
			return result;
		}

		void EnsureWidthsCacheIsLoaded()
		{
			if (this.charCodeToWidthCache == null)
			{
				this.charCodeToWidthCache = new Dictionary<int, double>();
				foreach (CidWidthsRange<int> cidWidthsRange in this.Widths.Value)
				{
					int num = cidWidthsRange.StartCharCode;
					foreach (int num2 in cidWidthsRange.Widths)
					{
						this.charCodeToWidthCache[num] = (double)num2;
						num++;
					}
				}
			}
		}

		readonly PdfProperty<string> lang;

		readonly PdfProperty<byte[]> cidSet;

		readonly PdfProperty<CidWidths<int>> widths;

		readonly PdfProperty<int> defaultWidth;

		readonly PdfProperty<CidWidths<CidWidthVertical>> widthsVertical;

		readonly PdfProperty<CidWidthVertical> defaultWidthsVertical;

		readonly PdfProperty<CidToGidMap> cidToGidMapping;

		readonly PdfProperty<CidSystemInfo> cidSystemInfo;

		readonly PdfProperty<CMapEncoding> encoding;

		Dictionary<int, double> charCodeToWidthCache;
	}
}
