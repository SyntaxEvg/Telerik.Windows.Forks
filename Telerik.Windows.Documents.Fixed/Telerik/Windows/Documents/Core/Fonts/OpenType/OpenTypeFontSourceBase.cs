using System;
using Telerik.Windows.Documents.Core.Fonts.Glyphs;
using Telerik.Windows.Documents.Core.Fonts.OpenType.Tables;
using Telerik.Windows.Documents.Core.Fonts.OpenType.Tables.TrueTypeOutlines;
using Telerik.Windows.Documents.Core.Fonts.OpenType.Tables.TrueTypeTables;
using Telerik.Windows.Documents.Core.Fonts.Type1.CFFFormat;

namespace Telerik.Windows.Documents.Core.Fonts.OpenType
{
	abstract class OpenTypeFontSourceBase : FontSource
	{
		public OpenTypeFontSourceBase(OpenTypeFontReader reader)
		{
			this.reader = reader;
			this.scaler = new OpenTypeGlyphScaler(this);
		}

		internal OpenTypeFontReader Reader
		{
			get
			{
				return this.reader;
			}
		}

		internal Outlines Outlines
		{
			get
			{
				Outlines value;
				lock (OpenTypeFontSourceBase.lockObject)
				{
					if (this.outlines == null)
					{
						lock (OpenTypeFontSourceBase.lockObject)
						{
							this.outlines = new Outlines?(this.GetOutlines());
						}
					}
					value = this.outlines.Value;
				}
				return value;
			}
		}

		internal CFFFontSource CFF
		{
			get
			{
				CFFFontSource result;
				lock (OpenTypeFontSourceBase.lockObject)
				{
					if (this.cff == null)
					{
						lock (OpenTypeFontSourceBase.lockObject)
						{
							this.cff = this.GetCFF();
						}
					}
					result = this.cff;
				}
				return result;
			}
		}

		internal CMap CMap
		{
			get
			{
				CMap result;
				lock (OpenTypeFontSourceBase.lockObject)
				{
					if (this.cMap == null)
					{
						lock (OpenTypeFontSourceBase.lockObject)
						{
							this.cMap = this.GetCMap();
						}
					}
					result = this.cMap;
				}
				return result;
			}
		}

		internal Head Head
		{
			get
			{
				Head result;
				lock (OpenTypeFontSourceBase.lockObject)
				{
					if (this.head == null)
					{
						lock (OpenTypeFontSourceBase.lockObject)
						{
							this.head = this.GetHead();
						}
					}
					result = this.head;
				}
				return result;
			}
		}

		internal HorizontalHeader HHea
		{
			get
			{
				HorizontalHeader result;
				lock (OpenTypeFontSourceBase.lockObject)
				{
					if (this.hHea == null)
					{
						lock (OpenTypeFontSourceBase.lockObject)
						{
							this.hHea = this.GetHHea();
						}
					}
					result = this.hHea;
				}
				return result;
			}
		}

		internal HorizontalMetrics HMtx
		{
			get
			{
				HorizontalMetrics result;
				lock (OpenTypeFontSourceBase.lockObject)
				{
					if (this.hMtx == null)
					{
						lock (OpenTypeFontSourceBase.lockObject)
						{
							this.hMtx = this.GetHMtx();
						}
					}
					result = this.hMtx;
				}
				return result;
			}
		}

		internal Kerning Kern
		{
			get
			{
				Kerning result;
				lock (OpenTypeFontSourceBase.lockObject)
				{
					if (this.kern == null)
					{
						lock (OpenTypeFontSourceBase.lockObject)
						{
							this.kern = this.GetKern();
						}
					}
					result = this.kern;
				}
				return result;
			}
		}

		internal GlyphSubstitution GSub
		{
			get
			{
				GlyphSubstitution result;
				lock (OpenTypeFontSourceBase.lockObject)
				{
					if (this.gSub == null)
					{
						lock (OpenTypeFontSourceBase.lockObject)
						{
							this.gSub = this.GetGSub();
						}
					}
					result = this.gSub;
				}
				return result;
			}
		}

