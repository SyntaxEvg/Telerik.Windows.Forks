using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.ColorSpaces;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.Fonts;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.XObjects;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.PdfConverters;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Utils;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core
{
	class PdfResourceOld : PdfObjectOld
	{
		public PdfResourceOld(PdfContentManager contentManager)
			: base(contentManager)
		{
			this.fonts = base.CreateLoadOnDemandProperty<PdfDictionaryOld>(new PdfPropertyDescriptor
			{
				Name = "Font"
			});
			this.extGState = base.CreateLoadOnDemandProperty<PdfDictionaryOld>(new PdfPropertyDescriptor
			{
				Name = "ExtGState"
			});
			this.colorSpaces = base.CreateLoadOnDemandProperty<PdfDictionaryOld>(new PdfPropertyDescriptor
			{
				Name = "ColorSpace"
			});
			this.xObjects = base.CreateLoadOnDemandProperty<PdfDictionaryOld>(new PdfPropertyDescriptor
			{
				Name = "XObject"
			});
			this.patterns = base.CreateLoadOnDemandProperty<PdfDictionaryOld>(new PdfPropertyDescriptor
			{
				Name = "Pattern"
			});
			this.shadings = base.CreateLoadOnDemandProperty<PdfDictionaryOld>(new PdfPropertyDescriptor
			{
				Name = "Shading"
			});
		}

		public PdfDictionaryOld ExtGState
		{
			get
			{
				return this.extGState.GetValue();
			}
			set
			{
				this.extGState.SetValue(value);
			}
		}

		public PdfDictionaryOld ColorSpaces
		{
			get
			{
				return this.colorSpaces.GetValue();
			}
			set
			{
				this.colorSpaces.SetValue(value);
			}
		}

		public PdfDictionaryOld XObjects
		{
			get
			{
				return this.xObjects.GetValue();
			}
			set
			{
				this.xObjects.SetValue(value);
			}
		}

		public PdfDictionaryOld Fonts
		{
			get
			{
				return this.fonts.GetValue();
			}
			set
			{
				this.fonts.SetValue(value);
			}
		}

		public PdfDictionaryOld Patterns
		{
			get
			{
				return this.patterns.GetValue();
			}
			set
			{
				this.patterns.SetValue(value);
			}
		}

		public PdfDictionaryOld Shadings
		{
			get
			{
				return this.shadings.GetValue();
			}
			set
			{
				this.shadings.SetValue(value);
			}
		}

		public FontBaseOld GetFont(PdfNameOld key)
		{
			Guard.ThrowExceptionIfNull<PdfNameOld>(key, "key");
			return this.Fonts.GetElement<FontBaseOld>(key.Value, Converters.FontConverter);
		}

		public XObject GetXObject(PdfNameOld key)
		{
			Guard.ThrowExceptionIfNull<PdfNameOld>(key, "key");
			return this.XObjects.GetElement<XObject>(key.Value, Converters.XObjectConverter);
		}

		public ExtGStateOld GetExtGState(PdfNameOld key)
		{
			Guard.ThrowExceptionIfNull<PdfNameOld>(key, "key");
			return this.ExtGState.GetElement<ExtGStateOld>(key.Value);
		}

		public ColorSpaceOld GetColorSpace(PdfNameOld key)
		{
			Guard.ThrowExceptionIfNull<PdfNameOld>(key, "key");
			return this.ColorSpaces.GetElement<ColorSpaceOld>(key.Value, Converters.ColorSpaceConverter);
		}

		public PatternOld GetPattern(PdfNameOld key)
		{
			Guard.ThrowExceptionIfNull<PdfNameOld>(key, "key");
			return this.Patterns.GetElement<PatternOld>(key.Value, Converters.PatternConverter);
		}

		public ShadingOld GetShading(PdfNameOld key)
		{
			Guard.ThrowExceptionIfNull<PdfNameOld>(key, "key");
			return this.Shadings.GetElement<ShadingOld>(key.Value, Converters.ShadingConverter);
		}

		readonly LoadOnDemandProperty<PdfDictionaryOld> fonts;

		readonly LoadOnDemandProperty<PdfDictionaryOld> extGState;

		readonly LoadOnDemandProperty<PdfDictionaryOld> colorSpaces;

		readonly LoadOnDemandProperty<PdfDictionaryOld> xObjects;

		readonly LoadOnDemandProperty<PdfDictionaryOld> patterns;

		readonly LoadOnDemandProperty<PdfDictionaryOld> shadings;
	}
}
