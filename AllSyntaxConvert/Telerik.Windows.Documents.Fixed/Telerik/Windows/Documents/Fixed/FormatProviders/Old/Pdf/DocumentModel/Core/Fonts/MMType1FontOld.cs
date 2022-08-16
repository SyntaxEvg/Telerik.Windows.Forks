using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Utils;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.Fonts
{
	[PdfClass(TypeName = "Font", SubtypeProperty = "Subtype", SubtypeValue = "MMType1")]
	class MMType1FontOld : Type1FontOld
	{
		public MMType1FontOld(PdfContentManager contentManager)
			: base(contentManager)
		{
		}
	}
}
