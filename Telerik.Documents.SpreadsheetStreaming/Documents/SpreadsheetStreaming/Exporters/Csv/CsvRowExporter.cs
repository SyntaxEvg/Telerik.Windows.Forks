using System;
using System.IO;
using System.Text;
using Telerik.Documents.SpreadsheetStreaming.Core;
using Telerik.Documents.SpreadsheetStreaming.Utilities;

namespace Telerik.Documents.SpreadsheetStreaming.Exporters.Csv
{
	class CsvRowExporter : EntityBase, IRowExporter, IDisposable
	{
		internal CsvRowExporter(StreamWriter writer)
		{
			this.isFirstCellInRow = true;
			this.writer = writer;
		}

		public ICellExporter CreateCellExporter()
		{
			if (!this.isFirstCellInRow)
			{
				this.Write(CsvRowExporter.valuesSeparator.ToString());
			}
			CsvCellExporter result = new CsvCellExporter(this.writer);
			this.isFirstCellInRow = false;
			return result;
		}

		public void SkipCells(int count)
		{
			Guard.ThrowExceptionIfLessThan<int>(0, count, "count");
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < count; i++)
			{
				stringBuilder.Append(CsvRowExporter.valuesSeparator);
			}
			this.Write(stringBuilder.ToString());
		}

		public void SetOutlineLevel(int value)
		{
		}

		public void SetHeightInPixels(double value)
		{
		}

		public void SetHeightInPoints(double value)
		{
		}

		public void SetHidden(bool value)
		{
		}

		internal override void CompleteWriteOverride()
		{
			this.writer.WriteLine();
			base.CompleteWriteOverride();
		}

		void Write(string value)
		{
			this.writer.Write(value);
		}

		static readonly char valuesSeparator = ',';

		readonly StreamWriter writer;

		bool isFirstCellInRow;
	}
}
