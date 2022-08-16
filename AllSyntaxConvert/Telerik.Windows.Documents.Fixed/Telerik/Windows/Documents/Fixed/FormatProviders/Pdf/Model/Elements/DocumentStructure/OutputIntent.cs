using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.ColorSpaces;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Common;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.DocumentStructure
{
	class OutputIntent : PdfObject
	{
		public OutputIntent()
		{
			this.ouputIntentType = base.RegisterDirectProperty<PdfName>(new PdfPropertyDescriptor("S", true));
			this.outputContition = base.RegisterDirectProperty<PdfLiteralString>(new PdfPropertyDescriptor("OutputCondition"));
			this.outputContitionIdentifier = base.RegisterDirectProperty<PdfLiteralString>(new PdfPropertyDescriptor("OutputConditionIdentifier", true));
			this.info = base.RegisterDirectProperty<PdfLiteralString>(new PdfPropertyDescriptor("Info"));
			this.iccProfile = base.RegisterReferenceProperty<IccProfileObject>(new PdfPropertyDescriptor("DestOutputProfile"));
			this.ouputIntentType.SetValue(new PdfName("GTS_PDFA1"));
			this.outputContition.SetValue(new PdfLiteralString("sRGB IEC61966-2.1", StringType.ASCII));
			this.outputContitionIdentifier.SetValue(new PdfLiteralString("Custom", StringType.ASCII));
			this.info.SetValue(new PdfLiteralString("sRGB", StringType.ASCII));
			this.iccProfile.SetValue(IccProfileObject.SRgbProfile);
		}

		readonly DirectProperty<PdfName> ouputIntentType;

		readonly DirectProperty<PdfLiteralString> outputContition;

		readonly DirectProperty<PdfLiteralString> outputContitionIdentifier;

		readonly DirectProperty<PdfLiteralString> info;

		readonly ReferenceProperty<IccProfileObject> iccProfile;
	}
}
