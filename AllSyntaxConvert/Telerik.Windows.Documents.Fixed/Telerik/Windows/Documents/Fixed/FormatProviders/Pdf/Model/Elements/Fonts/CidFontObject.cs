using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using Telerik.Windows.Documents.Core.Fonts;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Common;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Fonts.Cid;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities;
using Telerik.Windows.Documents.Fixed.Model.Data;
using Telerik.Windows.Documents.Fixed.Model.Fonts;
using Telerik.Windows.Documents.Fixed.Model.Fonts.Encoding;
using Telerik.Windows.Documents.Fixed.Model.Text;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Fonts
{
	abstract class CidFontObject : PdfObject
	{
		public CidFontObject()
		{
			this.baseFont = base.RegisterDirectProperty<PdfName>(new PdfPropertyDescriptor("BaseFont", true));
			this.cidSystemInfo = base.RegisterDirectProperty<CidSystemInfo>(new PdfPropertyDescriptor("CIDSystemInfo", true));
			this.defaultWidth = base.RegisterDirectProperty<PdfInt>(new PdfPropertyDescriptor("DW"));
			this.fontDescriptor = base.RegisterReferenceProperty<FontDescriptor>(new PdfPropertyDescriptor("FontDescriptor", true, PdfPropertyRestrictions.MustBeIndirectReference));
			this.widths = base.RegisterReferenceProperty<PdfArray>(new PdfPropertyDescriptor("W"));
			this.defaultWidthVertical = base.RegisterReferenceProperty<PdfArray>(new PdfPropertyDescriptor("DW2"));
			this.widthsVerticle = base.RegisterReferenceProperty<PdfArray>(new PdfPropertyDescriptor("W2"));
			this.charIdToGlyphIdMapping = base.RegisterReferenceProperty<CharIdToGlyphIdMappingBase>(new PdfPropertyDescriptor("CIDToGIDMap"));
		}

		public PdfName BaseFont
		{
			get
			{
				return this.baseFont.GetValue();
			}
			set
			{
				this.baseFont.SetValue(value);
			}
		}

		public CidSystemInfo CidSystemInfo
		{
			get
			{
				return this.cidSystemInfo.GetValue();
			}
			set
			{
				this.cidSystemInfo.SetValue(value);
			}
		}

		public FontDescriptor FontDescriptor
		{
			get
			{
				return this.fontDescriptor.GetValue();
			}
			set
			{
				this.fontDescriptor.SetValue(value);
			}
		}

		public PdfArray Widths
		{
			get
			{
				return this.widths.GetValue();
			}
			set
			{
				this.widths.SetValue(value);
			}
		}

		public PdfInt DefaultWidth
		{
			get
			{
				return this.defaultWidth.GetValue();
			}
			set
			{
				this.defaultWidth.SetValue(value);
			}
		}

		public PdfArray WidthsVertical
		{
			get
			{
				return this.widthsVerticle.GetValue();
			}
			set
			{
				this.widthsVerticle.SetValue(value);
			}
		}

		public PdfArray DefaultWidthVertical
		{
			get
			{
				return this.defaultWidthVertical.GetValue();
			}
			set
			{
				this.defaultWidthVertical.SetValue(value);
			}
		}

		public CharIdToGlyphIdMappingBase CharIdToGlyphIdMapping
		{
			get
			{
				return this.charIdToGlyphIdMapping.GetValue();
			}
			set
			{
				this.charIdToGlyphIdMapping.SetValue(value);
			}
		}

		public static CidFontObject CreateInstance(PdfName typeName)
		{
			Guard.ThrowExceptionIfNull<PdfName>(typeName, "typeName");
			string value;
			if ((value = typeName.Value) != null)
			{
				if (value == "CIDFontType0")
				{
					return new CidFontType0Object();
				}
				if (value == "CIDFontType2")
				{
					return new CidFontType2Object();
				}
			}
			throw new NotSupportedException(string.Format("Not supported CidFont: {0}", typeName.Value));
		}

		public void CopyPropertiesFrom(IPdfExportContext context, FontBase font)
		{
			Guard.ThrowExceptionIfNull<IPdfExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<FontBase>(font, "font");
			this.CopyPropertiesFromOverride(context, font);
		}

		internal bool TryGetFontFamily(out FontFamily fontFamily)
		{
			fontFamily = null;
			if (this.FontDescriptor != null && this.FontDescriptor.FontFamily != null)
			{
				fontFamily = new FontFamily(this.FontDescriptor.FontFamily.ToString());
			}
			return fontFamily != null;
		}

		internal abstract bool TryCreateFontFromFontFile(out FontBase font);

		internal abstract FontBase CreateFontWithoutFontFile();

		internal abstract bool TryCreateFontFromFontProperties(FontProperties fontProperties, out FontBase font);

		internal virtual void CopyPropertiesToFont(global::Telerik.Windows.Documents.Fixed.Model.Fonts.CidFontBase font, global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser.PostScriptReader reader, global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.IPdfImportContext context)
		{
			this.FontDescriptor.Lang.CopyToProperty(font.Lang, (global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfName language) => language.Value);
			this.FontDescriptor.CidSet.CopyToProperty(font.CidSet, (global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Fonts.Cid.CidSet cidSet) => cidSet.Data);
			this.CidSystemInfo.CopyToProperty(font.CidSystemInfo, (global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Fonts.CidSystemInfo info) => new global::Telerik.Windows.Documents.Fixed.Model.Fonts.Encoding.CidSystemInfo(info.Registry.ToString(), info.Ordering.ToString(), info.Supplement.Value));
			this.Widths.CopyToProperty(font.Widths, (global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfArray widthsArray) => global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Fonts.CidFontObject.CreateCidWidths<int>(widthsArray, 1, new global::System.Func<global::System.Collections.Generic.Queue<int>, int>(global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Fonts.CidFontObject.GetConstantWidth), new global::System.Func<global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfArray, global::System.Collections.Generic.IEnumerable<int>>(global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Fonts.CidFontObject.GetWidthsArrayRange), context));
			this.WidthsVertical.CopyToProperty(font.WidthsVertical, (global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfArray widthsArray) => global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Fonts.CidFontObject.CreateCidWidths<global::Telerik.Windows.Documents.Fixed.Model.Fonts.CidWidthVertical>(widthsArray, 3, new global::System.Func<global::System.Collections.Generic.Queue<int>, global::Telerik.Windows.Documents.Fixed.Model.Fonts.CidWidthVertical>(global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Fonts.CidFontObject.GetConstantWidthVertical), new global::System.Func<global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfArray, global::System.Collections.Generic.IEnumerable<global::Telerik.Windows.Documents.Fixed.Model.Fonts.CidWidthVertical>>(global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Fonts.CidFontObject.GetWidthsVerticalArrayRange), context));
			this.DefaultWidth.CopyToProperty(font.DefaultWidth, (global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfInt width) => width.Value);
			this.DefaultWidthVertical.CopyToProperty(font.DefaultWidthsVertical, (global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfArray array) => global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Fonts.CidFontObject.GetDefaultWidthVertical(array));
			this.CharIdToGlyphIdMapping.CopyToProperty(font.CharIdToGlyphIdMapping, (global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Fonts.CharIdToGlyphIdMappingBase mapping) => global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Fonts.CidFontObject.CreateCidToGidMap(mapping));
		}
		protected virtual void CopyPropertiesFromOverride(IPdfExportContext context, FontBase font)
		{
			Guard.ThrowExceptionIfNull<IPdfExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<FontBase>(font, "font");
			this.BaseFont = font.FontName.ToPdfName();
			this.FontDescriptor = new FontDescriptor();
			this.FontDescriptor.CopyPropertiesFrom(context, font);
			CidFontBase cidFontBase = (CidFontBase)font;
			this.CidSystemInfo = new CidSystemInfo();
			this.CidSystemInfo.CopyPropertiesFrom(context, cidFontBase);
			this.Widths = cidFontBase.Widths.ToPrimitive(new Func<CidWidths<int>, PdfArray>(CidFontObject.CalculateWidths), () => CidFontObject.CalculateWidths(context, font));
			this.DefaultWidth = cidFontBase.DefaultWidth.ToPrimitive((int width) => new PdfInt(width), null);
			this.WidthsVertical = cidFontBase.WidthsVertical.ToPrimitive(new Func<CidWidths<CidWidthVertical>, PdfArray>(CidFontObject.CalculateWidths), null);
			this.DefaultWidthVertical = cidFontBase.DefaultWidthsVertical.ToPrimitive(new Func<CidWidthVertical, PdfArray>(CidFontObject.CalculateDefaultWidth), null);
			this.CharIdToGlyphIdMapping = cidFontBase.CharIdToGlyphIdMapping.ToPrimitive(new Func<CidToGidMap, CharIdToGlyphIdMappingBase>(CidFontObject.CalculateCidToGidMap), null);
		}

		static PdfArray CalculateWidths(IPdfExportContext context, FontBase font)
		{
			if (font.FontFileInfo.HasValue)
			{
				return null;
			}
			return CidFontObject.CalculateUsedCharactersWidths(context, font);
		}

		static PdfArray CalculateUsedCharactersWidths(IPdfExportContext context, FontBase font)
		{
			PdfArray pdfArray = new PdfArray(new PdfPrimitive[0]);
			PdfArray pdfArray2 = null;
			CharCode charCode = null;
			CharInfo[] array = (from g in context.GetUsedCharacters(font)
				orderby g.CharCode.Code
				select g).ToArray<CharInfo>();
			foreach (CharInfo charInfo in array)
			{
				if (charCode == null || charCode.Size != charInfo.CharCode.Size || charCode.Code != charInfo.CharCode.Code - 1)
				{
					pdfArray.Add(new PdfInt(charInfo.CharCode.Code));
					pdfArray2 = new PdfArray(new PdfPrimitive[0]);
					pdfArray.Add(pdfArray2);
					charCode = charInfo.CharCode;
				}
				pdfArray2.Add(new PdfInt((int)font.GetWidth(charInfo.CharCode.Code)));
			}
			return pdfArray;
		}

		static CharIdToGlyphIdMappingBase CalculateCidToGidMap(CidToGidMap cidToGidMap)
		{
			if (cidToGidMap.IsIdentityMapping)
			{
				return CharIdToGlyphIdMappingBase.Identity;
			}
			return new CharIdToGlyphIdMapping
			{
				Data = cidToGidMap.Data
			};
		}

		static CidToGidMap CreateCidToGidMap(CharIdToGlyphIdMappingBase mapping)
		{
			if (mapping == CharIdToGlyphIdMappingBase.Identity)
			{
				return new CidToGidMap();
			}
			return new CidToGidMap(mapping.Data);
		}

		static PdfArray CalculateWidths(CidWidths<int> cidWidths)
		{
			return CidFontObject.CalculateWidths<int>(cidWidths, new Func<int, IEnumerable<int>>(CidFontObject.GetWidthComponents));
		}

		static PdfArray CalculateWidths(CidWidths<CidWidthVertical> cidWidths)
		{
			return CidFontObject.CalculateWidths<CidWidthVertical>(cidWidths, new Func<CidWidthVertical, IEnumerable<int>>(CidFontObject.GetWidthComponents));
		}

		static PdfArray CalculateWidths<T>(CidWidths<T> cidWidths, Func<T, IEnumerable<int>> getWidthComponents)
		{
			PdfArray pdfArray = new PdfArray(new PdfPrimitive[0]);
			foreach (CidWidthsRange<T> cidWidthsRange in cidWidths)
			{
				pdfArray.Add(new PdfInt(cidWidthsRange.StartCharCode));
				if (cidWidthsRange.IsConstantWidthRange)
				{
					pdfArray.Add(new PdfInt(cidWidthsRange.EndCharCode));
					using (IEnumerator<int> enumerator2 = getWidthComponents(cidWidthsRange.ConstantWidth).GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							int defaultValue = enumerator2.Current;
							pdfArray.Add(new PdfInt(defaultValue));
						}
						continue;
					}
				}
				PdfArray pdfArray2 = new PdfArray(new PdfPrimitive[0]);
				foreach (T arg in cidWidthsRange.Widths)
				{
					foreach (int defaultValue2 in getWidthComponents(arg))
					{
						pdfArray2.Add(new PdfInt(defaultValue2));
					}
				}
				pdfArray.Add(pdfArray2);
			}
			return pdfArray;
		}

		static CidWidths<T> CreateCidWidths<T>(PdfArray widthsArray, int widthRangeComponentsCount, Func<Queue<int>, T> getConstantWidth, Func<PdfArray, IEnumerable<T>> getArrayRange, IPdfImportContext context)
		{
			CidWidths<T> cidWidths = new CidWidths<T>();
			Queue<int> queue = new Queue<int>();
			foreach (PdfPrimitive primitive in widthsArray)
			{
				PdfPrimitive directPrimitive = CidFontObject.GetDirectPrimitive(primitive, context);
				switch (directPrimitive.Type)
				{
				case PdfElementType.PdfReal:
				case PdfElementType.PdfInt:
				{
					int integerFromNumber = CidFontObject.GetIntegerFromNumber(directPrimitive);
					queue.Enqueue(integerFromNumber);
					if (queue.Count == widthRangeComponentsCount + 2)
					{
						int startCharCode = queue.Dequeue();
						int endCharCode = queue.Dequeue();
						T constantWidth = getConstantWidth(queue);
						cidWidths.Add(new CidWidthsRange<T>(startCharCode, endCharCode, constantWidth));
					}
					break;
				}
				default:
				{
					int startCharCode2 = queue.Dequeue();
					PdfArray arg = (PdfArray)directPrimitive;
					cidWidths.Add(new CidWidthsRange<T>(startCharCode2, getArrayRange(arg)));
					break;
				}
				}
			}
			return cidWidths;
		}

		static int GetIntegerFromNumber(PdfPrimitive primitive)
		{
			int result;
			if (primitive.Type == PdfElementType.PdfInt)
			{
				PdfInt pdfInt = (PdfInt)primitive;
				result = pdfInt.Value;
			}
			else
			{
				PdfReal pdfReal = (PdfReal)primitive;
				result = (int)pdfReal.Value;
			}
			return result;
		}

		static PdfPrimitive GetDirectPrimitive(PdfPrimitive primitive, IPdfImportContext context)
		{
			if (primitive.Type == PdfElementType.IndirectReference)
			{
				IndirectReference reference = (IndirectReference)primitive;
				PdfPrimitive content;
				if (!context.TryGetIndirectObject(reference, out content))
				{
					content = context.ReadIndirectObject(reference).Content;
					context.RegisterIndirectObject(reference, content);
				}
				return content;
			}
			return primitive;
		}

		static CidWidthVertical GetConstantWidthVertical(Queue<int> numbers)
		{
			int displacementVectorY = numbers.Dequeue();
			int positionVectorX = numbers.Dequeue();
			int positionVectorY = numbers.Dequeue();
			return new CidWidthVertical(displacementVectorY, positionVectorX, positionVectorY);
		}

		static IEnumerable<CidWidthVertical> GetWidthsVerticalArrayRange(PdfArray array)
		{
			Queue<int> numbers = new Queue<int>(array.ToIntArray());
			while (numbers.Count > 0)
			{
				yield return CidFontObject.GetConstantWidthVertical(numbers);
			}
			yield break;
		}

		static int GetConstantWidth(Queue<int> numbers)
		{
			return numbers.Dequeue();
		}

		static IEnumerable<int> GetWidthsArrayRange(PdfArray array)
		{
			return array.ToIntArray();
		}

		static PdfArray CalculateDefaultWidth(CidWidthVertical width)
		{
			return new PdfArray(new PdfPrimitive[0])
			{
				new PdfInt(width.DisplacementVectorY),
				new PdfInt(width.PositionVectorY)
			};
		}

		static CidWidthVertical GetDefaultWidthVertical(PdfArray array)
		{
			int[] array2 = array.ToIntArray();
			return new CidWidthVertical(array2[0], 0, array2[1]);
		}

		static IEnumerable<int> GetWidthComponents(CidWidthVertical width)
		{
			yield return width.DisplacementVectorY;
			yield return width.PositionVectorX;
			yield return width.PositionVectorY;
			yield break;
		}

		static IEnumerable<int> GetWidthComponents(int width)
		{
			yield return width;
			yield break;
		}

		const int WidthIntegersCount = 1;

		const int WidthVerticalIntegersCount = 3;

		const int StartEndRangeIntegersCount = 2;

		public const string CidFontType0 = "CIDFontType0";

		public const string CidFontType2 = "CIDFontType2";

		readonly DirectProperty<PdfName> baseFont;

		readonly DirectProperty<CidSystemInfo> cidSystemInfo;

		readonly DirectProperty<PdfInt> defaultWidth;

		readonly ReferenceProperty<FontDescriptor> fontDescriptor;

		readonly ReferenceProperty<PdfArray> widths;

		readonly ReferenceProperty<PdfArray> defaultWidthVertical;

		readonly ReferenceProperty<PdfArray> widthsVerticle;

		readonly ReferenceProperty<CharIdToGlyphIdMappingBase> charIdToGlyphIdMapping;
	}
}
