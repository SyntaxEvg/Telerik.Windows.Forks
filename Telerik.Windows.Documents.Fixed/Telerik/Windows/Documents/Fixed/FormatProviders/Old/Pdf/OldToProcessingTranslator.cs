using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.ColorSpaces;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Filters;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.ColorSpaces;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf
{
	static class OldToProcessingTranslator
	{
		public static ColorSpaceObject GetColorSpaceObjectFromColorSpaceOld(ColorSpaceOld colorSpaceOld)
		{
			switch (colorSpaceOld.Type)
			{
			case ColorSpace.Gray:
				return new DeviceGrayColorSpaceObject();
			case ColorSpace.RGB:
				return new DeviceRgbColorSpaceObject();
			case ColorSpace.CMYK:
				return new DeviceCmykColorSpaceObject();
			case ColorSpace.Indexed:
			{
				IndexedOld indexedOld = (IndexedOld)colorSpaceOld;
				return new IndexedColorSpaceObject
				{
					Base = OldToProcessingTranslator.GetColorSpaceObjectFromColorSpaceOld(indexedOld.Base),
					HiVal = indexedOld.HiVal,
					Lookup = new IndexedLookupStream(indexedOld.Lookup.Data)
				};
			}
			case ColorSpace.ICCBased:
			{
				ICCBasedOld iccbasedOld = (ICCBasedOld)colorSpaceOld;
				return new IccBasedColorSpaceObject
				{
					IccProfile = 
					{
						N = new PdfInt(iccbasedOld.Profile.N.Value),
						Alternate = OldToProcessingTranslator.GetColorSpaceObjectFromColorSpaceOld(iccbasedOld.Profile.Alternate)
					}
				};
			}
			case ColorSpace.CalRgb:
				return new CalRgbColorSpaceObject();
			case ColorSpace.CalGray:
				return new CalGrayColorSpaceObject();
			}
			throw new NotImplementedException(string.Format("Color space conversion is not supported for object of type: {0}", colorSpaceOld.GetType()));
		}
	}
}
