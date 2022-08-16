using System;
using System.Windows.Media;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Objects;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities;
using Telerik.Windows.Documents.Fixed.Model.Data;
using Telerik.Windows.Documents.Fixed.Model.Objects;
using Telerik.Windows.Documents.Fixed.Model.Resources;
using Telerik.Windows.Documents.Media;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators.Objects
{
	class Paint : ContentStreamOperator
	{
		public override string Name
		{
			get
			{
				return "Do";
			}
		}

		public override void Execute(ContentStreamInterpreter interpreter, IPdfContentImportContext context)
		{
			Guard.ThrowExceptionIfNull<ContentStreamInterpreter>(interpreter, "interpreter");
			Guard.ThrowExceptionIfNull<IPdfContentImportContext>(context, "context");
			try
			{
				PdfName last = interpreter.Operands.GetLast<PdfName>(interpreter.Reader, context.Owner);
				XObjectBase xobject = context.GetXObject(interpreter.Reader, last);
				switch (xobject.XObjectType)
				{
				case XObjectType.Image:
				{
					ImageXObject imageXObject = (ImageXObject)xobject;
					Paint.PaintImage(interpreter, context, imageXObject);
					break;
				}
				case XObjectType.Form:
				{
					FormXObject form = (FormXObject)xobject;
					FormSource formSource = context.Owner.GetFormSource(interpreter.Reader, form);
					Form form2 = context.ContentRoot.Content.AddForm(formSource);
					form2.Position = this.CalculateFormPosition(interpreter, context, formSource);
					form2.Clipping = interpreter.GraphicState.Clipping;
					form2.Marker = context.CurrentMarker;
					break;
				}
				default:
					throw new NotSupportedException(string.Format("Not supported XObject type: {0}", xobject.XObjectType));
				}
			}
			catch
			{
			}
		}

		internal static void PaintImage(ContentStreamInterpreter interpreter, IPdfContentImportContext context, ImageXObject imageXObject)
		{
			Telerik.Windows.Documents.Fixed.Model.Resources.ImageSource imageSource = context.Owner.GetImageSource(interpreter.Reader, imageXObject);
			Image image = context.ContentRoot.Content.AddImage(imageSource);
			image.Position = Paint.CalculateImagePosition(interpreter, context, imageSource);
			image.Clipping = interpreter.GraphicState.Clipping;
			image.Marker = context.CurrentMarker;
			if (imageSource.ImageMask.Value)
			{
				image.StencilColor = interpreter.GraphicState.CalculateStencilColor(interpreter.Reader, context);
			}
		}

		IPosition CalculateFormPosition(ContentStreamInterpreter interpreter, IPdfContentImportContext context, FormSource formSource)
		{
			Matrix m = new Matrix(Paint.DipToPoint, 0.0, 0.0, -Paint.DipToPoint, 0.0, Paint.DipToPoint * formSource.Size.Height);
			MatrixPosition matrixPosition = new MatrixPosition(m.MultiplyBy(interpreter.GraphicState.Matrix));
			return new MatrixPosition(context.ContentRoot.ToTopLeftCoordinateSystem(matrixPosition.Matrix));
		}

		static MatrixPosition CalculateImagePosition(ContentStreamInterpreter interpreter, IPdfContentImportContext context, Telerik.Windows.Documents.Fixed.Model.Resources.ImageSource imageSource)
		{
			double m = ((imageSource.Width == 0) ? 1.0 : (1.0 / (double)imageSource.Width));
			double num = ((imageSource.Height == 0) ? 1.0 : (1.0 / (double)imageSource.Height));
			Matrix m2 = new Matrix(m, 0.0, 0.0, -num, 0.0, 1.0);
			MatrixPosition matrixPosition = new MatrixPosition(m2.MultiplyBy(interpreter.GraphicState.Matrix));
			return new MatrixPosition(context.ContentRoot.ToTopLeftCoordinateSystem(matrixPosition.Matrix));
		}

		public void Write(PdfWriter writer, IPdfContentExportContext context, string name)
		{
			base.WriteInternal(writer, context.Owner, new PdfPrimitive[]
			{
				new PdfName(name)
			});
		}

		const double ImageUnitSize = 1.0;

		static readonly double DipToPoint = Unit.DipToPoint(1.0);
	}
}
