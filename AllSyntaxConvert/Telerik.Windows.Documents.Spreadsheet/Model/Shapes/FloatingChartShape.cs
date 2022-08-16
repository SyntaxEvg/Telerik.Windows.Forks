using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.Model.Drawing.Charts;
using Telerik.Windows.Documents.Model.Drawing.Shapes;
using Telerik.Windows.Documents.Spreadsheet.Core.DataStructures;
using Telerik.Windows.Documents.Spreadsheet.Model.Charts;
using Telerik.Windows.Documents.Spreadsheet.PropertySystem;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model.Shapes
{
	public class FloatingChartShape : FloatingShapeBase
	{
		public override FloatingShapeType FloatingShapeType
		{
			get
			{
				return FloatingShapeType.Chart;
			}
		}

		public DocumentChart Chart
		{
			get
			{
				return this.chartShape.Chart;
			}
			set
			{
				if (this.chartShape.Chart != value)
				{
					Guard.ThrowExceptionIfNull<DocumentChart>(value, "value");
					this.chartShape.Chart.ChartChanged -= this.Chart_ChartChanged;
					this.chartShape.Chart = value;
					this.chartShape.Chart.ChartChanged += this.Chart_ChartChanged;
					base.OnShapeChanged();
				}
			}
		}

		public FloatingChartShape(Worksheet worksheet, CellIndex cellIndex, CellRange chartDataRange, params ChartType[] chartTypes)
			: this(worksheet, cellIndex, chartDataRange, SeriesRangesOrientation.Automatic, chartTypes)
		{
		}

		public FloatingChartShape(Worksheet worksheet, CellIndex cellIndex, CellRange chartDataRange, SeriesRangesOrientation seriesRangesOrientation, params ChartType[] chartTypes)
			: this(worksheet, cellIndex, new ChartShape())
		{
			DocumentChart chart = DocumentChartFactory.CreateChartFromRangeOfType(worksheet, chartDataRange, seriesRangesOrientation, chartTypes);
			this.chartShape.Chart = chart;
			this.chartShape.Chart.ChartChanged += this.Chart_ChartChanged;
			base.Outline.Fill = SpreadsheetDefaultValues.ChartLinesDefaultFill;
			base.Outline.Width = new double?(SpreadsheetDefaultValues.ChartLinesDefaultWidth);
			base.Fill = SpreadsheetDefaultValues.ChartDefaultFill;
		}

		internal FloatingChartShape(Worksheet worksheet, CellIndex cellIndex, ChartShape chartShape)
			: base(worksheet, chartShape, cellIndex, 0.0, 0.0)
		{
			this.chartShape = chartShape;
		}

		void Chart_ChartChanged(object sender, EventArgs e)
		{
			base.Worksheet.InvalidateLayout();
		}

		internal override FloatingShapeBase Copy(Worksheet worksheet, CellIndex cellIndex, double offsetX, double offsetY)
		{
			return new FloatingChartShape(worksheet, cellIndex, new ChartShape(this.chartShape))
			{
				OffsetX = offsetX,
				OffsetY = offsetY
			};
		}

		internal void OnWorksheetCellPropertyChanged(Worksheet source, CellPropertyChangedEventArgs e)
		{
			string name = e.Property.Name;
			if (name != CellPropertyDefinitions.FormatProperty.Name && name != CellPropertyDefinitions.StyleNameProperty.Name)
			{
				return;
			}
			CellRange cellRange = e.CellRange;
			SeriesGroup seriesGroup = this.Chart.SeriesGroups.First<SeriesGroup>();
			SeriesBase seriesBase = seriesGroup.Series.First<SeriesBase>();
			WorkbookFormulaChartData seriesData = seriesBase.VerticalSeriesData as WorkbookFormulaChartData;
			this.UpdateSeriesFormat(source, cellRange, seriesData);
		}

		void UpdateSeriesFormat(Worksheet source, CellRange cellRange, WorkbookFormulaChartData seriesData)
		{
			IEnumerable<CellRange> source2;
			Worksheet worksheet;
			if (seriesData != null && seriesData.TryEnumerateCellRanges(out source2, out worksheet))
			{
				CellRange cellRange2 = source2.First<CellRange>();
				CellIndex fromIndex = cellRange2.FromIndex;
				if (worksheet == source && cellRange.Contains(fromIndex))
				{
					long num = WorksheetPropertyBagBase.ConvertCellIndexToLong(fromIndex);
					CellValueFormat value = worksheet.Cells.GetPropertyValueRespectingStyle<CellValueFormat>(CellPropertyDefinitions.FormatProperty, num, num).Single<Range<long, CellValueFormat>>().Value;
					this.Chart.PrimaryAxes.ValueAxis.NumberFormat = value.FormatString;
				}
			}
		}

		readonly ChartShape chartShape;
	}
}
