using System;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core.Attributes;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Model.Elements.Styles
{
	class CellStyleElement : DirectElementBase<CellStyleInfo>
	{
		public CellStyleElement()
		{
			this.name = base.RegisterAttribute<OpenXmlAttribute<string>>(new OpenXmlAttribute<string>("name", false));
			this.formatId = base.RegisterAttribute<IntOpenXmlAttribute>(new IntOpenXmlAttribute("xfId", true));
			this.builtinId = base.RegisterAttribute<IntOpenXmlAttribute>(new IntOpenXmlAttribute("builtinId", false));
			this.customBuiltin = base.RegisterAttribute<BoolOpenXmlAttribute>(new BoolOpenXmlAttribute("customBuiltin"));
		}

		public override string ElementName
		{
			get
			{
				return "cellStyle";
			}
		}

		int BuiltinId
		{
			get
			{
				return this.builtinId.Value;
			}
			set
			{
				this.builtinId.Value = value;
			}
		}

		bool CustomBuiltin
		{
			set
			{
				this.customBuiltin.Value = value;
			}
		}

		string Name
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

		protected override void InitializeAttributesOverride(CellStyleInfo value)
		{
			this.Name = value.Name;
			this.FormatId = value.FormattingRecordId;
			if (value.BuiltInId != null)
			{
				this.BuiltinId = value.BuiltInId.Value;
			}
		}

		protected override void WriteChildElementsOverride(CellStyleInfo value)
		{
		}

		protected override void CopyAttributesOverride(ref CellStyleInfo value)
		{
			value.Name = this.Name;
			value.FormattingRecordId = this.FormatId;
			if (this.builtinId.HasValue)
			{
				value.BuiltInId = new int?(this.BuiltinId);
			}
		}

		protected override void ReadChildElementOverride(ElementBase element, ref CellStyleInfo value)
		{
		}

		readonly IntOpenXmlAttribute builtinId;

		readonly BoolOpenXmlAttribute customBuiltin;

		readonly OpenXmlAttribute<string> name;

		readonly IntOpenXmlAttribute formatId;
	}
}
