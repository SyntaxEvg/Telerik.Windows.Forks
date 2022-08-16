using System;
using System.Globalization;
using Telerik.Documents.SpreadsheetStreaming.Core;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core.Types;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Model.Elements.Worksheet;
using Telerik.Documents.SpreadsheetStreaming.Model.Formatting;
using Telerik.Documents.SpreadsheetStreaming.Utilities;

namespace Telerik.Documents.SpreadsheetStreaming.Exporters.Xlsx
{
	sealed class XlsxCellExporter : EntityBase, ICellExporter, IDisposable
	{
		internal XlsxCellExporter(XlsxRowExporter row, RowElement rowElement, StylesRepository stylesRepository, int rowIndex, int columnIndex)
		{
			Guard.ThrowExceptionIfGreaterThan<int>(DefaultValues.ColumnCount - 1, columnIndex, "columnIndex");
			this.row = row;
			this.stylesRepository = stylesRepository;
			this.rowIndex = rowIndex;
			this.columnIndex = columnIndex;
			this.cellElement = rowElement.CreateCellElementWriter();
		}

		public void SetValue(string value)
		{
			Guard.ThrowExceptionIfNull<string>(value, "value");
			this.value = value;
			this.valueType = new CellValueType?(CellValueType.Text);
		}

		public void SetValue(double value)
		{
			Guard.ThrowExceptionIfNaN(value, "value");
			Guard.ThrowExceptionIfInfinity(value, "value");
			this.value = value.ToString(CultureInfo.InvariantCulture);
		}

		public void SetValue(bool value)
		{
			this.value = (value ? 1 : 0).ToString(CultureInfo.InvariantCulture);
			this.valueType = new CellValueType?(CellValueType.Boolean);
		}

		public void SetValue(DateTime value)
		{
			this.value = value.ToString("s", CultureInfo.InvariantCulture);
			this.valueType = new CellValueType?(CellValueType.Date);
		}

		public void SetFormula(string value)
		{
			Guard.ThrowExceptionIfNull<string>(value, "value");
			this.value = value;
			this.valueType = new CellValueType?(CellValueType.Formula);
		}

		public void SetFormat(SpreadCellFormat cellFormat)
		{
			Guard.ThrowExceptionIfNull<SpreadCellFormat>(cellFormat, "cellFormat");
			this.cellStyleId = new int?(this.stylesRepository.RegisterCellFormat(cellFormat));
		}

		internal sealed override void CompleteWriteOverride()
		{
			if (this.value == null && this.cellStyleId == null)
			{
				return;
			}
			this.cellElement.Reference = new CellRef(this.rowIndex, this.columnIndex);
			if (this.cellStyleId != null)
			{
				this.cellElement.StyleIndex = this.cellStyleId.Value;
			}
			if (this.valueType != null && this.valueType.Value != CellValueType.Formula)
			{
				this.cellElement.CellDataType = CellTypesMapper.GetCellTypeName(this.valueType.Value);
			}
			this.cellElement.EnsureWritingStarted();
			if (this.valueType != null)
			{
				CellValueType? cellValueType = this.valueType;
				CellValueType valueOrDefault = cellValueType.GetValueOrDefault();
				if (cellValueType != null)
				{
					switch (valueOrDefault)
					{
					case CellValueType.Boolean:
					case CellValueType.Date:
					case CellValueType.Text:
						this.cellElement.WriteValue(this.value);
						goto IL_127;
					case CellValueType.Formula:
						this.cellElement.WriteFormula(this.value);
						goto IL_127;
					}
				}
				throw new NotImplementedException();
			}
			if (this.value != null)
			{
				this.cellElement.WriteValue(this.value);
			}
			IL_127:
			this.cellElement.EnsureWritingEnded();
		}

		internal override void DisposeOverride()
		{
			this.row.NotifyCellDisposing();
			this.row = null;
			base.DisposeOverride();
		}

		readonly StylesRepository stylesRepository;

		readonly int rowIndex;

		readonly int columnIndex;

		readonly CellElement cellElement;

		string value;

		CellValueType? valueType;

		int? cellStyleId;

		XlsxRowExporter row;
	}
}
