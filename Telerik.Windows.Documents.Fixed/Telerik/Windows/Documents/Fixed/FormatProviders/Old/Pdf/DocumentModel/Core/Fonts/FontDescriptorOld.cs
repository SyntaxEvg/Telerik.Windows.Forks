using System;
using System.Windows;
using Telerik.Windows.Documents.Core.Fonts.OpenType;
using Telerik.Windows.Documents.Core.Fonts.Type1;
using Telerik.Windows.Documents.Core.Fonts.Type1.CFFFormat;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.PdfConverters;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Utils;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Fonts;
using Telerik.Windows.Documents.Fixed.Model.Fonts;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.Fonts
{
	class FontDescriptorOld : PdfObjectOld, IFontDescriptor
	{
		public FontDescriptorOld(PdfContentManager contentManager)
			: base(contentManager)
		{
			this.fontName = base.CreateInstantLoadProperty<PdfNameOld>(new PdfPropertyDescriptor
			{
				Name = "FontName",
				IsRequired = true
			});
			this.fontFamily = base.CreateInstantLoadProperty<PdfStringOld>(new PdfPropertyDescriptor
			{
				Name = "FontFamily"
			}, Converters.PdfStringConverter);
			this.fontWeight = base.CreateInstantLoadProperty<PdfRealOld>(new PdfPropertyDescriptor
			{
				Name = "FontWeight"
			}, Converters.PdfRealConverter);
			this.flags = base.CreateInstantLoadProperty<PdfIntOld>(new PdfPropertyDescriptor
			{
				Name = "Flags",
				IsRequired = true
			});
			this.fontBBox = base.CreateLoadOnDemandProperty<PdfArrayOld>(new PdfPropertyDescriptor
			{
				Name = "FontBBox",
				IsRequired = true
			});
			this.italicAngle = base.CreateInstantLoadProperty<PdfRealOld>(new PdfPropertyDescriptor
			{
				Name = "ItalicAngle"
			}, Converters.PdfRealConverter);
			this.missingWidth = base.CreateInstantLoadProperty<PdfIntOld>(new PdfPropertyDescriptor
			{
				Name = "MissingWidth"
			}, new PdfIntOld(contentManager, 0));
			this.fontFile = base.CreateLoadOnDemandProperty<FontStreamOld>(new PdfPropertyDescriptor
			{
				Name = "FontFile"
			});
			this.fontFile2 = base.CreateLoadOnDemandProperty<FontStreamOld>(new PdfPropertyDescriptor
			{
				Name = "FontFile2"
			});
			this.fontFile3 = base.CreateLoadOnDemandProperty<FontStreamOld>(new PdfPropertyDescriptor
			{
				Name = "FontFile3"
			});
			this.ascent = base.CreateInstantLoadProperty<PdfRealOld>(new PdfPropertyDescriptor
			{
				Name = "Ascent",
				IsRequired = true
			}, Converters.PdfRealConverter);
			this.descent = base.CreateInstantLoadProperty<PdfRealOld>(new PdfPropertyDescriptor
			{
				Name = "Descent",
				IsRequired = true
			}, Converters.PdfRealConverter);
		}

		public bool IsSymbolic
		{
			get
			{
				return this.GetFlag(3);
			}
		}

		public bool IsNonSymbolic
		{
			get
			{
				return this.GetFlag(6);
			}
		}

		public PdfNameOld FontName
		{
			get
			{
				return this.fontName.GetValue();
			}
			set
			{
				this.fontName.SetValue(value);
			}
		}

		public PdfRealOld Ascent
		{
			get
			{
				return this.ascent.GetValue();
			}
			set
			{
				this.ascent.SetValue(value);
			}
		}

		public PdfRealOld Descent
		{
			get
			{
				return this.descent.GetValue();
			}
			set
			{
				this.descent.SetValue(value);
			}
		}

		public PdfStringOld FontFamily
		{
			get
			{
				return this.fontFamily.GetValue();
			}
			set
			{
				this.fontFamily.SetValue(value);
			}
		}

		public PdfRealOld FontWeight
		{
			get
			{
				return this.fontWeight.GetValue();
			}
			set
			{
				this.fontWeight.SetValue(value);
			}
		}

		public PdfIntOld Flags
		{
			get
			{
				return this.flags.GetValue();
			}
			set
			{
				this.flags.SetValue(value);
			}
		}

		public PdfArrayOld FontBBox
		{
			get
			{
				return this.fontBBox.GetValue();
			}
			set
			{
				this.fontBBox.SetValue(value);
			}
		}

		public PdfRealOld ItalicAngle
		{
			get
			{
				return this.italicAngle.GetValue();
			}
			set
			{
				this.italicAngle.SetValue(value);
			}
		}

		public PdfIntOld MissingWidth
		{
			get
			{
				return this.missingWidth.GetValue();
			}
			set
			{
				this.missingWidth.SetValue(value);
			}
		}

		public FontStreamOld FontFile
		{
			get
			{
				return this.fontFile.GetValue();
			}
			set
			{
				this.fontFile.SetValue(value);
			}
		}

		public FontStreamOld FontFile2
		{
			get
			{
				return this.fontFile2.GetValue();
			}
			set
			{
				this.fontFile2.SetValue(value);
			}
		}

		public FontStreamOld FontFile3
		{
			get
			{
				return this.fontFile3.GetValue();
			}
			set
			{
				this.fontFile3.SetValue(value);
			}
		}

		public bool GetFontFamily(out string fontFamily, out FontWeight fontWeight, out FontStyle fontStyle)
		{
			fontFamily = null;
			fontWeight = default(FontWeight);
			fontStyle = default(FontStyle);
			if (this.FontFamily == null || string.IsNullOrEmpty(this.FontFamily.ToString()))
			{
				return false;
			}
			FontsHelper.GetFontFamily(this.FontFamily.ToString(), out fontFamily, out fontWeight, out fontStyle);
			if (this.FontWeight != null)
			{
				fontWeight = FontWeights.Normal;
				int num;
				Helper.UnboxInt(this.FontWeight, out num);
				if (num >= 700)
				{
					fontWeight = FontWeights.Bold;
				}
			}
			if (this.ItalicAngle != null)
			{
				fontStyle = FontStyles.Normal;
				if (this.ItalicAngle.Value != 0.0)
				{
					fontStyle = FontStyles.Italic;
				}
			}
			return true;
		}

		public CFFFontSource GetCFFFontSource()
		{
			try
			{
				if (this.FontFile3 != null)
				{
					byte[] data = base.ContentManager.ReadData(this.FontFile3.Reference);
					if (this.FontFile3.Subtype.Value == "CIDFontType0C" || this.FontFile3.Subtype.Value == "Type1C")
					{
						CFFFontFile cfffontFile = new CFFFontFile(data);
						return cfffontFile.FontSource;
					}
					if (this.FontFile3.Subtype.Value == "OpenType")
					{
						OpenTypeFontSource openTypeFontSource = new OpenTypeFontSource(new OpenTypeFontReader(data));
						if (openTypeFontSource.Outlines == Outlines.OpenType)
						{
							return openTypeFontSource.CFF;
						}
					}
				}
			}
			catch (Exception)
			{
			}
			return null;
		}

		public OpenTypeFontSource GetOpenTypeFontSource()
		{
			OpenTypeFontSource result;
			try
			{
				byte[] data;
				string text;
				FontWeight weight;
				FontStyle style;
				if (this.TryGetFontData(this.FontFile2, out data))
				{
					result = new OpenTypeFontSource(new OpenTypeFontReader(data));
				}
				else if (this.FontFile3 != null && this.FontFile3.Subtype.Value == "OpenType" && this.TryGetFontData(this.FontFile3, out data))
				{
					result = new OpenTypeFontSource(new OpenTypeFontReader(data));
				}
				else if (this.GetFontFamily(out text, out weight, out style))
				{
					result = base.ContentManager.FontsManager.GetTrueTypeFontSource(text, style, weight);
				}
				else if (this.FontName != null && this.FontName.Value != null)
				{
					result = base.ContentManager.FontsManager.GetTrueTypeFontSource(this.FontName.Value);
				}
				else
				{
					result = null;
				}
			}
			catch (Exception)
			{
				result = null;
			}
			return result;
		}

		bool TryGetFontData(FontStreamOld fontStream, out byte[] data)
		{
			if (fontStream == null)
			{
				data = null;
				return false;
			}
			data = base.ContentManager.ReadData(fontStream.Reference);
			return data.Length != 0;
		}

		bool GetFlag(byte bit)
		{
			bit -= 1;
			return Helper.GetBit(this.Flags.Value, bit);
		}

		readonly InstantLoadProperty<PdfNameOld> fontName;

		readonly LoadOnDemandProperty<FontStreamOld> fontFile;

		readonly LoadOnDemandProperty<FontStreamOld> fontFile2;

		readonly LoadOnDemandProperty<FontStreamOld> fontFile3;

		readonly InstantLoadProperty<PdfStringOld> fontFamily;

		readonly InstantLoadProperty<PdfRealOld> fontWeight;

		readonly InstantLoadProperty<PdfIntOld> flags;

		readonly LoadOnDemandProperty<PdfArrayOld> fontBBox;

		readonly InstantLoadProperty<PdfRealOld> italicAngle;

		readonly InstantLoadProperty<PdfIntOld> missingWidth;

		readonly InstantLoadProperty<PdfRealOld> ascent;

		readonly InstantLoadProperty<PdfRealOld> descent;
	}
}
