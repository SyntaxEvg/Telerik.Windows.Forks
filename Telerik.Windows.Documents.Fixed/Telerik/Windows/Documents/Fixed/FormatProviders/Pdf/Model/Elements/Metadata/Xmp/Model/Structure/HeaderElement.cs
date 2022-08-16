using System;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Metadata.Xmp.Model.Structure
{
	class HeaderElement : XPacketElement
	{
		public HeaderElement()
		{
			base.RegisterAttribute<string>("begin", "\ufeff", true);
			base.RegisterAttribute<string>("id", "W5M0MpCehiHzreSzNTczkc9d", true);
		}
	}
}
