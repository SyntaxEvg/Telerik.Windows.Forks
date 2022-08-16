using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Common;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Annotations
{
	class IconFit : PdfObject
	{
		public IconFit()
		{
			this.scaleCondition = base.RegisterDirectProperty<PdfName>(new PdfPropertyDescriptor("SW"), new PdfName("A"));
			this.scaleType = base.RegisterDirectProperty<PdfName>(new PdfPropertyDescriptor("S"), new PdfName("P"));
			this.translationAfterProportionalScaling = base.RegisterReferenceProperty<PdfArray>(new PdfPropertyDescriptor("A"), new PdfArray(new PdfPrimitive[]
			{
				new PdfReal(0.5),
				new PdfReal(0.5)
			}));
			this.fitIgnoringBorderWidth = base.RegisterDirectProperty<PdfBool>(new PdfPropertyDescriptor("FB"), new PdfBool(false));
		}

		public PdfName ScaleCondition
		{
			get
			{
				return this.scaleCondition.GetValue();
			}
			set
			{
				this.scaleCondition.SetValue(value);
			}
		}

		public PdfName ScaleType
		{
			get
			{
				return this.scaleType.GetValue();
			}
			set
			{
				this.scaleType.SetValue(value);
			}
		}

		public PdfArray TranslationAfterProportionalScaling
		{
			get
			{
				return this.translationAfterProportionalScaling.GetValue();
			}
			set
			{
				this.translationAfterProportionalScaling.SetValue(value);
			}
		}

		public PdfBool FitIgnoringBorderWidth
		{
			get
			{
				return this.fitIgnoringBorderWidth.GetValue();
			}
			set
			{
				this.fitIgnoringBorderWidth.SetValue(value);
			}
		}

		readonly DirectProperty<PdfName> scaleCondition;

		readonly DirectProperty<PdfName> scaleType;

		readonly ReferenceProperty<PdfArray> translationAfterProportionalScaling;

		readonly DirectProperty<PdfBool> fitIgnoringBorderWidth;
	}
}
