using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Contexts;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Export;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.SharedStrings
{
	class TextElement : SharedStringElementBase
	{
		public TextElement(XlsxPartsManager partsManager)
			: base(partsManager)
		{
			this.space = base.RegisterSpaceAttribute();
		}

		public override string ElementName
		{
			get
			{
				return "t";
			}
		}

		public string Space
		{
			get
			{
				return this.space.Value;
			}
			set
			{
				this.space.Value = value;
			}
		}

		public void CopyPropertiesFrom(IXlsxWorkbookExportContext context, TextSharedString textSharedString)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorkbookExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<TextSharedString>(textSharedString, "textSharedString");
			base.InnerText = textSharedString.Text;
		}

		protected override bool ShouldExport(IXlsxWorkbookExportContext context)
		{
			return true;
		}

		readonly SpaceAttribute space;
	}
}
