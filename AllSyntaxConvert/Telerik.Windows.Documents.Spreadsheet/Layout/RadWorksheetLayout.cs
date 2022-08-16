using System;
using System.Collections.Generic;
using System.Windows;
using Telerik.Windows.Documents.Spreadsheet.Core.DataStructures;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Model.Shapes;
using Telerik.Windows.Documents.Spreadsheet.PropertySystem;
using Telerik.Windows.Documents.Spreadsheet.Utilities;
using Telerik.Windows.Documents.Utilities;
using Guard = Telerik.Windows.Documents.Spreadsheet.Utilities.Guard;

namespace Telerik.Windows.Documents.Spreadsheet.Layout
{
	class RadWorksheetLayout
	{
		public double Width
		{
			get
			{
				return this.size.Width;
			}
		}

		public double Height
		{
			get
			{
				return this.size.Height;
			}
		}

		public Size Size
		{
			get
			{
				return this.size;
			}
		}

		public Size ScrollableSize
		{
			get
			{
				return this.scrollableSize;
			}
		}

		public bool IsMeasureValid
		{
			get
			{
				return this.isMeasureValid;
			}
		}

		internal Worksheet Worksheet
		{
			get
			{
				return this.worksheet;
			}
			set
			{
				this.worksheet = value;
			}
		}

		internal SizeI VisibleSize
		{
			get
			{
				return this.visibleSize;
			}
			set
			{
				if (this.visibleSize != value)
				{
					Guard.ThrowExceptionIfOutOfRange<int>(0, SpreadsheetDefaultValues.RowCount, value.Height, "visibleSize");
					Guard.ThrowExceptionIfOutOfRange<int>(0, SpreadsheetDefaultValues.ColumnCount, value.Width, "visibleSize");
					this.visibleSize = value;
				}
			}
		}

		public RadWorksheetLayout(SizeI visibleSize)
		{
			Guard.ThrowExceptionIfOutOfRange<int>(0, SpreadsheetDefaultValues.RowCount, visibleSize.Height, "visibleSize");
			Guard.ThrowExceptionIfOutOfRange<int>(0, SpreadsheetDefaultValues.ColumnCount, visibleSize.Width, "visibleSize");
			this.visibleSize = visibleSize;
			this.rowsTop = new double[visibleSize.Height];
			this.columnsLeft = new double[visibleSize.Width];
			this.rowsIsEmpty = new bool[visibleSize.Height];
			this.columnsIsEmpty = new bool[visibleSize.Width];
			this.ResetIsRowsColumnsEmpty();
			this.indexToCellContentSizeCache = new Dictionary<long, Size>();
			this.shapeIdToShapeRectCache = new Dictionary<int, Rect>();
		}

		void ResetIsRowsColumnsEmpty()
		{
			for (int i = 0; i < this.visibleSize.Height; i++)
			{
				this.rowsIsEmpty[i] = true;
			}
			for (int j = 0; j < this.visibleSize.width; j++)
			{
				this.columnsIsEmpty[j] = true;
			}
		}

		void Cells_MergedCellsChanged(object sender, MergedCellRangesChangedEventArgs e)
		{
			this.InvalidateCellContentSizeCache(e.CellRange);
		}

		void RowsProperties_PropertyChanged(object sender, RowColumnPropertyChangedEventArgs e)
		{
			if (e.Property.AffectsLayout)
			{
				this.InvalidateCellContentSizeCache(CellRange.FromRowRange(e.FromIndex, e.ToIndex));
			}
		}

		void ColumnsProperties_PropertyChanged(object sender, RowColumnPropertyChangedEventArgs e)
		{
			if (e.Property.AffectsLayout)
			{
				this.InvalidateCellContentSizeCache(CellRange.FromColumnRange(e.FromIndex, e.ToIndex));
			}
		}

		void Properties_PropertyChanged(object sender, CellPropertyChangedEventArgs e)
		{
			if (e.ShouldRespectAffectLayoutProperty && e.Property.AffectsLayout)
			{
				this.InvalidateCellContentSizeCache(e.CellRange);
			}
		}

		void Worksheet_LayoutInvalidated(object sender, EventArgs e)
		{
			this.InvalidateLayout();
		}

		void InvalidateLayout()
		{
			this.isMeasureValid = false;
		}

		void InvalidateCellContentSizeCache(CellRange cellRange)
		{
			CellRange secondCellRange = cellRange;
			if (cellRange != null)
			{
				secondCellRange = CellRange.RestrictCellRange(cellRange, this.visibleSize);
			}
			this.maxInvalidatedCellRange = CellRange.MaxOrNull(this.maxInvalidatedCellRange, secondCellRange);
		}

		internal void SetWorksheet(Worksheet worksheet, bool isPrintingLayout)
		{
			this.isPrintingLayout = isPrintingLayout;
			if (worksheet == null || this.Worksheet == worksheet)
			{
				return;
			}
			this.DetachFromWorksheetEvents();
			this.Worksheet = worksheet;
			this.AttachToWorksheetEvents();
			this.ResetIsRowsColumnsEmpty();
			CellRange usedCellRange = this.Worksheet.UsedCellRange;
			if (usedCellRange.IsSingleCell && this.Worksheet.Cells.IsDefault(usedCellRange.FromIndex))
			{
				this.InvalidateCellContentSizeCache(null);
			}
			else
			{
				this.InvalidateCellContentSizeCache(CellRange.RestrictCellRange(this.Worksheet.UsedCellRange, this.visibleSize));
			}
			this.InvalidateLayout();
		}

