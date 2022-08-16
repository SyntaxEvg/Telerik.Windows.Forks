using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Filters;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities;
using Telerik.Windows.Documents.Fixed.Model.ColorSpaces;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.ColorSpaces
{
	class CalGrayColorSpaceObject : PdfRealColorSpaceObject
	{
		public CalGrayColorSpaceObject()
		{
			this.whitePoint = base.RegisterReferenceProperty<PdfArray>(new PdfPropertyDescriptor("WhitePoint", true));
			this.blackPoint = base.RegisterReferenceProperty<PdfArray>(new PdfPropertyDescriptor("BlackPoint"));
			this.gamma = base.RegisterDirectProperty<PdfReal>(new PdfPropertyDescriptor("Gamma"));
		}

		public override string Name
		{
			get
			{
				return "CalGray";
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
				return ColorSpace.CalGray;
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

		public PdfReal Gamma
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

		public override ColorObjectBase CreateColorObject(IPdfContentExportContext context, ColorBase color)
		{
			throw new NotImplementedException();
		}

		protected override ColorObjectBase GetColor(IPdfContentImportContext context, PostScriptReader reader, PdfReal[] components)
		{
			PdfReal g = components[0];
			return new GrayColorObject(g);
		}

		public override ColorSpaceBase ToColorSpace()
		{
			CalGray calGray = new CalGray();
			calGray.WhitePoint = this.WhitePoint.ToDoubleArray();
			if (this.blackPoint.HasValue)
			{
				calGray.BlackPoint = this.BlackPoint.ToDoubleArray();
			}
			if (this.gamma.HasValue)
			{
				calGray.Gamma = this.Gamma.Value;
			}
			return calGray;
		}

		public override void CopyPropertiesFrom(ColorSpaceBase colorSpaceBase)
		{
			CalGray calGray = (CalGray)colorSpaceBase;
			this.WhitePoint = calGray.WhitePoint.ToPdfArray();
			this.BlackPoint = calGray.BlackPoint.ToPdfArray();
			this.Gamma = calGray.Gamma.ToPdfReal();
		}

		public override void ImportFromArray(PostScriptReader reader, IPdfImportContext context, PdfArray array)
		{
			base.Load(reader, context, array[1]);
		}

		readonly ReferenceProperty<PdfArray> whitePoint;

		readonly ReferenceProperty<PdfArray> blackPoint;

		readonly DirectProperty<PdfReal> gamma;
	}
}
