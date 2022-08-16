using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.ColorSpaces;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.Model.ColorSpaces;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators.Color
{
	abstract class SetColorSpaceBase : ContentStreamOperator
	{
		public override void Execute(ContentStreamInterpreter contentStreamInterpreter, IPdfContentImportContext context)
		{
			PdfName lastAs = contentStreamInterpreter.Operands.GetLastAs<PdfName>();
			ColorSpaceObject colorSpace;
			if (ColorSpaceManager.IsColorSpaceName(lastAs.Value))
			{
				colorSpace = ColorSpaceManager.CreateColorSpaceObject(lastAs.Value);
			}
			else
			{
				colorSpace = context.GetColorSpace(contentStreamInterpreter.Reader, lastAs);
			}
			this.SetColorSpaceInternal(contentStreamInterpreter.GraphicState, colorSpace);
		}

		public void Write(PdfWriter writer, IPdfContentExportContext context, ColorSpaceBase colorSpace)
		{
			Guard.ThrowExceptionIfNull<PdfWriter>(writer, "writer");
			Guard.ThrowExceptionIfNull<IPdfContentExportContext>(context, "context");
			ColorSpaceObject colorSpaceObject = ColorSpaceManager.CreateColorSpaceObject(colorSpace);
			base.WriteInternal(writer, context.Owner, new PdfPrimitive[] { colorSpaceObject });
		}

		public void Write(PdfWriter writer, IPdfContentExportContext context, string resourceKey)
		{
			Guard.ThrowExceptionIfNull<PdfWriter>(writer, "writer");
			Guard.ThrowExceptionIfNull<IPdfContentExportContext>(context, "context");
			base.WriteInternal(writer, context.Owner, new PdfPrimitive[]
			{
				new PdfName(resourceKey)
			});
		}

		protected abstract void SetColorSpaceInternal(GraphicsState graphicsState, ColorSpaceObject colorSpace);
	}
}
