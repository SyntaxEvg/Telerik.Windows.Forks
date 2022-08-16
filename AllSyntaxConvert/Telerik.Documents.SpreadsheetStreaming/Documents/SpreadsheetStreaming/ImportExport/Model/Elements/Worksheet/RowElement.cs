using System;
using System.Collections.Generic;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core.Attributes;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core.Types;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Model.Elements.Worksheet
{
	class RowElement : ConsecutiveElementBase
	{
		public RowElement()
		{
			base.RegisterChildElement<CellElement>();
			this.rowIndex = base.RegisterAttribute<ConvertedOpenXmlAttribute<int?>>(new ConvertedOpenXmlAttribute<int?>("r", Converters.NullableIntValueConverter, false));
			this.rowHeight = base.RegisterAttribute<double>("ht", false);
			this.styleIndex = base.RegisterAttribute<IntOpenXmlAttribute>(new IntOpenXmlAttribute("s", 0, false));
			this.customFormat = base.RegisterAttribute<BoolOpenXmlAttribute>(new BoolOpenXmlAttribute("customFormat"));
			this.customHeight = base.RegisterAttribute<BoolOpenXmlAttribute>(new BoolOpenXmlAttribute("customHeight"));
			this.hidden = base.RegisterAttribute<BoolOpenXmlAttribute>(new BoolOpenXmlAttribute("hidden"));
			this.outlineLevel = base.RegisterAttribute<IntOpenXmlAttribute>(new IntOpenXmlAttribute("outlineLevel", false));
		}

		public override string ElementName
		{
			get
			{
				return "row";
			}
		}

		public double RowHeight
		{
			get
			{
				return this.rowHeight.Value;
			}
			set
			{
				this.rowHeight.Value = value;
			}
		}

		public int? RowIndex
		{
			get
			{
				return this.rowIndex.Value;
			}
			set
			{
				this.rowIndex.Value = value;
			}
		}

		public bool CustomFormat
		{
			set
			{
				this.customFormat.Value = value;
			}
		}

		public bool CustomHeight
		{
			get
			{
				return this.customHeight.Value;
			}
			set
			{
				this.customHeight.Value = value;
			}
		}

		public int StyleIndex
		{
			set
			{
				this.styleIndex.Value = value;
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

		public int OutlineLevel
		{
			get
			{
				return this.outlineLevel.Value;
			}
			set
			{
				this.outlineLevel.Value = value;
			}
		}

		public IEnumerable<CellElement> Cells
		{
			get
			{
				while (base.ReadToElement<CellElement>())
				{
					CellElement cellElement = base.CreateChildElement<CellElement>();
					cellElement.BeginReadElement();
					yield return cellElement;
					cellElement.EndReadElement();
				}
				yield break;
			}
		}

		public CellElement CreateCellElementWriter()
		{
			return base.CreateChildElement<CellElement>();
		}

		readonly OpenXmlAttribute<double> rowHeight;

		readonly ConvertedOpenXmlAttribute<int?> rowIndex;

		readonly BoolOpenXmlAttribute customFormat;

		readonly BoolOpenXmlAttribute customHeight;

		readonly IntOpenXmlAttribute styleIndex;

		readonly BoolOpenXmlAttribute hidden;

		readonly IntOpenXmlAttribute outlineLevel;
	}
}
