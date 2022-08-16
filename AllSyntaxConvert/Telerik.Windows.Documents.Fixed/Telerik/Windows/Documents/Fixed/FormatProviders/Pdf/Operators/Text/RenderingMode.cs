using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities;
using Telerik.Windows.Documents.Fixed.Model.Text;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators.Text
{
	class RenderingMode : global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators.ContentStreamOperator
	{
		public override string Name
		{
			get
			{
				return "Tr";
			}
		}

		public override void Execute(global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.ContentStreamInterpreter interpreter, global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.IPdfContentImportContext context)
		{
			global::Telerik.Windows.Documents.Fixed.Model.Text.RenderingMode value = (global::Telerik.Windows.Documents.Fixed.Model.Text.RenderingMode)interpreter.Operands.GetLast<global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfInt>(interpreter.Reader, context.Owner).Value;
			interpreter.TextState.RenderingMode = value;
		}

		public void Write(global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export.PdfWriter writer, global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export.IPdfContentExportContext context, global::Telerik.Windows.Documents.Fixed.Model.Text.RenderingMode renderingMode)
		{
			global::Telerik.Windows.Documents.Utilities.Guard.ThrowExceptionIfNull<global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export.PdfWriter>(writer, "writer");
			global::Telerik.Windows.Documents.Utilities.Guard.ThrowExceptionIfNull<global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export.IPdfContentExportContext>(context, "context");
			base.WriteInternal(writer, context.Owner, new global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfPrimitive[] { ((int)renderingMode).ToPdfInt() });
		}
	}
}
