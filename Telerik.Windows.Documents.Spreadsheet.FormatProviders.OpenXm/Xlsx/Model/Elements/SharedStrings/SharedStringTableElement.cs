using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Parts;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Contexts;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Export;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Common;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.SharedStrings
{
	class SharedStringTableElement : XlsxPartRootElementBase
	{
		public SharedStringTableElement(XlsxPartsManager partsManager, OpenXmlPartBase part)
			: base(partsManager, part)
		{
			this.count = base.RegisterCountAttribute();
		}

		public override string ElementName
		{
			get
			{
				return "sst";
			}
		}

		public override OpenXmlNamespace Namespace
		{
			get
			{
				return OpenXmlNamespaces.SpreadsheetMLNamespace;
			}
		}

		public int Count
		{
			get
			{
				return this.count.Value;
			}
			set
			{
				this.count.Value = value;
			}
		}

		protected override void OnBeforeWrite(IXlsxWorkbookExportContext context)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorkbookExportContext>(context, "context");
			this.Count = context.SharedStrings.Count<SharedString>();
		}

		protected override IEnumerable<OpenXmlElementBase> EnumerateChildElements(IXlsxWorkbookExportContext context)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorkbookExportContext>(context, "context");
			foreach (SharedString sharedString in context.SharedStrings)
			{
				StringItemElement stringItemElement = base.CreateElement<StringItemElement>("si");
				stringItemElement.CopyPropertiesFrom(context, sharedString);
				yield return stringItemElement;
			}
			yield break;
		}

		readonly OpenXmlCountAttribute count;
	}
}
