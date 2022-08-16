using System;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core.Attributes;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Model.Elements.Styles
{
	class NumberFormatElement : DirectElementBase<NumberFormat>
	{
		public NumberFormatElement()
		{
			this.formatCode = base.RegisterAttribute<string>("formatCode", true);
			this.formatId = base.RegisterAttribute<IntOpenXmlAttribute>(new IntOpenXmlAttribute("numFmtId", true));
		}

		public override string ElementName
		{
			get
			{
				return "numFmt";
			}
		}

		string FormatCode
		{
			get
			{
				return this.formatCode.Value;
			}
			set
			{
				this.formatCode.Value = value;
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

		protected override void InitializeAttributesOverride(NumberFormat value)
		{
			this.FormatCode = value.NumberFormatString;
			if (value.NumberFormatId != null)
			{
				this.FormatId = value.NumberFormatId.Value;
			}
		}

		protected override void CopyAttributesOverride(ref NumberFormat value)
		{
			value.NumberFormatString = this.FormatCode;
			if (this.formatId.HasValue)
			{
				value.NumberFormatId = new int?(this.FormatId);
			}
		}

		protected override void WriteChildElementsOverride(NumberFormat value)
		{
		}

		protected override void ReadChildElementOverride(ElementBase element, ref NumberFormat value)
		{
		}

		readonly OpenXmlAttribute<string> formatCode;

		readonly IntOpenXmlAttribute formatId;
	}
}
