using System;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core.Attributes;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core.Types;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Model.Elements.Worksheet
{
	class CellElement : ConsecutiveElementBase
	{
		public CellElement()
		{
			base.RegisterChildElement<CellValueElement>();
			base.RegisterChildElement<FormulaElement>();
			this.reference = base.RegisterAttribute<ConvertedOpenXmlAttribute<CellRef>>(new ConvertedOpenXmlAttribute<CellRef>("r", Converters.CellRefConverter, false));
			this.styleIndex = base.RegisterAttribute<IntOpenXmlAttribute>(new IntOpenXmlAttribute("s", 0, false));
			this.cellDataType = base.RegisterAttribute<string>("t", CellTypesMapper.Number, false);
		}

		public override string ElementName
		{
			get
			{
				return "c";
			}
		}

		public CellRef Reference
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

		public int StyleIndex
		{
			set
			{
				this.styleIndex.Value = value;
			}
		}

		public string CellDataType
		{
			get
			{
				return this.cellDataType.Value;
			}
			set
			{
				this.cellDataType.Value = value;
			}
		}

		public void WriteFormula(string value)
		{
			FormulaElement formulaElement = base.CreateChildElement<FormulaElement>();
			formulaElement.Write(value);
		}

		public void WriteValue(string value)
		{
			CellValueElement cellValueElement = base.CreateChildElement<CellValueElement>();
			cellValueElement.Write(value);
		}

		public void ReadFormula(ref string value)
		{
			FormulaElement formulaElement = base.CreateChildElement<FormulaElement>();
			formulaElement.Read(ref value);
		}

		public void ReadValue(ref string value)
		{
			CellValueElement cellValueElement = base.CreateChildElement<CellValueElement>();
			cellValueElement.Read(ref value);
		}

		readonly ConvertedOpenXmlAttribute<CellRef> reference;

		readonly IntOpenXmlAttribute styleIndex;

		readonly OpenXmlAttribute<string> cellDataType;
	}
}
