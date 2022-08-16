using System;
using System.Collections.Generic;
using System.IO;
using Telerik.Windows.Documents.Spreadsheet.Core;
using Telerik.Windows.Documents.Spreadsheet.Core.DataStructures;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.Contexts;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.TextBased.Core;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.PropertySystem;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.TextBased
{
	public abstract class DelimitedValuesFormatProviderBase : TextBasedWorkbookFormatProviderBase
	{
		public abstract CsvSettings Settings { get; }

		public override bool CanImport
		{
			get
			{
				return true;
			}
		}

		public override bool CanExport
		{
			get
			{
				return true;
			}
		}

		public bool TrimLeadingEmptyRowsAndColumns { get; set; }

		protected DelimitedValuesFormatProviderBase()
		{
			this.TrimLeadingEmptyRowsAndColumns = true;
		}

		protected override Workbook ImportOverride(Stream input)
		{
			Workbook workbook = new Workbook();
			UpdateScope updateScope = new UpdateScope(delegate()
			{
				workbook.History.IsEnabled = false;
				workbook.SuspendLayoutUpdate();
				workbook.SuspendPropertyChanged();
			}, delegate()
			{
				workbook.ResumePropertyChanged();
				workbook.ResumeLayoutUpdate();
				workbook.History.IsEnabled = true;
			});
			using (updateScope)
			{
				Worksheet worksheet = workbook.Worksheets.Add();
				using (StreamReader streamReader = new StreamReader(input, this.Settings.Encoding))
				{
					int num = 0;
					CsvParser csvParser = new CsvParser(streamReader, this.Settings);
					ICompressedList<ICellValue> propertyValueCollection = worksheet.Cells.PropertyBag.GetPropertyValueCollection<ICellValue>(CellPropertyDefinitions.ValueProperty);
					ICompressedList<CellValueFormat> propertyValueCollection2 = worksheet.Cells.PropertyBag.GetPropertyValueCollection<CellValueFormat>(CellPropertyDefinitions.FormatProperty);
					foreach (CsvRecord csvRecord in csvParser.Parse())
					{
						int num2 = 0;
						foreach (string value in csvRecord.GetValues())
						{
							ICellValue value2;
							CellValueFormat cellValueFormat;
							CellValueFactory.CreateIgnoreErrors(value, worksheet, num, num2, CellValueFormat.GeneralFormat, out value2, out cellValueFormat);
							long index = WorksheetPropertyBagBase.ConvertCellIndexToLong(num, num2);
							propertyValueCollection.SetValue(index, value2);
							if (cellValueFormat != CellValueFormat.GeneralFormat)
							{
								propertyValueCollection2.SetValue(index, cellValueFormat);
							}
							num2++;
						}
						num++;
					}
				}
			}
			return workbook;
		}

		protected override void ExportOverride(Workbook workbook, Stream output)
		{
			CsvWriter csvWriter = new CsvWriter(output, this.Settings);
			LicenseCheck.Validate(csvWriter);
			Worksheet activeWorksheet = workbook.ActiveWorksheet;
			WorksheetExportContext worksheetExportContext = new WorksheetExportContext(activeWorksheet);
			CellRange cellRange = worksheetExportContext.ValuePropertyDataInfo.GetUsedCellRange();
			if (cellRange == null)
			{
				return;
			}
			if (!this.TrimLeadingEmptyRowsAndColumns)
			{
				cellRange = new CellRange(new CellIndex(0, 0), cellRange.ToIndex);
			}
			for (int i = cellRange.FromIndex.RowIndex; i <= cellRange.ToIndex.RowIndex; i++)
			{
				Range range = worksheetExportContext.ValuePropertyDataInfo.GetRowUsedRange(i);
				if (range != null)
				{
					range = range.Expand(cellRange.FromIndex.ColumnIndex);
				}
				csvWriter.WriteRecord(this.GetRowValuesToExport(activeWorksheet, range, i));
			}
		}

		IEnumerable<string> GetRowValuesToExport(Worksheet worksheet, Range usedRange, int rowIndex)
		{
			if (usedRange != null)
			{
				for (int columnIndex = usedRange.Start; columnIndex <= usedRange.End; columnIndex++)
				{
					CellSelection selection = worksheet.Cells[rowIndex, columnIndex];
					ICellValue cellValue = selection.GetValue().Value;
					CellValueFormat formatting = selection.GetFormat().Value;
					FormulaCellValue formulaCellValue = cellValue as FormulaCellValue;
					if (formulaCellValue != null)
					{
						cellValue = formulaCellValue.GetResultValueAsCellValue();
					}
					yield return formatting.GetFormatResult(cellValue).ExtendedVisibleInfosText;
				}
			}
			yield break;
		}
	}
}
