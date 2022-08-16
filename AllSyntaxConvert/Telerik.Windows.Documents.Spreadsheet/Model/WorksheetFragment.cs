using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands;
using Telerik.Windows.Documents.Spreadsheet.Core;
using Telerik.Windows.Documents.Spreadsheet.Core.DataStructures;
using Telerik.Windows.Documents.Spreadsheet.Layout;
using Telerik.Windows.Documents.Spreadsheet.Model.Shapes;
using Telerik.Windows.Documents.Spreadsheet.PropertySystem;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model
{
	public class WorksheetFragment : IDisposable
	{
		public CellRange CellRange
		{
			get
			{
				return this.cellRange;
			}
		}

		internal IEnumerable<CellRange> FromCellRanges
		{
			get
			{
				return this.cellRanges;
			}
		}

		internal IEnumerable<FloatingShapeBase> Shapes
		{
			get
			{
				return this.shapes;
			}
		}

		public Workbook Workbook
		{
			get
			{
				return this.workbook;
			}
		}

		public Worksheet Worksheet
		{
			get
			{
				return this.worksheet;
			}
		}

		internal bool ShouldPasteAsText
		{
			get
			{
				return this.shouldPasteAsText;
			}
		}

		internal bool IsCellCopyPaste
		{
			get
			{
				return this.isCellCopyPaste;
			}
		}

		internal WorksheetFragment(CellRange cellRange, IEnumerable<FloatingShapeBase> shapes, Worksheet fromWorksheet, bool shouldPasteAsText, bool isCellCopyPaste)
		{
			if (isCellCopyPaste)
			{
				Guard.ThrowExceptionIfNull<CellRange>(cellRange, "cellRange");
				List<CellRange> fromCellRange = CopyPasteContext.GetFromCellRange(fromWorksheet, cellRange);
				this.cellRanges = fromCellRange;
				WorksheetFragment.fromCellRangesRowCount = SpreadsheetHelper.CountRows(this.FromCellRanges);
				WorksheetFragment.fromCellRangesColumnCount = SpreadsheetHelper.CountColumns(this.FromCellRanges);
				this.cellRange = new CellRange(0, 0, WorksheetFragment.fromCellRangesRowCount - 1, WorksheetFragment.fromCellRangesColumnCount - 1);
				this.shapes = new List<FloatingShapeBase>();
			}
			else
			{
				Guard.ThrowExceptionIfNull<IEnumerable<FloatingShapeBase>>(shapes, "shapes");
				this.shapes = new List<FloatingShapeBase>(shapes);
			}
			this.workbook = WorksheetFragment.WorkbookPool.GetWorkbook();
			this.worksheet = this.workbook.Worksheets.Add();
			this.shouldPasteAsText = shouldPasteAsText;
			this.isCellCopyPaste = isCellCopyPaste;
			CopyPasteContext context = new CopyPasteContext(fromWorksheet, cellRange, this.worksheet, this.cellRange, this.shapes, null, null, isCellCopyPaste, true, this.shouldPasteAsText);
			WorksheetFragment.CopyPaste(context);
		}

		internal bool Paste(Worksheet toWorksheet, CellRange toCellRange, IEnumerable<FloatingShapeBase> toShapes, PasteOptions pasteOptions, out List<CellRange> affectedCellRanges, out IEnumerable<FloatingShapeBase> newShapes, bool isCellCopyPaste)
		{
			this.EnsureNotDisposed();
			Guard.ThrowExceptionIfNull<Worksheet>(toWorksheet, "toWorksheet");
			Guard.ThrowExceptionIfTrue(toCellRange == null && toShapes == null, "toCellRange && toShapes");
			CopyPasteContext context = new CopyPasteContext(this.worksheet, this.cellRange, toWorksheet, toCellRange, this.shapes, toShapes, pasteOptions, isCellCopyPaste, false, this.ShouldPasteAsText);
			return WorksheetFragment.CopyPaste(context, out affectedCellRanges, out newShapes);
		}

		internal static CellRange EnsureProportionalPasteCellRange(IEnumerable<CellRange> fromCellRange, CellRange toCellRange)
		{
			if (fromCellRange == null || (toCellRange.RowCount % WorksheetFragment.fromCellRangesRowCount == 0 && toCellRange.ColumnCount % WorksheetFragment.fromCellRangesColumnCount == 0))
			{
				return toCellRange;
			}
			CellIndex cellIndex = toCellRange.FromIndex.Offset(WorksheetFragment.fromCellRangesRowCount - 1, WorksheetFragment.fromCellRangesColumnCount - 1);
			if (cellIndex == null)
			{
				return null;
			}
			return new CellRange(toCellRange.FromIndex, cellIndex);
		}

		internal static bool TrySplitToPasteCellRanges(IEnumerable<CellRange> fromCellRange, CellRange toCellRange, out List<CellRange> cellRanges)
		{
			cellRanges = new List<CellRange>();
			toCellRange = WorksheetFragment.EnsureProportionalPasteCellRange(fromCellRange, toCellRange);
			if (toCellRange == null)
			{
				return false;
			}
			for (int i = toCellRange.FromIndex.RowIndex; i <= toCellRange.ToIndex.RowIndex; i += WorksheetFragment.fromCellRangesRowCount)
			{
				for (int j = toCellRange.FromIndex.ColumnIndex; j <= toCellRange.ToIndex.ColumnIndex; j += WorksheetFragment.fromCellRangesColumnCount)
				{
					int num = i;
					int num2 = j;
					int num3 = num + WorksheetFragment.fromCellRangesRowCount - 1;
					int num4 = num2 + WorksheetFragment.fromCellRangesColumnCount - 1;
					if (!TelerikHelper.IsValidRowIndex(num3) || !TelerikHelper.IsValidColumnIndex(num4))
					{
						return false;
					}
					CellRange item = new CellRange(num, num2, num3, num4);
					cellRanges.Add(item);
				}
			}
			return true;
		}

		internal static long GetPasteCellRangesCount(IEnumerable<CellRange> fromCellRange, CellRange proportionalToCellRange)
		{
			int num = SpreadsheetHelper.CountRows(fromCellRange);
			int num2 = SpreadsheetHelper.CountColumns(fromCellRange);
			long num3 = (long)(proportionalToCellRange.RowCount / num);
			long num4 = (long)(proportionalToCellRange.ColumnCount / num2);
			return num3 * num4;
		}

		static bool CopyPaste(CopyPasteContext context)
		{
			List<CellRange> list;
			IEnumerable<FloatingShapeBase> enumerable;
			return WorksheetFragment.CopyPaste(context, out list, out enumerable);
		}

		static bool CopyPaste(CopyPasteContext context, out List<CellRange> affectedCellRanges, out IEnumerable<FloatingShapeBase> newShapes)
		{
			Guard.ThrowExceptionIfNull<CopyPasteContext>(context, "context");
			newShapes = new List<FloatingShapeBase>();
			if (!context.IsCellCopyPaste)
			{
				WorksheetFragment.CopyPasteShapes(context, out newShapes);
				affectedCellRanges = new List<CellRange>
				{
					new CellRange(0, 0, 0, 0)
				};
				context.AffectedCellRanges = affectedCellRanges;
				return true;
			}
			if (!WorksheetFragment.TrySplitToPasteCellRanges(context.FromCellRange, context.ToCellRange, out affectedCellRanges))
			{
				return false;
			}
			context.AffectedCellRanges = affectedCellRanges;
			using (new UpdateScope(new Action(context.ToWorksheet.BeginUndoGroup), new Action(context.ToWorksheet.EndUndoGroup)))
			{
				WorksheetFragment.CopyPasteMergedCellRanges(context);
				WorksheetFragment.CopyPasteHyperlinks(context);
				WorksheetFragment.CopyPasteColumnProperties(context);
				WorksheetFragment.CopyPasteRowProperties(context);
				WorksheetFragment.CopyPasteCellProperties(context);
			}
			return true;
		}

		static void CopyPasteMergedCellRanges(CopyPasteContext context)
		{
			if (context.PasteOptions.SkipMergedCells)
			{
				return;
			}
			ISet<CellRange> set = new HashSet<CellRange>();
			foreach (CellRange cellRange in context.FromCellRange)
			{
				ISet<CellRange> intersectingMergedRanges = context.FromWorksheet.Cells.MergedCellRanges.GetIntersectingMergedRanges(cellRange);
				set.UnionWith(intersectingMergedRanges);
			}
			if (set.Count == 0)
			{
				return;
			}
			foreach (CellRange cellRange2 in context.AffectedCellRanges)
			{
				WorkbookCommands.UnmergeCells.Execute(new UnmergeCellsCommandContext(context.ToWorksheet, cellRange2));
				int rowOffset = cellRange2.FromIndex.RowIndex - context.FromCellRange.First<CellRange>().FromIndex.RowIndex;
				int columnOffset = cellRange2.FromIndex.ColumnIndex - context.FromCellRange.First<CellRange>().FromIndex.ColumnIndex;
				foreach (CellRange cellRange3 in set)
				{
					context.ToWorksheet.Cells[cellRange3.Offset(rowOffset, columnOffset)].Merge();
				}
			}
		}

		static void CopyPasteHyperlinks(CopyPasteContext context)
		{
			if (!context.ShouldCopyPasteHyperlinks())
			{
				return;
			}
			List<SpreadsheetHyperlink> list = new List<SpreadsheetHyperlink>();
			IEnumerable<SpreadsheetHyperlink> enumerable = new List<SpreadsheetHyperlink>();
			foreach (CellRange cellRange in context.FromCellRange)
			{
				IEnumerable<SpreadsheetHyperlink> intersectingHyperlinks = context.FromWorksheet.Hyperlinks.GetIntersectingHyperlinks(cellRange);
				enumerable = enumerable.Union(intersectingHyperlinks);
			}
			using (IEnumerator<SpreadsheetHyperlink> enumerator2 = enumerable.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					SpreadsheetHyperlink hyperlink = enumerator2.Current;
					if (context.FromCellRange.Any((CellRange x) => x.Contains(hyperlink.Range)))
					{
						list.Add(hyperlink);
					}
					else
					{
						foreach (CellRange cellRange2 in context.FromCellRange)
						{
							CellRange cellRange3 = cellRange2.Intersect(hyperlink.Range);
							if (cellRange3 != CellRange.Empty)
							{
								list.Add(new SpreadsheetHyperlink(cellRange3, hyperlink.HyperlinkInfo));
								break;
							}
						}
					}
				}
			}
			int rowOffset = context.ToCellRange.FromIndex.RowIndex - context.FromCellRange.First<CellRange>().FromIndex.RowIndex;
			int columnOffset = context.ToCellRange.FromIndex.ColumnIndex - context.FromCellRange.First<CellRange>().FromIndex.ColumnIndex;
			foreach (SpreadsheetHyperlink spreadsheetHyperlink in list)
			{
				context.ToWorksheet.Hyperlinks.Add(spreadsheetHyperlink.Range.Offset(rowOffset, columnOffset), spreadsheetHyperlink.HyperlinkInfo);
			}
		}

		static void CopyPasteShapes(CopyPasteContext context, out IEnumerable<FloatingShapeBase> newShapes)
		{
			Point topLeftOfShapeGroup = WorksheetFragment.GetTopLeftOfShapeGroup(context.FromWorksheet, context.FromShapes);
			RadWorksheetLayout worksheetLayout = context.ToWorksheet.Workbook.GetWorksheetLayout(context.ToWorksheet, false);
			RadWorksheetLayout worksheetLayout2 = context.FromWorksheet.Workbook.GetWorksheetLayout(context.FromWorksheet, false);
			Point point;
			if (context.ToCellRange != null)
			{
				CellIndex fromIndex = context.ToCellRange.FromIndex;
				point = worksheetLayout.GetTopLeftPointFromCellIndex(fromIndex);
			}
			else if (context.ToShapes != null)
			{
				Point topLeftOfShapeGroup2 = WorksheetFragment.GetTopLeftOfShapeGroup(context.ToWorksheet, context.ToShapes);
				point = topLeftOfShapeGroup2;
				point.Offset(ShapeCollection.InsertDistance, ShapeCollection.InsertDistance);
			}
			else
			{
				point = new Point(0.0, 0.0);
			}
			List<FloatingShapeBase> list = new List<FloatingShapeBase>();
			foreach (FloatingShapeBase floatingShapeBase in context.FromShapes)
			{
				Point shapeTopLeft = worksheetLayout2.GetShapeTopLeft(floatingShapeBase);
				double num = shapeTopLeft.X - topLeftOfShapeGroup.X;
				double num2 = shapeTopLeft.Y - topLeftOfShapeGroup.Y;
				Point point2 = new Point(point.X + num, point.Y + num2);
				CellIndex cellIndexFromPoint = worksheetLayout.GetCellIndexFromPoint(point2.X, point2.Y, false, false, false, false);
				Point topLeftPointFromCellIndex = worksheetLayout.GetTopLeftPointFromCellIndex(cellIndexFromPoint);
				double offsetX = point2.X - topLeftPointFromCellIndex.X;
				double offsetY = point2.Y - topLeftPointFromCellIndex.Y;
				list.Add(floatingShapeBase.Copy(context.ToWorksheet, cellIndexFromPoint, offsetX, offsetY));
			}
			context.ToWorksheet.Shapes.Add(list);
			newShapes = list;
		}

		static Point GetTopLeftOfShapeGroup(Worksheet worksheet, IEnumerable<FloatingShapeBase> shapes)
		{
			if (shapes.Count<FloatingShapeBase>() == 0)
			{
				return new Point(0.0, 0.0);
			}
			double num = double.MaxValue;
			double num2 = double.MaxValue;
			foreach (FloatingShapeBase shape in shapes)
			{
				Point shapeTopLeft = worksheet.Workbook.GetWorksheetLayout(worksheet, false).GetShapeTopLeft(shape);
				double x = shapeTopLeft.X;
				double y = shapeTopLeft.Y;
				if (x < num)
				{
					num = x;
				}
				if (y < num2)
				{
					num2 = y;
				}
			}
			return new Point(num, num2);
		}

		static void CopyPasteCellProperties(CopyPasteContext context)
		{
			if (context.PasteOptions.PasteType == PasteType.ColumnWidths)
			{
				return;
			}
			CellsPropertyBag targetPropertyBag = context.ToWorksheet.Cells.PropertyBag;
			using (new UpdateScope(new Action(targetPropertyBag.SuspendPropertyChanged), delegate()
			{
				targetPropertyBag.ResumePropertyChanged(context.ToCellRange);
			}))
			{
				List<LongRange> list = new List<LongRange>();
				foreach (CellRange cellRange in context.FromCellRange)
				{
					Range<long, ICellValue> range = null;
					foreach (LongRange longRange in WorksheetPropertyBagBase.ConvertCellRangeToLongRanges(cellRange))
					{
						ICompressedList<ICellValue> value = context.FromWorksheet.Cells.PropertyBag.GetPropertyValueCollection<ICellValue>(CellPropertyDefinitions.ValueProperty).GetValue(longRange.Start, longRange.End);
						if (!context.PasteOptions.SkipBlanks)
						{
							list.Add(longRange);
						}
						else
						{
							foreach (Range<long, ICellValue> range2 in value.GetNonDefaultRanges())
							{
								if (!TelerikHelper.EqualsOfT<ICellValue>(range2.Value, EmptyCellValue.EmptyValue))
								{
									if (range == null)
									{
										range = range2;
									}
									else if (range2.Start - 1L == range.End)
									{
										range.End = range2.End;
									}
									else
									{
										LongRange item = new LongRange(range.Start, range.End);
										list.Add(item);
										range = range2;
									}
								}
							}
							if (range != null)
							{
								LongRange item2 = new LongRange(range.Start, range.End);
								list.Add(item2);
							}
						}
					}
				}
				foreach (CellRange cellRange2 in context.AffectedCellRanges)
				{
					foreach (LongRange longRange2 in list)
					{
						int num;
						int num2;
						WorksheetPropertyBagBase.ConvertLongToRowAndColumnIndexes(longRange2.Start, out num, out num2);
						int num3;
						int num4;
						WorksheetPropertyBagBase.ConvertLongToRowAndColumnIndexes(longRange2.End, out num3, out num4);
						int num5 = cellRange2.FromIndex.RowIndex - context.FromCellRange.First<CellRange>().FromIndex.RowIndex;
						int num6 = cellRange2.FromIndex.ColumnIndex - context.FromCellRange.First<CellRange>().FromIndex.ColumnIndex;
						for (int i = 0; i < context.HiddenRows.Length; i++)
						{
							int num7 = context.HiddenRows[i];
							if (num7 < num)
							{
								num5--;
							}
						}
						for (int j = 0; j < context.HiddenColumns.Length; j++)
						{
							int num8 = context.HiddenColumns[j];
							if (num8 < num2)
							{
								num6--;
							}
						}
						num += num5;
						num3 += num5;
						num2 += num6;
						num4 += num6;
						long start = WorksheetPropertyBagBase.ConvertCellIndexToLong(num, num2);
						long end = WorksheetPropertyBagBase.ConvertCellIndexToLong(num3, num4);
						WorksheetFragment.CopyPasteCellProperties(context, longRange2, new LongRange(start, end));
					}
				}
			}
		}

		static void CopyPasteCellProperties(CopyPasteContext context, LongRange fromLongRange, LongRange toLongRange)
		{
			Guard.ThrowExceptionIfNotEqual<long>(fromLongRange.Length, toLongRange.Length, "Length");
			HashSet<IPropertyDefinition> nonDefaultProperties = WorksheetFragment.GetNonDefaultProperties(context, fromLongRange);
			switch (context.PasteOptions.PasteType)
			{
			case PasteType.Formulas:
				WorksheetFragment.CopyPasteCellValueProperty(context, nonDefaultProperties, fromLongRange, toLongRange, false);
				return;
			case PasteType.Values:
				WorksheetFragment.CopyPasteCellValueProperty(context, nonDefaultProperties, fromLongRange, toLongRange, true);
				return;
			case PasteType.Formats:
				WorksheetFragment.CopyPasteWorkbookStyles(context, fromLongRange);
				WorksheetFragment.CopyPasteIsLockedCellProperty(context, fromLongRange, toLongRange);
				WorksheetFragment.CopyPastePropertiesButIsLockedAndValues(context, nonDefaultProperties, fromLongRange, toLongRange);
				return;
			case PasteType.FormulasAndNumberFormats:
				WorksheetFragment.CopyPasteCellValueProperty(context, nonDefaultProperties, fromLongRange, toLongRange, false);
				WorksheetFragment.CopyPasteFormatCellProperty(context, nonDefaultProperties, fromLongRange, toLongRange);
				return;
			case PasteType.ValuesAndNumberFormats:
				WorksheetFragment.CopyPasteCellValueProperty(context, nonDefaultProperties, fromLongRange, toLongRange, true);
				WorksheetFragment.CopyPasteFormatCellProperty(context, nonDefaultProperties, fromLongRange, toLongRange);
				return;
			}
			WorksheetFragment.CopyPasteCellValueProperty(context, nonDefaultProperties, fromLongRange, toLongRange, false);
			WorksheetFragment.CopyPasteWorkbookStyles(context, fromLongRange);
			WorksheetFragment.CopyPasteIsLockedCellProperty(context, fromLongRange, toLongRange);
			WorksheetFragment.CopyPastePropertiesButIsLockedAndValues(context, nonDefaultProperties, fromLongRange, toLongRange);
		}

		static HashSet<IPropertyDefinition> GetNonDefaultProperties(CopyPasteContext context, LongRange fromLongRange)
		{
			HashSet<IPropertyDefinition> hashSet = new HashSet<IPropertyDefinition>();
			foreach (IProperty property in context.FromWorksheet.Cells.Properties)
			{
				CellsPropertyBag propertyBag = context.FromWorksheet.Cells.PropertyBag;
				bool flag = propertyBag.GetPropertyValueCollection(property.PropertyDefinition).ContainsNonDefaultValues(fromLongRange.Start, fromLongRange.End);
				if (flag)
				{
					hashSet.Add(property.PropertyDefinition);
				}
			}
			return hashSet;
		}

		static void CopyPastePropertiesButIsLockedAndValues(CopyPasteContext context, HashSet<IPropertyDefinition> nonDefaultProperties, LongRange fromLongRange, LongRange toLongRange)
		{
			foreach (IProperty property in context.FromWorksheet.Cells.Properties)
			{
				if (nonDefaultProperties.Contains(property.PropertyDefinition) && property.PropertyDefinition != CellPropertyDefinitions.ValueProperty && property.PropertyDefinition != CellPropertyDefinitions.IsLockedProperty)
				{
					WorksheetFragment.CopyPasteCellProperty(context, property, fromLongRange, toLongRange);
				}
			}
		}

		static void CopyPasteWorkbookStyles(CopyPasteContext context, LongRange fromLongRange)
		{
			Cells cells = context.FromWorksheet.Cells;
			IEnumerable<Range<long, string>> nonDefaultRanges = cells.PropertyBag.GetPropertyValueCollection<string>(CellPropertyDefinitions.StyleNameProperty).GetValue(fromLongRange.Start, fromLongRange.End).GetNonDefaultRanges();
			CellStyleCollection styles = context.FromWorksheet.Workbook.Styles;
			CellStyleCollection styles2 = context.ToWorksheet.Workbook.Styles;
			foreach (Range<long, string> range in nonDefaultRanges)
			{
				string value = range.Value;
				if (!styles2.Contains(value))
				{
					CellStyle cellStyle = styles[value];
					styles2.Add(cellStyle.Name, cellStyle.Category, cellStyle.IsRemovable).CopyPropertiesFrom(cellStyle);
				}
			}
		}

		static void CopyPasteCellValueProperty(CopyPasteContext context, HashSet<IPropertyDefinition> nonDefaultProperties, LongRange fromLongRange, LongRange toLongRange, bool transformValueIfFormulaCellValue)
		{
			if (!nonDefaultProperties.Contains(CellPropertyDefinitions.ValueProperty))
			{
				return;
			}
			Func<ICellValue, ICellValue> func = delegate(ICellValue cellValue)
			{
				FormulaCellValue formulaCellValue = cellValue as FormulaCellValue;
				if (formulaCellValue != null && transformValueIfFormulaCellValue)
				{
					return formulaCellValue.GetResultValueAsCellValue();
				}
				return cellValue;
			};
			ICompressedList<ICellValue> value = context.FromWorksheet.Cells.PropertyBag.GetPropertyValueCollection<ICellValue>(CellPropertyDefinitions.ValueProperty).GetValue(fromLongRange.Start, fromLongRange.End);
			CompressedList<ICellValue> compressedList = new CompressedList<ICellValue>(toLongRange.Start, toLongRange.End, value.GetDefaultValue());
			foreach (Range<long, ICellValue> range in value)
			{
				ICellValue cellValue2 = func(range.Value);
				long num = range.Start - fromLongRange.Start;
				long num2 = range.End - range.Start + 1L;
				long num3 = toLongRange.Start + num;
				long toIndex = num3 + num2 - 1L;
				bool flag = false;
				ICompressedList<ICellValue> value2 = context.ToWorksheet.Cells.PropertyBag.GetPropertyValueCollection<ICellValue>(CellPropertyDefinitions.ValueProperty).GetValue(num3, toIndex);
				foreach (Range<long, ICellValue> range2 in value2)
				{
					if (!TelerikHelper.EqualsOfT<ICellValue>(cellValue2, range2.Value))
					{
						flag = true;
						cellValue2 = WorksheetFragment.GetTransformedValue(context, cellValue2, range2.Start, range.Value, num3);
						break;
					}
				}
				if (flag || cellValue2 != value2.GetDefaultValue())
				{
					compressedList.SetValue(num3, toIndex, cellValue2);
				}
			}
			WorkbookCommands.SetCellProperty.Execute(new SetPropertyValuesCommandContext(context.ToWorksheet, CellPropertyDefinitions.ValueProperty, compressedList));
		}

		static ICellValue GetTransformedValue(CopyPasteContext context, ICellValue transformedValue, long currentToRangeStart, ICellValue currentCellValue, long toRangeStart)
		{
			if (!context.IsCopying)
			{
				FormulaCellValue formulaCellValue = transformedValue as FormulaCellValue;
				if (formulaCellValue != null)
				{
					int targetRowIndex;
					int targetColumnIndex;
					WorksheetPropertyBagBase.ConvertLongToRowAndColumnIndexes(currentToRangeStart, out targetRowIndex, out targetColumnIndex);
					return formulaCellValue.CloneAndTranslate(context.ToWorksheet, targetRowIndex, targetColumnIndex, false);
				}
			}
			if (context.ShouldPasteAsText)
			{
				int rowIndex;
				int columnIndex;
				WorksheetPropertyBagBase.ConvertLongToRowAndColumnIndexes(toRangeStart, out rowIndex, out columnIndex);
				ICellValue result;
				CellValueFormat cellValueFormat;
				CellValueFactory.CreateIgnoreErrors(currentCellValue.RawValue, context.ToWorksheet, rowIndex, columnIndex, CellValueFormat.GeneralFormat, out result, out cellValueFormat);
				return result;
			}
			return transformedValue;
		}

		static void CopyPasteCellProperty(CopyPasteContext context, IProperty cellProperty, LongRange fromLongRange, LongRange toLongRange)
		{
			CellsPropertyBag propertyBag = context.FromWorksheet.Cells.PropertyBag;
			ICompressedList value = propertyBag.GetPropertyValueCollection(cellProperty.PropertyDefinition).GetValue(fromLongRange.Start, fromLongRange.End);
			ICompressedList newValues = value.Offset(toLongRange.Start - fromLongRange.Start);
			WorkbookCommands.SetCellProperty.Execute(new SetPropertyValuesCommandContext(context.ToWorksheet, cellProperty.PropertyDefinition, newValues));
		}

		static void CopyPasteIsLockedCellProperty(CopyPasteContext context, LongRange fromLongRange, LongRange toLongRange)
		{
			CellsPropertyBag propertyBag = context.ToWorksheet.Cells.PropertyBag;
			ICompressedList newValues;
			if (context.ToWorksheet.IsProtected)
			{
				newValues = propertyBag.GetPropertyValueRespectingStyle<bool>(CellPropertyDefinitions.IsLockedProperty, context.ToWorksheet, toLongRange.Start, toLongRange.End);
			}
			else
			{
				ICompressedList value = propertyBag.GetPropertyValueCollection(context.FromWorksheet.Cells.IsLockedProperty.PropertyDefinition).GetValue(fromLongRange.Start, fromLongRange.End);
				newValues = value.Offset(toLongRange.Start - fromLongRange.Start);
			}
			WorkbookCommands.SetCellProperty.Execute(new SetPropertyValuesCommandContext(context.ToWorksheet, context.FromWorksheet.Cells.IsLockedProperty.PropertyDefinition, newValues));
		}

		static void CopyPasteFormatCellProperty(CopyPasteContext context, HashSet<IPropertyDefinition> nonDefaultProperties, LongRange fromLongRange, LongRange toLongRange)
		{
			if (!nonDefaultProperties.Contains(CellPropertyDefinitions.ValueProperty))
			{
				return;
			}
			ICompressedList<CellValueFormat> propertyValueRespectingStyle = context.FromWorksheet.Cells.PropertyBag.GetPropertyValueRespectingStyle<CellValueFormat>(CellPropertyDefinitions.FormatProperty, context.FromWorksheet, fromLongRange.Start, fromLongRange.End);
			ICompressedList newValues = propertyValueRespectingStyle.Offset(toLongRange.Start - fromLongRange.Start);
			WorkbookCommands.SetCellProperty.Execute(new SetPropertyValuesCommandContext(context.ToWorksheet, context.FromWorksheet.Cells.FormatProperty.PropertyDefinition, newValues));
		}

		static void CopyPasteColumnProperties(CopyPasteContext context)
		{
			WorksheetFragment.CopyPasteColumnWidth(context);
			Columns columns = context.FromWorksheet.Columns;
			WorksheetFragment.CopyPasteEntireRowColumnProperties(context, columns.PropertyBag, columns.Properties, WorkbookCommands.SetColumnProperty, (CellRange range) => range.RowCount == SpreadsheetDefaultValues.RowCount || context.IsCopying);
			WorksheetFragment.CopyPasteRowColumnProperties(context, columns.PropertyBag, columns.Properties, (CellRange range) => range.RowCount != SpreadsheetDefaultValues.RowCount && !context.IsCopying, new Action<CopyPasteContext, RowColumnPropertyBagBase, ICompressedList, CellRange, IProperty>(WorksheetFragment.SetColumnValuesForCells));
		}

		static void CopyPasteRowProperties(CopyPasteContext context)
		{
			Rows rows = context.FromWorksheet.Rows;
			WorksheetFragment.CopyPasteEntireRowColumnProperties(context, rows.PropertyBag, rows.Properties, WorkbookCommands.SetRowProperty, (CellRange range) => range.ColumnCount == SpreadsheetDefaultValues.ColumnCount || context.IsCopying);
			WorksheetFragment.CopyPasteRowColumnProperties(context, rows.PropertyBag, rows.Properties, (CellRange range) => range.ColumnCount != SpreadsheetDefaultValues.ColumnCount && !context.IsCopying, new Action<CopyPasteContext, RowColumnPropertyBagBase, ICompressedList, CellRange, IProperty>(WorksheetFragment.SetRowValuesForCells));
		}

		static void CopyPasteEntireRowColumnProperties(CopyPasteContext context, RowColumnPropertyBagBase propertyBag, IEnumerable<IProperty> properties, UndoableWorksheetCommandBase<SetPropertyValuesCommandContext> command, Func<CellRange, bool> shouldCopyPasteColumn)
		{
			Dictionary<IProperty, ICompressedList> dictionary = new Dictionary<IProperty, ICompressedList>();
			foreach (CellRange cellRange in context.FromCellRange)
			{
				if (shouldCopyPasteColumn(cellRange))
				{
					foreach (IProperty property in properties)
					{
						ICompressedList propertyValueCollection = propertyBag.GetPropertyValueCollection(property.PropertyDefinition);
						ICompressedList value = propertyValueCollection.GetValue((long)propertyBag.GetRowColumnIndex(cellRange.FromIndex), (long)propertyBag.GetRowColumnIndex(cellRange.ToIndex));
						if (!dictionary.ContainsKey(property))
						{
							dictionary.Add(property, value);
						}
						else
						{
							dictionary[property].SetValue(value);
						}
					}
				}
			}
			foreach (CellRange cellRange2 in context.AffectedCellRanges)
			{
				foreach (KeyValuePair<IProperty, ICompressedList> keyValuePair in dictionary)
				{
					IProperty key = keyValuePair.Key;
					ICompressedList value2 = keyValuePair.Value;
					int num = propertyBag.GetRowColumnIndex(cellRange2.FromIndex) - propertyBag.GetRowColumnIndex(context.FromCellRange[0].FromIndex);
					ICompressedList newValues = value2.Offset((long)num);
					command.Execute(new SetPropertyValuesCommandContext(context.ToWorksheet, key.PropertyDefinition, newValues));
				}
			}
		}

		static void CopyPasteRowColumnProperties(CopyPasteContext context, RowColumnPropertyBagBase propertyBag, IEnumerable<IProperty> properties, Func<CellRange, bool> shouldCopyPasteColumn, Action<CopyPasteContext, RowColumnPropertyBagBase, ICompressedList, CellRange, IProperty> setRowColumnValuesForCellsAction)
		{
			Dictionary<IProperty, ICompressedList> dictionary = new Dictionary<IProperty, ICompressedList>();
			foreach (CellRange cellRange in context.FromCellRange)
			{
				if (shouldCopyPasteColumn(cellRange))
				{
					foreach (IProperty property in properties)
					{
						ICompressedList propertyValueCollection = propertyBag.GetPropertyValueCollection(property.PropertyDefinition);
						ICompressedList value = propertyValueCollection.GetValue((long)propertyBag.GetRowColumnIndex(cellRange.FromIndex), (long)propertyBag.GetRowColumnIndex(cellRange.ToIndex));
						if (!dictionary.ContainsKey(property))
						{
							dictionary.Add(property, value);
						}
						else
						{
							dictionary[property].SetValue(value);
						}
					}
				}
			}
			foreach (CellRange arg in context.AffectedCellRanges)
			{
				foreach (KeyValuePair<IProperty, ICompressedList> keyValuePair in dictionary)
				{
					IProperty key = keyValuePair.Key;
					ICompressedList value2 = keyValuePair.Value;
					if (context.ToWorksheet.Cells.ContainsPropertyDefinition(key.PropertyDefinition))
					{
						setRowColumnValuesForCellsAction(context, propertyBag, value2, arg, key);
					}
				}
			}
		}

		static void SetColumnValuesForCells(CopyPasteContext context, RowColumnPropertyBagBase propertyBag, ICompressedList newValues, CellRange range, IProperty property)
		{
			if (!newValues.ContainsNonDefaultValues(newValues.FromIndex, newValues.ToIndex))
			{
				return;
			}
			int num = propertyBag.GetRowColumnIndex(range.FromIndex) - propertyBag.GetRowColumnIndex(context.FromCellRange[0].FromIndex);
			ICompressedList compressedList = newValues.Offset((long)num);
			for (int i = range.FromIndex.ColumnIndex; i <= range.ToIndex.ColumnIndex; i++)
			{
				if (compressedList.ContainsNonDefaultValues((long)i, (long)i))
				{
					long fromIndex = WorksheetPropertyBagBase.ConvertCellIndexToLong(range.FromIndex.RowIndex, i);
					long toIndex = WorksheetPropertyBagBase.ConvertCellIndexToLong(range.ToIndex.RowIndex, i);
					ICompressedList compressedList2 = context.ToWorksheet.Cells.PropertyBag.GetPropertyValueCollection(property.PropertyDefinition).CreateInstance(fromIndex, toIndex);
					ICompressedList value = compressedList.GetValue((long)i, (long)i);
					compressedList2.SetValue(fromIndex, toIndex, value);
					WorkbookCommands.SetCellProperty.Execute(new SetPropertyValuesCommandContext(context.ToWorksheet, property.PropertyDefinition, compressedList2));
				}
			}
		}

		static void SetRowValuesForCells(CopyPasteContext context, RowColumnPropertyBagBase propertyBag, ICompressedList newValues, CellRange range, IProperty property)
		{
			if (!newValues.ContainsNonDefaultValues(newValues.FromIndex, newValues.ToIndex))
			{
				return;
			}
			int num = propertyBag.GetRowColumnIndex(range.FromIndex) - propertyBag.GetRowColumnIndex(context.FromCellRange[0].FromIndex);
			ICompressedList compressedList = newValues.Offset((long)num);
			for (int i = range.FromIndex.RowIndex; i <= range.ToIndex.RowIndex; i++)
			{
				if (compressedList.ContainsNonDefaultValues((long)i, (long)i))
				{
					for (int j = range.FromIndex.ColumnIndex; j <= range.ToIndex.ColumnIndex; j++)
					{
						long num2 = WorksheetPropertyBagBase.ConvertCellIndexToLong(i, j);
						ICompressedList compressedList2 = context.ToWorksheet.Cells.PropertyBag.GetPropertyValueCollection(property.PropertyDefinition).CreateInstance(num2, num2);
						ICompressedList value = compressedList.GetValue((long)i, (long)i);
						compressedList2.SetValue(num2, num2, value);
						WorkbookCommands.SetCellProperty.Execute(new SetPropertyValuesCommandContext(context.ToWorksheet, property.PropertyDefinition, compressedList2));
					}
				}
			}
		}

		static void CopyPasteColumnWidth(CopyPasteContext context)
		{
			if (!context.ShouldCopyPasteColumnWidths())
			{
				return;
			}
			ICompressedList<ColumnWidth> compressedList = null;
			foreach (CellRange cellRange in context.FromCellRange)
			{
				ICompressedList<ColumnWidth> propertyValue = context.FromWorksheet.Columns.PropertyBag.GetPropertyValue<ColumnWidth>(ColumnsPropertyBag.WidthProperty, cellRange.FromIndex.ColumnIndex, cellRange.ToIndex.ColumnIndex);
				if (compressedList == null)
				{
					compressedList = propertyValue;
				}
				else
				{
					compressedList.SetValue(propertyValue);
				}
			}
			foreach (CellRange cellRange2 in context.AffectedCellRanges)
			{
				ICompressedList newValues = compressedList.Offset((long)(cellRange2.FromIndex.ColumnIndex - context.FromCellRange[0].FromIndex.ColumnIndex));
				WorkbookCommands.SetColumnProperty.Execute(new SetPropertyValuesCommandContext(context.ToWorksheet, ColumnsPropertyBag.WidthProperty, newValues));
			}
		}

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool cleanUpManagedResources)
		{
			if (this.alreadyDisposed)
			{
				return;
			}
			if (cleanUpManagedResources && this.workbook != null)
			{
				WorksheetFragment.WorkbookPool.ReturnWorkbook(this.workbook);
			}
			this.alreadyDisposed = true;
		}

		void EnsureNotDisposed()
		{
			if (this.alreadyDisposed)
			{
				throw new ObjectDisposedException("WorksheetFragment");
			}
		}

		internal static readonly int MaxAllowedAffectedRanges = 500000;

		internal static readonly int MaxAllowedAffectedRangesWithUndo = 200000;

		readonly Workbook workbook;

		readonly Worksheet worksheet;

		readonly CellRange cellRange;

		readonly IEnumerable<CellRange> cellRanges;

		readonly IEnumerable<FloatingShapeBase> shapes;

		readonly bool shouldPasteAsText;

		bool alreadyDisposed;

		readonly bool isCellCopyPaste;

		static int fromCellRangesRowCount;

		static int fromCellRangesColumnCount;

		static class WorkbookPool
		{
			public static Workbook GetWorkbook()
			{
				if (WorksheetFragment.WorkbookPool.innerList.Count > 0)
				{
					lock (WorksheetFragment.WorkbookPool.lockObject)
					{
						if (WorksheetFragment.WorkbookPool.innerList.Count > 0)
						{
							return WorksheetFragment.WorkbookPool.innerList.Dequeue();
						}
						return WorksheetFragment.WorkbookPool.CreateWorkbook();
					}
				}
				return WorksheetFragment.WorkbookPool.CreateWorkbook();
			}

			static Workbook CreateWorkbook()
			{
				return new Workbook
				{
					History = 
					{
						IsEnabled = false
					}
				};
			}

			public static void ReturnWorkbook(Workbook workbook)
			{
				if (WorksheetFragment.WorkbookPool.innerList.Count >= WorksheetFragment.WorkbookPool.maxCount)
				{
					workbook.Dispose();
					return;
				}
				workbook.Worksheets.Clear();
				WorksheetFragment.WorkbookPool.innerList.Enqueue(workbook);
			}

			static readonly int maxCount = 5;

			static readonly Queue<Workbook> innerList = new Queue<Workbook>(WorksheetFragment.WorkbookPool.maxCount);

			static readonly object lockObject = new object();
		}
	}
}
