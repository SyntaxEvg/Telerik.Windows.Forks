using System;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Metadata.Xmp.Model.Structure
{
	class TrailerElement : XPacketElement
	{
		public TrailerElement()
		{
			base.RegisterAttribute<string>("end", "w", true);
		}
	}
}
