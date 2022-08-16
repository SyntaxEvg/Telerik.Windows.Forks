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
		public static global::System.Collections.Generic.IEnumerable<global::Telerik.Web.Spreadsheet.Column> ImportColumns(this global::Telerik.Windows.Documents.Spreadsheet.Model.Worksheet worksheet)
		{
			return global::Telerik.Web.Spreadsheet.DocumentWorksheetExtensions.GetCols(worksheet).Values;
		}

		public static global::System.Collections.Generic.IEnumerable<global::Telerik.Web.Spreadsheet.Row> ImportRows(this global::Telerik.Windows.Documents.Spreadsheet.Model.Worksheet worksheet)
		{
			global::System.Collections.Generic.SortedDictionary<int, global::Telerik.Web.Spreadsheet.Row> rows = global::Telerik.Web.Spreadsheet.DocumentWorksheetExtensions.GetRows(worksheet);
			foreach (global::System.Collections.Generic.KeyValuePair<int, global::System.Collections.Generic.SortedDictionary<int, global::Telerik.Web.Spreadsheet.Cell>> keyValuePair in global::Telerik.Web.Spreadsheet.DocumentWorksheetExtensions.GetCellsWithProperties(worksheet))
			{
				int key = keyValuePair.Key;
				global::System.Collections.Generic.List<global::Telerik.Web.Spreadsheet.Cell> cells = keyValuePair.Value.Select((global::System.Collections.Generic.KeyValuePair<int, global::Telerik.Web.Spreadsheet.Cell> i) => i.Value).ToList<global::Telerik.Web.Spreadsheet.Cell>();
				if (rows.ContainsKey(key))
				{
					rows[key].AddCells(cells);
				}
				else
				{
					rows.Add(key, new global::Telerik.Web.Spreadsheet.Row
					{
						Index = new int?(key),
						Cells = cells
					});
				}
			}
			return rows.Values;
		}

		private static global::System.Collections.Generic.SortedDictionary<int, global::Telerik.Web.Spreadsheet.Column> GetCols(global::Telerik.Windows.Documents.Spreadsheet.Model.Worksheet worksheet)
		{
			global::System.Collections.Generic.SortedDictionary<int, global::Telerik.Web.Spreadsheet.Column> sortedDictionary = new global::System.Collections.Generic.SortedDictionary<int, global::Telerik.Web.Spreadsheet.Column>();
			foreach (global::Telerik.Windows.Documents.Spreadsheet.Core.DataStructures.Range<long, global::Telerik.Windows.Documents.Spreadsheet.Model.ColumnWidth> range in worksheet.Columns.PropertyBag.GetColumnWidthPropertyValueRespectingHidden().GetNonDefaultRanges())
			{
				double value = range.Value.Value;
				for (long num = range.Start; num <= range.End; num += 1L)
				{
					int num2 = (int)num;
					global::Telerik.Windows.Documents.Spreadsheet.Model.ColumnSelection columnSelection = worksheet.Columns[num2];
					bool value2 = columnSelection.GetHidden().Value;
					global::Telerik.Web.Spreadsheet.Column column = new global::Telerik.Web.Spreadsheet.Column
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

		private static global::System.Collections.Generic.SortedDictionary<int, global::Telerik.Web.Spreadsheet.Row> GetRows(global::Telerik.Windows.Documents.Spreadsheet.Model.Worksheet worksheet)
		{
			global::System.Collections.Generic.SortedDictionary<int, global::Telerik.Web.Spreadsheet.Row> sortedDictionary = new global::System.Collections.Generic.SortedDictionary<int, global::Telerik.Web.Spreadsheet.Row>();
			foreach (global::Telerik.Windows.Documents.Spreadsheet.Core.DataStructures.Range<long, global::Telerik.Windows.Documents.Spreadsheet.Model.RowHeight> range in worksheet.Rows.PropertyBag.GetRowHeightPropertyValueRespectingHidden().GetNonDefaultRanges())
			{
				double value = range.Value.Value;
				for (long num = range.Start; num <= range.End; num += 1L)
				{
					int num2 = (int)num;
					global::Telerik.Windows.Documents.Spreadsheet.Model.RowSelection rowSelection = worksheet.Rows[num2];
					bool value2 = rowSelection.GetHidden().Value;
					global::Telerik.Web.Spreadsheet.Row row = new global::Telerik.Web.Spreadsheet.Row
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

		private static global::System.Collections.Generic.SortedDictionary<int, global::System.Collections.Generic.SortedDictionary<int, global::Telerik.Web.Spreadsheet.Cell>> GetCellsWithProperties(global::Telerik.Windows.Documents.Spreadsheet.Model.Worksheet worksheet)
		{
			global::System.Collections.Generic.SortedDictionary<int, global::System.Collections.Generic.SortedDictionary<int, global::Telerik.Web.Spreadsheet.Cell>> sortedDictionary = new global::System.Collections.Generic.SortedDictionary<int, global::System.Collections.Generic.SortedDictionary<int, global::Telerik.Web.Spreadsheet.Cell>>();
			global::Telerik.Web.Spreadsheet.DocumentWorksheetExtensions.GetCellProperty<global::Telerik.Windows.Documents.Spreadsheet.Model.ThemableColor>(worksheet, global::Telerik.Windows.Documents.Spreadsheet.PropertySystem.CellPropertyDefinitions.ForeColorProperty, sortedDictionary);
			global::Telerik.Web.Spreadsheet.DocumentWorksheetExtensions.GetCellProperty<global::Telerik.Windows.Documents.Spreadsheet.Model.CellValueFormat>(worksheet, global::Telerik.Windows.Documents.Spreadsheet.PropertySystem.CellPropertyDefinitions.FormatProperty, sortedDictionary);
			global::Telerik.Web.Spreadsheet.DocumentWorksheetExtensions.GetCellProperty<global::Telerik.Windows.Documents.Spreadsheet.Model.ICellValue>(worksheet, global::Telerik.Windows.Documents.Spreadsheet.PropertySystem.CellPropertyDefinitions.ValueProperty, sortedDictionary);
			global::Telerik.Web.Spreadsheet.DocumentWorksheetExtensions.GetCellProperty<global::Telerik.Windows.Documents.Spreadsheet.Model.IFill>(worksheet, global::Telerik.Windows.Documents.Spreadsheet.PropertySystem.CellPropertyDefinitions.FillProperty, sortedDictionary);
			global::Telerik.Web.Spreadsheet.DocumentWorksheetExtensions.GetCellProperty<bool>(worksheet, global::Telerik.Windows.Documents.Spreadsheet.PropertySystem.CellPropertyDefinitions.IsBoldProperty, sortedDictionary);
			global::Telerik.Web.Spreadsheet.DocumentWorksheetExtensions.GetCellProperty<bool>(worksheet, global::Telerik.Windows.Documents.Spreadsheet.PropertySystem.CellPropertyDefinitions.IsItalicProperty, sortedDictionary);
			global::Telerik.Web.Spreadsheet.DocumentWorksheetExtensions.GetCellProperty<bool>(worksheet, global::Telerik.Windows.Documents.Spreadsheet.PropertySystem.CellPropertyDefinitions.IsWrappedProperty, sortedDictionary);
			global::Telerik.Web.Spreadsheet.DocumentWorksheetExtensions.GetCellProperty<global::Telerik.Windows.Documents.Spreadsheet.Model.UnderlineType>(worksheet, global::Telerik.Windows.Documents.Spreadsheet.PropertySystem.CellPropertyDefinitions.UnderlineProperty, sortedDictionary);
			global::Telerik.Web.Spreadsheet.DocumentWorksheetExtensions.GetCellProperty<global::Telerik.Windows.Documents.Spreadsheet.Model.RadVerticalAlignment>(worksheet, global::Telerik.Windows.Documents.Spreadsheet.PropertySystem.CellPropertyDefinitions.VerticalAlignmentProperty, sortedDictionary);
			global::Telerik.Web.Spreadsheet.DocumentWorksheetExtensions.GetCellProperty<global::Telerik.Windows.Documents.Spreadsheet.Model.RadHorizontalAlignment>(worksheet, global::Telerik.Windows.Documents.Spreadsheet.PropertySystem.CellPropertyDefinitions.HorizontalAlignmentProperty, sortedDictionary);
			global::Telerik.Web.Spreadsheet.DocumentWorksheetExtensions.GetCellProperty<double>(worksheet, global::Telerik.Windows.Documents.Spreadsheet.PropertySystem.CellPropertyDefinitions.FontSizeProperty, sortedDictionary);
			global::Telerik.Web.Spreadsheet.DocumentWorksheetExtensions.GetCellProperty<global::Telerik.Windows.Documents.Spreadsheet.Model.ThemableFontFamily>(worksheet, global::Telerik.Windows.Documents.Spreadsheet.PropertySystem.CellPropertyDefinitions.FontFamilyProperty, sortedDictionary);
			global::Telerik.Web.Spreadsheet.DocumentWorksheetExtensions.GetCellProperty<global::Telerik.Windows.Documents.Spreadsheet.Model.CellBorder>(worksheet, global::Telerik.Windows.Documents.Spreadsheet.PropertySystem.CellPropertyDefinitions.BottomBorderProperty, sortedDictionary);
			global::Telerik.Web.Spreadsheet.DocumentWorksheetExtensions.GetCellProperty<global::Telerik.Windows.Documents.Spreadsheet.Model.CellBorder>(worksheet, global::Telerik.Windows.Documents.Spreadsheet.PropertySystem.CellPropertyDefinitions.TopBorderProperty, sortedDictionary);
			global::Telerik.Web.Spreadsheet.DocumentWorksheetExtensions.GetCellProperty<global::Telerik.Windows.Documents.Spreadsheet.Model.CellBorder>(worksheet, global::Telerik.Windows.Documents.Spreadsheet.PropertySystem.CellPropertyDefinitions.LeftBorderProperty, sortedDictionary);
			global::Telerik.Web.Spreadsheet.DocumentWorksheetExtensions.GetCellProperty<global::Telerik.Windows.Documents.Spreadsheet.Model.CellBorder>(worksheet, global::Telerik.Windows.Documents.Spreadsheet.PropertySystem.CellPropertyDefinitions.RightBorderProperty, sortedDictionary);
			global::Telerik.Web.Spreadsheet.DocumentWorksheetExtensions.GetCellProperty<global::Telerik.Windows.Documents.Spreadsheet.Model.DataValidation.IDataValidationRule>(worksheet, global::Telerik.Windows.Documents.Spreadsheet.PropertySystem.CellPropertyDefinitions.DataValidationRuleProperty, sortedDictionary);
			global::Telerik.Web.Spreadsheet.DocumentWorksheetExtensions.GetCellHyperLinks(worksheet, sortedDictionary);
			return sortedDictionary;
		}

		private static void GetCellProperty<T>(global::Telerik.Windows.Documents.Spreadsheet.Model.Worksheet worksheet, global::Telerik.Windows.Documents.Spreadsheet.PropertySystem.IPropertyDefinition<T> propertyDefinition, global::System.Collections.Generic.SortedDictionary<int, global::System.Collections.Generic.SortedDictionary<int, global::Telerik.Web.Spreadsheet.Cell>> state)
		{
			global::System.Collections.Generic.IEnumerable<global::Telerik.Windows.Documents.Spreadsheet.Core.DataStructures.Range<long, T>> nonDefaultRanges = worksheet.Cells.PropertyBag.GetPropertyValueCollection<T>(propertyDefinition).GetNonDefaultRanges();
			global::System.Action<global::Telerik.Web.Spreadsheet.Cell, global::Telerik.Windows.Documents.Spreadsheet.Theming.DocumentTheme, object> setter = global::Telerik.Web.Spreadsheet.DocumentWorksheetExtensions.CellSetters[propertyDefinition];
			foreach (global::Telerik.Windows.Documents.Spreadsheet.Core.DataStructures.Range<long, T> range in nonDefaultRanges)
			{
				global::Telerik.Web.Spreadsheet.DocumentWorksheetExtensions.SetCellValue(global::Telerik.Windows.Documents.Spreadsheet.PropertySystem.WorksheetPropertyBagBase.ConvertLongCellRangeToCellRange(range.Start, range.End), worksheet, state, setter, range.Value);
			}
		}

		private static void GetCellHyperLinks(global::Telerik.Windows.Documents.Spreadsheet.Model.Worksheet worksheet, global::System.Collections.Generic.SortedDictionary<int, global::System.Collections.Generic.SortedDictionary<int, global::Telerik.Web.Spreadsheet.Cell>> state)
		{
			foreach (global::Telerik.Windows.Documents.Spreadsheet.Model.SpreadsheetHyperlink spreadsheetHyperlink in worksheet.Hyperlinks)
			{
				global::Telerik.Web.Spreadsheet.DocumentWorksheetExtensions.SetCellValue(spreadsheetHyperlink.Range, worksheet, state, new global::System.Action<global::Telerik.Web.Spreadsheet.Cell, global::Telerik.Windows.Documents.Spreadsheet.Theming.DocumentTheme, object>(global::Telerik.Web.Spreadsheet.DocumentWorksheetExtensions.SetHyperlink), spreadsheetHyperlink.HyperlinkInfo.Address);
			}
		}

		private static void SetCellValue(global::Telerik.Windows.Documents.Spreadsheet.Model.CellRange cellRange, global::Telerik.Windows.Documents.Spreadsheet.Model.Worksheet worksheet, global::System.Collections.Generic.SortedDictionary<int, global::System.Collections.Generic.SortedDictionary<int, global::Telerik.Web.Spreadsheet.Cell>> state, global::System.Action<global::Telerik.Web.Spreadsheet.Cell, global::Telerik.Windows.Documents.Spreadsheet.Theming.DocumentTheme, object> setter, object value)
		{
			for (int i = cellRange.FromIndex.RowIndex; i <= cellRange.ToIndex.RowIndex; i++)
			{
				for (int j = cellRange.FromIndex.ColumnIndex; j <= cellRange.ToIndex.ColumnIndex; j++)
				{
					global::Telerik.Web.Spreadsheet.Cell cell;
					if (state.ContainsKey(i))
					{
						global::System.Collections.Generic.SortedDictionary<int, global::Telerik.Web.Spreadsheet.Cell> sortedDictionary = state[i];
						if (sortedDictionary.ContainsKey(j))
						{
							cell = sortedDictionary[j];
						}
						else
						{
							cell = new global::Telerik.Web.Spreadsheet.Cell
							{
								Index = new int?(j)
							};
							sortedDictionary.Add(j, cell);
						}
					}
					else
					{
						cell = new global::Telerik.Web.Spreadsheet.Cell
						{
							Index = new int?(j)
						};
						state.Add(i, new global::System.Collections.Generic.SortedDictionary<int, global::Telerik.Web.Spreadsheet.Cell> { { j, cell } });
					}
					setter(cell, worksheet.Workbook.Theme, value);
				}
			}
		}

		private static void SetColor(global::Telerik.Web.Spreadsheet.Cell cell, global::Telerik.Windows.Documents.Spreadsheet.Theming.DocumentTheme theme, object value)
		{
			cell.Color = ((global::Telerik.Windows.Documents.Spreadsheet.Model.ThemableColor)value).GetActualValue(theme.ColorScheme).ToHex();
		}

		private static void SetBackground(global::Telerik.Web.Spreadsheet.Cell cell, global::Telerik.Windows.Documents.Spreadsheet.Theming.DocumentTheme theme, object value)
		{
			cell.Background = ((global::Telerik.Windows.Documents.Spreadsheet.Model.PatternFill)value).PatternColor.GetActualValue(theme.ColorScheme).ToHex();
		}

		private static void SetBold(global::Telerik.Web.Spreadsheet.Cell cell, global::Telerik.Windows.Documents.Spreadsheet.Theming.DocumentTheme theme, object value)
		{
			cell.Bold = new bool?((bool)value);
		}

		private static void SetItalic(global::Telerik.Web.Spreadsheet.Cell cell, global::Telerik.Windows.Documents.Spreadsheet.Theming.DocumentTheme theme, object value)
		{
			cell.Italic = new bool?((bool)value);
		}

		private static void SetWrap(global::Telerik.Web.Spreadsheet.Cell cell, global::Telerik.Windows.Documents.Spreadsheet.Theming.DocumentTheme theme, object value)
		{
			cell.Wrap = new bool?((bool)value);
		}

		private static void SetUnderline(global::Telerik.Web.Spreadsheet.Cell cell, global::Telerik.Windows.Documents.Spreadsheet.Theming.DocumentTheme theme, object value)
		{
			cell.Underline = new bool?((global::Telerik.Windows.Documents.Spreadsheet.Model.UnderlineType)value > global::Telerik.Windows.Documents.Spreadsheet.Model.UnderlineType.None);
		}

		private static void SetVerticalAlign(global::Telerik.Web.Spreadsheet.Cell cell, global::Telerik.Windows.Documents.Spreadsheet.Theming.DocumentTheme theme, object value)
		{
			cell.VerticalAlign = ((global::Telerik.Windows.Documents.Spreadsheet.Model.RadVerticalAlignment)value).AsString();
		}

		private static void SetHorizontalAlign(global::Telerik.Web.Spreadsheet.Cell cell, global::Telerik.Windows.Documents.Spreadsheet.Theming.DocumentTheme theme, object value)
		{
			cell.TextAlign = ((global::Telerik.Windows.Documents.Spreadsheet.Model.RadHorizontalAlignment)value).ToString().ToLower();
		}

		private static void SetFontSize(global::Telerik.Web.Spreadsheet.Cell cell, global::Telerik.Windows.Documents.Spreadsheet.Theming.DocumentTheme theme, object value)
		{
			cell.FontSize = new double?(global::Telerik.Windows.Documents.Spreadsheet.Utilities.UnitHelper.DipToPoint((double)value));
		}

		private static void SetFontFamily(global::Telerik.Web.Spreadsheet.Cell cell, global::Telerik.Windows.Documents.Spreadsheet.Theming.DocumentTheme theme, object value)
		{
			cell.FontFamily = ((global::Telerik.Windows.Documents.Spreadsheet.Model.ThemableFontFamily)value).GetActualValue(theme).FamilyNames.Values.First<string>();
		}

		private static void SetBorderBottom(global::Telerik.Web.Spreadsheet.Cell cell, global::Telerik.Windows.Documents.Spreadsheet.Theming.DocumentTheme theme, object value)
		{
			cell.BorderBottom = ((global::Telerik.Windows.Documents.Spreadsheet.Model.CellBorder)value).ToBorderStyle(theme);
		}

		private static void SetBorderTop(global::Telerik.Web.Spreadsheet.Cell cell, global::Telerik.Windows.Documents.Spreadsheet.Theming.DocumentTheme theme, object value)
		{
			cell.BorderTop = ((global::Telerik.Windows.Documents.Spreadsheet.Model.CellBorder)value).ToBorderStyle(theme);
		}

		private static void SetBorderLeft(global::Telerik.Web.Spreadsheet.Cell cell, global::Telerik.Windows.Documents.Spreadsheet.Theming.DocumentTheme theme, object value)
		{
			cell.BorderLeft = ((global::Telerik.Windows.Documents.Spreadsheet.Model.CellBorder)value).ToBorderStyle(theme);
		}

		private static void SetBorderRight(global::Telerik.Web.Spreadsheet.Cell cell, global::Telerik.Windows.Documents.Spreadsheet.Theming.DocumentTheme theme, object value)
		{
			cell.BorderRight = ((global::Telerik.Windows.Documents.Spreadsheet.Model.CellBorder)value).ToBorderStyle(theme);
		}

		private static void SetFormat(global::Telerik.Web.Spreadsheet.Cell cell, global::Telerik.Windows.Documents.Spreadsheet.Theming.DocumentTheme theme, object value)
		{
			cell.Format = ((global::Telerik.Windows.Documents.Spreadsheet.Model.CellValueFormat)value).FormatString;
		}

		private static void SetHyperlink(global::Telerik.Web.Spreadsheet.Cell cell, global::Telerik.Windows.Documents.Spreadsheet.Theming.DocumentTheme theme, object value)
		{
			cell.Link = value.ToString();
		}

		private static void SetValidation(global::Telerik.Web.Spreadsheet.Cell cell, global::Telerik.Windows.Documents.Spreadsheet.Theming.DocumentTheme theme, object value)
		{
			cell.Validation = ((global::Telerik.Windows.Documents.Spreadsheet.Model.DataValidation.IDataValidationRule)value).ToCellValidation();
		}

		private static void SetValue(global::Telerik.Web.Spreadsheet.Cell cell, global::Telerik.Windows.Documents.Spreadsheet.Theming.DocumentTheme theme, object value)
		{
			global::Telerik.Windows.Documents.Spreadsheet.Model.ICellValue cellValue = (global::Telerik.Windows.Documents.Spreadsheet.Model.ICellValue)value;
			string formula = null;
			global::Telerik.Windows.Documents.Spreadsheet.Model.FormulaCellValue formulaCellValue = cellValue as global::Telerik.Windows.Documents.Spreadsheet.Model.FormulaCellValue;
			if (formulaCellValue != null)
			{
				cellValue = formulaCellValue.GetResultValueAsCellValue();
				formula = formulaCellValue.RawValue.Substring(1);
			}
			value = cellValue.RawValue;
			if (cellValue.ValueType == global::Telerik.Windows.Documents.Spreadsheet.Model.CellValueType.Number)
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
			if (cellValue.ValueType == global::Telerik.Windows.Documents.Spreadsheet.Model.CellValueType.Boolean && bool.TryParse(cellValue.RawValue, out flag))
			{
				value = flag;
			}
			cell.Value = value;
			cell.Formula = formula;
		}

		private static global::System.Collections.Generic.Dictionary<global::Telerik.Windows.Documents.Spreadsheet.PropertySystem.IPropertyDefinition, global::System.Action<global::Telerik.Web.Spreadsheet.Cell, global::Telerik.Windows.Documents.Spreadsheet.Theming.DocumentTheme, object>> CellSetters = new global::System.Collections.Generic.Dictionary<global::Telerik.Windows.Documents.Spreadsheet.PropertySystem.IPropertyDefinition, global::System.Action<global::Telerik.Web.Spreadsheet.Cell, global::Telerik.Windows.Documents.Spreadsheet.Theming.DocumentTheme, object>>
		{
			{
				global::Telerik.Windows.Documents.Spreadsheet.PropertySystem.CellPropertyDefinitions.ForeColorProperty,
				new global::System.Action<global::Telerik.Web.Spreadsheet.Cell, global::Telerik.Windows.Documents.Spreadsheet.Theming.DocumentTheme, object>(global::Telerik.Web.Spreadsheet.DocumentWorksheetExtensions.SetColor)
			},
			{
				global::Telerik.Windows.Documents.Spreadsheet.PropertySystem.CellPropertyDefinitions.FillProperty,
				new global::System.Action<global::Telerik.Web.Spreadsheet.Cell, global::Telerik.Windows.Documents.Spreadsheet.Theming.DocumentTheme, object>(global::Telerik.Web.Spreadsheet.DocumentWorksheetExtensions.SetBackground)
			},
			{
				global::Telerik.Windows.Documents.Spreadsheet.PropertySystem.CellPropertyDefinitions.IsBoldProperty,
				new global::System.Action<global::Telerik.Web.Spreadsheet.Cell, global::Telerik.Windows.Documents.Spreadsheet.Theming.DocumentTheme, object>(global::Telerik.Web.Spreadsheet.DocumentWorksheetExtensions.SetBold)
			},
			{
				global::Telerik.Windows.Documents.Spreadsheet.PropertySystem.CellPropertyDefinitions.IsItalicProperty,
				new global::System.Action<global::Telerik.Web.Spreadsheet.Cell, global::Telerik.Windows.Documents.Spreadsheet.Theming.DocumentTheme, object>(global::Telerik.Web.Spreadsheet.DocumentWorksheetExtensions.SetItalic)
			},
			{
				global::Telerik.Windows.Documents.Spreadsheet.PropertySystem.CellPropertyDefinitions.IsWrappedProperty,
				new global::System.Action<global::Telerik.Web.Spreadsheet.Cell, global::Telerik.Windows.Documents.Spreadsheet.Theming.DocumentTheme, object>(global::Telerik.Web.Spreadsheet.DocumentWorksheetExtensions.SetWrap)
			},
			{
				global::Telerik.Windows.Documents.Spreadsheet.PropertySystem.CellPropertyDefinitions.UnderlineProperty,
				new global::System.Action<global::Telerik.Web.Spreadsheet.Cell, global::Telerik.Windows.Documents.Spreadsheet.Theming.DocumentTheme, object>(global::Telerik.Web.Spreadsheet.DocumentWorksheetExtensions.SetUnderline)
			},
			{
				global::Telerik.Windows.Documents.Spreadsheet.PropertySystem.CellPropertyDefinitions.VerticalAlignmentProperty,
				new global::System.Action<global::Telerik.Web.Spreadsheet.Cell, global::Telerik.Windows.Documents.Spreadsheet.Theming.DocumentTheme, object>(global::Telerik.Web.Spreadsheet.DocumentWorksheetExtensions.SetVerticalAlign)
			},
			{
				global::Telerik.Windows.Documents.Spreadsheet.PropertySystem.CellPropertyDefinitions.HorizontalAlignmentProperty,
				new global::System.Action<global::Telerik.Web.Spreadsheet.Cell, global::Telerik.Windows.Documents.Spreadsheet.Theming.DocumentTheme, object>(global::Telerik.Web.Spreadsheet.DocumentWorksheetExtensions.SetHorizontalAlign)
			},
			{
				global::Telerik.Windows.Documents.Spreadsheet.PropertySystem.CellPropertyDefinitions.FontSizeProperty,
				new global::System.Action<global::Telerik.Web.Spreadsheet.Cell, global::Telerik.Windows.Documents.Spreadsheet.Theming.DocumentTheme, object>(global::Telerik.Web.Spreadsheet.DocumentWorksheetExtensions.SetFontSize)
			},
			{
				global::Telerik.Windows.Documents.Spreadsheet.PropertySystem.CellPropertyDefinitions.FontFamilyProperty,
				new global::System.Action<global::Telerik.Web.Spreadsheet.Cell, global::Telerik.Windows.Documents.Spreadsheet.Theming.DocumentTheme, object>(global::Telerik.Web.Spreadsheet.DocumentWorksheetExtensions.SetFontFamily)
			},
			{
				global::Telerik.Windows.Documents.Spreadsheet.PropertySystem.CellPropertyDefinitions.BottomBorderProperty,
				new global::System.Action<global::Telerik.Web.Spreadsheet.Cell, global::Telerik.Windows.Documents.Spreadsheet.Theming.DocumentTheme, object>(global::Telerik.Web.Spreadsheet.DocumentWorksheetExtensions.SetBorderBottom)
			},
			{
				global::Telerik.Windows.Documents.Spreadsheet.PropertySystem.CellPropertyDefinitions.TopBorderProperty,
				new global::System.Action<global::Telerik.Web.Spreadsheet.Cell, global::Telerik.Windows.Documents.Spreadsheet.Theming.DocumentTheme, object>(global::Telerik.Web.Spreadsheet.DocumentWorksheetExtensions.SetBorderTop)
			},
			{
				global::Telerik.Windows.Documents.Spreadsheet.PropertySystem.CellPropertyDefinitions.LeftBorderProperty,
				new global::System.Action<global::Telerik.Web.Spreadsheet.Cell, global::Telerik.Windows.Documents.Spreadsheet.Theming.DocumentTheme, object>(global::Telerik.Web.Spreadsheet.DocumentWorksheetExtensions.SetBorderLeft)
			},
			{
				global::Telerik.Windows.Documents.Spreadsheet.PropertySystem.CellPropertyDefinitions.RightBorderProperty,
				new global::System.Action<global::Telerik.Web.Spreadsheet.Cell, global::Telerik.Windows.Documents.Spreadsheet.Theming.DocumentTheme, object>(global::Telerik.Web.Spreadsheet.DocumentWorksheetExtensions.SetBorderRight)
			},
			{
				global::Telerik.Windows.Documents.Spreadsheet.PropertySystem.CellPropertyDefinitions.FormatProperty,
				new global::System.Action<global::Telerik.Web.Spreadsheet.Cell, global::Telerik.Windows.Documents.Spreadsheet.Theming.DocumentTheme, object>(global::Telerik.Web.Spreadsheet.DocumentWorksheetExtensions.SetFormat)
			},
			{
				global::Telerik.Windows.Documents.Spreadsheet.PropertySystem.CellPropertyDefinitions.ValueProperty,
				new global::System.Action<global::Telerik.Web.Spreadsheet.Cell, global::Telerik.Windows.Documents.Spreadsheet.Theming.DocumentTheme, object>(global::Telerik.Web.Spreadsheet.DocumentWorksheetExtensions.SetValue)
			},
			{
				global::Telerik.Windows.Documents.Spreadsheet.PropertySystem.CellPropertyDefinitions.DataValidationRuleProperty,
				new global::System.Action<global::Telerik.Web.Spreadsheet.Cell, global::Telerik.Windows.Documents.Spreadsheet.Theming.DocumentTheme, object>(global::Telerik.Web.Spreadsheet.DocumentWorksheetExtensions.SetValidation)
			}
		};
	}
}