		void AttachToWorksheetEvents()
		{
			if (this.Worksheet != null)
			{
				this.Worksheet.LayoutInvalidated += this.Worksheet_LayoutInvalidated;
				this.Worksheet.Cells.MergedCellsChanged += this.Cells_MergedCellsChanged;
				this.Worksheet.Cells.PropertyBag.PropertyChanged += this.Properties_PropertyChanged;
				this.Worksheet.Rows.PropertyBag.PropertyChanged += this.RowsProperties_PropertyChanged;
				this.Worksheet.Columns.PropertyBag.PropertyChanged += this.ColumnsProperties_PropertyChanged;
			}
		}

		void DetachFromWorksheetEvents()
		{
			if (this.Worksheet != null)
			{
				this.Worksheet.LayoutInvalidated -= this.Worksheet_LayoutInvalidated;
				this.Worksheet.Cells.MergedCellsChanged -= this.Cells_MergedCellsChanged;
				this.Worksheet.Cells.PropertyBag.PropertyChanged -= this.Properties_PropertyChanged;
				this.Worksheet.Rows.PropertyBag.PropertyChanged -= this.RowsProperties_PropertyChanged;
				this.Worksheet.Columns.PropertyBag.PropertyChanged -= this.ColumnsProperties_PropertyChanged;
			}
		}

		void UpdateCellContentSizesCache()
		{
			if (this.maxInvalidatedCellRange == null)
			{
				return;
			}
			for (int i = this.maxInvalidatedCellRange.FromIndex.RowIndex; i <= this.maxInvalidatedCellRange.ToIndex.RowIndex; i++)
			{
				this.rowsIsEmpty[i] = true;
			}
			for (int j = this.maxInvalidatedCellRange.FromIndex.ColumnIndex; j <= this.maxInvalidatedCellRange.ToIndex.ColumnIndex; j++)
			{
				this.columnsIsEmpty[j] = true;
			}
			Dictionary<long, Size> dictionary = this.indexToCellContentSizeCache;
			this.indexToCellContentSizeCache = new Dictionary<long, Size>();
			foreach (KeyValuePair<long, Size> keyValuePair in dictionary)
			{
				int rowIndex;
				int columnIndex;
				WorksheetPropertyBagBase.ConvertLongToRowAndColumnIndexes(keyValuePair.Key, out rowIndex, out columnIndex);
				if (!this.maxInvalidatedCellRange.Contains(rowIndex, columnIndex))
				{
					this.indexToCellContentSizeCache.Add(keyValuePair.Key, keyValuePair.Value);
				}
			}
			List<long> list = new List<long>();
			foreach (Range<long, ICellValue> range in this.Worksheet.Cells.PropertyBag.GetPropertyValueCollection<ICellValue>(CellPropertyDefinitions.ValueProperty).GetNonDefaultRanges())
			{
				for (long num = range.Start; num <= range.End; num += 1L)
				{
					int num2;
					int num3;
					WorksheetPropertyBagBase.ConvertLongToRowAndColumnIndexes(num, out num2, out num3);
					if (num3 >= this.visibleSize.Width || num2 >= this.visibleSize.Height)
					{
						break;
					}
					bool flag = false;
					bool flag2 = false;
					CellRange cellRange;
					if (this.Worksheet.Cells.TryGetContainingMergedRange(num2, num3, out cellRange))
					{
						flag = true;
						flag2 = cellRange.ColumnCount >= 1 && cellRange.RowCount == 1;
					}
					if ((this.maxInvalidatedCellRange.Contains(num2, num3) && (!flag || (flag && flag2))) || this.IsFormulaCellValue(num))
					{
						list.Add(num);
					}
					this.rowsIsEmpty[num2] = false;
					this.columnsIsEmpty[num3] = false;
				}
			}
			CellProperties[] cellsFontProperties = this.CalculateCellProperties(list);
			this.CalculateCellContentSizes(list, cellsFontProperties);
			this.maxInvalidatedCellRange = null;
		}

		bool IsFormulaCellValue(long index)
		{
			return this.Worksheet.Cells.PropertyBag.GetPropertyValue<ICellValue>(CellPropertyDefinitions.ValueProperty, index) is FormulaCellValue;
		}

		void CalculateCellContentSizes(List<long> cellsToCalculateContentSize, CellProperties[] cellsFontProperties)
		{
			if (cellsToCalculateContentSize.Count == 0)
			{
				return;
			}
			int num = cellsToCalculateContentSize.Count;
			int num2 = 1;
			if (cellsToCalculateContentSize.Count > 8)
			{
				num = (int)Math.Ceiling((double)((float)cellsToCalculateContentSize.Count / 8f));
				num2 = cellsToCalculateContentSize.Count / num + ((cellsToCalculateContentSize.Count % num > 0) ? 1 : 0);
			}
			List<Action> list = new List<Action>(cellsToCalculateContentSize.Count);
			for (int i = 0; i < num2; i++)
			{
				int fromIndex = i * num;
				int toIndex = System.Math.Min(cellsToCalculateContentSize.Count, fromIndex + num);
				list.Add(delegate
				{
					for (int j = fromIndex; j < toIndex; j++)
					{
						CellProperties cellProperties = cellsFontProperties[j];
						long num3 = cellsToCalculateContentSize[j];
						int num4 = LayoutHelper.CalculateIndent(cellProperties.HorizontalAlignment, cellProperties.Indent);
						double cellIndent = (double)num4 * SpreadsheetDefaultValues.IndentStep;
						int rowIndex;
						int columnIndex;
						WorksheetPropertyBagBase.ConvertLongToRowAndColumnIndexes(num3, out rowIndex, out columnIndex);
						double num5 = this.GetColumnWidth(columnIndex);
						CellRange cellRange;
						if (this.Worksheet.Cells.TryGetContainingMergedRange(rowIndex, columnIndex, out cellRange))
						{
							num5 = 0.0;
							for (int k = cellRange.FromIndex.ColumnIndex; k <= cellRange.ToIndex.ColumnIndex; k++)
							{
								num5 += this.GetColumnWidth(k);
							}
						}
						double? wrappingWidth = LayoutHelper.GetWrappingWidth(num5, cellIndent, cellProperties.IsWrapped);
						Size value = LayoutHelper.CalculateCellContentSize(cellProperties.Value, cellProperties.Format, cellProperties.FontProperties, cellIndent, wrappingWidth, new Func<string, FontProperties, double?, Size>(LayoutHelper.DefaultTextMeasuringMethod));
						lock (this.indexToCellContentSizeCache)
						{
							this.indexToCellContentSizeCache[num3] = value;
						}
					}
				});
			}
			TasksHelper.DoAsync(list);
		}