		internal ushort GlyphCount
		{
			get
			{
				ushort value;
				lock (OpenTypeFontSourceBase.lockObject)
				{
					if (this.glyphCount == null)
					{
						lock (OpenTypeFontSourceBase.lockObject)
						{
							this.glyphCount = new ushort?(this.GetGlyphCount());
						}
					}
					value = this.glyphCount.Value;
				}
				return value;
			}
		}

		internal abstract Outlines GetOutlines();

		internal abstract CFFFontSource GetCFF();

		internal abstract CMap GetCMap();

		internal abstract Head GetHead();

		internal abstract HorizontalHeader GetHHea();

		internal abstract HorizontalMetrics GetHMtx();

		internal abstract Kerning GetKern();

		internal abstract GlyphSubstitution GetGSub();

		internal abstract ushort GetGlyphCount();

		internal ushort NumberOfHorizontalMetrics
		{
			get
			{
				return this.HHea.NumberOfHMetrics;
			}
		}

		internal OpenTypeGlyphScaler Scaler
		{
			get
			{
				return this.scaler;
			}
		}

		public override void GetAdvancedWidthOverride(Glyph glyph)
		{
			glyph.AdvancedWidth = this.scaler.GetAdvancedWidth(glyph.GlyphId, 1.0);
		}

		internal Script GetScript(uint tag)
		{
			return this.GSub.GetScript(tag);
		}

		internal Feature GetFeature(ushort index)
		{
			return this.GSub.GetFeature(index);
		}

		internal Lookup GetLookup(ushort index)
		{
			return this.GSub.GetLookup(index);
		}

		internal abstract GlyphData GetGlyphData(ushort glyphID);

		public override bool TryGetGlyphId(int unicode, out ushort glyphId)
		{
			return this.CMap.TryGetGlyphId(unicode, out glyphId);
		}

		public double GetLineHeight(double fontSize)
		{
			double units = (double)(Math.Abs(this.HHea.Ascender) + Math.Abs(this.HHea.Descender) + this.HHea.LineGap);
			return this.Scaler.FUnitsToPixels(units, fontSize);
		}

		public double GetBaselineOffset(double fontSize)
		{
			double units = (double)(Math.Abs(this.HHea.Ascender) + Math.Abs(this.HHea.LineGap));
			return this.Scaler.FUnitsToPixels(units, fontSize);
		}

		public override void InitializeGlyphOutlinesOverride(Glyph glyph, double fontSize)
		{
			this.scaler.GetScaleGlyphOutlines(glyph, fontSize);
		}

		public override double GetAscent()
		{
			return this.Scaler.FUnitsToPixels((int)this.HHea.Ascender, 1000.0);
		}

		public override double GetDescent()
		{
			return this.Scaler.FUnitsToPixels((int)this.HHea.Descender, 1000.0);
		}

		public override double GetLineGap()
		{
			return this.Scaler.FUnitsToPixels((int)this.HHea.LineGap, 1000.0);
		}

		internal void Write(FontWriter writer)
		{
			writer.WriteString(base.FontFamily);
			ushort num = 0;
			if (base.IsBold)
			{
				num |= 1;
			}
			if (base.IsItalic)
			{
				num |= 2;
			}
			writer.WriteUShort(num);
			writer.WriteUShort(this.GlyphCount);
			this.Head.Write(writer);
			this.CMap.Write(writer);
			this.HHea.Write(writer);
			this.HMtx.Write(writer);
			this.Kern.Write(writer);
			this.GSub.Write(writer);
		}

		static object lockObject = new object();

		readonly OpenTypeGlyphScaler scaler;

		readonly OpenTypeFontReader reader;

		Outlines? outlines;

		CFFFontSource cff;

		CMap cMap;

		Head head;

		HorizontalHeader hHea;

		HorizontalMetrics hMtx;

		Kerning kern;

		GlyphSubstitution gSub;

		ushort? glyphCount;
	}
}
