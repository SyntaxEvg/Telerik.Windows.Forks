using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Core.Data;
using Telerik.Windows.Documents.Core.Fonts;
using Telerik.Windows.Documents.Core.Fonts.Glyphs;
using Telerik.Windows.Documents.Fixed.Exceptions;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.Enums;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.PdfConverters;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Text;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Utils;
using Telerik.Windows.Documents.Fixed.Model.Internal;
using Telerik.Windows.Documents.Fixed.Model.Internal.Classes;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.Fonts
{
	abstract class FontBaseOld : PdfObjectOld
	{
		public FontBaseOld(PdfContentManager contentManager)
			: base(contentManager)
		{
			this.unicode = base.CreateInstantLoadProperty<CMapStream>(new PdfPropertyDescriptor
			{
				Name = "ToUnicode"
			}, Converters.CMapStreamConverter);
			this.glyphOutlinesCache = new Dictionary<CharCodeOld, GlyphOutlinesCollection>();
		}

		public CMapStream ToUnicode
		{
			get
			{
				return this.unicode.GetValue();
			}
			set
			{
				this.unicode.SetValue(value);
			}
		}

		protected FontsManagerOld FontsManager
		{
			get
			{
				return base.ContentManager.FontsManager;
			}
		}

		protected CMapOld Mapping
		{
			get
			{
				if (this.mapping == null)
				{
					this.mapping = this.FontsManager.GetFontToUnicode(this);
				}
				return this.mapping;
			}
		}

		protected abstract FontSource FontSource { get; }

		public static FontBaseOld CreateFont(PdfContentManager contentManager, PdfDictionaryOld dictionary)
		{
			PdfNameOld element = dictionary.GetElement<PdfNameOld>("Subtype");
			string value;
			if ((value = element.Value) != null)
			{
				FontBaseOld fontBaseOld;
				if (!(value == "Type0"))
				{
					if (!(value == "Type1"))
					{
						if (!(value == "MMType1"))
						{
							if (!(value == "TrueType"))
							{
								goto IL_70;
							}
							fontBaseOld = new TrueTypeFontOld(contentManager);
						}
						else
						{
							fontBaseOld = new MMType1FontOld(contentManager);
						}
					}
					else
					{
						fontBaseOld = new Type1FontOld(contentManager);
					}
				}
				else
				{
					fontBaseOld = new Type0FontOld(contentManager);
				}
				fontBaseOld.Load(dictionary);
				return fontBaseOld;
			}
			IL_70:
			throw new NotSupportedFontException(element.Value);
		}

		public virtual double GetGlyphWidth(GlyphOld glyph)
		{
			if (this.FontSource != null)
			{
				this.FontSource.GetAdvancedWidth(glyph);
			}
			else
			{
				glyph.AdvancedWidth = 1.0;
			}
			return glyph.AdvancedWidth;
		}

		public virtual IEnumerable<GlyphOld> RenderGlyphs(PdfContext context, PdfStringOld str)
		{
			foreach (GlyphOld g in this.GetGlyphs(str))
			{
				this.RenderGlyph(context, g);
				yield return g;
			}
			yield break;
		}

		protected virtual IEnumerable<GlyphOld> GetGlyphs(PdfStringOld str)
		{
			if (str != null && str.Value != null)
			{
				if (this.Mapping != null)
				{
					IEnumerable<Tuple<string, CharCodeOld>> text = this.Mapping.ToUnicode(str.Value);
					foreach (Tuple<string, CharCodeOld> p in text)
					{
						yield return new GlyphOld
						{
							CharId = p.Item2,
							ToUnicode = p.Item1
						};
					}
				}
				else
				{
					foreach (byte b in str.Value)
					{
						GlyphOld glyphOld = new GlyphOld();
						glyphOld.CharId = new CharCodeOld(b);
						GlyphOld glyphOld2 = glyphOld;
						char c = (char)b;
						glyphOld2.ToUnicode = c.ToString();
						yield return glyphOld;
					}
				}
			}
			yield break;
		}

		protected void GetGlyphOutlines(GlyphOld glyph)
		{
			if (this.FontSource != null)
			{
				GlyphOutlinesCollection outlines;
				if (this.glyphOutlinesCache.TryGetValue(glyph.CharId, out outlines))
				{
					glyph.Outlines = outlines;
					return;
				}
				this.FontSource.InitializeGlyphOutlines(glyph, 100.0);
				this.glyphOutlinesCache[glyph.CharId] = glyph.Outlines;
			}
		}

		protected virtual void RenderGlyph(PdfContext context, GlyphOld glyph)
		{
			glyph.Key = base.ContentManager.ResourceManager.GetFontKey(this);
			glyph.FontSize = context.TextState.FontSize;
			glyph.CharSpacing = context.TextState.CharSpacing;
			glyph.WordSpacing = (glyph.IsSpace ? context.TextState.WordSpacing : 0.0);
			glyph.Rise = context.TextState.Rise;
			glyph.Fill = context.GraphicsState.GetBrushWithAlpha();
			glyph.Stroke = context.GraphicsState.GetStrokeBrushWithAlpha();
			glyph.HorizontalScaling = context.TextState.HorizontalScaling;
			RenderingMode renderingMode = context.TextState.RenderingMode;
			glyph.IsFilled = renderingMode == RenderingMode.Fill || renderingMode == RenderingMode.FillClipping || renderingMode == RenderingMode.FillStroke || renderingMode == RenderingMode.FillStrokeClipping;
			glyph.IsStroked = renderingMode == RenderingMode.FillStroke || renderingMode == RenderingMode.FillStrokeClipping || renderingMode == RenderingMode.Stroke || renderingMode == RenderingMode.StrokeClipping;
			glyph.StrokeThickness = context.GraphicsState.LineWidth;
			glyph.TransformMatrix = context.GetTextRenderingMatrix();
			this.GetGlyphOutlines(glyph);
			this.GetGlyphToUnicode(glyph);
			glyph.Width = this.GetGlyphWidth(glyph);
			context.UpdateTextMatrix(glyph);
			glyph.FreezeGlyphInfo();
		}

		void GetGlyphToUnicode(GlyphOld glyph)
		{
			string toUnicode;
			if (this.Mapping != null && this.Mapping.TryGetToUnicode(glyph.CharId, out toUnicode))
			{
				glyph.ToUnicode = toUnicode;
				return;
			}
			glyph.ToUnicode = ((char)glyph.CharId.IntValue).ToString();
		}

		public const int FontSize = 100;

		const string Type0 = "Type0";

		const string Type1 = "Type1";

		const string Type3 = "Type3";

		const string TrueType = "TrueType";

		const string MMType1 = "MMType1";

		readonly InstantLoadProperty<CMapStream> unicode;

		readonly Dictionary<CharCodeOld, GlyphOutlinesCollection> glyphOutlinesCache;

		CMapOld mapping;
	}
}
