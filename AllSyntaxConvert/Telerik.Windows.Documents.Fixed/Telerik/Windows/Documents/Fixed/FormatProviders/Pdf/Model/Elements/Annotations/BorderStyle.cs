using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Common;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities;
using Telerik.Windows.Documents.Fixed.Model.Annotations;
using Telerik.Windows.Documents.Media;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Annotations
{
	class BorderStyle : PdfObject
	{
		public BorderStyle()
		{
			this.width = base.RegisterDirectProperty<PdfReal>(new PdfPropertyDescriptor("W"), 1.ToPdfReal());
			this.style = base.RegisterDirectProperty<PdfName>(new PdfPropertyDescriptor("S"), "S".ToPdfName());
			this.dashArray = base.RegisterDirectProperty<PdfArray>(new PdfPropertyDescriptor("D"), new double[] { 3.0 }.ToPdfArray());
		}

		public PdfReal Width
		{
			get
			{
				return this.width.GetValue();
			}
			set
			{
				this.width.SetValue(value);
			}
		}

		public PdfName Style
		{
			get
			{
				return this.style.GetValue();
			}
			set
			{
				this.style.SetValue(value);
			}
		}

		public PdfArray DashArray
		{
			get
			{
				return this.dashArray.GetValue();
			}
			set
			{
				this.dashArray.SetValue(value);
			}
		}

		public void CopyPropertiesFrom(IPdfExportContext context, AnnotationBorder border)
		{
			Guard.ThrowExceptionIfNull<IPdfExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<AnnotationBorder>(border, "border");
			this.Width = Unit.DipToPoint(border.Width).ToPdfReal();
			switch (border.Style)
			{
			case AnnotationBorderStyle.Dashed:
				this.Style = new PdfName("D");
				this.DashArray = BorderStyle.GetDashArrayInPoints(border).ToPdfArray();
				return;
			case AnnotationBorderStyle.Beveled:
				this.Style = new PdfName("B");
				return;
			case AnnotationBorderStyle.Inset:
				this.Style = new PdfName("I");
				return;
			case AnnotationBorderStyle.None:
				this.Width = 0.ToPdfReal();
				return;
			default:
				this.Style = new PdfName("S");
				return;
			}
		}

		public void CopyPropertiesTo(AnnotationBorder border)
		{
			border.Width = Unit.PointToDip(this.Width.Value);
			border.DashArray = this.ImportDashArray();
			string value;
			if ((value = this.Style.Value) != null)
			{
				if (value == "D")
				{
					border.Style = AnnotationBorderStyle.Dashed;
					return;
				}
				if (value == "I")
				{
					border.Style = AnnotationBorderStyle.Inset;
					return;
				}
				if (value == "B")
				{
					border.Style = AnnotationBorderStyle.Beveled;
					return;
				}
			}
			border.Style = AnnotationBorderStyle.Solid;
		}

		static IEnumerable<double> GetDashArrayInPoints(AnnotationBorder border)
		{
			foreach (double num in border.DashArray)
			{
				double value = num;
				yield return Unit.DipToPoint(value);
			}
			yield break;
		}

		double[] ImportDashArray()
		{
			double[] array = this.DashArray.ToDoubleArray();
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = Unit.PointToDip(array[i]);
			}
			return array;
		}

		readonly DirectProperty<PdfReal> width;

		readonly DirectProperty<PdfName> style;

		readonly DirectProperty<PdfArray> dashArray;
	}
}
