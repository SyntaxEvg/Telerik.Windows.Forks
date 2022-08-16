using System;
using System.Globalization;
using Telerik.Windows.Documents.Common.Model.Data;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Utilities
{
	static class XlsxHelper
	{
		public static SpreadsheetCultureHelper CultureInfo
		{
			get
			{
				return XlsxHelper.cultureInfo;
			}
		}

		public static double ConvertColumnPixelWidthToExcelWidth(Workbook workbook, double columnWidthInPixels)
		{
			Guard.ThrowExceptionIfNull<Workbook>(workbook, "workbook");
			double result = 0.0;
			if (columnWidthInPixels > 0.0)
			{
				result = UnitHelper.PixelWidthToExcelColumnWidth(workbook, columnWidthInPixels);
			}
			return result;
		}

		public static double ConvertColumnExcelWidthToPixelWidth(Workbook workbook, double excelWidth)
		{
			Guard.ThrowExceptionIfNull<Workbook>(workbook, "workbook");
			return UnitHelper.ExcelColumnWidthToPixelWidth(workbook, excelWidth);
		}

		public static string CreateResourceName(IResource resource)
		{
			return string.Format("/xl/media/{0}", resource.Name);
		}

		static readonly SpreadsheetCultureHelper cultureInfo = new SpreadsheetCultureHelper(System.Globalization.CultureInfo.InvariantCulture);
	}
}
