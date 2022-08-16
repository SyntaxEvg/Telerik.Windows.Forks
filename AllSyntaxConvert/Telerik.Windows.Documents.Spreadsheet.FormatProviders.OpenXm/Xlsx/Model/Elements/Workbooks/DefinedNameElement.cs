using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Contexts;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Export;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Import;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Workbooks
{
	class DefinedNameElement : WorkbookElementBase
	{
		public DefinedNameElement(XlsxPartsManager partsManager)
			: base(partsManager)
		{
			this.name = base.RegisterAttribute<string>("name", true);
			this.localSheetId = base.RegisterAttribute<IntOpenXmlAttribute>(new IntOpenXmlAttribute("localSheetId", -1, false));
			this.comment = base.RegisterAttribute<string>("comment", string.Empty, false);
			this.hidden = base.RegisterAttribute<BoolOpenXmlAttribute>(new BoolOpenXmlAttribute("hidden"));
		}

		public string Name
		{
			get
			{
				return this.name.Value;
			}
			set
			{
				this.name.Value = value;
			}
		}

		public int LocalSheetId
		{
			get
			{
				return this.localSheetId.Value;
			}
			set
			{
				this.localSheetId.Value = value;
			}
		}

		public string Comment
		{
			get
			{
				return this.comment.Value;
			}
			set
			{
				this.comment.Value = value;
			}
		}

		public bool Hidden
		{
			get
			{
				return this.hidden.Value;
			}
			set
			{
				this.hidden.Value = value;
			}
		}

		public override string ElementName
		{
			get
			{
				return "definedName";
			}
		}

		public void CopyPropertiesFrom(IXlsxWorkbookExportContext context, DefinedNameInfo nameInfo)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorkbookExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<DefinedNameInfo>(nameInfo, "nameInfo");
			this.Name = nameInfo.Name;
			if (nameInfo.Value.Length > 1 && SpreadsheetCultureHelper.IsCharEqualTo(nameInfo.Value[0], new string[] { "=" }))
			{
				base.InnerText = nameInfo.Value.Substring(1);
			}
			else
			{
				base.InnerText = nameInfo.Value;
			}
			if (nameInfo.LocalSheetId != -1)
			{
				this.LocalSheetId = nameInfo.LocalSheetId;
			}
			if (!string.IsNullOrEmpty(nameInfo.Comment))
			{
				this.Comment = nameInfo.Comment;
			}
			this.Hidden = nameInfo.Hidden;
		}

		protected override void OnAfterRead(IXlsxWorkbookImportContext context)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorkbookImportContext>(context, "context");
			DefinedNameInfo resource = new DefinedNameInfo(this.Name, SpreadsheetCultureHelper.PrepareFormulaValue(base.InnerText), this.LocalSheetId, this.Comment, this.Hidden);
			context.DefinedNames.Add(resource);
		}

		readonly OpenXmlAttribute<string> name;

		readonly IntOpenXmlAttribute localSheetId;

		readonly OpenXmlAttribute<string> comment;

		readonly BoolOpenXmlAttribute hidden;
	}
}