		void UpdateShapesSizeAndPosition()
		{
			foreach (FloatingShapeBase floatingShapeBase in this.Worksheet.Shapes)
			{
				Rect value = this.CalculateShapeBoundingRect(floatingShapeBase);
				this.shapeIdToShapeRectCache[floatingShapeBase.Id] = value;
			}
		}

		internal Size GetRowHeadingSize(CellRange visibleRange, int rowIndex)
		{
			RowLayoutBox rowLayoutBox = this.GetRowLayoutBox(rowIndex);
			double height = rowLayoutBox.Height;
			string rowName = this.Worksheet.HeaderNameRenderingConverter.ConvertRowIndexToName(new HeaderNameRenderingConverterContext(this.Worksheet, visibleRange), rowIndex);
			double rowHeadingDesiredWidth = RadWorksheetLayout.GetRowHeadingDesiredWidth(rowName);
			return new Size(rowHeadingDesiredWidth, height);
		}

		internal static double GetRowHeadingDesiredWidth(string rowName)
		{
			int length = rowName.Length;
			double val = (double)((int)(6.5 * (double)length + SpreadsheetDefaultValues.RowColumnHeadingsPadding.Left + SpreadsheetDefaultValues.RowColumnHeadingsPadding.Right));
			return Math.Max(SpreadsheetDefaultValues.RowColumnHeadingMinimumSize.Width, val);
		}

		internal Size GetColumnHeadingSize(int columnIndex)
		{
			ColumnLayoutBox columnLayoutBox = this.GetColumnLayoutBox(columnIndex);
			return new Size(columnLayoutBox.Width, SpreadsheetDefaultValues.RowColumnHeadingMinimumSize.Height);
		}

		Rect CalculateShapeBoundingRect(FloatingShapeBase shape)
		{
			return SpreadsheetHelper.CalculateShapeBoundingRect(this.GetShapeTopLeft(shape), shape);
		}

		public Point GetShapeTopLeft(FloatingShapeBase shape)
		{
			CellLayoutBox cellLayoutBox = this.GetCellLayoutBox(shape.CellIndex);
			double num = cellLayoutBox.Top;
			double num2 = cellLayoutBox.Left;
			num += shape.OffsetY;
			num2 += shape.OffsetX;
			return new Point(num2, num);
		}

		CellProperties[] CalculateCellProperties(List<long> cellIndexes)
		{
			CellProperties[] cellProperties = new CellProperties[cellIndexes.Count];
			int num = 100;
			int num2 = cellProperties.Length / num + ((cellProperties.Length % num > 0) ? 1 : 0);
			Action[] array = new Action[num2];
			for (int i = 0; i < num2; i++)
			{
				int fromIndex = i * num;
				int toIndex = System.Math.Min(cellIndexes.Count, fromIndex + num);
				Action action = delegate()
				{
					int num3 = int.MaxValue;
					int num4 = int.MaxValue;
					int num5 = 0;
					int num6 = 0;
					for (int k = fromIndex; k < toIndex; k++)
					{
						int val;
						int val2;
						WorksheetPropertyBagBase.ConvertLongToRowAndColumnIndexes(cellIndexes[k], out val, out val2);
						num3 = System.Math.Min(val, num3);
						num4 = System.Math.Min(val2, num4);
						num5 = Math.Max(val, num5);
						num6 = Math.Max(val2, num6);
					}
					CellRange cellRange = new CellRange(num3, num4, num5, num6);
					CellRangeFontProperties fontProperties = this.Worksheet.Cells.GetFontProperties(cellRange, true);
					ICompressedList<bool> propertyValueRespectingStyle = this.Worksheet.Cells.GetPropertyValueRespectingStyle<bool>(CellPropertyDefinitions.IsWrappedProperty, cellRange);
					ICompressedList<ICellValue> propertyValue = this.Worksheet.Cells.PropertyBag.GetPropertyValue<ICellValue>(CellPropertyDefinitions.ValueProperty, cellRange);
					ICompressedList<RadHorizontalAlignment> propertyValueRespectingStyle2 = this.Worksheet.Cells.GetPropertyValueRespectingStyle<RadHorizontalAlignment>(CellPropertyDefinitions.HorizontalAlignmentProperty, cellRange);
					ICompressedList<int> propertyValueRespectingStyle3 = this.Worksheet.Cells.GetPropertyValueRespectingStyle<int>(CellPropertyDefinitions.IndentProperty, cellRange);
					ICompressedList<CellValueFormat> propertyValueRespectingStyle4 = this.Worksheet.Cells.GetPropertyValueRespectingStyle<CellValueFormat>(CellPropertyDefinitions.FormatProperty, cellRange);
					for (int l = fromIndex; l < toIndex; l++)
					{
						long index = cellIndexes[l];
						int num7;
						int num8;
						WorksheetPropertyBagBase.ConvertLongToRowAndColumnIndexes(index, out num7, out num8);
						cellProperties[l].FontProperties = fontProperties.GetFontProperties(index, this.Worksheet.Workbook.Theme);
						cellProperties[l].IsWrapped = propertyValueRespectingStyle.GetValue(index);
						cellProperties[l].Value = propertyValue.GetValue(index);
						cellProperties[l].HorizontalAlignment = propertyValueRespectingStyle2.GetValue(index);
						cellProperties[l].Indent = propertyValueRespectingStyle3.GetValue(index);
						cellProperties[l].Format = propertyValueRespectingStyle4.GetValue(index);
					}
				};
				array[i] = action;
			}
			TasksHelper.DoAsync(array);
			for (int j = 0; j < cellProperties.Length; j++)
			{
				FormulaCellValue formulaCellValue = cellProperties[j].Value as FormulaCellValue;
				if (formulaCellValue != null)
				{
					cellProperties[j].Value = formulaCellValue.GetResultValueAsCellValue();
				}
			}
			return cellProperties;
		}

