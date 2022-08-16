using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.DocumentStructure;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities;
using Telerik.Windows.Documents.Fixed.Model.ColorSpaces;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Patterns
{
	class Tiling : ContentStream, IResourceHolder
	{
		public Tiling()
		{
			this.matrix = base.RegisterDirectProperty<PdfArray>(new PdfPropertyDescriptor("Matrix"), PdfArray.MatrixIdentity);
			this.paintType = base.RegisterDirectProperty<PdfInt>(new PdfPropertyDescriptor("PaintType", true));
			this.tilingType = base.RegisterDirectProperty<PdfInt>(new PdfPropertyDescriptor("TilingType", true));
			this.bbox = base.RegisterDirectProperty<PdfArray>(new PdfPropertyDescriptor("BBox", true));
			this.xStep = base.RegisterDirectProperty<PdfReal>(new PdfPropertyDescriptor("XStep", true));
			this.yStep = base.RegisterDirectProperty<PdfReal>(new PdfPropertyDescriptor("YStep", true));
			this.resources = base.RegisterDirectProperty<PdfResource>(new PdfPropertyDescriptor("Resources", true));
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

		public PdfInt PaintType
		{
			get
			{
				return this.paintType.GetValue();
			}
			set
			{
				this.paintType.SetValue(value);
			}
		}

		public PdfInt TilingType
		{
			get
			{
				return this.tilingType.GetValue();
			}
			set
			{
				this.tilingType.SetValue(value);
			}
		}

		public PdfReal XStep
		{
			get
			{
				return this.xStep.GetValue();
			}
			set
			{
				this.xStep.SetValue(value);
			}
		}

		public PdfReal YStep
		{
			get
			{
				return this.yStep.GetValue();
			}
			set
			{
				this.yStep.SetValue(value);
			}
		}

		public PdfArray BBox
		{
			get
			{
				return this.bbox.GetValue();
			}
			set
			{
				this.bbox.SetValue(value);
			}
		}

		public PdfResource Resources
		{
			get
			{
				return this.resources.GetValue();
			}
			set
			{
				this.resources.SetValue(value);
			}
		}

		public bool IsColored
		{
			get
			{
				return this.PaintType.Value == 1;
			}
		}

		public void CopyPropertiesFrom(IPdfExportContext context, TilingPatternObject tilingPattern, TilingBase tilingColor)
		{
			Guard.ThrowExceptionIfNull<IPdfExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<TilingPatternObject>(tilingPattern, "tilingPattern");
			Guard.ThrowExceptionIfNull<TilingBase>(tilingColor, "tilingColor");
			this.Resources = new PdfResource();
			bool canWriteColors = context.CanWriteColors;
			if (tilingColor.PaintType == Telerik.Windows.Documents.Fixed.Model.ColorSpaces.PaintType.Uncolored)
			{
				context.CanWriteColors = false;
			}
			base.CopyPropertiesFrom(context, tilingPattern.Tiling, tilingColor);
			context.CanWriteColors = canWriteColors;
			this.Matrix = tilingPattern.Matrix;
			this.BBox = tilingColor.BoundingBox.ToPdfArray();
			this.XStep = tilingColor.HorizontalSpacing.ToPdfReal();
			this.YStep = tilingColor.VerticalSpacing.ToPdfReal();
			this.TilingType = ((int)tilingColor.TilingType).ToPdfInt();
			this.PaintType = ((int)tilingColor.PaintType).ToPdfInt();
		}

		readonly DirectProperty<PdfInt> paintType;

		readonly DirectProperty<PdfInt> tilingType;

		readonly DirectProperty<PdfArray> bbox;

		readonly DirectProperty<PdfReal> xStep;

		readonly DirectProperty<PdfReal> yStep;

		readonly DirectProperty<PdfResource> resources;

		readonly DirectProperty<PdfArray> matrix;
	}
}
