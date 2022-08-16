using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Common;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.GraphicsState
{
	class ExtGStateObject : PdfObject
	{
		public ExtGStateObject()
		{
			this.alphaConstant = base.RegisterDirectProperty<PdfReal>(new PdfPropertyDescriptor("ca"));
			this.strokeAlphaConstant = base.RegisterDirectProperty<PdfReal>(new PdfPropertyDescriptor("CA"));
			this.alphaSource = base.RegisterDirectProperty<PdfBool>(new PdfPropertyDescriptor("AIS"));
		}

		public PdfReal AlphaConstant
		{
			get
			{
				return this.alphaConstant.GetValue();
			}
			set
			{
				this.alphaConstant.SetValue(value);
			}
		}

		public PdfReal StrokeAlphaConstant
		{
			get
			{
				return this.strokeAlphaConstant.GetValue();
			}
			set
			{
				this.strokeAlphaConstant.SetValue(value);
			}
		}

		public PdfBool AlphaSource
		{
			get
			{
				return this.alphaSource.GetValue();
			}
			set
			{
				this.alphaSource.SetValue(value);
			}
		}

		internal void CopyPropertiesFrom(IPdfExportContext context, ExtGState state)
		{
			Guard.ThrowExceptionIfNull<IPdfExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<ExtGState>(state, "resource");
			if (state.AlphaConstant != null)
			{
				this.AlphaConstant = new PdfReal(state.AlphaConstant.Value);
			}
			if (state.StrokeAlphaConstant != null)
			{
				this.StrokeAlphaConstant = new PdfReal(state.StrokeAlphaConstant.Value);
			}
			if (state.AlphaSource != null)
			{
				this.AlphaSource = new PdfBool(state.AlphaSource.Value);
			}
		}

		internal ExtGState ToExtGState()
		{
			ExtGState extGState = new ExtGState();
			if (this.AlphaSource != null)
			{
				extGState.AlphaSource = new bool?(this.AlphaSource.Value);
			}
			if (this.AlphaConstant != null)
			{
				extGState.AlphaConstant = new double?(this.AlphaConstant.Value);
			}
			if (this.StrokeAlphaConstant != null)
			{
				extGState.StrokeAlphaConstant = new double?(this.StrokeAlphaConstant.Value);
			}
			return extGState;
		}

		readonly DirectProperty<PdfReal> alphaConstant;

		readonly DirectProperty<PdfReal> strokeAlphaConstant;

		readonly DirectProperty<PdfBool> alphaSource;
	}
}
