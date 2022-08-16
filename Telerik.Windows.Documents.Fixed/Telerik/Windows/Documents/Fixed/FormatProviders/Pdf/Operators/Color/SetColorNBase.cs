using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.ColorSpaces;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators.Color
{
	abstract class SetColorNBase : ContentStreamOperator
	{
		public override void Execute(ContentStreamInterpreter interpreter, IPdfContentImportContext context)
		{
			Guard.ThrowExceptionIfNull<ContentStreamInterpreter>(interpreter, "contentStreamInterpreter");
			Guard.ThrowExceptionIfNull<IPdfContentImportContext>(context, "context");
			IEnumerable<PdfPrimitive> initialValues = this.PopAllOperands(interpreter).Reverse<PdfPrimitive>();
			PdfArray pdfArray = new PdfArray(initialValues);
			ColorSpaceObject colorSpaceObject;
			if (!this.TryGetColorSpace(interpreter, out colorSpaceObject))
			{
				switch (pdfArray.Count)
				{
				case 1:
					colorSpaceObject = new DeviceGrayColorSpaceObject();
					goto IL_7B;
				case 3:
					colorSpaceObject = new DeviceRgbColorSpaceObject();
					goto IL_7B;
				case 4:
					colorSpaceObject = new DeviceCmykColorSpaceObject();
					goto IL_7B;
				}
				throw new InvalidOperationException("Components count must be 1, 3 or 4!");
			}
			IL_7B:
			ColorObjectBase color = colorSpaceObject.GetColor(context, interpreter.Reader, pdfArray);
			if (color != null)
			{
				this.SetColor(interpreter, color);
			}
		}

		public void Write(PdfWriter writer, IPdfContentExportContext context, string resourceName)
		{
			Guard.ThrowExceptionIfNull<PdfWriter>(writer, "writer");
			Guard.ThrowExceptionIfNull<IPdfContentExportContext>(context, "context");
			base.WriteInternal(writer, context.Owner, new PdfPrimitive[]
			{
				new PdfName(resourceName)
			});
		}

		public void Write(PdfWriter writer, IPdfContentExportContext context, int[] colorComponents, string resourceName)
		{
			Guard.ThrowExceptionIfNull<PdfWriter>(writer, "writer");
			Guard.ThrowExceptionIfNull<IPdfContentExportContext>(context, "context");
			PdfPrimitive[] array = new PdfPrimitive[colorComponents.Length + 1];
			for (int i = 0; i < colorComponents.Length; i++)
			{
				array[i] = colorComponents[i].ToPdfInt();
			}
			array[colorComponents.Length] = new PdfName(resourceName);
			base.WriteInternal(writer, context.Owner, array);
		}

		public void Write(PdfWriter writer, IPdfContentExportContext context, ColorObjectBase color, string resourceName)
		{
			Guard.ThrowExceptionIfNull<PdfWriter>(writer, "writer");
			Guard.ThrowExceptionIfNull<ColorObjectBase>(color, "color");
			Guard.ThrowExceptionIfNull<IPdfContentExportContext>(context, "context");
			base.WriteInternal(writer, context.Owner, new PdfPrimitive[]
			{
				color,
				new PdfName(resourceName)
			});
		}

		protected abstract bool TryGetColorSpace(ContentStreamInterpreter interpreter, out ColorSpaceObject colorSpace);

		protected abstract void SetColor(ContentStreamInterpreter interpreter, ColorObjectBase color);

		IEnumerable<PdfPrimitive> PopAllOperands(ContentStreamInterpreter interpreter)
		{
			while (interpreter.Operands.Count > 0)
			{
				PdfPrimitive primitive = interpreter.Operands.GetLast();
				yield return primitive;
			}
			yield break;
		}
	}
}
