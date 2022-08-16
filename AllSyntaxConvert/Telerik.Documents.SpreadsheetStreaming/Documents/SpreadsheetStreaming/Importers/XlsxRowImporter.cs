using System;
using System.Collections.Generic;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Model.Elements.Worksheet;
using Telerik.Documents.SpreadsheetStreaming.Utilities;

namespace Telerik.Documents.SpreadsheetStreaming.Importers
{
	class XlsxRowImporter : IRowImporter
	{
		internal XlsxRowImporter(RowElement rowElement)
		{
			this.rowElement = rowElement;
		}

		public int RowIndex
		{
			get
			{
				return this.rowElement.RowIndex.Value - 1;
			}
		}

		public int OutlineLevel
		{
			get
			{
				return this.rowElement.OutlineLevel;
			}
		}

		public bool IsCustomHeight
		{
			get
			{
				return this.rowElement.CustomHeight;
			}
		}

		public double HeightInPixels
		{
			get
			{
				return UnitHelper.PointToDip(this.HeightInPoints);
			}
		}

		public double HeightInPoints
		{
			get
			{
				if (this.rowElement.CustomHeight)
				{
					return this.rowElement.RowHeight;
				}
				return DefaultValues.DefaultRowHeightInPoints;
			}
		}

		public bool IsHidden
		{
			get
			{
				return this.rowElement.Hidden;
			}
		}

		public IEnumerable<ICellImporter> Cells
		{
			get
			{
				foreach (CellElement cellElement in this.rowElement.Cells)
				{
					yield return new XlsxCellImporter(cellElement);
				}
				yield break;
			}
		}

		readonly RowElement rowElement;
	}
}
