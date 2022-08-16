using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Filters;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities;
using Telerik.Windows.Documents.Fixed.Model.ColorSpaces;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.ColorSpaces
{
	class CalRgbColorSpaceObject : PdfRealColorSpaceObject
	{
		public CalRgbColorSpaceObject()
		{
			this.whitePoint = base.RegisterReferenceProperty<PdfArray>(new PdfPropertyDescriptor("WhitePoint", true));
			this.blackPoint = base.RegisterReferenceProperty<PdfArray>(new PdfPropertyDescriptor("BlackPoint"));
			this.gamma = base.RegisterReferenceProperty<PdfArray>(new PdfPropertyDescriptor("Gamma"));
			this.matrix = base.RegisterReferenceProperty<PdfArray>(new PdfPropertyDescriptor("Matrix"));
		}

		public override string Name
		{
			get
			{
				return "CalRGB";
			}
		}

		public override ColorObjectBase DefaultColor
		{
			get
			{
				return new RgbColorObject(0.0, 0.0, 0.0);
			}
		}

		public override ColorSpace Public
		{
			get
			{
				return ColorSpace.CalRgb;
			}
		}

		public PdfArray WhitePoint
		{
			get
			{
				return this.whitePoint.GetValue();
			}
			set
			{
				this.whitePoint.SetValue(value);
			}
		}

		public PdfArray BlackPoint
		{
			get
			{
				return this.blackPoint.GetValue();
			}
			set
			{
				this.blackPoint.SetValue(value);
			}
		}

		public PdfArray Gamma
		{
			get
			{
				return this.gamma.GetValue();
			}
			set
			{
				this.gamma.SetValue(value);
			}
		}

		public PdfArray Matrix
		{
			get
			{
				return this.matrix.GetValue();
			}
			set
			{
				this.matrix.SetValue(value);
			}
		}

		public override ColorObjectBase CreateColorObject(IPdfContentExportContext context, ColorBase color)
		{
			throw new NotImplementedException();
		}

		protected override ColorObjectBase GetColor(IPdfContentImportContext context, PostScriptReader reader, PdfReal[] components)
		{
			PdfReal r = components[0];
			PdfReal g = components[1];
			PdfReal b = components[2];
			return new RgbColorObject(r, g, b);
		}

		public override ColorSpaceBase ToColorSpace()
		{
			CalRgb calRgb = new CalRgb();
			calRgb.WhitePoint = this.WhitePoint.ToDoubleArray();
			if (this.blackPoint.HasValue)
			{
				calRgb.BlackPoint = this.BlackPoint.ToDoubleArray();
			}
			if (this.gamma.HasValue)
			{
				calRgb.Gamma = this.Gamma.ToDoubleArray();
			}
			if (this.matrix.HasValue)
			{
				calRgb.Matrix = this.Matrix.ToDoubleArray();
			}
			return calRgb;
		}

		public override void CopyPropertiesFrom(ColorSpaceBase colorSpaceBase)
		{
			CalRgb calRgb = (CalRgb)colorSpaceBase;
			this.WhitePoint = calRgb.WhitePoint.ToPdfArray();
			this.BlackPoint = calRgb.BlackPoint.ToPdfArray();
			this.Gamma = calRgb.Gamma.ToPdfArray();
			this.Matrix = calRgb.Matrix.ToPdfArray();
		}

		public override void ImportFromArray(PostScriptReader reader, IPdfImportContext context, PdfArray array)
		{
			base.Load(reader, context, array[1]);
		}

		public override void Write(PdfWriter writer, IPdfExportContext context)
		{
			Guard.ThrowExceptionIfNull<PdfWriter>(writer, "writer");
			Guard.ThrowExceptionIfNull<IPdfExportContext>(context, "context");
			writer.Write(PdfNames.PdfArrayStart);
			base.Write(writer, context);
			writer.Write(PdfNames.PdfArrayEnd);
		}

		readonly ReferenceProperty<PdfArray> whitePoint;

		readonly ReferenceProperty<PdfArray> blackPoint;

		readonly ReferenceProperty<PdfArray> gamma;

		readonly ReferenceProperty<PdfArray> matrix;
	}
}
