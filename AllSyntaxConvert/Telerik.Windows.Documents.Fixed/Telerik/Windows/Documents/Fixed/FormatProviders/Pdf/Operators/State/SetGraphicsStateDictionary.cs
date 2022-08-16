using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.GraphicsState;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators.State
{
	class SetGraphicsStateDictionary : ContentStreamOperator
	{
		public override string Name
		{
			get
			{
				return "gs";
			}
		}

		public override void Execute(ContentStreamInterpreter interpreter, IPdfContentImportContext context)
		{
			Guard.ThrowExceptionIfNull<ContentStreamInterpreter>(interpreter, "interpreter");
			Guard.ThrowExceptionIfNull<IPdfContentImportContext>(context, "context");
			PdfName last = interpreter.Operands.GetLast<PdfName>(interpreter.Reader, context.Owner);
			ExtGStateObject extGState = context.GetExtGState(interpreter.Reader, last);
			ExtGState extGState2 = context.Owner.GetExtGState(interpreter.Reader, extGState);
			interpreter.GraphicState.UpdateProperties(extGState2);
		}

		public void Write(PdfWriter writer, IPdfContentExportContext context, string dictionaryName)
		{
			base.WriteInternal(writer, context.Owner, new PdfPrimitive[]
			{
				new PdfName(dictionaryName)
			});
		}
	}
}
