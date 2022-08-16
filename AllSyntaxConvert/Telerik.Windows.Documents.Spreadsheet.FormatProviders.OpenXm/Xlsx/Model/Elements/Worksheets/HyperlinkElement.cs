using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Export;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Import;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Parts;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Types;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Worksheets
{
	class HyperlinkElement : WorksheetElementBase
	{
		public HyperlinkElement(XlsxPartsManager partsManager)
			: base(partsManager)
		{
			this.reference = base.RegisterAttribute<ConvertedOpenXmlAttribute<CellRefRange>>(new ConvertedOpenXmlAttribute<CellRefRange>("ref", XlsxConverters.CellRefRangeConverter, true));
			this.relationId = base.RegisterAttribute<OpenXmlAttribute<string>>(new OpenXmlAttribute<string>("id", OpenXmlNamespaces.OfficeDocumentRelationshipsNamespace, false));
			this.tooltip = base.RegisterAttribute<OpenXmlAttribute<string>>(new OpenXmlAttribute<string>("tooltip", false));
			this.location = base.RegisterAttribute<OpenXmlAttribute<string>>(new OpenXmlAttribute<string>("location", false));
			this.display = base.RegisterAttribute<OpenXmlAttribute<string>>(new OpenXmlAttribute<string>("display", false));
		}

		public override string ElementName
		{
			get
			{
				return "hyperlink";
			}
		}

		public CellRefRange Reference
		{
			get
			{
				return this.reference.Value;
			}
			set
			{
				this.reference.Value = value;
			}
		}

		public string RelationshipId
		{
			get
			{
				return this.relationId.Value;
			}
			set
			{
				this.relationId.Value = value;
			}
		}

		public string Tooltip
		{
			get
			{
				return this.tooltip.Value;
			}
			set
			{
				this.tooltip.Value = value;
			}
		}

		public string Location
		{
			get
			{
				return this.location.Value;
			}
			set
			{
				this.location.Value = value;
			}
		}

		public string Display
		{
			get
			{
				return this.display.Value;
			}
			set
			{
				this.display.Value = value;
			}
		}

		public void CopyPropertiesFrom(IXlsxWorksheetExportContext context, SpreadsheetHyperlink hyperlink)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorksheetExportContext>(context, "context");
			this.Reference = new CellRefRange(hyperlink.Range);
			if (!string.IsNullOrEmpty(hyperlink.HyperlinkInfo.SubAddress))
			{
				this.Location = hyperlink.HyperlinkInfo.SubAddress;
			}
			else
			{
				string target;
				if (SpreadsheetHelper.EmailRegex.IsMatch(hyperlink.HyperlinkInfo.Address))
				{
					target = hyperlink.HyperlinkInfo.GetEmailAddress();
				}
				else
				{
					target = hyperlink.HyperlinkInfo.Address;
				}
				this.RelationshipId = base.PartsManager.CreateWorksheetRelationship((WorksheetPart)base.Part, target, XlsxRelationshipTypes.HyperlinkRelationshipType, "External");
			}
			if (!string.IsNullOrEmpty(hyperlink.HyperlinkInfo.ScreenTip))
			{
				this.Tooltip = hyperlink.HyperlinkInfo.ScreenTip;
			}
		}

		protected override void OnAfterRead(IXlsxWorksheetImportContext context)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorksheetImportContext>(context, "context");
			string address = string.Empty;
			string empty = string.Empty;
			string empty2 = string.Empty;
			string text = this.Location;
			string screenTip = this.Tooltip;
			if (this.RelationshipId != null)
			{
				string worksheetRelationshipTarget = base.PartsManager.GetWorksheetRelationshipTarget((WorksheetPart)base.Part, this.RelationshipId);
				if (!SpreadsheetHelper.TryParseMailToAddress(worksheetRelationshipTarget, out empty, out empty2))
				{
					address = worksheetRelationshipTarget;
				}
			}
			HyperlinkInfo hyperlinkInfo;
			if (!string.IsNullOrEmpty(text))
			{
				hyperlinkInfo = HyperlinkInfo.CreateInDocumentHyperlink(text, screenTip);
			}
			else if (!string.IsNullOrEmpty(empty))
			{
				hyperlinkInfo = HyperlinkInfo.CreateMailtoHyperlink(empty, empty2, screenTip);
			}
			else
			{
				hyperlinkInfo = HyperlinkInfo.CreateHyperlink(address, screenTip);
			}
			context.Worksheet.Hyperlinks.Add(this.Reference.ToCellRange(), hyperlinkInfo);
		}

		readonly ConvertedOpenXmlAttribute<CellRefRange> reference;

		readonly OpenXmlAttribute<string> relationId;

		readonly OpenXmlAttribute<string> tooltip;

		readonly OpenXmlAttribute<string> location;

		readonly OpenXmlAttribute<string> display;
	}
}
