using System;
using Telerik.Windows.Documents.Fixed.Exceptions;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Objects
{
	static class XObjectFactory
	{
		public static XObjectBase CreateInstance(PdfName subType, PdfBool imageMask)
		{
			Guard.ThrowExceptionIfNull<PdfName>(subType, "subType");
			string value;
			if ((value = subType.Value) != null)
			{
				if (!(value == "Image"))
				{
					if (value == "Form")
					{
						return new FormXObject();
					}
				}
				else
				{
					if (imageMask == null || !imageMask.Value)
					{
						return new ImageXObject();
					}
					return new Mask();
				}
			}
			throw new NotSupportedXObjectTypeException(string.Format("Not supported subtype value: {0}!", subType.Value));
		}

		public const string ImageSubtype = "Image";

		public const string FormSubtype = "Form";
	}
}
