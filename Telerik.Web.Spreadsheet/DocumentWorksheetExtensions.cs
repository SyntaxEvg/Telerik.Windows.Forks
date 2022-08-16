using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.Spreadsheet.Core.DataStructures;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Model.DataValidation;
using Telerik.Windows.Documents.Spreadsheet.PropertySystem;
using Telerik.Windows.Documents.Spreadsheet.Theming;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Web.Spreadsheet
{
	public static class DocumentWorksheetExtensions
	{
		public static IEnumerable<Column> ImportColumns(this Worksheet worksheet)
		{
			return DocumentWorksheetExtensions.GetCols(worksheet).Values;
		}

		public static IEnumerable<Row> ImportRows(this Worksheet worksheet)
		{
			SortedDictionary<int, Row> rows = DocumentWorksheetExtensions.GetRows(worksheet);
			foreach (KeyValuePair<int, SortedDictionary<int, Cell>> keyValuePair in DocumentWorksheetExtensions.GetCellsWithProperties(worksheet))
			{
				int key = keyValuePair.Key;
				List<Cell> cells = (from i in keyValuePair.Value
					select i.Value).ToList<Cell>();
				if (rows.ContainsKey(key))
				{
					rows[key].AddCells(cells);
				}
				else
				{
					rows.Add(key, new Row
					{
						Index = new int?(key),
						Cells = cells
					});
				}
			}
			return rows.Values;
		}

		static SortedDictionary<int, Column> GetCols(Worksheet worksheet)
		{
			SortedDictionary<int, Column> sortedDictionary = new SortedDictionary<int, Column>();
			foreach (Range<long, ColumnWidth> range in worksheet.Columns.PropertyBag.GetColumnWidthPropertyValueRespectingHidden().GetNonDefaultRanges())
			{
				double value = range.Value.Value;
				for (long num = range.Start; num <= range.End; num += 1L)
				{
					int num2 = (int)num;
					ColumnSelection columnSelection = worksheet.Columns[num2];
					bool value2 = columnSelection.GetHidden().Value;
					Column column = new Column
					{
						Index = new int?(num2),
						Width = new double?(value)
					};
					if (value2)
					{
						double value3 = columnSelection.GetWidth().Value.Value;
						column.Hidden = new double?(value3);
						column.Width = new double?(0.0);
					}
					sortedDictionary.Add(num2, column);
				}
			}
			return sortedDictionary;
		}

		static SortedDictionary<int, Row> GetRows(Worksheet worksheet)
		{
			SortedDictionary<int, Row> sortedDictionary = new SortedDictionary<int, Row>();
			foreach (Range<long, RowHeight> range in worksheet.Rows.PropertyBag.GetRowHeightPropertyValueRespectingHidden().GetNonDefaultRanges())
			{
				double value = range.Value.Value;
				for (long num = range.Start; num <= range.End; num += 1L)
				{
					int num2 = (int)num;
					RowSelection rowSelection = worksheet.Rows[num2];
					bool value2 = rowSelection.GetHidden().Value;
					Row row = new Row
					{
						Index = new int?(num2),
						Height = new double?(value)
					};
					if (value2)
					{
						double value3 = rowSelection.GetHeight().Value.Value;
						row.Hidden = new double?(value3);
						row.Height = new double?(0.0);
					}
					sortedDictionary.Add(num2, row);
				}
			}
			return sortedDictionary;
		}

		static SortedDictionary<int, SortedDictionary<int, Cell>> GetCellsWithProperties(Worksheet worksheet)
		{
			SortedDictionary<int, SortedDictionary<int, Cell>> sortedDictionary = new SortedDictionary<int, SortedDictionary<int, Cell>>();
			DocumentWorksheetExtensions.GetCellProperty<ThemableColor>(worksheet, CellPropertyDefinitions.ForeColorProperty, sortedDictionary);
			DocumentWorksheetExtensions.GetCellProperty<CellValueFormat>(worksheet, CellPropertyDefinitions.FormatProperty, sortedDictionary);
			DocumentWorksheetExtensions.GetCellProperty<ICellValue>(worksheet, CellPropertyDefinitions.ValueProperty, sortedDictionary);
			DocumentWorksheetExtensions.GetCellProperty<IFill>(worksheet, CellPropertyDefinitions.FillProperty, sortedDictionary);
			DocumentWorksheetExtensions.GetCellProperty<bool>(worksheet, CellPropertyDefinitions.IsBoldProperty, sortedDictionary);
			DocumentWorksheetExtensions.GetCellProperty<bool>(worksheet, CellPropertyDefinitions.IsItalicProperty, sortedDictionary);
			DocumentWorksheetExtensions.GetCellProperty<bool>(worksheet, CellPropertyDefinitions.IsWrappedProperty, sortedDictionary);
			DocumentWorksheetExtensions.GetCellProperty<UnderlineType>(worksheet, CellPropertyDefinitions.UnderlineProperty, sortedDictionary);
			DocumentWorksheetExtensions.GetCellProperty<RadVerticalAlignment>(worksheet, CellPropertyDefinitions.VerticalAlignmentProperty, sortedDictionary);
			DocumentWorksheetExtensions.GetCellProperty<RadHorizontalAlignment>(worksheet, CellPropertyDefinitions.HorizontalAlignmentProperty, sortedDictionary);
			DocumentWorksheetExtensions.GetCellProperty<double>(worksheet, CellPropertyDefinitions.FontSizeProperty, sortedDictionary);
			DocumentWorksheetExtensions.GetCellProperty<ThemableFontFamily>(worksheet, CellPropertyDefinitions.FontFamilyProperty, sortedDictionary);
			DocumentWorksheetExtensions.GetCellProperty<CellBorder>(worksheet, CellPropertyDefinitions.BottomBorderProperty, sortedDictionary);
			DocumentWorksheetExtensions.GetCellProperty<CellBorder>(worksheet, CellPropertyDefinitions.TopBorderProperty, sortedDictionary);
			DocumentWorksheetExtensions.GetCellProperty<CellBorder>(worksheet, CellPropertyDefinitions.LeftBorderProperty, sortedDictionary);
			DocumentWorksheetExtensions.GetCellProperty<CellBorder>(worksheet, CellPropertyDefinitions.RightBorderProperty, sortedDictionary);
			DocumentWorksheetExtensions.GetCellProperty<IDataValidationRule>(worksheet, CellPropertyDefinitions.DataValidationRuleProperty, sortedDictionary);
			DocumentWorksheetExtensions.GetCellHyperLinks(worksheet, sortedDictionary);
			return sortedDictionary;
		}

		static void GetCellProperty<T>(Worksheet worksheet, IPropertyDefinition<T> propertyDefinition, SortedDictionary<int, SortedDictionary<int, Cell>> state)
		{
			IEnumerable<Range<long, T>> nonDefaultRanges = worksheet.Cells.PropertyBag.GetPropertyValueCollection<T>(propertyDefinition).GetNonDefaultRanges();
			Action<Cell, DocumentTheme, object> setter = DocumentWorksheetExtensions.CellSetters[propertyDefinition];
			foreach (Range<long, T> range in nonDefaultRanges)
			{
				DocumentWorksheetExtensions.SetCellValue(WorksheetPropertyBagBase.ConvertLongCellRangeToCellRange(range.Start, range.End), worksheet, state, setter, range.Value);
			}
		}

		static void GetCellHyperLinks(Worksheet worksheet, SortedDictionary<int, SortedDictionary<int, Cell>> state)
		{
			foreach (SpreadsheetHyperlink spreadsheetHyperlink in worksheet.Hyperlinks)
			{
				DocumentWorksheetExtensions.SetCellValue(spreadsheetHyperlink.Range, worksheet, state, new Action<Cell, DocumentTheme, object>(DocumentWorksheetExtensions.SetHyperlink), spreadsheetHyperlink.HyperlinkInfo.Address);
			}
		}

		static void SetCellValue(CellRange cellRange, Worksheet worksheet, SortedDictionary<int, SortedDictionary<int, Cell>> state, Action<Cell, DocumentTheme, object> setter, object value)
		{
			for (int i = cellRange.FromIndex.RowIndex; i <= cellRange.ToIndex.RowIndex; i++)
			{
				for (int j = cellRange.FromIndex.ColumnIndex; j <= cellRange.ToIndex.ColumnIndex; j++)
				{
					Cell cell;
					if (state.ContainsKey(i))
					{
						SortedDictionary<int, Cell> sortedDictionary = state[i];
						if (sortedDictionary.ContainsKey(j))
						{
							cell = sortedDictionary[j];
						}
						else
						{
							cell = new Cell
							{
								Index = new int?(j)
							};
							sortedDictionary.Add(j, cell);
						}
					}
					else
					{
						cell = new Cell
						{
							Index = new int?(j)
						};
						state.Add(i, new SortedDictionary<int, Cell> { { j, cell } });
					}
					setter(cell, worksheet.Workbook.Theme, value);
				}
			}
		}

		static void SetColor(Cell cell, DocumentTheme theme, object value)
		{
			cell.Color = ((ThemableColor)value).GetActualValue(theme.ColorScheme).ToHex();
		}

		static void SetBackground(Cell cell, DocumentTheme theme, object value)
		{
			cell.Background = ((PatternFill)value).PatternColor.GetActualValue(theme.ColorScheme).ToHex();
		}

		static void SetBold(Cell cell, DocumentTheme theme, object value)
		{
			cell.Bold = new bool?((bool)value);
		}

		static void SetItalic(Cell cell, DocumentTheme theme, object value)
		{
			cell.Italic = new bool?((bool)value);
		}

		static void SetWrap(Cell cell, DocumentTheme theme, object value)
		{
			cell.Wrap = new bool?((bool)value);
		}

		static void SetUnderline(Cell cell, DocumentTheme theme, object value)
		{
			cell.Underline = new bool?((UnderlineType)value > UnderlineType.None);
		}

		static void SetVerticalAlign(Cell cell, DocumentTheme theme, object value)
		{
			cell.VerticalAlign = ((RadVerticalAlignment)value).AsString();
		}

		static void SetHorizontalAlign(Cell cell, DocumentTheme theme, object value)
		{
			cell.TextAlign = ((RadHorizontalAlignment)value).ToString().ToLower();
		}

		static void SetFontSize(Cell cell, DocumentTheme theme, object value)
		{
			cell.FontSize = new double?(UnitHelper.DipToPoint((double)value));
		}

		static void SetFontFamily(Cell cell, DocumentTheme theme, object value)
		{
			cell.FontFamily = ((ThemableFontFamily)value).GetActualValue(theme).FamilyNames.Values.First<string>();
		}

		static void SetBorderBottom(Cell cell, DocumentTheme theme, object value)
		{
			cell.BorderBottom = ((CellBorder)value).ToBorderStyle(theme);
		}

		static void SetBorderTop(Cell cell, DocumentTheme theme, object value)
		{
			cell.BorderTop = ((CellBorder)value).ToBorderStyle(theme);
		}

		static void SetBorderLeft(Cell cell, DocumentTheme theme, object value)
		{
			cell.BorderLeft = ((CellBorder)value).ToBorderStyle(theme);
		}

		static void SetBorderRight(Cell cell, DocumentTheme theme, object value)
		{
			cell.BorderRight = ((CellBorder)value).ToBorderStyle(theme);
		}

		static void SetFormat(Cell cell, DocumentTheme theme, object value)
		{
			cell.Format = ((CellValueFormat)value).FormatString;
		}

		static void SetHyperlink(Cell cell, DocumentTheme theme, object value)
		{
			cell.Link = value.ToString();
		}

		static void SetValidation(Cell cell, DocumentTheme theme, object value)
		{
			cell.Validation = ((IDataValidationRule)value).ToCellValidation();
		}

		static void SetValue(Cell cell, DocumentTheme theme, object value)
		{
			ICellValue cellValue = (ICellValue)value;
			string formula = null;
			FormulaCellValue formulaCellValue = cellValue as FormulaCellValue;
			if (formulaCellValue != null)
			{
				cellValue = formulaCellValue.GetResultValueAsCellValue();
				formula = formulaCellValue.RawValue.Substring(1);
			}
			value = cellValue.RawValue;
			if (cellValue.ValueType == CellValueType.Number)
			{
				int num;
				double num2;
				if (int.TryParse(cellValue.RawValue, out num))
				{
					value = num;
				}
				else if (double.TryParse(cellValue.RawValue, out num2))
				{
					value = num2;
				}
			}
			bool flag;
			if (cellValue.ValueType == CellValueType.Boolean && bool.TryParse(cellValue.RawValue, out flag))
			{
				value = flag;
			}
			cell.Value = value;
			cell.Formula = formula;
		}

		static Dictionary<IPropertyDefinition, Action<Cell, DocumentTheme, object>> CellSetters = new Dictionary<IPropertyDefinition, Action<Cell, DocumentTheme, object>>
		{
			{
				CellPropertyDefinitions.ForeColorProperty,
				new Action<Cell, DocumentTheme, object>(DocumentWorksheetExtensions.SetColor)
			},
			{
				CellPropertyDefinitions.FillProperty,
				new Action<Cell, DocumentTheme, object>(DocumentWorksheetExtensions.SetBackground)
			},
			{
				CellPropertyDefinitions.IsBoldProperty,
				new Action<Cell, DocumentTheme, object>(DocumentWorksheetExtensions.SetBold)
			},
			{
				CellPropertyDefinitions.IsItalicProperty,
				new Action<Cell, DocumentTheme, object>(DocumentWorksheetExtensions.SetItalic)
			},
			{
				CellPropertyDefinitions.IsWrappedProperty,
				new Action<Cell, DocumentTheme, object>(DocumentWorksheetExtensions.SetWrap)
			},
			{
				CellPropertyDefinitions.UnderlineProperty,
				new Action<Cell, DocumentTheme, object>(DocumentWorksheetExtensions.SetUnderline)
			},
			{
				CellPropertyDefinitions.VerticalAlignmentProperty,
				new Action<Cell, DocumentTheme, object>(DocumentWorksheetExtensions.SetVerticalAlign)
			},
			{
				CellPropertyDefinitions.HorizontalAlignmentProperty,
				new Action<Cell, DocumentTheme, object>(DocumentWorksheetExtensions.SetHorizontalAlign)
			},
			{
				CellPropertyDefinitions.FontSizeProperty,
				new Action<Cell, DocumentTheme, object>(DocumentWorksheetExtensions.SetFontSize)
			},
			{
				CellPropertyDefinitions.FontFamilyProperty,
				new Action<Cell, DocumentTheme, object>(DocumentWorksheetExtensions.SetFontFamily)
			},
			{
				CellPropertyDefinitions.BottomBorderProperty,
				new Action<Cell, DocumentTheme, object>(DocumentWorksheetExtensions.SetBorderBottom)
			},
			{
				CellPropertyDefinitions.TopBorderProperty,
				new Action<Cell, DocumentTheme, object>(DocumentWorksheetExtensions.SetBorderTop)
			},
			{
				CellPropertyDefinitions.LeftBorderProperty,
				new Action<Cell, DocumentTheme, object>(DocumentWorksheetExtensions.SetBorderLeft)
			},
			{
				CellPropertyDefinitions.RightBorderProperty,
				new Action<Cell, DocumentTheme, object>(DocumentWorksheetExtensions.SetBorderRight)
			},
			{
				CellPropertyDefinitions.FormatProperty,
				new Action<Cell, DocumentTheme, object>(DocumentWorksheetExtensions.SetFormat)
			},
			{
				CellPropertyDefinitions.ValueProperty,
				new Action<Cell, DocumentTheme, object>(DocumentWorksheetExtensions.SetValue)
			},
			{
				CellPropertyDefinitions.DataValidationRuleProperty,
				new Action<Cell, DocumentTheme, object>(DocumentWorksheetExtensions.SetValidation)
			}
		};
	}
}
