using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Pictures
{
	abstract class PictureElementBase : OpenXmlElementBase
	{
		public PictureElementBase(OpenXmlPartsManager partsManager)
			: base(partsManager)
		{
		}

		public override OpenXmlNamespace Namespace
		{
			get
			{
				return OpenXmlNamespaces.PictureDrawingMLNamespace;
			}
		}
	}
}