		public void Measure(CellIndex frozenCellIndex)
		{
			this.frozenCellIndex = frozenCellIndex;
			this.Measure();
		}

		public void Measure()
		{
			if (this.isMeasureValid)
			{
				return;
			}
			Size size = default(Size);
			if (this.Worksheet == null)
			{
				size = new Size(0.0, 0.0);
			}
			else
			{
				double width = this.CalculateFinalColumnWidths();
				this.UpdateCellContentSizesCache();
				double height = this.CalculateFinalRowHeights();
				size.Width = width;
				size.Height = height;
				this.UpdateShapesSizeAndPosition();
			}
			this.size = size;
			double width2 = ((this.frozenCellIndex == null) ? this.Width : (this.Width - this.columnsLeft[this.frozenCellIndex.ColumnIndex]));
			double height2 = ((this.frozenCellIndex == null) ? this.Height : (this.Height - this.rowsTop[this.frozenCellIndex.RowIndex]));
			this.scrollableSize = new Size(width2, height2);
			this.isMeasureValid = true;
			this.OnMeasureExecuted();
		}

		double CalculateFinalColumnWidths()
		{
			ICompressedList<ColumnWidth> columnWidthPropertyValueRespectingHidden = this.Worksheet.Columns.PropertyBag.GetColumnWidthPropertyValueRespectingHidden();
			double num = 0.0;
			foreach (Range<long, ColumnWidth> range in columnWidthPropertyValueRespectingHidden)
			{
				int num2 = (int)range.Start;
				while ((long)num2 <= range.End && num2 < SpreadsheetDefaultValues.ColumnCount)
				{
					double value = range.Value.Value;
					this.columnsLeft[num2] = num;
					num += value;
					num2++;
				}
			}
			return num;
		}

		double CalculateFinalRowHeights()
		{
			ICompressedList<RowHeight> rowHeightPropertyValueRespectingHidden = this.Worksheet.Rows.PropertyBag.GetRowHeightPropertyValueRespectingHidden();
			double num = 0.0;
			foreach (Range<long, RowHeight> range in rowHeightPropertyValueRespectingHidden)
			{
				if (range.Value.IsCustom)
				{
					double num2 = range.Value.Value;
					int num3 = (int)range.Start;
					while ((long)num3 <= range.End)
					{
						if (num3 >= SpreadsheetDefaultValues.RowCount)
						{
							break;
						}
						this.rowsTop[num3] = num;
						num += num2;
						num3++;
					}
				}
				else
				{
					int num4 = (int)range.Start;
					while ((long)num4 <= range.End && num4 < SpreadsheetDefaultValues.RowCount)
					{
						double num2 = this.CalculateAutoRowHeight(num4, rowHeightPropertyValueRespectingHidden.GetDefaultValue().Value);
						this.rowsTop[num4] = num;
						num += num2;
						num4++;
					}
				}
			}
			return num;
		}

		double CalculateAutoRowHeight(int rowIndex, double rowHeight)
		{
			if (!this.rowsIsEmpty[rowIndex])
			{
				CellRange cellRange = CellRange.RestrictCellRange(this.Worksheet.UsedCellRange, this.VisibleSize);
				for (int i = 0; i <= cellRange.ToIndex.ColumnIndex; i++)
				{
					Size size;
					if (!this.columnsIsEmpty[i] && this.indexToCellContentSizeCache.TryGetValue(WorksheetPropertyBagBase.ConvertCellIndexToLong(rowIndex, i), out size))
					{
						rowHeight = Math.Max(size.Height, rowHeight);
					}
				}
			}
			return rowHeight;
		}

		static CellLayoutBox GetCellLayoutBox(RowLayoutBox rowBox, ColumnLayoutBox columnBox, CellMergeState mergeState)
		{
			return new CellLayoutBox(rowBox.RowIndex, columnBox.ColumnIndex, new Rect(columnBox.Left, rowBox.Top, columnBox.Width, rowBox.Height), mergeState);
		}

		internal CellLayoutBox GetTopLeftMergedCellLayoutBox(CellRange mergedRange)
		{
			RowLayoutBox rowLayoutBox = this.GetRowLayoutBox(mergedRange.FromIndex.RowIndex);
			ColumnLayoutBox columnLayoutBox = this.GetColumnLayoutBox(mergedRange.FromIndex.ColumnIndex);
			RowLayoutBox rowLayoutBox2 = this.GetRowLayoutBox(mergedRange.ToIndex.RowIndex);
			ColumnLayoutBox columnLayoutBox2 = this.GetColumnLayoutBox(mergedRange.ToIndex.ColumnIndex);
			double width = columnLayoutBox2.BoundingRectangle.Right - columnLayoutBox.Left;
			double height = rowLayoutBox2.BoundingRectangle.Bottom - rowLayoutBox.Top;
			return new CellLayoutBox(rowLayoutBox.RowIndex, columnLayoutBox.ColumnIndex, new Rect(columnLayoutBox.Left, rowLayoutBox.Top, width, height), CellMergeState.TopLeftCellInMergedRange);
		}

