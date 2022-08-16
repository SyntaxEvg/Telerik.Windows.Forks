using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Common;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Styles
{
	abstract class BordersElement : DocxElementBase
	{
		public BordersElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
		}

		public void SetBorderElementProperties(OpenXmlChildElement<BorderElement> borderElement, Border border, DocumentElementPropertiesBase properties, BorderStyle borderStyleToCompare)
		{
			Guard.ThrowExceptionIfNull<OpenXmlChildElement<BorderElement>>(borderElement, "borderElement");
			Guard.ThrowExceptionIfNull<Border>(border, "border");
			Guard.ThrowExceptionIfNull<DocumentElementPropertiesBase>(properties, "properties");
			if (border.Style != borderStyleToCompare)
			{
				base.CreateElement(borderElement);
				borderElement.Element.SetAssociatedFlowModelElement(properties);
				borderElement.Element.Value = border;
			}
		}

		public void SetBorderValue(OpenXmlChildElement<BorderElement> borderElement, Func<Border, ParagraphBorders> setBorderFunc, ref ParagraphBorders borders, ref bool hasLocalBorderValue)
		{
			Guard.ThrowExceptionIfNull<OpenXmlChildElement<BorderElement>>(borderElement, "borderElement");
			Guard.ThrowExceptionIfNull<Func<Border, ParagraphBorders>>(setBorderFunc, "setBorderFunc");
			if (borderElement.Element != null)
			{
				hasLocalBorderValue = true;
				borders = setBorderFunc(borderElement.Element.Value);
			}
		}

		public void SetBorderValue(OpenXmlChildElement<BorderElement> borderElement, Func<Border, TableBorders> setBorderFunc, ref TableBorders borders, ref bool hasLocalBorderValue)
		{
			Guard.ThrowExceptionIfNull<OpenXmlChildElement<BorderElement>>(borderElement, "borderElement");
			Guard.ThrowExceptionIfNull<Func<Border, TableBorders>>(setBorderFunc, "setBorderFunc");
			if (borderElement.Element != null)
			{
				hasLocalBorderValue = true;
				borders = setBorderFunc(borderElement.Element.Value);
			}
		}

		public void SetBorderValue(OpenXmlChildElement<BorderElement> borderElement, Func<Border, TableCellBorders> setBorderFunc, ref TableCellBorders borders, ref bool hasLocalBorderValue)
		{
			Guard.ThrowExceptionIfNull<OpenXmlChildElement<BorderElement>>(borderElement, "borderElement");
			Guard.ThrowExceptionIfNull<Func<Border, TableCellBorders>>(setBorderFunc, "setBorderFunc");
			if (borderElement.Element != null)
			{
				hasLocalBorderValue = true;
				borders = setBorderFunc(borderElement.Element.Value);
			}
		}
	}
}
