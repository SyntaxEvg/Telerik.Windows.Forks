using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core.Attributes;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core.Types;
using Telerik.Documents.SpreadsheetStreaming.Model;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Model.Elements.Worksheet
{
	class SelectionElement : DirectElementBase<Selection>
	{
		public SelectionElement()
		{
			this.activeCell = base.RegisterAttribute<ConvertedOpenXmlAttribute<CellRef>>(new ConvertedOpenXmlAttribute<CellRef>("activeCell", Converters.CellRefConverter, true));
			this.sqref = base.RegisterAttribute<ConvertedOpenXmlAttribute<CellRefRangeSequence>>(new ConvertedOpenXmlAttribute<CellRefRangeSequence>("sqref", Converters.CellRefRangeSequenceConverter, true));
			this.pane = base.RegisterAttribute<MappedOpenXmlAttribute<SpreadPane>>(new MappedOpenXmlAttribute<SpreadPane>("pane", null, SpreadPaneMapper.Instance, false));
		}

		public override string ElementName
		{
			get
			{
				return "selection";
			}
		}

		public CellRef ActiveCell
		{
			get
			{
				return this.activeCell.Value;
			}
			set
			{
				this.activeCell.Value = value;
			}
		}

		public CellRefRangeSequence SqRef
		{
			get
			{
				return this.sqref.Value;
			}
			set
			{
				this.sqref.Value = value;
			}
		}

		public SpreadPane Pane
		{
			get
			{
				return this.pane.Value;
			}
			set
			{
				this.pane.Value = value;
			}
		}

		protected override void InitializeAttributesOverride(Selection value)
		{
			int num;
			int num2;
			value.GetActiveCell(out num, out num2);
			this.ActiveCell = new CellRef(num, num2);
			if (value.SelectionRanges == null)
			{
				SpreadCellRange range = new SpreadCellRange(num, num2, num, num2);
				CellRefRange cellRefRange = new CellRefRange(range);
				this.SqRef = new CellRefRangeSequence(new CellRefRange[] { cellRefRange });
			}
			else
			{
				IEnumerable<CellRefRange> cellRangesSequences = from p in value.SelectionRanges
					select new CellRefRange(p);
				this.SqRef = new CellRefRangeSequence(cellRangesSequences);
			}
			this.Pane = value.Pane;
		}

		protected override void WriteChildElementsOverride(Selection value)
		{
		}

		readonly ConvertedOpenXmlAttribute<CellRef> activeCell;

		readonly ConvertedOpenXmlAttribute<CellRefRangeSequence> sqref;

		readonly MappedOpenXmlAttribute<SpreadPane> pane;
	}
}
