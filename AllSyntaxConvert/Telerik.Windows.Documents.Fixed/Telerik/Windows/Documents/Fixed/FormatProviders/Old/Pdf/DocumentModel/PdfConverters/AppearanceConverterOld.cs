using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.Annot;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.XObjects;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.PdfConverters
{
	class AppearanceConverterOld : IndirectReferenceConverterBase
	{
		protected override object ConvertFromIndirectReference(Type type, PdfContentManager contentManager, IndirectReferenceOld indirectReference)
		{
			object obj;
			AppearanceOld appearanceOld;
			if (contentManager.TryGetPdfObject(indirectReference, out obj))
			{
				XForm xform = obj as XForm;
				appearanceOld = ((xform == null) ? ((AppearanceOld)obj) : new AppearanceOld(contentManager, xform));
			}
			else
			{
				appearanceOld = (AppearanceOld)base.ConvertFromIndirectReference(type, contentManager, indirectReference);
				if (appearanceOld.SingleStateAppearance != null)
				{
					appearanceOld.SingleStateAppearance.Reference = appearanceOld.Reference;
					contentManager.RegisterIndirectReference(indirectReference, appearanceOld.SingleStateAppearance);
				}
			}
			return appearanceOld;
		}

		protected override object ConvertFromPdfDataStream(Type type, PdfContentManager contentManager, PdfDataStream stream)
		{
			XForm singleStateAppearance = (XForm)Converters.XObjectConverter.Convert(typeof(XForm), contentManager, stream);
			return new AppearanceOld(contentManager, singleStateAppearance);
		}

		protected override object ConvertFromPdfDictionary(Type type, PdfContentManager contentManager, PdfDictionaryOld dictionary)
		{
			Dictionary<string, XForm> dictionary2 = new Dictionary<string, XForm>();
			foreach (KeyValuePair<string, object> keyValuePair in dictionary)
			{
				XForm value = (XForm)Converters.XObjectConverter.Convert(typeof(XForm), contentManager, keyValuePair.Value);
				dictionary2[keyValuePair.Key] = value;
			}
			return new AppearanceOld(contentManager, dictionary2);
		}
	}
}
