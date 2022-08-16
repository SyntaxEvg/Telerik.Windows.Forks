using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Telerik.Windows.Documents.Spreadsheet.Core.DataStructures;
using Telerik.Windows.Documents.Spreadsheet.Maths;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Model.DataValidation;
using Telerik.Windows.Documents.Spreadsheet.Model.Shapes;
using Telerik.Windows.Documents.Spreadsheet.PropertySystem;
using Telerik.Windows.Documents.Spreadsheet.Theming;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Layout.Layers
{
	class WorksheetRenderUpdateContext : RenderUpdateContext
	{
		internal RadWorksheetLayout Layout
		{
			get
			{
				return this.layout;
			}
		}

		public Worksheet Worksheet
		{
			get
			{
				return this.layout.Worksheet;
			}
		}

		public DocumentTheme CurrentTheme
		{
			get
			{
				return this.Worksheet.Workbook.Theme;
			}
		}

		internal Dictionary<long, HyperlinkInfo> CellIndexToHyperlinkInfo
		{
			get
			{
				return this.cellIndexToHyperlinkInfo;
			}
		}

		internal Dictionary<long, Rect> CellIndexToHyperlinkArea
		{
			get
			{
				return this.cellIndexToHyperlinkArea;
			}
		}

		public IEnumerable<RowLayoutBox> VisibleRowBoxes
		{
			get
			{
				return this.visibleRowBoxes;
			}
		}

		public IEnumerable<ColumnLayoutBox> VisibleColumnBoxes
		{
			get
			{
				return this.visibleColumnBoxes;
			}
		}

		public Dictionary<ViewportPaneType, IEnumerable<CellLayoutBox>> VisibleCellLayoutBoxes
		{
			get
			{
				return this.visibleCellBoxes;
			}
		}

		public IEnumerable<FloatingShapeBase> Shapes
		{
			get
			{
				return this.shapes;
			}
		}

		internal WorksheetRenderUpdateContext(RadWorksheetLayout worksheetLayout, SheetViewport sheetViewport, Size scaleFactor, WorksheetRenderUpdateContext oldContext = null)
			: base(sheetViewport, scaleFactor)
		{
			this.layout = worksheetLayout;
			this.Layout.GetVisibleBoxes(base.SheetViewport, out this.visibleRowBoxes, out this.visibleColumnBoxes, out this.visibleCellBoxes);
			this.defaultColumnWidth = this.Worksheet.DefaultColumnWidth;
			this.defaultRowHeight = this.Worksheet.DefaultRowHeight;
			this.InitializeInvalidatedRanges(oldContext);
			this.cellIndexToValue = new Dictionary<long, ICellValue>();
			this.cellIndexToFormat = new Dictionary<long, CellValueFormat>();
			this.cellIndexToIndentPropertyValue = new Dictionary<long, int>();
			this.cellIndexToHorizontalAlignment = new Dictionary<long, RadHorizontalAlignment>();
			this.cellIndexToVerticalAlignment = new Dictionary<long, RadVerticalAlignment>();
			this.cellindexToIsWrapped = new Dictionary<long, bool>();
			this.cellIndexToFontProperties = new Dictionary<long, FontProperties>();
			this.cellIndexToContentSize = new Dictionary<long, Size>();
			this.cellIndexToActualBoundingRectangle = new Dictionary<long, Rect>();
			this.cellIndexToClipping = new Dictionary<long, Rect>();
			this.cellIndexToLeftBorder = new Dictionary<long, CellBorder>();
			this.cellIndexToTopBorder = new Dictionary<long, CellBorder>();
			this.cellIndexToRightBorder = new Dictionary<long, CellBorder>();
			this.cellIndexToBottomBorder = new Dictionary<long, CellBorder>();
			this.cellIndexToDiagonalUpBorder = new Dictionary<long, CellBorder>();
			this.cellIndexToDiagonalDownBorder = new Dictionary<long, CellBorder>();
			this.cellIndexToFill = new Dictionary<long, IFill>();
			this.cellIndexToPreviousNextNonEmptyNonMergedCells = new Dictionary<long, Tuple<CellLayoutBox, CellLayoutBox>>();
			this.shapes = new List<FloatingShapeBase>();
			this.cellIndexToHyperlinkInfo = new Dictionary<long, HyperlinkInfo>();
			this.cellIndexToHyperlinkArea = new Dictionary<long, Rect>();
			this.cellIndexToDataValidationRule = new Dictionary<long, IDataValidationRule>();
			this.cellIndexToDataValidationResult = new Dictionary<long, bool>();
			this.InitCaches(oldContext);
			this.AddCellsWithLargeContentToVisibleCellBoxes();
		}

		void InitializeInvalidatedRanges(WorksheetRenderUpdateContext oldContext)
		{
			List<CellRange> list = new List<CellRange>();
			foreach (object obj in Enum.GetValues(typeof(ViewportPaneType)))
			{
				ViewportPaneType pane = (ViewportPaneType)obj;
				ViewportPane viewportPane = base.SheetViewport[pane];
				if (!viewportPane.IsEmpty)
				{
					CellRange visibleRange = viewportPane.VisibleRange;
					if (oldContext != null)
					{
						ViewportPane viewportPane2 = oldContext.SheetViewport[pane];
						if (!viewportPane2.IsEmpty)
						{
							CellRange visibleRange2 = viewportPane2.VisibleRange;
							if (this.AreVisibleRowsOrColumnsChanged(oldContext, visibleRange2, visibleRange))
							{
								if (this.ContainsRowsColumnsWithChangedSize(oldContext, viewportPane))
								{
									list.Add(visibleRange);
								}
								else
								{
									list.AddRange(CellRange.GetDifference(visibleRange, visibleRange2));
								}
							}
							else if (visibleRange2.RowCount != visibleRange.RowCount || visibleRange2.ColumnCount != visibleRange.ColumnCount)
							{
								list.Add(visibleRange);
							}
							else
							{
								list.AddRange(CellRange.GetDifference(visibleRange, visibleRange2));
							}
						}
					}
					else
					{
						list.Add(visibleRange);
					}
				}
			}
			this.invalidatedCellRanges = list.Distinct<CellRange>().ToArray<CellRange>();
		}

		bool ContainsRowsColumnsWithChangedSize(WorksheetRenderUpdateContext oldContext, ViewportPane newSheetViewport)
		{
			CellRange visibleRange = newSheetViewport.VisibleRange;
			for (int i = visibleRange.FromIndex.RowIndex; i <= visibleRange.ToIndex.RowIndex; i++)
			{
				RowLayoutBox visibleRowLayoutBox = this.GetVisibleRowLayoutBox(i);
				RowLayoutBox visibleRowLayoutBox2 = oldContext.GetVisibleRowLayoutBox(i);
				if (visibleRowLayoutBox2 != null && !visibleRowLayoutBox.BoundingRectangle.Equals(visibleRowLayoutBox2.BoundingRectangle))
				{
					return true;
				}
			}
			for (int j = visibleRange.FromIndex.ColumnIndex; j <= visibleRange.ToIndex.ColumnIndex; j++)
			{
				ColumnLayoutBox visibleColumnLayoutBox = this.GetVisibleColumnLayoutBox(j);
				ColumnLayoutBox visibleColumnLayoutBox2 = oldContext.GetVisibleColumnLayoutBox(j);
				if (visibleColumnLayoutBox2 != null && !visibleColumnLayoutBox.BoundingRectangle.Equals(visibleColumnLayoutBox2.BoundingRectangle))
				{
					return true;
				}
			}
			return false;
		}

		bool AreVisibleRowsOrColumnsChanged(WorksheetRenderUpdateContext oldContext, CellRange oldVisibleRange, CellRange newVisibleRange)
		{
			if (!newVisibleRange.IntersectsWith(oldVisibleRange))
			{
				return false;
			}
			if (oldVisibleRange.RowCount == newVisibleRange.RowCount && oldVisibleRange.FromIndex.RowIndex != newVisibleRange.FromIndex.RowIndex)
			{
				return true;
			}
			if (oldVisibleRange.ColumnCount == newVisibleRange.ColumnCount && oldVisibleRange.FromIndex.ColumnIndex != newVisibleRange.FromIndex.ColumnIndex)
			{
				return true;
			}
			CellRange intersectionRange = newVisibleRange.Intersect(oldVisibleRange);
			bool flag = this.AreVisibleRowsChanged(oldContext, intersectionRange) || !this.defaultColumnWidth.Equals(oldContext.defaultColumnWidth);
			bool flag2 = this.AreVisibleColumnsChanged(oldContext, intersectionRange) || !this.defaultRowHeight.Equals(oldContext.defaultRowHeight);
			return flag || flag2;
		}

		bool AreVisibleRowsChanged(WorksheetRenderUpdateContext oldContext, CellRange intersectionRange)
		{
			List<int> sortedVisibleRowIndexesInRange = this.GetSortedVisibleRowIndexesInRange(intersectionRange);
			List<int> sortedVisibleRowIndexesInRange2 = oldContext.GetSortedVisibleRowIndexesInRange(intersectionRange);
			if (sortedVisibleRowIndexesInRange.Count != sortedVisibleRowIndexesInRange2.Count)
			{
				return true;
			}
			for (int i = 0; i < sortedVisibleRowIndexesInRange.Count; i++)
			{
				if (sortedVisibleRowIndexesInRange[i] != sortedVisibleRowIndexesInRange2[i])
				{
					return true;
				}
			}
			return false;
		}

		bool AreVisibleColumnsChanged(WorksheetRenderUpdateContext oldContext, CellRange intersectionRange)
		{
			List<int> sortedVisibleColumnIndexesInRange = this.GetSortedVisibleColumnIndexesInRange(intersectionRange);
			List<int> sortedVisibleColumnIndexesInRange2 = oldContext.GetSortedVisibleColumnIndexesInRange(intersectionRange);
			if (sortedVisibleColumnIndexesInRange.Count != sortedVisibleColumnIndexesInRange2.Count)
			{
				return true;
			}
			for (int i = 0; i < sortedVisibleColumnIndexesInRange.Count; i++)
			{
				if (sortedVisibleColumnIndexesInRange[i] != sortedVisibleColumnIndexesInRange2[i])
				{
					return true;
				}
			}
			return false;
		}

		List<int> GetSortedVisibleRowIndexesInRange(CellRange range)
		{
			List<int> list = new List<int>();
			foreach (RowLayoutBox rowLayoutBox in this.VisibleRowBoxes)
			{
				if (range.FromIndex.RowIndex <= rowLayoutBox.RowIndex && rowLayoutBox.RowIndex <= range.ToIndex.RowIndex)
				{
					list.Add(rowLayoutBox.RowIndex);
				}
			}
			return list;
		}

		List<int> GetSortedVisibleColumnIndexesInRange(CellRange range)
		{
			List<int> list = new List<int>();
			foreach (ColumnLayoutBox columnLayoutBox in this.VisibleColumnBoxes)
			{
				if (range.FromIndex.ColumnIndex <= columnLayoutBox.ColumnIndex && columnLayoutBox.ColumnIndex <= range.ToIndex.ColumnIndex)
				{
					list.Add(columnLayoutBox.ColumnIndex);
				}
			}
			return list;
		}

		void InitCaches(WorksheetRenderUpdateContext oldContext)
		{
			List<Action> list = new List<Action>();
			list.Add(delegate
			{
				this.InitCellValues(oldContext);
				this.InitPreviousNextNonEmptyNonMergeCells();
			});
			list.Add(delegate
			{
				this.InitCellFontProperties(oldContext);
			});
			list.Add(delegate
			{
				this.InitPropertyCache<CellValueFormat>(CellPropertyDefinitions.FormatProperty, this.cellIndexToFormat, (oldContext == null) ? null : oldContext.cellIndexToFormat);
			});
			list.Add(delegate
			{
				this.InitPropertyCache<int>(CellPropertyDefinitions.IndentProperty, this.cellIndexToIndentPropertyValue, (oldContext == null) ? null : oldContext.cellIndexToIndentPropertyValue);
			});
			list.Add(delegate
			{
				this.InitPropertyCache<bool>(CellPropertyDefinitions.IsWrappedProperty, this.cellindexToIsWrapped, (oldContext == null) ? null : oldContext.cellindexToIsWrapped);
			});
			list.Add(delegate
			{
				this.InitPropertyCache<RadHorizontalAlignment>(CellPropertyDefinitions.HorizontalAlignmentProperty, this.cellIndexToHorizontalAlignment, (oldContext == null) ? null : oldContext.cellIndexToHorizontalAlignment);
			});
			list.Add(delegate
			{
				this.InitPropertyCache<RadVerticalAlignment>(CellPropertyDefinitions.VerticalAlignmentProperty, this.cellIndexToVerticalAlignment, (oldContext == null) ? null : oldContext.cellIndexToVerticalAlignment);
			});
			list.Add(delegate
			{
				this.InitPropertyCache<IFill>(CellPropertyDefinitions.FillProperty, this.cellIndexToFill, (oldContext == null) ? null : oldContext.cellIndexToFill);
			});
			list.Add(delegate
			{
				this.InitPropertyCache<CellBorder>(CellPropertyDefinitions.LeftBorderProperty, this.cellIndexToLeftBorder, (oldContext == null) ? null : oldContext.cellIndexToLeftBorder);
			});
			list.Add(delegate
			{
				this.InitPropertyCache<CellBorder>(CellPropertyDefinitions.TopBorderProperty, this.cellIndexToTopBorder, (oldContext == null) ? null : oldContext.cellIndexToTopBorder);
			});
			list.Add(delegate
			{
				this.InitPropertyCache<CellBorder>(CellPropertyDefinitions.RightBorderProperty, this.cellIndexToRightBorder, (oldContext == null) ? null : oldContext.cellIndexToRightBorder);
			});
			list.Add(delegate
			{
				this.InitPropertyCache<CellBorder>(CellPropertyDefinitions.BottomBorderProperty, this.cellIndexToBottomBorder, (oldContext == null) ? null : oldContext.cellIndexToBottomBorder);
			});
			list.Add(delegate
			{
				this.InitPropertyCache<CellBorder>(CellPropertyDefinitions.DiagonalUpBorderProperty, this.cellIndexToDiagonalUpBorder, (oldContext == null) ? null : oldContext.cellIndexToDiagonalUpBorder);
			});
			list.Add(delegate
			{
				this.InitPropertyCache<CellBorder>(CellPropertyDefinitions.DiagonalDownBorderProperty, this.cellIndexToDiagonalDownBorder, (oldContext == null) ? null : oldContext.cellIndexToDiagonalDownBorder);
			});
			if (this.Worksheet.ViewState.CircleInvalidData)
			{
				list.Add(delegate
				{
					this.InitPropertyCache<IDataValidationRule>(CellPropertyDefinitions.DataValidationRuleProperty, this.cellIndexToDataValidationRule, (oldContext == null) ? null : oldContext.cellIndexToDataValidationRule);
				});
			}
			Parallel.Invoke(list.ToArray());
			if (this.Worksheet.ViewState.CircleInvalidData)
			{
				this.InitDataValidationRuleResults(oldContext);
			}
			this.InitCellContentSizes(oldContext);
			this.InitCellActualBoundingRectangleByHorizontalAlignment(oldContext);
			this.InitCellClippings(oldContext);
			this.InitHyperlinks();
			this.InitShapes();
		}

		bool IsContainedInInvalidatedCellRange(long index)
		{
			int rowIndex;
			int columnIndex;
			WorksheetPropertyBagBase.ConvertLongToRowAndColumnIndexes(index, out rowIndex, out columnIndex);
			for (int i = 0; i < this.invalidatedCellRanges.Length; i++)
			{
				if (this.invalidatedCellRanges[i].Contains(rowIndex, columnIndex))
				{
					return true;
				}
			}
			return false;
		}

		void InitCellValues(WorksheetRenderUpdateContext oldContext)
		{
			foreach (KeyValuePair<ViewportPaneType, IEnumerable<CellLayoutBox>> keyValuePair in this.VisibleCellLayoutBoxes)
			{
				foreach (CellLayoutBox cellLayoutBox in keyValuePair.Value)
				{
					if (cellLayoutBox.ColumnIndex == 1)
					{
						int rowIndex = cellLayoutBox.RowIndex;
					}
					long longIndex = cellLayoutBox.LongIndex;
					if (!this.cellIndexToValue.ContainsKey(longIndex))
					{
						if (!this.IsContainedInInvalidatedCellRange(longIndex) && oldContext != null && oldContext.cellIndexToValue.ContainsKey(longIndex))
						{
							this.cellIndexToValue.Add(longIndex, oldContext.cellIndexToValue[longIndex]);
						}
						else
						{
							this.cellIndexToValue.Add(longIndex, this.Worksheet.Cells.PropertyBag.GetPropertyValueRespectingStyle<ICellValue>(CellPropertyDefinitions.ValueProperty, this.Worksheet, longIndex));
						}
					}
				}
			}
		}

		static void GetPreviousNextNonEmptyNonMergedCells(CellLayoutBox cellBox, WorksheetRenderUpdateContext updateContext, out CellLayoutBox prevCellBox, out CellLayoutBox nextCellBox)
		{
			if (cellBox.MergeState == CellMergeState.NonTopLeftCellInMergedRange)
			{
				CellRange cellRange;
				updateContext.Worksheet.Cells.TryGetContainingMergedRange(cellBox.RowIndex, cellBox.ColumnIndex, out cellRange);
				cellBox = updateContext.GetVisibleCellBox(cellRange.FromIndex.RowIndex, cellRange.FromIndex.ColumnIndex);
			}
			if (cellBox.MergeState == CellMergeState.TopLeftCellInMergedRange)
			{
				prevCellBox = cellBox;
				nextCellBox = cellBox;
				return;
			}
			int num = SpreadsheetDefaultValues.ColumnCount - 1;
			int num2 = 0;
			foreach (ViewportPane viewportPane in updateContext.SheetViewport.ViewportPanes)
			{
				num = System.Math.Min(num, viewportPane.VisibleRange.FromIndex.ColumnIndex);
				num2 = Math.Max(num2, viewportPane.VisibleRange.ToIndex.ColumnIndex);
			}
			int num3 = cellBox.ColumnIndex + 1;
			while (num3 >= num && num3 <= num2)
			{
				nextCellBox = updateContext.GetVisibleCellBox(cellBox.RowIndex, num3);
				if (updateContext.GetCellValue(nextCellBox.LongIndex) != EmptyCellValue.EmptyValue || nextCellBox.MergeState != CellMergeState.NotMerged)
				{
					break;
				}
				num3++;
			}
			num3--;
			nextCellBox = updateContext.GetVisibleCellBox(cellBox.RowIndex, num3);
			int num4 = cellBox.ColumnIndex - 1;
			while (num4 >= num && num4 <= num2)
			{
				prevCellBox = updateContext.GetVisibleCellBox(cellBox.RowIndex, num4);
				if (updateContext.GetCellValue(prevCellBox.LongIndex) != EmptyCellValue.EmptyValue || prevCellBox.MergeState != CellMergeState.NotMerged)
				{
					break;
				}
				num4--;
			}
			num4++;
			prevCellBox = updateContext.GetVisibleCellBox(cellBox.RowIndex, num4);
		}

		void InitPreviousNextNonEmptyNonMergeCells()
		{
			foreach (KeyValuePair<ViewportPaneType, IEnumerable<CellLayoutBox>> keyValuePair in this.VisibleCellLayoutBoxes)
			{
				foreach (CellLayoutBox cellLayoutBox in keyValuePair.Value)
				{
					long longIndex = cellLayoutBox.LongIndex;
					if (!this.cellIndexToPreviousNextNonEmptyNonMergedCells.ContainsKey(longIndex))
					{
						CellLayoutBox item;
						CellLayoutBox item2;
						WorksheetRenderUpdateContext.GetPreviousNextNonEmptyNonMergedCells(cellLayoutBox, this, out item, out item2);
						this.cellIndexToPreviousNextNonEmptyNonMergedCells.Add(longIndex, new Tuple<CellLayoutBox, CellLayoutBox>(item, item2));
					}
				}
			}
		}

		void InitHyperlinks()
		{
			foreach (KeyValuePair<ViewportPaneType, IEnumerable<CellLayoutBox>> keyValuePair in this.VisibleCellLayoutBoxes)
			{
				foreach (CellLayoutBox cellLayoutBox in keyValuePair.Value)
				{
					long longIndex = cellLayoutBox.LongIndex;
					ICellValue cellValue = this.GetCellValue(longIndex);
					SpreadsheetHyperlink spreadsheetHyperlink;
					HyperlinkInfo hyperlinkInfo;
					if (this.Worksheet.Hyperlinks.TryGetHyperlink(cellLayoutBox.RowIndex, cellLayoutBox.ColumnIndex, out spreadsheetHyperlink))
					{
						this.CellIndexToHyperlinkInfo.Add(longIndex, spreadsheetHyperlink.HyperlinkInfo);
						hyperlinkInfo = spreadsheetHyperlink.HyperlinkInfo;
					}
					else if (SpreadsheetHelper.TryGetHyperLinkInfo(cellValue, out hyperlinkInfo))
					{
						this.CellIndexToHyperlinkInfo.Add(longIndex, hyperlinkInfo);
					}
					if (hyperlinkInfo != null)
					{
						CellValueFormat cellFormat = this.GetCellFormat(longIndex);
						string valueAsString = cellValue.GetValueAsString(cellFormat);
						Rect value;
						if (string.IsNullOrEmpty(valueAsString))
						{
							value = cellLayoutBox.BoundingRectangle;
						}
						else
						{
							value = this.GetActualBoundingRectangleByContentAlignment(cellLayoutBox);
						}
						this.CellIndexToHyperlinkArea.Add(longIndex, value);
					}
				}
			}
		}

		void InitShapes()
		{
			HashSet<FloatingShapeBase> hashSet = new HashSet<FloatingShapeBase>();
			foreach (FloatingShapeBase floatingShapeBase in this.Worksheet.Shapes)
			{
				ShapeLayoutBox shapeLayoutBox = this.Layout.GetShapeLayoutBox(floatingShapeBase.Id);
				if (base.SheetViewport.ViewportPanes.Any((ViewportPane p) => p.Rect.IntersectsWith(shapeLayoutBox.BoundingRectangle)) && !hashSet.Contains(floatingShapeBase))
				{
					hashSet.Add(floatingShapeBase);
				}
			}
			this.shapes.AddRange(hashSet);
		}

		void InitCellFontProperties(WorksheetRenderUpdateContext oldContext)
		{
			Dictionary<CellRange, CellRangeFontProperties> dictionary = new Dictionary<CellRange, CellRangeFontProperties>();
			foreach (ViewportPane viewportPane in base.SheetViewport.ViewportPanes)
			{
				CellRange visibleRange = viewportPane.VisibleRange;
				dictionary.Add(visibleRange, this.Worksheet.Cells.GetFontProperties(visibleRange, false));
			}
			foreach (KeyValuePair<ViewportPaneType, IEnumerable<CellLayoutBox>> keyValuePair in this.VisibleCellLayoutBoxes)
			{
				foreach (CellLayoutBox cellLayoutBox in keyValuePair.Value)
				{
					long longIndex = cellLayoutBox.LongIndex;
					if (base.SheetViewport.Contains(cellLayoutBox.RowIndex, cellLayoutBox.ColumnIndex) && !this.cellIndexToFontProperties.ContainsKey(longIndex))
					{
						if (!this.IsContainedInInvalidatedCellRange(longIndex) && oldContext != null && oldContext.cellIndexToFontProperties.ContainsKey(longIndex))
						{
							this.cellIndexToFontProperties.Add(longIndex, oldContext.cellIndexToFontProperties[longIndex]);
						}
						else
						{
							CellRange cellRangeContainingCellIndex = base.SheetViewport.GetCellRangeContainingCellIndex(cellLayoutBox.RowIndex, cellLayoutBox.ColumnIndex);
							CellRangeFontProperties cellRangeFontProperties = dictionary[cellRangeContainingCellIndex];
							this.cellIndexToFontProperties.Add(longIndex, cellRangeFontProperties.GetFontProperties(longIndex, this.CurrentTheme));
						}
					}
				}
			}
		}

		void InitPropertyCache<T>(IPropertyDefinition<T> property, Dictionary<long, T> cellIndexToPropertyValue, Dictionary<long, T> oldCellIndexToPropertyValue)
		{
			Dictionary<CellRange, ICompressedList<T>> dictionary = new Dictionary<CellRange, ICompressedList<T>>();
			foreach (ViewportPane viewportPane in base.SheetViewport.ViewportPanes)
			{
				CellRange visibleRange = viewportPane.VisibleRange;
				dictionary.Add(visibleRange, this.Worksheet.Cells.GetPropertyValueRespectingStyle<T>(property, visibleRange));
			}
			foreach (KeyValuePair<ViewportPaneType, IEnumerable<CellLayoutBox>> keyValuePair in this.VisibleCellLayoutBoxes)
			{
				foreach (CellLayoutBox cellLayoutBox in keyValuePair.Value)
				{
					long longIndex = cellLayoutBox.LongIndex;
					if (base.SheetViewport.Contains(cellLayoutBox.LongIndex) && !cellIndexToPropertyValue.ContainsKey(longIndex))
					{
						if (!this.IsContainedInInvalidatedCellRange(longIndex) && oldCellIndexToPropertyValue != null && oldCellIndexToPropertyValue.ContainsKey(longIndex))
						{
							cellIndexToPropertyValue.Add(longIndex, oldCellIndexToPropertyValue[longIndex]);
						}
						else
						{
							CellRange cellRangeContainingCellIndex = base.SheetViewport.GetCellRangeContainingCellIndex(cellLayoutBox.RowIndex, cellLayoutBox.ColumnIndex);
							ICompressedList<T> compressedList = dictionary[cellRangeContainingCellIndex];
							cellIndexToPropertyValue.Add(longIndex, compressedList.GetValue(longIndex));
						}
					}
				}
			}
		}

		void InitCellContentSizes(WorksheetRenderUpdateContext oldContext)
		{
			foreach (KeyValuePair<ViewportPaneType, IEnumerable<CellLayoutBox>> keyValuePair in this.VisibleCellLayoutBoxes)
			{
				foreach (CellLayoutBox cellLayoutBox in keyValuePair.Value)
				{
					long longIndex = cellLayoutBox.LongIndex;
					if (!this.cellIndexToContentSize.ContainsKey(longIndex))
					{
						if (!this.IsContainedInInvalidatedCellRange(longIndex) && oldContext != null && oldContext.cellIndexToContentSize.ContainsKey(longIndex))
						{
							this.cellIndexToContentSize.Add(longIndex, oldContext.cellIndexToContentSize[longIndex]);
						}
						else
						{
							Size cellContentSize = this.Layout.GetCellContentSize(longIndex, cellLayoutBox, this.GetIsWrapped(longIndex), this.GetFontProperties(longIndex));
							this.cellIndexToContentSize.Add(longIndex, cellContentSize);
						}
					}
				}
			}
		}

		void InitCellClippings(WorksheetRenderUpdateContext oldContext)
		{
			foreach (KeyValuePair<ViewportPaneType, IEnumerable<CellLayoutBox>> keyValuePair in this.VisibleCellLayoutBoxes)
			{
				foreach (CellLayoutBox cellLayoutBox in keyValuePair.Value)
				{
					long longIndex = cellLayoutBox.LongIndex;
					if (!this.cellIndexToClipping.ContainsKey(longIndex))
					{
						if (!this.IsContainedInInvalidatedCellRange(longIndex) && oldContext != null && oldContext.cellIndexToClipping.ContainsKey(longIndex))
						{
							this.cellIndexToClipping.Add(longIndex, oldContext.cellIndexToClipping[longIndex]);
						}
						else
						{
							Rect value = CellBoxRenderHelper.CalculateCellClipping(cellLayoutBox, this);
							this.cellIndexToClipping.Add(longIndex, value);
						}
					}
				}
			}
		}

		void InitCellActualBoundingRectangleByHorizontalAlignment(WorksheetRenderUpdateContext oldContext)
		{
			Dictionary<ViewportPaneType, HashSet<CellLayoutBox>> dictionary = new Dictionary<ViewportPaneType, HashSet<CellLayoutBox>>();
			foreach (object obj in Enum.GetValues(typeof(ViewportPaneType)))
			{
				ViewportPaneType key = (ViewportPaneType)obj;
				dictionary.Add(key, new HashSet<CellLayoutBox>());
			}
			foreach (KeyValuePair<ViewportPaneType, IEnumerable<CellLayoutBox>> keyValuePair in this.VisibleCellLayoutBoxes)
			{
				foreach (CellLayoutBox cellLayoutBox in keyValuePair.Value)
				{
					long longIndex = cellLayoutBox.LongIndex;
					if (!this.cellIndexToActualBoundingRectangle.ContainsKey(longIndex))
					{
						if (!this.IsContainedInInvalidatedCellRange(longIndex) && oldContext != null && oldContext.cellIndexToActualBoundingRectangle.ContainsKey(longIndex))
						{
							this.cellIndexToActualBoundingRectangle.Add(longIndex, oldContext.cellIndexToActualBoundingRectangle[longIndex]);
						}
						else
						{
							Rect rect = CellBoxRenderHelper.CalculateActualBoundingRectangleByContentAlignment(cellLayoutBox, this);
							this.AddCellBoxToTheOtherViewportIfNeeded(cellLayoutBox, rect, dictionary);
							this.cellIndexToActualBoundingRectangle.Add(longIndex, rect);
						}
					}
				}
			}
			foreach (KeyValuePair<ViewportPaneType, HashSet<CellLayoutBox>> keyValuePair2 in dictionary)
			{
				HashSet<CellLayoutBox> hashSet = this.visibleCellBoxes[keyValuePair2.Key] as HashSet<CellLayoutBox>;
				foreach (CellLayoutBox item in keyValuePair2.Value)
				{
					hashSet.Add(item);
				}
			}
		}

		void InitDataValidationRuleResults(WorksheetRenderUpdateContext oldContext)
		{
			foreach (KeyValuePair<ViewportPaneType, IEnumerable<CellLayoutBox>> keyValuePair in this.VisibleCellLayoutBoxes)
			{
				foreach (CellLayoutBox cellLayoutBox in keyValuePair.Value)
				{
					long longIndex = cellLayoutBox.LongIndex;
					if (!this.cellIndexToDataValidationResult.ContainsKey(longIndex))
					{
						if (!this.IsContainedInInvalidatedCellRange(longIndex) && oldContext != null && oldContext.cellIndexToDataValidationResult.ContainsKey(longIndex))
						{
							this.cellIndexToDataValidationResult.Add(longIndex, oldContext.cellIndexToDataValidationResult[longIndex]);
						}
						else
						{
							IDataValidationRule dataValidationRule = this.GetDataValidationRule(longIndex);
							int rowIndex;
							int columnIndex;
							WorksheetPropertyBagBase.ConvertLongToRowAndColumnIndexes(longIndex, out rowIndex, out columnIndex);
							this.cellIndexToDataValidationResult.Add(longIndex, dataValidationRule.Evaluate(this.Worksheet, rowIndex, columnIndex));
						}
					}
				}
			}
		}

		void AddCellBoxToTheOtherViewportIfNeeded(CellLayoutBox cellBox, Rect boundingRectangle, Dictionary<ViewportPaneType, HashSet<CellLayoutBox>> cellBoxesToAdd)
		{
			Rect rect = new Rect(boundingRectangle.X, boundingRectangle.Y, Math.Max(boundingRectangle.Width - 1.0, 0.0), Math.Max(boundingRectangle.Height - 1.0, 0.0));
			Point point = new Point(rect.Left, rect.Top);
			Point point2 = new Point(rect.Right, rect.Top);
			Point point3 = new Point(rect.Right, rect.Bottom);
			Point point4 = new Point(rect.Left, rect.Bottom);
			ViewportPaneType viewportPaneType = base.SheetViewport.GetViewportPaneFromDocumentPoint(point).ViewportPaneType;
			ViewportPaneType viewportPaneType2 = base.SheetViewport.GetViewportPaneFromDocumentPoint(point2).ViewportPaneType;
			ViewportPaneType viewportPaneType3 = base.SheetViewport.GetViewportPaneFromDocumentPoint(point3).ViewportPaneType;
			ViewportPaneType viewportPaneType4 = base.SheetViewport.GetViewportPaneFromDocumentPoint(point4).ViewportPaneType;
			if (viewportPaneType != viewportPaneType2 && base.SheetViewport[viewportPaneType2].Rect.IntersectsWith(rect) && !cellBoxesToAdd[viewportPaneType2].Contains(cellBox))
			{
				cellBoxesToAdd[viewportPaneType2].Add(cellBox);
			}
			if (viewportPaneType != viewportPaneType3 && base.SheetViewport[viewportPaneType3].Rect.IntersectsWith(rect) && !cellBoxesToAdd[viewportPaneType3].Contains(cellBox))
			{
				cellBoxesToAdd[viewportPaneType3].Add(cellBox);
			}
			if (viewportPaneType != viewportPaneType4 && base.SheetViewport[viewportPaneType4].Rect.IntersectsWith(rect) && !cellBoxesToAdd[viewportPaneType4].Contains(cellBox))
			{
				cellBoxesToAdd[viewportPaneType4].Add(cellBox);
			}
		}

		void AddCellsWithLargeContentToVisibleCellBoxes()
		{
			foreach (ViewportPane viewportPane in base.SheetViewport.ViewportPanes)
			{
				int rowIndex = viewportPane.VisibleRange.FromIndex.RowIndex;
				int num = System.Math.Min(viewportPane.VisibleRange.ToIndex.RowIndex, this.Worksheet.UsedCellRange.ToIndex.RowIndex);
				for (int i = rowIndex; i <= num; i++)
				{
					if (this.Layout.GetRowHeight(i) != WorksheetRenderUpdateContext.HiddenCellSize.Height)
					{
						int num2 = viewportPane.VisibleRange.FromIndex.ColumnIndex - 1;
						int num3 = 0;
						while (num2 >= 0 && !base.SheetViewport.ContainsColumnIndex(num2) && this.GetCellValue(WorksheetPropertyBagBase.ConvertCellIndexToLong(i, num2)) is EmptyCellValue && num3 != WorksheetRenderUpdateContext.MaxColumnsToCheckForLongText)
						{
							num3++;
							num2--;
						}
						if (num2 >= 0 && !base.SheetViewport.ContainsColumnIndex(num2) && this.Layout.GetColumnWidth(num2) != WorksheetRenderUpdateContext.HiddenCellSize.Width)
						{
							this.AddCellBoxToVisibleBoxesIfPartOfItIntersectsWithTheViewport(i, num2, viewportPane);
						}
					}
				}
			}
		}

		void AddCellBoxToVisibleBoxesIfPartOfItIntersectsWithTheViewport(int row, int column, ViewportPane pane)
		{
			CellLayoutBox visibleCellBox = this.GetVisibleCellBox(row, column);
			if (visibleCellBox.Width == WorksheetRenderUpdateContext.HiddenCellSize.Width || visibleCellBox.Height == WorksheetRenderUpdateContext.HiddenCellSize.Height)
			{
				return;
			}
			Rect rect = CellBoxRenderHelper.CalculateActualBoundingRectangleByContentAlignment(visibleCellBox, this);
			if (pane.Rect.IntersectsWith(rect))
			{
				HashSet<CellLayoutBox> hashSet = this.visibleCellBoxes[pane.ViewportPaneType] as HashSet<CellLayoutBox>;
				hashSet.Add(visibleCellBox);
			}
		}

		public RowLayoutBox GetVisibleRowLayoutBox(int rowIndex)
		{
			RowLayoutBox rowLayoutBox = null;
			if (base.SheetViewport.ContainsRowIndex(rowIndex))
			{
				foreach (RowLayoutBox rowLayoutBox2 in this.VisibleRowBoxes)
				{
					if (rowLayoutBox2.RowIndex == rowIndex)
					{
						rowLayoutBox = rowLayoutBox2;
						break;
					}
				}
				rowLayoutBox = rowLayoutBox ?? this.Layout.GetRowLayoutBox(rowIndex);
			}
			return rowLayoutBox;
		}

		public ColumnLayoutBox GetVisibleColumnLayoutBox(int columnIndex)
		{
			ColumnLayoutBox columnLayoutBox = null;
			if (base.SheetViewport.ContainsColumnIndex(columnIndex))
			{
				foreach (ColumnLayoutBox columnLayoutBox2 in this.VisibleColumnBoxes)
				{
					if (columnLayoutBox2.ColumnIndex == columnIndex)
					{
						columnLayoutBox = columnLayoutBox2;
						break;
					}
				}
				columnLayoutBox = columnLayoutBox ?? this.Layout.GetColumnLayoutBox(columnIndex);
			}
			return columnLayoutBox;
		}

		public CellLayoutBox GetVisibleCellBox(long index)
		{
			int rowIndex;
			int columnIndex;
			WorksheetPropertyBagBase.ConvertLongToRowAndColumnIndexes(index, out rowIndex, out columnIndex);
			return this.GetVisibleCellBox(rowIndex, columnIndex);
		}

		public CellLayoutBox GetVisibleCellBox(int rowIndex, int columnIndex)
		{
			if (this.cellIndexToCellLayoutBox == null)
			{
				this.cellIndexToCellLayoutBox = new Dictionary<ViewportPaneType, Dictionary<long, CellLayoutBox>>();
				foreach (KeyValuePair<ViewportPaneType, IEnumerable<CellLayoutBox>> keyValuePair in this.VisibleCellLayoutBoxes)
				{
					Dictionary<long, CellLayoutBox> dictionary = new Dictionary<long, CellLayoutBox>();
					foreach (CellLayoutBox cellLayoutBox in keyValuePair.Value)
					{
						dictionary.Add(cellLayoutBox.LongIndex, cellLayoutBox);
					}
					this.cellIndexToCellLayoutBox.Add(keyValuePair.Key, dictionary);
				}
			}
			ViewportPaneType viewportPaneType = base.SheetViewport.GetViewportPaneContainingCellIndex(rowIndex, columnIndex).ViewportPaneType;
			long key = WorksheetPropertyBagBase.ConvertCellIndexToLong(rowIndex, columnIndex);
			CellLayoutBox cellLayoutBox2 = null;
			if (!this.cellIndexToCellLayoutBox[viewportPaneType].TryGetValue(key, out cellLayoutBox2))
			{
				cellLayoutBox2 = this.Layout.GetCellLayoutBox(rowIndex, columnIndex);
				this.cellIndexToCellLayoutBox[viewportPaneType].Add(key, cellLayoutBox2);
			}
			return cellLayoutBox2;
		}

		public Rect GetActualBoundingRectangleByContentAlignment(CellLayoutBox cellBox)
		{
			long longIndex = cellBox.LongIndex;
			Rect rect;
			if (!this.cellIndexToActualBoundingRectangle.TryGetValue(longIndex, out rect))
			{
				rect = CellBoxRenderHelper.CalculateActualBoundingRectangleByContentAlignment(cellBox, this);
				this.cellIndexToActualBoundingRectangle.Add(longIndex, rect);
			}
			return rect;
		}

		public HyperlinkInfo GetHyperlinkInfo(long index)
		{
			HyperlinkInfo result;
			if (!this.CellIndexToHyperlinkInfo.TryGetValue(index, out result))
			{
				return null;
			}
			return result;
		}

		public Rect GetHyperlinkArea(long index)
		{
			Rect result;
			if (!this.CellIndexToHyperlinkArea.TryGetValue(index, out result))
			{
				return Rect.Empty;
			}
			return result;
		}

		public void AddHyperlinkArea(long index, Rect rect)
		{
			this.CellIndexToHyperlinkArea[index] = rect;
		}

		public Rect GetCellClipping(CellLayoutBox cellBox)
		{
			long longIndex = cellBox.LongIndex;
			Rect rect;
			if (!this.cellIndexToClipping.TryGetValue(longIndex, out rect))
			{
				rect = CellBoxRenderHelper.CalculateCellClipping(cellBox, this);
				this.cellIndexToClipping.Add(longIndex, rect);
			}
			return rect;
		}

		public Size GetCellContentSize(CellLayoutBox cellBox)
		{
			long longIndex = cellBox.LongIndex;
			Size cellContentSize;
			if (!this.cellIndexToContentSize.TryGetValue(longIndex, out cellContentSize))
			{
				cellContentSize = this.Layout.GetCellContentSize(longIndex, cellBox, this.GetIsWrapped(longIndex), this.GetFontProperties(longIndex));
			}
			return cellContentSize;
		}

		public int CalculateIndent(long index)
		{
			return LayoutHelper.CalculateIndent(this.GetHorizontalAlignment(index), this.GetIndentPropertyValue(index));
		}

		T GetPropertyValue<T>(IPropertyDefinition<T> propertyDefinition, Dictionary<long, T> cache, long index)
		{
			T propertyValueRespectingStyle;
			if (!cache.TryGetValue(index, out propertyValueRespectingStyle))
			{
				propertyValueRespectingStyle = this.Worksheet.Cells.PropertyBag.GetPropertyValueRespectingStyle<T>(propertyDefinition, this.Worksheet, index);
				cache.Add(index, propertyValueRespectingStyle);
			}
			return propertyValueRespectingStyle;
		}

		public RadHorizontalAlignment GetHorizontalAlignment(long index)
		{
			return this.GetPropertyValue<RadHorizontalAlignment>(CellPropertyDefinitions.HorizontalAlignmentProperty, this.cellIndexToHorizontalAlignment, index);
		}

		public RadVerticalAlignment GetVerticalAlignment(long index)
		{
			return this.GetPropertyValue<RadVerticalAlignment>(CellPropertyDefinitions.VerticalAlignmentProperty, this.cellIndexToVerticalAlignment, index);
		}

		public IFill GetFill(long index)
		{
			return this.GetPropertyValue<IFill>(CellPropertyDefinitions.FillProperty, this.cellIndexToFill, index);
		}

		public CellBorder GetLeftBorder(long index)
		{
			return this.GetPropertyValue<CellBorder>(CellPropertyDefinitions.LeftBorderProperty, this.cellIndexToLeftBorder, index);
		}

		public CellBorder GetTopBorder(long index)
		{
			return this.GetPropertyValue<CellBorder>(CellPropertyDefinitions.TopBorderProperty, this.cellIndexToTopBorder, index);
		}

		public CellBorder GetRightBorder(long index)
		{
			return this.GetPropertyValue<CellBorder>(CellPropertyDefinitions.RightBorderProperty, this.cellIndexToRightBorder, index);
		}

		public CellBorder GetBottomBorder(long index)
		{
			return this.GetPropertyValue<CellBorder>(CellPropertyDefinitions.BottomBorderProperty, this.cellIndexToBottomBorder, index);
		}

		public CellBorder GetDiagonalUpBorder(long index)
		{
			return this.GetPropertyValue<CellBorder>(CellPropertyDefinitions.DiagonalUpBorderProperty, this.cellIndexToDiagonalUpBorder, index);
		}

		public CellBorder GetDiagonalDownBorder(long index)
		{
			return this.GetPropertyValue<CellBorder>(CellPropertyDefinitions.DiagonalDownBorderProperty, this.cellIndexToDiagonalDownBorder, index);
		}

		public ICellValue GetCellValue(long index)
		{
			return this.GetPropertyValue<ICellValue>(CellPropertyDefinitions.ValueProperty, this.cellIndexToValue, index);
		}

		public CellValueFormat GetCellFormat(long index)
		{
			return this.GetPropertyValue<CellValueFormat>(CellPropertyDefinitions.FormatProperty, this.cellIndexToFormat, index);
		}

		public FontProperties GetFontProperties(long index)
		{
			FontProperties fontProperties;
			if (!this.cellIndexToFontProperties.TryGetValue(index, out fontProperties))
			{
				fontProperties = this.Worksheet.Cells.GetFontProperties(index, false);
				this.cellIndexToFontProperties.Add(index, fontProperties);
			}
			return fontProperties;
		}

		public bool GetIsWrapped(long index)
		{
			return this.GetPropertyValue<bool>(CellPropertyDefinitions.IsWrappedProperty, this.cellindexToIsWrapped, index);
		}

		public int GetIndentPropertyValue(long index)
		{
			return this.GetPropertyValue<int>(CellPropertyDefinitions.IndentProperty, this.cellIndexToIndentPropertyValue, index);
		}

		public void GetPreviousNextNonEmptyNonMergedCells(long index, out CellLayoutBox previousCellBox, out CellLayoutBox nextCellBox)
		{
			CellLayoutBox visibleCellBox = this.GetVisibleCellBox(index);
			if (visibleCellBox.MergeState == CellMergeState.NonTopLeftCellInMergedRange)
			{
				CellRange cellRange;
				this.Worksheet.Cells.TryGetContainingMergedRange(visibleCellBox.RowIndex, visibleCellBox.ColumnIndex, out cellRange);
				visibleCellBox = this.GetVisibleCellBox(cellRange.FromIndex.RowIndex, cellRange.FromIndex.ColumnIndex);
			}
			Tuple<CellLayoutBox, CellLayoutBox> tuple = null;
			if (!this.cellIndexToPreviousNextNonEmptyNonMergedCells.TryGetValue(index, out tuple))
			{
				WorksheetRenderUpdateContext.GetPreviousNextNonEmptyNonMergedCells(this.GetVisibleCellBox(index), this, out previousCellBox, out nextCellBox);
				tuple = new Tuple<CellLayoutBox, CellLayoutBox>(previousCellBox, nextCellBox);
				this.cellIndexToPreviousNextNonEmptyNonMergedCells.Add(index, tuple);
			}
			previousCellBox = tuple.Item1;
			nextCellBox = tuple.Item2;
		}

		public FloatingShapeBase GetShapeFromPoint(Point point)
		{
			foreach (FloatingShapeBase floatingShapeBase in this.Worksheet.Shapes.ReverseEnumerate())
			{
				Point shapeTopLeft = this.Layout.GetShapeTopLeft(floatingShapeBase);
				double x = shapeTopLeft.X;
				double y = shapeTopLeft.Y;
				double num = (floatingShapeBase.DoesRotationAngleRequireCellIndexChange() ? floatingShapeBase.Height : floatingShapeBase.Width);
				double num2 = (floatingShapeBase.DoesRotationAngleRequireCellIndexChange() ? floatingShapeBase.Width : floatingShapeBase.Height);
				double angleInDegrees = (floatingShapeBase.DoesRotationAngleRequireCellIndexChange() ? (-(floatingShapeBase.RotationAngle - 90.0)) : (-floatingShapeBase.RotationAngle));
				Matrix identity = Matrix.Identity;
				Point point2 = identity.RotateMatrixAt(angleInDegrees, x + num / 2.0, y + num2 / 2.0).Transform(point);
				Rect rect = new Rect(x, y, num, num2);
				if (rect.Contains(point2))
				{
					return floatingShapeBase;
				}
			}
			return null;
		}

		public IDataValidationRule GetDataValidationRule(long index)
		{
			return this.GetPropertyValue<IDataValidationRule>(CellPropertyDefinitions.DataValidationRuleProperty, this.cellIndexToDataValidationRule, index);
		}

		public bool GetDataValidationRuleResult(long index)
		{
			bool flag;
			if (!this.cellIndexToDataValidationResult.TryGetValue(index, out flag))
			{
				int rowIndex;
				int columnIndex;
				WorksheetPropertyBagBase.ConvertLongToRowAndColumnIndexes(index, out rowIndex, out columnIndex);
				flag = this.GetDataValidationRule(index).Evaluate(this.Worksheet, rowIndex, columnIndex);
				this.cellIndexToDataValidationResult.Add(index, flag);
			}
			return flag;
		}

		static readonly int MaxColumnsToCheckForLongText = 30;

		static readonly Size HiddenCellSize = new Size(0.0, 0.0);

		readonly RadWorksheetLayout layout;

		readonly List<RowLayoutBox> visibleRowBoxes;

		readonly List<ColumnLayoutBox> visibleColumnBoxes;

		readonly Dictionary<ViewportPaneType, IEnumerable<CellLayoutBox>> visibleCellBoxes;

		Dictionary<ViewportPaneType, Dictionary<long, CellLayoutBox>> cellIndexToCellLayoutBox;

		readonly Dictionary<long, ICellValue> cellIndexToValue;

		readonly Dictionary<long, CellValueFormat> cellIndexToFormat;

		readonly Dictionary<long, bool> cellindexToIsWrapped;

		readonly Dictionary<long, FontProperties> cellIndexToFontProperties;

		readonly Dictionary<long, IFill> cellIndexToFill;

		readonly Dictionary<long, Rect> cellIndexToActualBoundingRectangle;

		readonly Dictionary<long, Rect> cellIndexToClipping;

		readonly Dictionary<long, int> cellIndexToIndentPropertyValue;

		readonly Dictionary<long, RadHorizontalAlignment> cellIndexToHorizontalAlignment;

		readonly Dictionary<long, RadVerticalAlignment> cellIndexToVerticalAlignment;

		readonly Dictionary<long, Size> cellIndexToContentSize;

		readonly Dictionary<long, CellBorder> cellIndexToLeftBorder;

		readonly Dictionary<long, CellBorder> cellIndexToTopBorder;

		readonly Dictionary<long, CellBorder> cellIndexToRightBorder;

		readonly Dictionary<long, CellBorder> cellIndexToBottomBorder;

		readonly Dictionary<long, CellBorder> cellIndexToDiagonalUpBorder;

		readonly Dictionary<long, CellBorder> cellIndexToDiagonalDownBorder;

		readonly Dictionary<long, Tuple<CellLayoutBox, CellLayoutBox>> cellIndexToPreviousNextNonEmptyNonMergedCells;

		readonly Dictionary<long, HyperlinkInfo> cellIndexToHyperlinkInfo;

		readonly Dictionary<long, Rect> cellIndexToHyperlinkArea;

		readonly Dictionary<long, IDataValidationRule> cellIndexToDataValidationRule;

		readonly Dictionary<long, bool> cellIndexToDataValidationResult;

		readonly List<FloatingShapeBase> shapes;

		readonly RowHeight defaultRowHeight;

		readonly ColumnWidth defaultColumnWidth;

		CellRange[] invalidatedCellRanges;
	}
}
