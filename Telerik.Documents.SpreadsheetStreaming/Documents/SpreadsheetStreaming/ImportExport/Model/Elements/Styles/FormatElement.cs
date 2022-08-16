using System;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core.Attributes;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Model.Writers.Worksheet.Styles;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Model.Elements.Styles
{
	class FormatElement : DirectElementBase<DiferentialFormat>
	{
		public FormatElement()
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
		}

		public override string ElementName
		{
			get
			{
				return "xf";
			}
		}

		bool ApplyAlignment
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

		bool ApplyBorder
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

		bool ApplyFill
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

		bool ApplyFont
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

		bool ApplyNumberFormat
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

		bool ApplyProtection
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

		int BorderId
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

		int FillId
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

		int FontId
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

		int NumberFormatId
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

		int FormatId
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

		protected override void InitializeAttributesOverride(DiferentialFormat value)
		{
			if (value.FormatId != null)
			{
				this.FormatId = value.FormatId.Value;
			}
			if (value.FontId != null)
			{
				this.FontId = value.FontId.Value;
			}
			if (value.BorderId != null)
			{
				this.BorderId = value.BorderId.Value;
			}
			if (value.FillId != null)
			{
				this.FillId = value.FillId.Value;
			}
			if (value.NumFmtId != null)
			{
				this.NumberFormatId = value.NumFmtId.Value;
			}
			if (value.ApplyNumberFormat != null)
			{
				this.ApplyNumberFormat = value.ApplyNumberFormat.Value;
			}
			if (value.ApplyAlignment != null)
			{
				this.ApplyAlignment = value.ApplyAlignment.Value;
			}
			if (value.ApplyFont != null)
			{
				this.ApplyFont = value.ApplyFont.Value;
			}
			if (value.ApplyBorder != null)
			{
				this.ApplyBorder = value.ApplyBorder.Value;
			}
			if (value.ApplyFill != null)
			{
				this.ApplyFill = value.ApplyFill.Value;
			}
			if (value.ApplyProtection != null)
			{
				this.ApplyProtection = value.ApplyProtection.Value;
			}
		}

		protected override void WriteChildElementsOverride(DiferentialFormat value)
		{
			AlignmentElement alignmentElement = base.CreateChildElement<AlignmentElement>();
			alignmentElement.Write(value);
			if (value.IsLocked != null)
			{
				ProtectionElement protectionElement = base.CreateChildElement<ProtectionElement>();
				protectionElement.Write(value);
			}
		}

		protected override void CopyAttributesOverride(ref DiferentialFormat value)
		{
			if (this.formatId.HasValue)
			{
				value.FormatId = new int?(this.FormatId);
			}
			if (this.fontId.HasValue)
			{
				value.FontId = new int?(this.FontId);
			}
			if (this.borderId.HasValue)
			{
				value.BorderId = new int?(this.BorderId);
			}
			if (this.fillId.HasValue)
			{
				value.FillId = new int?(this.FillId);
			}
			if (this.numberFormatId.HasValue)
			{
				value.NumFmtId = new int?(this.NumberFormatId);
			}
			if (this.applyNumberFormat.HasValue)
			{
				value.ApplyNumberFormat = new bool?(this.ApplyNumberFormat);
			}
			if (this.applyAlignment.HasValue)
			{
				value.ApplyAlignment = new bool?(this.ApplyAlignment);
			}
			if (this.applyFont.HasValue)
			{
				value.ApplyFont = new bool?(this.ApplyFont);
			}
			if (this.applyBorder.HasValue)
			{
				value.ApplyBorder = new bool?(this.ApplyBorder);
			}
			if (this.applyFill.HasValue)
			{
				value.ApplyFill = new bool?(this.ApplyFill);
			}
			if (this.applyProtection.HasValue)
			{
				value.ApplyProtection = new bool?(this.ApplyProtection);
			}
		}

		protected override void ReadChildElementOverride(ElementBase element, ref DiferentialFormat value)
		{
			string elementName;
			if ((elementName = element.ElementName) != null)
			{
				if (elementName == "alignment")
				{
					AlignmentElement alignmentElement = element as AlignmentElement;
					alignmentElement.Read(ref value);
					return;
				}
				if (elementName == "protection")
				{
					ProtectionElement protectionElement = element as ProtectionElement;
					protectionElement.Read(ref value);
					return;
				}
			}
			throw new InvalidOperationException();
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
	}
}
