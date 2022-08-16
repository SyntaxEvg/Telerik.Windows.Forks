using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Filters;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Common;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.Model.ColorSpaces;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.ColorSpaces
{
	abstract class ColorSpaceObject : Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Common.PdfObject
	{
		public abstract string Name { get; }

		public abstract ColorObjectBase DefaultColor { get; }

		public abstract ColorSpace Public { get; }

		public override void Write(PdfWriter writer, IPdfExportContext context)
		{
			Guard.ThrowExceptionIfNull<PdfWriter>(writer, "writer");
			Guard.ThrowExceptionIfNull<IPdfExportContext>(context, "context");
			writer.WritePdfName(this.Name);
			Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Common.PdfObject.WritePdfPropertiesDictionary(this, writer, context, false);
		}

		public abstract ColorObjectBase CreateColorObject(IPdfContentExportContext context, ColorBase color);

		public abstract ColorObjectBase GetColor(IPdfContentImportContext context, PostScriptReader reader, PdfArray components);

		public abstract void ImportFromArray(PostScriptReader reader, IPdfImportContext context, PdfArray array);

		public abstract ColorSpaceBase ToColorSpace();

		public abstract void CopyPropertiesFrom(ColorSpaceBase colorSpaceBase);
	}
}
