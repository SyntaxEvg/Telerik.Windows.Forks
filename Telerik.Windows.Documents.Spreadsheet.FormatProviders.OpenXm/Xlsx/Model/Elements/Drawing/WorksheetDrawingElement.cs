using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Parts;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Export;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Common;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Parts;
using Telerik.Windows.Documents.Spreadsheet.Model.Shapes;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Drawing
{
	class WorksheetDrawingElement : XlsxPartRootElementBase
	{
		public WorksheetDrawingElement(XlsxPartsManager partsManager, OpenXmlPartBase part)
			: base(partsManager, part)
		{
		}

		public override string ElementName
		{
			get
			{
				return "wsDr";
			}
		}

		public override OpenXmlNamespace Namespace
		{
			get
			{
				return OpenXmlNamespaces.SpreadsheetDrawingMLNamespace;
			}
		}

		public override IEnumerable<OpenXmlNamespace> Namespaces
		{
			get
			{
				yield return OpenXmlNamespaces.DrawingMLNamespace;
				yield break;
			}
		}

		protected override IEnumerable<OpenXmlElementBase> EnumerateChildElements(IXlsxWorkbookExportContext context)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorkbookExportContext>(context, "context");
			IXlsxWorksheetExportContext worksheetContext = context.GetWorksheetContextFromDrawingPart((DrawingPart)base.Part);
			foreach (FloatingShapeBase shape in worksheetContext.Shapes)
			{
				TwoCellAnchorElement twoCellAnchorElement = base.CreateElement<TwoCellAnchorElement>("twoCellAnchor");
				twoCellAnchorElement.CopyPropertiesFrom(worksheetContext, shape);
				yield return twoCellAnchorElement;
			}
			yield break;
		}
	}
}
