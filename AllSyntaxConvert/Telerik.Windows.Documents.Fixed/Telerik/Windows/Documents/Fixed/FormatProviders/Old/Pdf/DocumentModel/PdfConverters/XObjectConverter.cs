using System;
using Telerik.Windows.Documents.Fixed.Exceptions;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.XObjects;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.PdfConverters
{
	class XObjectConverter : IndirectReferenceConverterBase
	{
		protected override object ConvertFromPdfDataStream(Type type, PdfContentManager contentManager, PdfDataStream stream)
		{
			Guard.ThrowExceptionIfNull<Type>(type, "type");
			Guard.ThrowExceptionIfNull<PdfContentManager>(contentManager, "contentManager");
			Guard.ThrowExceptionIfNull<PdfDataStream>(stream, "stream");
			XObject xobject;
			if (stream.Dictionary.ContainsKey("Subtype"))
			{
				PdfNameOld element = stream.Dictionary.GetElement<PdfNameOld>("Subtype");
				string value;
				if ((value = element.Value) != null)
				{
					if (!(value == "Image"))
					{
						if (value == "Form")
						{
							xobject = new XForm(contentManager);
							goto IL_F4;
						}
					}
					else
					{
						if (stream.Dictionary.ContainsKey("ImageMask") && stream.Dictionary.GetElement<PdfBoolOld>("ImageMask").Value)
						{
							xobject = new ImageMaskOld(contentManager, stream.ReadData(contentManager));
							goto IL_F4;
						}
						xobject = new XImage(contentManager);
						goto IL_F4;
					}
				}
				throw new NotSupportedXObjectTypeException(string.Format("Not supported subtype value: {0}!", element.Value));
			}
			if (!stream.Dictionary.ContainsKey("BBox"))
			{
				throw new NotSupportedXObjectTypeException("XObject has missing Required properties!");
			}
			xobject = new XForm(contentManager);
			IL_F4:
			xobject.Load(stream);
			return xobject;
		}
	}
}