		public RowLayoutBox GetRowLayoutBox(int rowIndex)
		{
			Guard.ThrowExceptionIfInvalidRowIndex(rowIndex);
			double y = this.rowsTop[rowIndex];
			double x = 0.0;
			double width = this.Width;
			double height;
			if (rowIndex < this.rowsTop.Length - 1)
			{
				height = this.rowsTop[rowIndex + 1] - this.rowsTop[rowIndex];
			}
			else
			{
				height = this.Height - this.rowsTop[rowIndex];
			}
			return new RowLayoutBox(rowIndex, new Rect(x, y, width, height));
		}

		public ColumnLayoutBox GetColumnLayoutBox(int columnIndex)
		{
			Guard.ThrowExceptionIfInvalidColumnIndex(columnIndex);
			double y = 0.0;
			double x = this.columnsLeft[columnIndex];
			double width;
			if (columnIndex < this.columnsLeft.Length - 1)
			{
				width = this.columnsLeft[columnIndex + 1] - this.columnsLeft[columnIndex];
			}
			else
			{
				width = this.Width - this.columnsLeft[columnIndex];
			}
			double height = this.Height;
			return new ColumnLayoutBox(columnIndex, new Rect(x, y, width, height));
		}

		public CellLayoutBox GetCellLayoutBox(CellIndex cellIndex)
		{
			return this.GetCellLayoutBox(cellIndex.RowIndex, cellIndex.ColumnIndex);
		}

		public CellLayoutBox GetCellLayoutBox(int rowIndex, int columnIndex)
		{
			RowLayoutBox rowLayoutBox = this.GetRowLayoutBox(rowIndex);
			ColumnLayoutBox columnLayoutBox = this.GetColumnLayoutBox(columnIndex);
			CellMergeState mergeState = this.Worksheet.Cells.GetMergeState(rowIndex, columnIndex);
			return RadWorksheetLayout.GetCellLayoutBox(rowLayoutBox, columnLayoutBox, mergeState);
		}

		public ShapeLayoutBox GetShapeLayoutBox(int shapeId)
		{
			Rect rect;
			if (this.shapeIdToShapeRectCache.TryGetValue(shapeId, out rect))
			{
				return new ShapeLayoutBox(shapeId, rect);
			}
			return null;
		}

		IEnumerable<RowLayoutBox> GetVisibleRowBoxes(SheetViewport sheetViewport)
		{
			ViewportPane fixedPane = sheetViewport[ViewportPaneType.Fixed];
			fixedPane = (fixedPane.IsEmpty ? sheetViewport[ViewportPaneType.HorizontalScrollable] : fixedPane);
			if (!fixedPane.IsEmpty)
			{
				foreach (RowLayoutBox item in this.GetVisibleRowBoxes(fixedPane))
				{
					yield return item;
				}
			}
			ViewportPane scrollablePane = sheetViewport[ViewportPaneType.Scrollable];
			scrollablePane = (scrollablePane.IsEmpty ? sheetViewport[ViewportPaneType.VerticalScrollable] : scrollablePane);
			if (!scrollablePane.IsEmpty)
			{
				foreach (RowLayoutBox item2 in this.GetVisibleRowBoxes(scrollablePane))
				{
					yield return item2;
				}
			}
			yield break;
		}

		IEnumerable<ColumnLayoutBox> GetVisibleColumnBoxes(SheetViewport sheetViewport)
		{
			ViewportPane fixedPane = sheetViewport[ViewportPaneType.Fixed];
			fixedPane = (fixedPane.IsEmpty ? sheetViewport[ViewportPaneType.VerticalScrollable] : fixedPane);
			if (!fixedPane.IsEmpty)
			{
				foreach (ColumnLayoutBox item in this.GetVisibleColumnBoxes(fixedPane))
				{
					yield return item;
				}
			}
			ViewportPane scrollablePane = sheetViewport[ViewportPaneType.Scrollable];
			scrollablePane = (scrollablePane.IsEmpty ? sheetViewport[ViewportPaneType.HorizontalScrollable] : scrollablePane);
			if (!scrollablePane.IsEmpty)
			{
				foreach (ColumnLayoutBox item2 in this.GetVisibleColumnBoxes(scrollablePane))
				{
					yield return item2;
				}
			}
			yield break;
		}

		public void GetVisibleBoxes(SheetViewport sheetViewport, out List<RowLayoutBox> rowLayoutBoxes, out List<ColumnLayoutBox> columnLayoutBoxes, out Dictionary<ViewportPaneType, IEnumerable<CellLayoutBox>> cellBoxes)
		{
			rowLayoutBoxes = new List<RowLayoutBox>(this.GetVisibleRowBoxes(sheetViewport));
			columnLayoutBoxes = new List<ColumnLayoutBox>(this.GetVisibleColumnBoxes(sheetViewport));
			cellBoxes = new Dictionary<ViewportPaneType, IEnumerable<CellLayoutBox>>();
			Dictionary<ViewportPaneType, HashSet<CellIndex>> dictionary = new Dictionary<ViewportPaneType, HashSet<CellIndex>>();
			foreach (object obj in Enum.GetValues(typeof(ViewportPaneType)))
			{
				ViewportPaneType key = (ViewportPaneType)obj;
				cellBoxes.Add(key, new HashSet<CellLayoutBox>());
				dictionary.Add(key, new HashSet<CellIndex>());
			}
			foreach (RowLayoutBox rowLayoutBox in rowLayoutBoxes)
			{
				foreach (ColumnLayoutBox columnLayoutBox in columnLayoutBoxes)
				{
					ViewportPane viewportPaneFromDocumentPoint = sheetViewport.GetViewportPaneFromDocumentPoint(new Point(columnLayoutBox.Left, rowLayoutBox.Top));
					ViewportPaneType viewportPaneType = viewportPaneFromDocumentPoint.ViewportPaneType;
					HashSet<CellLayoutBox> hashSet = cellBoxes[viewportPaneType] as HashSet<CellLayoutBox>;
					CellRange cellRange;
					if (this.Worksheet.Cells.TryGetContainingMergedRange(rowLayoutBox.RowIndex, columnLayoutBox.ColumnIndex, out cellRange))
					{
						CellRange cellRange2 = CellRange.RestrictCellRange(cellRange, this.visibleSize);
						if (!dictionary[viewportPaneType].Contains(cellRange2.FromIndex))
						{
							hashSet.Add(this.GetTopLeftMergedCellLayoutBox(cellRange2));
							dictionary[viewportPaneType].Add(cellRange2.FromIndex);
						}
						else if (cellRange2.FromIndex.RowIndex == rowLayoutBox.RowIndex && cellRange2.FromIndex.ColumnIndex == columnLayoutBox.ColumnIndex)
						{
							hashSet.Add(RadWorksheetLayout.GetCellLayoutBox(rowLayoutBox, columnLayoutBox, CellMergeState.NonTopLeftCellInMergedRange));
						}
					}
					else
					{
						hashSet.Add(RadWorksheetLayout.GetCellLayoutBox(rowLayoutBox, columnLayoutBox, CellMergeState.NotMerged));
					}
				}
			}
		}

