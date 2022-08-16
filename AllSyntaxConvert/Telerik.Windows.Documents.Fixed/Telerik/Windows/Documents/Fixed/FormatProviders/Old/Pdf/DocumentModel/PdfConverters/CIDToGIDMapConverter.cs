﻿using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.Fonts;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.PdfConverters
{
	class CIDToGIDMapConverter : IndirectReferenceConverterBase
	{
		protected override object ConvertFromPdfName(Type type, PdfContentManager contentManager, PdfNameOld name)
		{
			Guard.ThrowExceptionIfNull<Type>(type, "type");
			Guard.ThrowExceptionIfNull<PdfContentManager>(contentManager, "contentManager");
			Guard.ThrowExceptionIfNull<PdfNameOld>(name, "name");
			return new CIDToGIDMapOld(contentManager);
		}
	}
}
