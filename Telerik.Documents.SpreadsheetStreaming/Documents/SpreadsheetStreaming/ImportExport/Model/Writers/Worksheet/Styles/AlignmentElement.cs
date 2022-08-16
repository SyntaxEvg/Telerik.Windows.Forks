using System;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core.Attributes;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core.Types;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Model.Writers.Worksheet.Styles
{
	class AlignmentElement : DirectElementBase<DiferentialFormat>
	{
		public AlignmentElement()
		{
			this.horizontalAlignment = base.RegisterAttribute<OpenXmlAttribute<string>>(new OpenXmlAttribute<string>("horizontal", false));
			this.verticalAlignment = base.RegisterAttribute<OpenXmlAttribute<string>>(new OpenXmlAttribute<string>("vertical", false));
			this.indent = base.RegisterAttribute<IntOpenXmlAttribute>(new IntOpenXmlAttribute("indent", false));
			this.wrapText = base.RegisterAttribute<BoolOpenXmlAttribute>(new BoolOpenXmlAttribute("wrapText"));
		}

		public override string ElementName
		{
			get
			{
				return "alignment";
			}
		}

		protected override bool AlwaysExport
		{
			get
			{
				return false;
			}
		}

		string HorizontalAlignment
		{
			get
			{
				return this.horizontalAlignment.Value;
			}
			set
			{
				this.horizontalAlignment.Value = value;
			}
		}

		string VerticalAlignment
		{
			get
			{
				return this.verticalAlignment.Value;
			}
			set
			{
				this.verticalAlignment.Value = value;
			}
		}

		bool WrapText
		{
			get
			{
				return this.wrapText.Value;
			}
			set
			{
				this.wrapText.Value = value;
			}
		}

		int Indent
		{
			get
			{
				return this.indent.Value;
			}
			set
			{
				this.indent.Value = value;
			}
		}

		protected override void InitializeAttributesOverride(DiferentialFormat value)
		{
			if (value.HorizontalAlignment != null)
			{
				this.HorizontalAlignment = HorizontalAlignmentsMapper.GetHorizontalAlignmentName(value.HorizontalAlignment.Value);
			}
			if (value.VerticalAlignment != null)
			{
				this.VerticalAlignment = VerticalAlignmentsMapper.GetVerticalAlignmentName(value.VerticalAlignment.Value);
			}
			if (value.Indent != null)
			{
				this.Indent = value.Indent.Value;
			}
			if (value.WrapText != null)
			{
				this.WrapText = value.WrapText.Value;
			}
		}

		protected override void WriteChildElementsOverride(DiferentialFormat value)
		{
		}

		protected override void CopyAttributesOverride(ref DiferentialFormat value)
		{
			if (this.horizontalAlignment.HasValue)
			{
				value.HorizontalAlignment = new SpreadHorizontalAlignment?(HorizontalAlignmentsMapper.GetHorizontalAlignmentValue(this.HorizontalAlignment));
			}
			if (this.verticalAlignment.HasValue)
			{
				value.VerticalAlignment = VerticalAlignmentsMapper.GetVerticalAlignmentValue(this.VerticalAlignment);
			}
			if (this.indent.HasValue)
			{
				value.Indent = new int?(this.Indent);
			}
			if (this.wrapText.HasValue)
			{
				value.WrapText = new bool?(this.WrapText);
			}
		}

		protected override void ReadChildElementOverride(ElementBase element, ref DiferentialFormat value)
		{
		}

		readonly OpenXmlAttribute<string> horizontalAlignment;

		readonly OpenXmlAttribute<string> verticalAlignment;

		readonly BoolOpenXmlAttribute wrapText;

		readonly IntOpenXmlAttribute indent;
	}
}
