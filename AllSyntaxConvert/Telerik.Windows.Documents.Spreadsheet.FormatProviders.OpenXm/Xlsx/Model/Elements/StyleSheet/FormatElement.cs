using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Contexts;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Export;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Import;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.StyleSheet
{
	class FormatElement : StyleSheetElementBase
	{
		public FormatElement(XlsxPartsManager partsManager)
			: base(partsManager)
		{
			this.numberFormatId = base.RegisterAttribute<IntOpenXmlAttribute>(new IntOpenXmlAttribute("numFmtId", false));
			this.fontId = base.RegisterAttribute<IntOpenXmlAttribute>(new IntOpenXmlAttribute("fontId", false));
			this.fillId = base.RegisterAttribute<IntOpenXmlAttribute>(new IntOpenXmlAttribute("fillId", false));
			this.borderId = base.RegisterAttribute<IntOpenXmlAttribute>(new IntOpenXmlAttribute("borderId", false));
			this.formatId = base.RegisterAttribute<IntOpenXmlAttribute>(new IntOpenXmlAttribute("xfId", false));
			this.applyNumberFormat = base.RegisterAttribute<BoolOpenXmlAttribute>(new BoolOpenXmlAttribute("applyNumberFormat"));
			this.applyFont = base.RegisterAttribute<BoolOpenXmlAttribute>(new BoolOpenXmlAttribute("applyFont"));
			this.applyFill = base.RegisterAttribute<BoolOpenXmlAttribute>(new BoolOpenXmlAttribute("applyFill"));
			this.applyBorder = base.RegisterAttribute<BoolOpenXmlAttribute>(new BoolOpenXmlAttribute("applyBorder"));
			this.applyAlignment = base.RegisterAttribute<BoolOpenXmlAttribute>(new BoolOpenXmlAttribute("applyAlignment"));
			this.applyProtection = base.RegisterAttribute<BoolOpenXmlAttribute>(new BoolOpenXmlAttribute("applyProtection"));
			this.alignments = base.RegisterChildElement<AlignmentElement>("alignment");
			this.protection = base.RegisterChildElement<ProtectionElement>("protection");
		}

		public bool ApplyAlignment
		{
			get
			{
				return this.applyAlignment.Value;
			}
			set
			{
				this.applyAlignment.Value = value;
			}
		}

		public bool ApplyBorder
		{
			get
			{
				return this.applyBorder.Value;
			}
			set
			{
				this.applyBorder.Value = value;
			}
		}

		public bool ApplyFill
		{
			get
			{
				return this.applyFill.Value;
			}
			set
			{
				this.applyFill.Value = value;
			}
		}

		public bool ApplyFont
		{
			get
			{
				return this.applyFont.Value;
			}
			set
			{
				this.applyFont.Value = value;
			}
		}

		public bool ApplyNumberFormat
		{
			get
			{
				return this.applyNumberFormat.Value;
			}
			set
			{
				this.applyNumberFormat.Value = value;
			}
		}

		public bool ApplyProtection
		{
			get
			{
				return this.applyProtection.Value;
			}
			set
			{
				this.applyProtection.Value = value;
			}
		}

		public int BorderId
		{
			get
			{
				return this.borderId.Value;
			}
			set
			{
				this.borderId.Value = value;
			}
		}

		public int FillId
		{
			get
			{
				return this.fillId.Value;
			}
			set
			{
				this.fillId.Value = value;
			}
		}

		public int FontId
		{
			get
			{
				return this.fontId.Value;
			}
			set
			{
				this.fontId.Value = value;
			}
		}

		public int NumberFormatId
		{
			get
			{
				return this.numberFormatId.Value;
			}
			set
			{
				this.numberFormatId.Value = value;
			}
		}

		public int FormatId
		{
			get
			{
				return this.formatId.Value;
			}
			set
			{
				this.formatId.Value = value;
			}
		}

		public AlignmentElement AlignmentsElement
		{
			get
			{
				return this.alignments.Element;
			}
			set
			{
				this.alignments.Element = value;
			}
		}

		public ProtectionElement ProtectionElement
		{
			get
			{
				return this.protection.Element;
			}
			set
			{
				this.protection.Element = value;
			}
		}

		public override string ElementName
		{
			get
			{
				return "xf";
			}
		}

		public void CopyPropertiesFrom(IXlsxWorkbookExportContext context, FormattingRecord format)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorkbookExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<FormattingRecord>(format, "format");
			base.CreateElement(this.alignments);
			base.CreateElement(this.protection);
			this.FormatId = 0;
			if (format.StyleFormattingRecordId != null)
			{
				this.FormatId = format.StyleFormattingRecordId.Value;
			}
			FormattingRecord format2 = context.StyleSheet.StyleFormattingTable[this.FormatId];
			this.CopyPropertiesFromStyleInternal(context, format2);
			this.CopyPropertiesFromDirectFormatInternal(context, format);
		}

		public FormattingRecord CreateFormattingRecord(IXlsxWorkbookImportContext context)
		{
			FormattingRecord result = default(FormattingRecord);
			if (this.formatId.HasValue)
			{
				result.StyleFormattingRecordId = new int?(this.FormatId);
			}
			if (this.fontId.HasValue)
			{
				result.FontInfoId = new int?(this.FontId);
			}
			if (this.borderId.HasValue)
			{
				result.BordersInfoId = new int?(this.BorderId);
			}
			if (this.fillId.HasValue)
			{
				result.FillId = new int?(this.FillId);
			}
			if (this.numberFormatId.HasValue)
			{
				result.NumberFormatId = new int?(this.NumberFormatId);
			}
			if (this.applyNumberFormat.HasValue)
			{
				result.ApplyNumberFormat = new bool?(this.ApplyNumberFormat);
			}
			if (this.applyAlignment.HasValue)
			{
				result.ApplyAlignment = new bool?(this.ApplyAlignment);
			}
			if (this.applyFont.HasValue)
			{
				result.ApplyFont = new bool?(this.ApplyFont);
			}
			if (this.applyBorder.HasValue)
			{
				result.ApplyBorder = new bool?(this.ApplyBorder);
			}
			if (this.applyFill.HasValue)
			{
				result.ApplyFill = new bool?(this.ApplyFill);
			}
			if (this.applyProtection.HasValue)
			{
				result.ApplyProtection = new bool?(this.ApplyProtection);
			}
			if (this.AlignmentsElement != null)
			{
				this.AlignmentsElement.ApplyAllignment(context, ref result);
				base.ReleaseElement(this.alignments);
			}
			if (this.ProtectionElement != null)
			{
				this.ProtectionElement.ApplyProtection(context, ref result);
				base.ReleaseElement(this.protection);
			}
			return result;
		}

		void CopyPropertiesFromDirectFormatInternal(IXlsxWorkbookExportContext context, FormattingRecord format)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorkbookExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<FormattingRecord>(format, "format");
			if (format.FontInfoId != null)
			{
				this.FontId = format.FontInfoId.Value;
				this.ApplyFont = true;
			}
			if (format.BordersInfoId != null)
			{
				this.BorderId = format.BordersInfoId.Value;
				this.ApplyBorder = true;
			}
			if (format.FillId != null)
			{
				this.FillId = format.FillId.Value;
				this.ApplyFill = true;
			}
			if (format.NumberFormatId != null)
			{
				this.NumberFormatId = format.NumberFormatId.Value;
				this.ApplyNumberFormat = true;
			}
			this.AlignmentsElement.CopyPropertiesFrom(context, format);
			this.ProtectionElement.CopyPropertiesFrom(context, format);
			if (format.ApplyNumberFormat != null)
			{
				this.ApplyNumberFormat = format.ApplyNumberFormat.Value;
			}
			if (format.ApplyAlignment != null)
			{
				this.ApplyAlignment = format.ApplyAlignment.Value;
			}
			if (format.ApplyFont != null)
			{
				this.ApplyFont = format.ApplyFont.Value;
			}
			if (format.ApplyBorder != null)
			{
				this.ApplyBorder = format.ApplyBorder.Value;
			}
			if (format.ApplyFill != null)
			{
				this.ApplyFill = format.ApplyFill.Value;
			}
			if (format.ApplyProtection != null)
			{
				this.ApplyProtection = format.ApplyProtection.Value;
			}
		}

		void CopyPropertiesFromStyleInternal(IXlsxWorkbookExportContext context, FormattingRecord format)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorkbookExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<FormattingRecord>(format, "format");
			if (format.FontInfoId != null && format.ApplyFont == true)
			{
				this.FontId = format.FontInfoId.Value;
			}
			if (format.BordersInfoId != null && format.ApplyBorder == true)
			{
				this.BorderId = format.BordersInfoId.Value;
			}
			if (format.FillId != null && format.ApplyFill == true)
			{
				this.FillId = format.FillId.Value;
			}
			if (format.NumberFormatId != null && format.ApplyNumberFormat == true)
			{
				this.NumberFormatId = format.NumberFormatId.Value;
			}
			if (format.ApplyAlignment == true)
			{
				this.AlignmentsElement.CopyPropertiesFrom(context, format);
			}
			if (format.ApplyProtection == true)
			{
				this.ProtectionElement.CopyPropertiesFrom(context, format);
			}
		}

		readonly BoolOpenXmlAttribute applyAlignment;

		readonly BoolOpenXmlAttribute applyBorder;

		readonly BoolOpenXmlAttribute applyFill;

		readonly BoolOpenXmlAttribute applyFont;

		readonly BoolOpenXmlAttribute applyNumberFormat;

		readonly BoolOpenXmlAttribute applyProtection;

		readonly IntOpenXmlAttribute borderId;

		readonly IntOpenXmlAttribute fillId;

		readonly IntOpenXmlAttribute fontId;

		readonly IntOpenXmlAttribute numberFormatId;

		readonly IntOpenXmlAttribute formatId;

		readonly OpenXmlChildElement<AlignmentElement> alignments;

		readonly OpenXmlChildElement<ProtectionElement> protection;
	}
}
