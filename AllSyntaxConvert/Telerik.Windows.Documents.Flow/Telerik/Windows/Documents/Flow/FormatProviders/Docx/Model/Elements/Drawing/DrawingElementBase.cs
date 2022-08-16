using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Common;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Drawing
{
	abstract class DrawingElementBase : DocxElementBase
	{
		public DrawingElementBase(DocxPartsManager partsManager)
			: base(partsManager)
		{
		}

		public override OpenXmlNamespace Namespace
		{
			get
			{
				return OpenXmlNamespaces.WordprocessingDrawingMLNamespace;
			}
		}
	}
}
