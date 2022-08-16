using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Parts;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Export;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Import;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Common;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Workbooks
{
	class WorkbookElement : XlsxPartRootElementBase
	{
		public WorkbookElement(XlsxPartsManager partsManager, OpenXmlPartBase part)
			: base(partsManager, part)
		{
			this.workbookProtection = base.RegisterChildElement<WorkbookProtectionElement>("workbookProtection");
			this.bookViews = base.RegisterChildElement<BookViewsElement>("bookViews");
			this.sheets = base.RegisterChildElement<SheetsElement>("sheets");
			this.definedNames = base.RegisterChildElement<DefinedNamesElement>("definedNames");
		}

		public override string ElementName
		{
			get
			{
				return "workbook";
			}
		}

		public override OpenXmlNamespace Namespace
		{
			get
			{
				return OpenXmlNamespaces.SpreadsheetMLNamespace;
			}
		}

		public override IEnumerable<OpenXmlNamespace> Namespaces
		{
			get
			{
				yield return OpenXmlNamespaces.OfficeDocumentRelationshipsNamespace;
				yield break;
			}
		}

		protected override void OnBeforeWrite(IXlsxWorkbookExportContext context)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorkbookExportContext>(context, "context");
			base.CreateElement(this.workbookProtection);
			base.CreateElement(this.bookViews);
			base.CreateElement(this.sheets);
			base.CreateElement(this.definedNames);
		}

		protected override void OnAfterRead(IXlsxWorkbookImportContext context)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorkbookImportContext>(context, "context");
			base.ReleaseElement(this.workbookProtection);
			base.ReleaseElement(this.bookViews);
			base.ReleaseElement(this.sheets);
			base.ReleaseElement(this.definedNames);
		}

		readonly OpenXmlChildElement<WorkbookProtectionElement> workbookProtection;

		readonly OpenXmlChildElement<BookViewsElement> bookViews;

		readonly OpenXmlChildElement<SheetsElement> sheets;

		readonly OpenXmlChildElement<DefinedNamesElement> definedNames;
	}
}
