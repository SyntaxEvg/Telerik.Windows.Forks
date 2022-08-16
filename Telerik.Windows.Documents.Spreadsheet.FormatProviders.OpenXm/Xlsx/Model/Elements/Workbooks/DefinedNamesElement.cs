using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Contexts;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Export;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Workbooks
{
	class DefinedNamesElement : WorkbookElementBase
	{
		public DefinedNamesElement(XlsxPartsManager partsManager)
			: base(partsManager)
		{
		}

		public override string ElementName
		{
			get
			{
				return "definedNames";
			}
		}

		protected override bool ShouldExport(IXlsxWorkbookExportContext context)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorkbookExportContext>(context, "context");
			return context.DefinedNames.Any<DefinedNameInfo>() || base.ShouldExport(context);
		}

		protected override IEnumerable<OpenXmlElementBase> EnumerateChildElements(IXlsxWorkbookExportContext context)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorkbookExportContext>(context, "context");
			foreach (DefinedNameInfo definedNameInfo in context.DefinedNames)
			{
				DefinedNameElement definedNameElement = base.CreateElement<DefinedNameElement>("definedName");
				definedNameElement.CopyPropertiesFrom(context, definedNameInfo);
				yield return definedNameElement;
			}
			yield break;
		}
	}
}