		IEnumerable<RowLayoutBox> GetVisibleRowBoxes(ViewportPane viewportPane)
		{
			double bottomDelta = ((viewportPane.ViewportPaneType == ViewportPaneType.Fixed || viewportPane.ViewportPaneType == ViewportPaneType.HorizontalScrollable || this.isPrintingLayout) ? 0.1 : 0.0);
			int firstVisibleRowIndex = viewportPane.VisibleRange.FromIndex.RowIndex;
			double top = this.rowsTop[firstVisibleRowIndex];
			double left = 0.0;
			double width = this.Width;
			int i = firstVisibleRowIndex;
			if (i != this.VisibleSize.Height)
			{
				do
				{
					if (!this.worksheet.Rows.GetIsHidden(i))
					{
						double height = this.GetRowHeight(i);
						if (!height.IsZero(1E-08))
						{
							yield return new RowLayoutBox(i, new Rect(left, top, width, height));
						}
						top += height;
					}
					i++;
				}
				while (top <= viewportPane.Rect.Bottom - bottomDelta && i < this.visibleSize.Height && i <= viewportPane.VisibleRange.ToIndex.RowIndex);
			}
			yield break;
		}

		IEnumerable<ColumnLayoutBox> GetVisibleColumnBoxes(ViewportPane viewportPane)
		{
			double rightDelta = ((viewportPane.ViewportPaneType == ViewportPaneType.Fixed || viewportPane.ViewportPaneType == ViewportPaneType.VerticalScrollable || this.isPrintingLayout) ? 0.1 : 0.0);
			int firstVisibleColumnIndex = viewportPane.VisibleRange.FromIndex.ColumnIndex;
			double left = this.columnsLeft[firstVisibleColumnIndex];
			double top = 0.0;
			double height = this.Height;
			int i = firstVisibleColumnIndex;
			if (i != this.VisibleSize.Width)
			{
				do
				{
					if (!this.worksheet.Columns.GetIsHidden(i))
					{
						double width = this.GetColumnWidth(i);
						if (!width.IsZero(1E-08))
						{
							yield return new ColumnLayoutBox(i, new Rect(left, top, width, height));
						}
						left += width;
					}
					i++;
				}
				while (left <= viewportPane.Rect.Right - rightDelta && i < this.visibleSize.Width && i <= viewportPane.VisibleRange.ToIndex.ColumnIndex);
			}
			yield break;
		}

		internal void GetNearestTopLeftViewportContainingCell(int rowIndex, int columnIndex, Rect viewport, out int resultRowIndex, out int resultColumnIndex)
		{
			double columnWidth = this.GetColumnWidth(columnIndex);
			double num = viewport.Width - columnWidth;
			if (num > 0.0 && columnIndex > 0)
			{
				columnWidth = this.GetColumnWidth(columnIndex - 1);
				while (num > columnWidth)
				{
					num -= columnWidth;
					columnIndex--;
					if (columnIndex == 0)
					{
						break;
					}
					columnWidth = this.GetColumnWidth(columnIndex - 1);
				}
			}
			double rowHeight = this.GetRowHeight(rowIndex);
			double num2 = viewport.Height - rowHeight;
			if (num2 > 0.0 && rowIndex > 0)
			{
				rowHeight = this.GetRowHeight(rowIndex - 1);
				while (num2 > rowHeight)
				{
					num2 -= rowHeight;
					rowIndex--;
					if (rowIndex == 0)
					{
						break;
					}
					rowHeight = this.GetRowHeight(rowIndex - 1);
				}
			}
			resultRowIndex = rowIndex;
			resultColumnIndex = columnIndex;
		}

		internal CellLayoutBox GetNearestViewPortContainingCell(SheetViewport viewport, CellIndex cellIndex)
		{
			ViewportPane viewportPane = viewport[ViewportPaneType.Scrollable];
			int columnIndex = viewportPane.VisibleRange.FromIndex.ColumnIndex;
			int rowIndex = viewportPane.VisibleRange.FromIndex.RowIndex;
			double num = viewportPane.Rect.Left;
			double num2 = viewportPane.Rect.Top;
			CellLayoutBox cellLayoutBox = this.GetCellLayoutBox(cellIndex);
			int num3;
			int num4;
			this.GetNearestTopLeftViewportContainingCell(cellIndex.RowIndex, cellIndex.ColumnIndex, viewportPane.Rect, out num3, out num4);
			if (cellLayoutBox.BoundingRectangle.Right > viewportPane.Rect.Right)
			{
				columnIndex = num4;
				num = this.GetColumnLeft(columnIndex);
			}
			if (cellLayoutBox.Left < num)
			{
				columnIndex = cellLayoutBox.ColumnIndex;
				num = cellLayoutBox.Left;
			}
			if (cellLayoutBox.BoundingRectangle.Bottom > viewportPane.Rect.Bottom)
			{
				rowIndex = num3;
				num2 = this.GetRowTop(rowIndex);
			}
			if (cellLayoutBox.Top < num2)
			{
				rowIndex = cellLayoutBox.RowIndex;
				num2 = cellLayoutBox.Top;
			}
			return this.GetCellLayoutBox(rowIndex, columnIndex);
		}

