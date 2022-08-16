using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Common;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.Utilities;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.ColorSpaces
{
	class IccProfileObject : PdfStreamObjectBase
	{
		static IccProfileObject()
		{
			string resourceName = "Telerik.Windows.Documents.Fixed.Resources.sRGBProfile.icc";
			IccProfileObject.sRgbProfile = new IccProfileObject(3, ResourcesHelper.GetApplicationResourceStream(resourceName).ReadAllBytes());
		}

		public IccProfileObject(int colorComponents, byte[] data)
		{
			Guard.ThrowExceptionIfNull<byte[]>(data, "data");
			this.data = data;
			this.colorComponents = base.RegisterDirectProperty<PdfInt>(new PdfPropertyDescriptor("N"));
			this.colorComponents.SetValue(new PdfInt(colorComponents));
		}

		public static IccProfileObject SRgbProfile
		{
			get
			{
				return IccProfileObject.sRgbProfile;
			}
		}

		protected override byte[] GetData(IPdfExportContext context)
		{
			return this.data;
		}

		static readonly IccProfileObject sRgbProfile;

		readonly DirectProperty<PdfInt> colorComponents;

		readonly byte[] data;
	}
}
