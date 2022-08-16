using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Common;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.ColorSpaces
{
	class IccProfileStreamObject : PdfStreamObjectBase
	{
		public IccProfileStreamObject(byte[] data)
			: this()
		{
			this.data = data;
		}

		public IccProfileStreamObject()
		{
			this.n = base.RegisterDirectProperty<PdfInt>(new PdfPropertyDescriptor("N", true));
			this.alternate = base.RegisterDirectProperty<ColorSpaceObject>(new PdfPropertyDescriptor("Alternate"));
			this.range = base.RegisterReferenceProperty<PdfArray>(new PdfPropertyDescriptor("Range"));
		}

		public IEnumerable<byte> Data
		{
			get
			{
				return this.data;
			}
		}

		public PdfInt N
		{
			get
			{
				return this.n.GetValue();
			}
			set
			{
				this.n.SetValue(value);
			}
		}

		public ColorSpaceObject Alternate
		{
			get
			{
				return this.alternate.GetValue();
			}
			set
			{
				this.alternate.SetValue(value);
			}
		}

		public PdfArray Range
		{
			get
			{
				return this.range.GetValue();
			}
			set
			{
				this.range.SetValue(value);
			}
		}

		public bool TryGetAlternateColorSpace(out ColorSpaceObject alternate)
		{
			alternate = null;
			if (this.Alternate != null)
			{
				alternate = this.Alternate;
			}
			else if (this.N != null)
			{
				switch (this.N.Value)
				{
				case 1:
					alternate = new DeviceGrayColorSpaceObject();
					break;
				case 3:
					alternate = new DeviceRgbColorSpaceObject();
					break;
				case 4:
					alternate = new DeviceCmykColorSpaceObject();
					break;
				}
			}
			return alternate != null;
		}

		protected override void InterpretData(PostScriptReader reader, IPdfImportContext context, PdfStreamBase stream)
		{
			this.data = stream.ReadDecodedPdfData();
		}

		protected override byte[] GetData(IPdfExportContext context)
		{
			return this.data;
		}

		byte[] data;

		readonly DirectProperty<PdfInt> n;

		readonly DirectProperty<ColorSpaceObject> alternate;

		readonly ReferenceProperty<PdfArray> range;
	}
}