		int FindColumnIndexAtOffset(double offset, bool isPreviousWithPriority)
		{
			int num = 0;
			int num2 = this.columnsLeft.Length - 1;
			Func<int, double> func = (int index) => this.columnsLeft[index];
			Func<int, double> func2 = (int index) => this.GetColumnRight(index);
			double num3 = func(num);
			double num4 = func2(num2);
			offset = Math.Max(num3, offset);
			offset = System.Math.Min(num4, offset);
			if (offset.EqualsDouble(num3))
			{
				return num;
			}
			if (offset.EqualsDouble(num4))
			{
				return num2;
			}
			return this.FindRangeIndexAtOffset(offset, num, num2, func, func2, isPreviousWithPriority);
		}

		int FindRowIndexAtOffset(double offset, bool isPreviousWithPriority)
		{
			int num = 0;
			int num2 = this.rowsTop.Length - 1;
			Func<int, double> func = (int index) => Math.Round(this.rowsTop[index], 3);
			Func<int, double> func2 = (int index) => Math.Round(this.GetRowBottom(index), 3);
			double num3 = func(num);
			double num4 = func2(num2);
			offset = Math.Max(num3, offset);
			offset = System.Math.Min(num4, offset);
			offset = Math.Round(offset, 3);
			if (offset.EqualsDouble(num3))
			{
				return num;
			}
			if (offset.EqualsDouble(num4))
			{
				return num2;
			}
			return this.FindRangeIndexAtOffset(offset, num, num2, func, func2, isPreviousWithPriority);
		}

		int FindRangeIndexAtOffset(double offset, int startIndex, int endIndex, Func<int, double> getRangeStart, Func<int, double> getRangeEnd, bool isPreviousWithPriority)
		{
			int val = startIndex;
			int val2 = endIndex;
			int num = endIndex - startIndex;
			int num2 = startIndex + num / 2;
			double num3 = getRangeStart(num2);
			double num4 = getRangeEnd(num2);
			while (num3 > offset || offset.GreaterOrEqualDouble(num4))
			{
				if (offset.EqualsDouble(num3))
				{
					if (!isPreviousWithPriority)
					{
						return Math.Min(val2, num2);
					}
					return Math.Max(val, num2 - 1);
				}
				else if (offset.EqualsDouble(num4))
				{
					if (!isPreviousWithPriority)
					{
						return Math.Min(val2, num2 + 1);
					}
					return Math.Max(val, num2);
				}
				else
				{
					if (num3 < offset && offset < num4)
					{
						return num2;
					}
					if (offset < num3)
					{
						endIndex = num2 - 1;
					}
					if (num4 < offset)
					{
						startIndex = num2 + 1;
					}
					num = endIndex - startIndex;
					if (num <= 0)
					{
						return startIndex;
					}
					num2 = startIndex + num / 2;
					num3 = getRangeStart(num2);
					num4 = getRangeEnd(num2);
				}
			}
			return num2;
		}

		double GetRowBottom(int index)
		{
			double result;
			if (index < this.rowsTop.Length - 1)
			{
				result = this.rowsTop[index + 1];
			}
			else
			{
				result = this.rowsTop[index] + this.GetRowHeight(index) - 0.1;
			}
			return result;
		}

		double GetColumnRight(int index)
		{
			double result;
			if (index < this.columnsLeft.Length - 1)
			{
				result = this.columnsLeft[index + 1];
			}
			else
			{
				result = this.columnsLeft[index] + this.GetColumnWidth(index) - 0.1;
			}
			return result;
		}

		public CellIndex GetCellIndexFromPoint(double x, double y, bool getLeftCellWithPriority, bool getTopCellWithPriority, bool respectHiddenCollumns = false, bool respectHiddenRows = false)
		{
			int rowIndex;
			int columnIndex;
			this.GetCellIndexFromPoint(x, y, getLeftCellWithPriority, getTopCellWithPriority, out rowIndex, out columnIndex, respectHiddenCollumns, respectHiddenRows);
			return new CellIndex(rowIndex, columnIndex);
		}

		public void GetCellIndexFromPoint(double x, double y, bool getLeftCellWithPriority, bool getTopCellWithPriority, out int rowIndex, out int columnIndex, bool respectHiddenCollumns = false, bool respectHiddenRows = false)
		{
			rowIndex = this.GetRowColumnIndexFromOffset(y, SpreadsheetDefaultValues.RowCount - 1, getTopCellWithPriority, respectHiddenCollumns, new Func<double, bool, int>(this.FindRowIndexAtOffset), new Func<int, double>(this.GetRowHeight));
			columnIndex = this.GetRowColumnIndexFromOffset(x, SpreadsheetDefaultValues.ColumnCount - 1, getLeftCellWithPriority, respectHiddenRows, new Func<double, bool, int>(this.FindColumnIndexAtOffset), new Func<int, double>(this.GetColumnWidth));
		}

