using System;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core.Attributes;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core.Types;
using Telerik.Documents.SpreadsheetStreaming.Model;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Model.Elements.Worksheet
{
	class PaneElement : DirectElementBase<Pane>
	{
		public PaneElement()
		{
			this.xSplit = base.RegisterAttribute<IntOpenXmlAttribute>(new IntOpenXmlAttribute("xSplit", 0, false));
			this.ySplit = base.RegisterAttribute<IntOpenXmlAttribute>(new IntOpenXmlAttribute("ySplit", 0, false));
			this.topLeftCell = base.RegisterAttribute<ConvertedOpenXmlAttribute<CellRef>>(new ConvertedOpenXmlAttribute<CellRef>("topLeftCell", Converters.CellRefConverter, true));
			this.activePane = base.RegisterAttribute<MappedOpenXmlAttribute<SpreadPane>>(new MappedOpenXmlAttribute<SpreadPane>("activePane", null, SpreadPaneMapper.Instance, false));
			this.state = base.RegisterAttribute<MappedOpenXmlAttribute<PaneState>>(new MappedOpenXmlAttribute<PaneState>("state", null, ViewportStateMapper.Instance, false));
		}

		public override string ElementName
		{
			get
			{
				return "pane";
			}
		}

		int XSplit
		{
			set
			{
				this.xSplit.Value = value;
			}
		}

		int YSplit
		{
			set
			{
				this.ySplit.Value = value;
			}
		}

		CellRef TopLeftCell
		{
			set
			{
				this.topLeftCell.Value = value;
			}
		}

		SpreadPane ActivePane
		{
			set
			{
				this.activePane.Value = value;
			}
		}

		PaneState State
		{
			set
			{
				this.state.Value = value;
			}
		}

		protected override void InitializeAttributesOverride(Pane value)
		{
			if (value.XSplit != null)
			{
				this.XSplit = value.XSplit.Value;
			}
			if (value.YSplit != null)
			{
				this.YSplit = value.YSplit.Value;
			}
			int rowIndex;
			int columnIndex;
			value.GetTopLeftCell(out rowIndex, out columnIndex);
			this.TopLeftCell = new CellRef(rowIndex, columnIndex);
			if (value.ActivePane != null)
			{
				this.ActivePane = value.ActivePane.Value;
			}
			if (value.State != null)
			{
				this.State = value.State.Value;
			}
		}

		protected override void WriteChildElementsOverride(Pane value)
		{
		}

		readonly IntOpenXmlAttribute xSplit;

		readonly IntOpenXmlAttribute ySplit;

		readonly ConvertedOpenXmlAttribute<CellRef> topLeftCell;

		readonly MappedOpenXmlAttribute<PaneState> state;

		readonly MappedOpenXmlAttribute<SpreadPane> activePane;
	}
}
