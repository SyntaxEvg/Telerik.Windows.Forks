using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Core.Fonts;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Common;
using Telerik.Windows.Documents.Fixed.Model.Fonts;
using Telerik.Windows.Documents.Fixed.Model.Text;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Fonts
{
	abstract class FontStream : PdfStreamObject
	{
		public FontStream()
		{
			this.metadata = base.RegisterReferenceProperty<PdfStreamObject>(new PdfPropertyDescriptor("Metadata"));
		}

		public PdfStreamObject Metadata
		{
			get
			{
				return this.metadata.GetValue();
			}
			set
			{
				this.metadata.SetValue(value);
			}
		}

		public void CopyPropertiesFrom(IPdfExportContext context, FontBase font)
		{
			Guard.ThrowExceptionIfNull<IPdfExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<FontBase>(font, "font");
			this.CopyPropertiesFromOverride(context, font);
		}

		protected virtual void CopyPropertiesFromOverride(IPdfExportContext context, FontBase font)
		{
			Guard.ThrowExceptionIfNull<IPdfExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<FontBase>(font, "font");
			IEnumerable<CharInfo> usedCharacters = context.GetUsedCharacters(font);
			base.Data = font.ComputeSubset(usedCharacters);
			this.CopyPropertiesFromData();
		}

		public abstract FontFileInfo ToFileInfo();

		public abstract FontSource ToFontSource(FontType fontType);

		protected virtual void CopyPropertiesFromData()
		{
		}

		readonly ReferenceProperty<PdfStreamObject> metadata;
	}
}
