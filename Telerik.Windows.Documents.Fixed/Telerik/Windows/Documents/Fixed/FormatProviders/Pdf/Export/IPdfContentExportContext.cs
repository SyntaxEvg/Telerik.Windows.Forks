using System;
using System.Collections.Generic;
using System.Windows.Media;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.DocumentStructure;
using Telerik.Windows.Documents.Fixed.Model;
using Telerik.Windows.Documents.Fixed.Model.ColorSpaces;
using Telerik.Windows.Documents.Fixed.Model.Common;
using Telerik.Windows.Documents.Fixed.Model.Fonts;
using Telerik.Windows.Documents.Fixed.Model.Resources;
using Telerik.Windows.Documents.Fixed.Model.Text;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export
{
	interface IPdfContentExportContext
	{
		IPdfExportContext Owner { get; }

		IResourceHolder ResourceHolder { get; }

		IContentRootElement ContentRoot { get; }

		Matrix RootDipToPointTransformation { get; }

		MarkedContentExportInfo GetCurrentContentMarkerExportInfo(Marker currentContentMarker);

		ResourceEntry GetResource(FontBase font);

		ResourceEntry GetResource(FormSource formSource);

		ResourceEntry GetResource(Telerik.Windows.Documents.Fixed.Model.Resources.ImageSource imageSource);

		ResourceEntry GetResource(PatternColor pattern);

		ResourceEntry GetResource(ExtGState extGState);

		ResourceEntry GetResource(ColorSpaceBase first, ColorSpaceBase second);

		void SetUsedCharacters(FontBase font, TextCollection glyphs);

		IEnumerable<CharInfo> GetUsedCharacters(FontBase font);

		IEnumerable<ContentElementBase> GetClippingChildren(Clipping clipping);

		IEnumerable<ContentElementBase> GetRootElements();
	}
}