		int GetRowColumnIndexFromOffset(double offset, int maxValue, bool getPreviousIndexWithPriority, bool respectHiddenIndexes, Func<double, bool, int> findIndexAtOffset, Func<int, double> getWidthHeight)
		{
			int num = findIndexAtOffset(offset, getPreviousIndexWithPriority);
			if (respectHiddenIndexes)
			{
				int num2 = num;
				if (getPreviousIndexWithPriority)
				{
					while (num2 - 1 >= 0)
					{
						if (getWidthHeight(num2) != 0.0)
						{
							break;
						}
						num2--;
					}
				}
				else
				{
					while (num2 + 1 <= maxValue && getWidthHeight(num2) == 0.0)
					{
						num2++;
					}
				}
				if (getWidthHeight(num2) == 0.0)
				{
					if (num2 == 0)
					{
						while (num2 + 1 <= maxValue && getWidthHeight(num2) == 0.0)
						{
							num2++;
						}
					}
					if (num2 == maxValue)
					{
						while (num2 - 1 >= 0 && getWidthHeight(num2) == 0.0)
						{
							num2--;
						}
					}
				}
				num = num2;
			}
			return num;
		}

		public Point GetTopLeftPointFromCellIndex(int rowIndex, int columnIndex, bool ignoreMergedState)
		{
			if (ignoreMergedState)
			{
				return new Point(this.GetColumnLeft(columnIndex), this.GetRowTop(rowIndex));
			}
			CellLayoutBox cellLayoutBox = this.GetCellLayoutBox(rowIndex, columnIndex);
			return new Point(cellLayoutBox.Left, cellLayoutBox.Top);
		}

		public Point GetTopLeftPointFromCellIndex(CellIndex cellIndex)
		{
			return this.GetTopLeftPointFromCellIndex(cellIndex.RowIndex, cellIndex.ColumnIndex);
		}

		public Point GetTopLeftPointFromCellIndex(int rowIndex, int columnIndex)
		{
			return this.GetTopLeftPointFromCellIndex(rowIndex, columnIndex, false);
		}

		public Point GetPointFromCellIndexAndOffset(CellIndex cellIndex, double offsetX, double offsetY)
		{
			return this.GetPointFromCellIndexAndOffset(cellIndex.RowIndex, cellIndex.ColumnIndex, offsetX, offsetY);
		}

		public Point GetPointFromCellIndexAndOffset(int rowIndex, int columnIndex, double offsetX, double offsetY)
		{
			Point topLeftPointFromCellIndex = this.GetTopLeftPointFromCellIndex(rowIndex, columnIndex);
			return new Point(topLeftPointFromCellIndex.X + offsetX, topLeftPointFromCellIndex.Y + offsetY);
		}

		public CellIndex GetCellIndexAndOffsetFromPoint(Point point, out double offsetX, out double offsetY)
		{
			int rowIndex;
			int columnIndex;
			this.GetCellIndexFromPoint(point.X, point.Y, false, false, out rowIndex, out columnIndex, false, false);
			Point topLeftPointFromCellIndex = this.GetTopLeftPointFromCellIndex(rowIndex, columnIndex);
			offsetX = point.X - topLeftPointFromCellIndex.X;
			offsetY = point.Y - topLeftPointFromCellIndex.Y;
			return new CellIndex(rowIndex, columnIndex);
		}

		internal bool TryGetCellContentSize(long cellIndex, out Size result)
		{
			return this.indexToCellContentSizeCache.TryGetValue(cellIndex, out result);
		}

		internal Size GetCellContentSize(long cellIndex, CellLayoutBox cellBox, bool isWrapped, FontProperties fontProperties)
		{
			this.UpdateCellContentSizesCache();
			Size result;
			if (!this.TryGetCellContentSize(cellIndex, out result))
			{
				int num;
				int num2;
				WorksheetPropertyBagBase.ConvertLongToRowAndColumnIndexes(cellIndex, out num, out num2);
				result = LayoutHelper.CalculateCellContentSize(this.worksheet, cellBox, isWrapped, new FontProperties?(fontProperties), null);
			}
			return result;
		}

		internal double GetRowTop(int rowIndex)
		{
			return this.rowsTop[rowIndex];
		}

		internal double GetColumnLeft(int columnIndex)
		{
			return this.columnsLeft[columnIndex];
		}

		internal double GetRowHeight(int rowIndex)
		{
			if (rowIndex == SpreadsheetDefaultValues.RowCount - 1)
			{
				return this.Height - this.rowsTop[rowIndex];
			}
			return this.rowsTop[rowIndex + 1] - this.rowsTop[rowIndex];
		}

		internal double GetColumnWidth(int columnIndex)
		{
			if (columnIndex == SpreadsheetDefaultValues.ColumnCount - 1)
			{
				return this.Width - this.columnsLeft[columnIndex];
			}
			return this.columnsLeft[columnIndex + 1] - this.columnsLeft[columnIndex];
		}

		internal Point GetMaxBottomRight(SizeI visibleSize)
		{
			Point result = new Point(this.Width, this.Height);
			if (visibleSize.Height < SpreadsheetDefaultValues.RowCount)
			{
				result.Y = this.GetRowTop(visibleSize.Height);
			}
			if (visibleSize.Width < SpreadsheetDefaultValues.ColumnCount)
			{
				result.X = this.GetColumnLeft(visibleSize.Width);
			}
			return result;
		}

		public event EventHandler MeasureExecuted;

		protected virtual void OnMeasureExecuted()
		{
			if (this.MeasureExecuted != null)
			{
				this.MeasureExecuted(this, EventArgs.Empty);
			}
		}

		Worksheet worksheet;

		Size size;

		Size scrollableSize;

		CellIndex frozenCellIndex;

		bool isPrintingLayout;

		bool isMeasureValid;

		readonly double[] rowsTop;

		readonly double[] columnsLeft;

		SizeI visibleSize;

		readonly bool[] rowsIsEmpty;

		readonly bool[] columnsIsEmpty;

		Dictionary<long, Size> indexToCellContentSizeCache;

		readonly Dictionary<int, Rect> shapeIdToShapeRectCache;

		CellRange maxInvalidatedCellRange;
	}
}
