using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Utils;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.Fonts
{
	[PdfClass(TypeName = "FontDescriptor")]
	class CIDFontDescriptorOld : FontDescriptorOld
	{
		public CIDFontDescriptorOld(PdfContentManager contentManager)
			: base(contentManager)
		{
		}
	}
}
