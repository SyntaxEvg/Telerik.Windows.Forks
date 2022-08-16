using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.ColorSpaces
{
	abstract class CIEBasedOld : ColorSpaceOld
	{
		public CIEBasedOld(PdfContentManager contentManager)
			: base(contentManager)
		{
		}
	}
}
