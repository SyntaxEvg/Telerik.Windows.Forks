using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Telerik.Windows.Documents.Spreadsheet.Core;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Model.DataValidation;
using Telerik.Windows.Documents.Spreadsheet.Model.Filtering;
using Telerik.Windows.Documents.Spreadsheet.Model.Sorting;
using Telerik.Windows.Documents.Spreadsheet.PropertySystem;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Web.Spreadsheet
{
	[global::System.Runtime.Serialization.DataContract]
	public class Workbook
	{
		public static global::Telerik.Web.Spreadsheet.Workbook FromJson(string json)
		{
			return global::Newtonsoft.Json.JsonConvert.DeserializeObject<global::Telerik.Web.Spreadsheet.Workbook>(json);
		}

		public string ToJson()
		{
			return global::Newtonsoft.Json.JsonConvert.SerializeObject(this);
		}

		public static global::Telerik.Web.Spreadsheet.Workbook FromDocument(global::Telerik.Windows.Documents.Spreadsheet.Model.Workbook document)
		{
			global::Telerik.Web.Spreadsheet.Workbook workbook = new global::Telerik.Web.Spreadsheet.Workbook();
			workbook.ActiveSheet = document.ActiveSheet.Name;
			workbook.NamedRanges = document.Names.GetNamedRanges();
			foreach (global::Telerik.Windows.Documents.Spreadsheet.Model.Worksheet worksheet in document.Worksheets)
			{
				global::Telerik.Web.Spreadsheet.Worksheet worksheet2 = workbook.AddSheet();
				worksheet2.Name = worksheet.Name;
				worksheet2.ActiveCell = global::Telerik.Windows.Documents.Spreadsheet.Utilities.NameConverter.ConvertCellIndexToName(worksheet.ViewState.SelectionState.ActiveCellIndex);
				global::Telerik.Windows.Documents.Spreadsheet.Model.CellRange cellRange = worksheet.ViewState.SelectionState.SelectedRanges.First<global::Telerik.Windows.Documents.Spreadsheet.Model.CellRange>();
				worksheet2.Selection = global::Telerik.Windows.Documents.Spreadsheet.Utilities.NameConverter.ConvertCellRangeToName(cellRange.FromIndex, cellRange.ToIndex);
				worksheet2.Columns = worksheet.ImportColumns().ToList<global::Telerik.Web.Spreadsheet.Column>();
				worksheet2.Rows = worksheet.ImportRows().ToList<global::Telerik.Web.Spreadsheet.Row>();
				worksheet2.MergedCells = global::Telerik.Web.Spreadsheet.Workbook.GetMergedCells(worksheet).ToList<string>();
				worksheet2.ShowGridLines = new bool?(worksheet.ViewState.ShowGridLines);
				global::Telerik.Windows.Documents.Spreadsheet.Model.Pane pane = worksheet.ViewState.Pane;
				if (pane != null && pane.State == global::Telerik.Windows.Documents.Spreadsheet.Model.PaneState.Frozen)
				{
					worksheet2.FrozenRows = new int?(pane.TopLeftCellIndex.RowIndex);
					worksheet2.FrozenColumns = new int?(pane.TopLeftCellIndex.ColumnIndex);
				}
				worksheet2.Sort = global::Telerik.Web.Spreadsheet.Workbook.GetSorting(worksheet);
				worksheet2.Filter = global::Telerik.Web.Spreadsheet.Workbook.GetFilters(worksheet);
			}
			return workbook;
		}

		private static global::System.Collections.Generic.IEnumerable<string> GetMergedCells(global::Telerik.Windows.Documents.Spreadsheet.Model.Worksheet worksheet)
		{
			foreach (global::Telerik.Windows.Documents.Spreadsheet.Model.CellRange cellRange in worksheet.Cells.GetMergedCellRanges())
			{
				yield return global::Telerik.Windows.Documents.Spreadsheet.Utilities.NameConverter.ConvertCellRangeToName(cellRange.FromIndex, cellRange.ToIndex);
			}
			global::System.Collections.Generic.IEnumerator<global::Telerik.Windows.Documents.Spreadsheet.Model.CellRange> enumerator = null;
			yield break;
			yield break;
		}

		private static global::Telerik.Web.Spreadsheet.Sort GetSorting(global::Telerik.Windows.Documents.Spreadsheet.Model.Worksheet worksheet)
		{
			global::Telerik.Windows.Documents.Spreadsheet.Model.CellRange sortRange = worksheet.SortState.SortRange;
			if (sortRange == null)
			{
				return null;
			}
			global::Telerik.Web.Spreadsheet.Sort sort = new global::Telerik.Web.Spreadsheet.Sort();
			sort.Ref = global::Telerik.Windows.Documents.Spreadsheet.Utilities.NameConverter.ConvertCellRangeToName(sortRange.FromIndex, sortRange.ToIndex);
			sort.Columns = worksheet.SortState.SortConditions.OfType<global::Telerik.Windows.Documents.Spreadsheet.Model.Sorting.ValuesSortCondition>().Select((global::Telerik.Windows.Documents.Spreadsheet.Model.Sorting.ValuesSortCondition cond) => new global::Telerik.Web.Spreadsheet.SortColumn
			{
				Ascending = new bool?(cond.SortOrder == global::Telerik.Windows.Documents.Spreadsheet.Model.Sorting.SortOrder.Ascending),
				Index = new double?((double)cond.RelativeIndex)
			}).ToList<global::Telerik.Web.Spreadsheet.SortColumn>();
			return sort;
		}

		private static global::Telerik.Web.Spreadsheet.Filter GetFilters(global::Telerik.Windows.Documents.Spreadsheet.Model.Worksheet worksheet)
		{
			global::Telerik.Windows.Documents.Spreadsheet.Model.Filtering.AutoFilter filter = worksheet.Filter;
			global::Telerik.Windows.Documents.Spreadsheet.Model.CellRange filterRange = filter.FilterRange;
			if (filterRange == null)
			{
				return null;
			}
			global::Telerik.Web.Spreadsheet.Filter filter2 = new global::Telerik.Web.Spreadsheet.Filter();
			filter2.Ref = global::Telerik.Windows.Documents.Spreadsheet.Utilities.NameConverter.ConvertCellRangeToName(filterRange.FromIndex, filterRange.ToIndex);
			filter2.Columns = filter.Filters.Select((global::Telerik.Windows.Documents.Spreadsheet.Model.Filtering.IFilter item) => item.ToFilterColumn()).SkipWhile((global::Telerik.Web.Spreadsheet.FilterColumn column) => column == null).ToList<global::Telerik.Web.Spreadsheet.FilterColumn>();
			return filter2;
		}

		public global::Telerik.Windows.Documents.Spreadsheet.Model.Workbook ToDocument()
		{
			global::Telerik.Windows.Documents.Spreadsheet.Model.Workbook workbook = new global::Telerik.Windows.Documents.Spreadsheet.Model.Workbook();
			workbook.History.IsEnabled = false;
			using (new global::Telerik.Windows.Documents.Spreadsheet.Core.UpdateScope(new global::System.Action(workbook.SuspendLayoutUpdate), new global::System.Action(workbook.ResumeLayoutUpdate)))
			{
				foreach (global::Telerik.Web.Spreadsheet.Worksheet worksheet in this.Sheets.GetOrDefault<global::Telerik.Web.Spreadsheet.Worksheet>())
				{
					global::Telerik.Windows.Documents.Spreadsheet.Model.Worksheet worksheet2 = workbook.Worksheets.Add();
					if (!string.IsNullOrEmpty(worksheet.Name))
					{
						worksheet2.Name = worksheet.Name;
					}
					if (worksheet.Name == this.ActiveSheet)
					{
						workbook.ActiveWorksheet = worksheet2;
					}
					worksheet2.ViewState.SelectionState = this.CreateSelectionState(worksheet, worksheet2);
					foreach (global::Telerik.Web.Spreadsheet.Row row in worksheet.Rows.GetOrDefault<global::Telerik.Web.Spreadsheet.Row>())
					{
						this.SetCells(row, worksheet2);
						if (row.Height != null)
						{
							double? num = row.Height;
							double value = global::Telerik.Windows.Documents.Spreadsheet.PropertySystem.RowsPropertyBag.HeightProperty.DefaultValue.Value;
							if (!((num.GetValueOrDefault() == value) & (num != null)))
							{
								worksheet2.Rows[row.Index.Value].SetHeight(new global::Telerik.Windows.Documents.Spreadsheet.Model.RowHeight(row.Height.Value, true));
							}
						}
						if (row.Hidden != null)
						{
							worksheet2.Rows[row.Index.Value].SetHidden(true);
							worksheet2.Rows[row.Index.Value].SetHeight(new global::Telerik.Windows.Documents.Spreadsheet.Model.RowHeight(row.Hidden.Value, true));
						}
					}
					foreach (global::Telerik.Web.Spreadsheet.Column column in worksheet.Columns.GetOrDefault<global::Telerik.Web.Spreadsheet.Column>())
					{
						if (column.Width != null)
						{
							double? num = column.Width;
							double value = global::Telerik.Windows.Documents.Spreadsheet.PropertySystem.ColumnsPropertyBag.WidthProperty.DefaultValue.Value;
							if (!((num.GetValueOrDefault() == value) & (num != null)))
							{
								worksheet2.Columns[column.Index.Value].SetWidth(new global::Telerik.Windows.Documents.Spreadsheet.Model.ColumnWidth(column.Width.Value, true));
							}
						}
						if (column.Hidden != null)
						{
							worksheet2.Columns[column.Index.Value].SetHidden(true);
							worksheet2.Columns[column.Index.Value].SetWidth(new global::Telerik.Windows.Documents.Spreadsheet.Model.ColumnWidth(column.Hidden.Value, true));
						}
					}
					foreach (string cellRangeRef in worksheet.MergedCells.GetOrDefault<string>())
					{
						worksheet2.Cells.GetCellSelection(cellRangeRef).Merge();
					}
					if (worksheet.FrozenColumns.GetValueOrDefault() > 0 || worksheet.FrozenRows.GetValueOrDefault() > 0)
					{
						worksheet2.ViewState.FreezePanes(worksheet.FrozenRows.GetValueOrDefault(), worksheet.FrozenColumns.GetValueOrDefault());
					}
					worksheet2.ViewState.ShowGridLines = worksheet.ShowGridLines.GetValueOrDefault(false);
					this.SetSortState(worksheet2, worksheet.Sort);
					this.SetFilterState(worksheet2, worksheet.Filter);
				}
				if (workbook.Worksheets.Count > 0)
				{
					workbook.ActiveWorksheet = workbook.Worksheets[0];
					foreach (global::Telerik.Web.Spreadsheet.NamedRange namedRange in this.NamedRanges)
					{
						workbook.Names.Add(namedRange.Name, string.Format("={0}", namedRange.Value), 0, 0, null, true);
					}
				}
				for (int i = 0; i < this.Sheets.Count; i++)
				{
					foreach (global::Telerik.Web.Spreadsheet.Row srcRow in this.Sheets[i].Rows.GetOrDefault<global::Telerik.Web.Spreadsheet.Row>())
					{
						this.SetCells(srcRow, workbook.Worksheets[i]);
					}
				}
			}
			workbook.History.IsEnabled = true;
			return workbook;
		}

		private global::Telerik.Windows.Documents.Spreadsheet.Model.SelectionState CreateSelectionState(global::Telerik.Web.Spreadsheet.Worksheet sheet, global::Telerik.Windows.Documents.Spreadsheet.Model.Worksheet documentSheet)
		{
			if (sheet.Selection != null)
			{
				return new global::Telerik.Windows.Documents.Spreadsheet.Model.SelectionState(sheet.Selection.ToCellRange(), sheet.ActiveCell.ToCellIndex(), documentSheet.ViewState.SelectionState.Pane);
			}
			return new global::Telerik.Windows.Documents.Spreadsheet.Model.SelectionState();
		}

		private void SetCells(global::Telerik.Web.Spreadsheet.Row srcRow, global::Telerik.Windows.Documents.Spreadsheet.Model.Worksheet documentSheet)
		{
			foreach (global::Telerik.Web.Spreadsheet.Cell cell in srcRow.Cells.GetOrDefault<global::Telerik.Web.Spreadsheet.Cell>())
			{
				string text = ((cell.Value == null) ? null : cell.Value.ToString());
				global::Telerik.Windows.Documents.Spreadsheet.Model.CellSelection cellSelection = documentSheet.Cells[srcRow.Index.Value, cell.Index.Value];
				global::Telerik.Windows.Documents.Spreadsheet.Model.CellIndex cellIndex = new global::Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(srcRow.Index.Value, cell.Index.Value);
				string formula = cell.Formula;
				double value;
				if (!string.IsNullOrEmpty(formula))
				{
					cellSelection.SetValueAsFormula("=" + formula);
				}
				else if (double.TryParse(text, out value) && cell.Format != "@")
				{
					cellSelection.SetValue(value);
				}
				else if (!string.IsNullOrEmpty(text))
				{
					cellSelection.SetValueAsText(text);
				}
				if (!string.IsNullOrEmpty(cell.Format))
				{
					cellSelection.SetFormat(new global::Telerik.Windows.Documents.Spreadsheet.Model.CellValueFormat(cell.Format));
				}
				if (!string.IsNullOrEmpty(cell.Color))
				{
					cellSelection.SetForeColor(new global::Telerik.Windows.Documents.Spreadsheet.Model.ThemableColor(cell.Color.ToColor()));
				}
				if (!string.IsNullOrEmpty(cell.Background))
				{
					global::Telerik.Windows.Documents.Spreadsheet.Model.IFill fill = global::Telerik.Windows.Documents.Spreadsheet.Model.PatternFill.CreateSolidFill(cell.Background.ToColor());
					cellSelection.SetFill(fill);
				}
				if (cell.Bold != null)
				{
					cellSelection.SetIsBold(cell.Bold.Value);
				}
				if (cell.Italic != null)
				{
					cellSelection.SetIsItalic(cell.Italic.Value);
				}
				if (cell.Wrap != null)
				{
					cellSelection.SetIsWrapped(cell.Wrap.Value);
				}
				if (cell.Underline != null)
				{
					bool? underline = cell.Underline;
					bool flag = true;
					if ((underline.GetValueOrDefault() == flag) & (underline != null))
					{
						cellSelection.SetUnderline(global::Telerik.Windows.Documents.Spreadsheet.Model.UnderlineType.Single);
					}
				}
				cellSelection.SetBorders(this.CreateCellBorders(cell));
				if (!string.IsNullOrEmpty(cell.VerticalAlign))
				{
					cellSelection.SetVerticalAlignment(cell.VerticalAlign.ToVerticalAlignment());
				}
				if (!string.IsNullOrEmpty(cell.TextAlign))
				{
					cellSelection.SetHorizontalAlignment(this.ConvertToHorizontalAlignment(cell.TextAlign));
				}
				if (!string.IsNullOrEmpty(cell.FontFamily))
				{
					cellSelection.SetFontFamily(new global::Telerik.Windows.Documents.Spreadsheet.Model.ThemableFontFamily(cell.FontFamily));
				}
				if (cell.FontSize != null)
				{
					cellSelection.SetFontSize(global::Telerik.Windows.Documents.Spreadsheet.Utilities.UnitHelper.PointToDip(cell.FontSize.Value));
				}
				if (!string.IsNullOrEmpty(cell.Link))
				{
					global::Telerik.Windows.Documents.Spreadsheet.Model.HyperlinkInfo hyperlinkInfo = global::Telerik.Windows.Documents.Spreadsheet.Model.HyperlinkInfo.CreateHyperlink(cell.Link, text);
					documentSheet.Hyperlinks.Add(cellSelection.CellRanges.First<global::Telerik.Windows.Documents.Spreadsheet.Model.CellRange>(), hyperlinkInfo);
				}
				if (cell.Validation != null)
				{
					if (cell.Validation.DataType == "list")
					{
						cellSelection.SetDataValidationRule(new global::Telerik.Windows.Documents.Spreadsheet.Model.DataValidation.ListDataValidationRule(new global::Telerik.Windows.Documents.Spreadsheet.Model.DataValidation.ListDataValidationRuleContext(documentSheet, cellIndex)
						{
							Argument1 = string.Format("={0}", cell.Validation.From),
							ErrorAlertContent = cell.Validation.MessageTemplate,
							ErrorAlertTitle = cell.Validation.TitleTemplate,
							IgnoreBlank = cell.Validation.AllowNulls.Value,
							ErrorStyle = ((cell.Validation.Type == "reject") ? global::Telerik.Windows.Documents.Spreadsheet.Model.DataValidation.ErrorStyle.Stop : global::Telerik.Windows.Documents.Spreadsheet.Model.DataValidation.ErrorStyle.Warning)
						}));
					}
					else
					{
						global::Telerik.Windows.Documents.Spreadsheet.Model.DataValidation.NumberDataValidationRuleContext numberDataValidationRuleContext = new global::Telerik.Windows.Documents.Spreadsheet.Model.DataValidation.NumberDataValidationRuleContext(documentSheet, cellIndex);
						numberDataValidationRuleContext.Argument1 = string.Format("={0}", cell.Validation.From);
						numberDataValidationRuleContext.Argument2 = string.Format("={0}", cell.Validation.To);
						numberDataValidationRuleContext.ErrorAlertContent = cell.Validation.MessageTemplate;
						numberDataValidationRuleContext.ErrorAlertTitle = cell.Validation.TitleTemplate;
						numberDataValidationRuleContext.IgnoreBlank = cell.Validation.AllowNulls.Value;
						numberDataValidationRuleContext.ErrorStyle = ((cell.Validation.Type == "reject") ? global::Telerik.Windows.Documents.Spreadsheet.Model.DataValidation.ErrorStyle.Stop : global::Telerik.Windows.Documents.Spreadsheet.Model.DataValidation.ErrorStyle.Warning);
						string comparerType = cell.Validation.ComparerType;
						global::Telerik.Windows.Documents.Spreadsheet.Model.ComparisonOperator comparisonOperator = (this.ValidationComparisonOperators.ContainsKey(comparerType) ? this.ValidationComparisonOperators[comparerType] : global::Telerik.Windows.Documents.Spreadsheet.Model.ComparisonOperator.EqualsTo);
						numberDataValidationRuleContext.ComparisonOperator = comparisonOperator;
						if (cell.Validation.DataType == "date")
						{
							cellSelection.SetDataValidationRule(new global::Telerik.Windows.Documents.Spreadsheet.Model.DataValidation.DateDataValidationRule(numberDataValidationRuleContext));
						}
						else
						{
							cellSelection.SetDataValidationRule(new global::Telerik.Windows.Documents.Spreadsheet.Model.DataValidation.DecimalDataValidationRule(numberDataValidationRuleContext));
						}
					}
				}
			}
		}

		private global::Telerik.Windows.Documents.Spreadsheet.Model.CellBorders CreateCellBorders(global::Telerik.Web.Spreadsheet.Cell cell)
		{
			global::Telerik.Windows.Documents.Spreadsheet.Model.CellBorders cellBorders = new global::Telerik.Windows.Documents.Spreadsheet.Model.CellBorders();
			if (cell.BorderTop != null)
			{
				cellBorders.Top = new global::Telerik.Windows.Documents.Spreadsheet.Model.CellBorder(global::Telerik.Windows.Documents.Spreadsheet.Model.CellBorderStyle.Thin, new global::Telerik.Windows.Documents.Spreadsheet.Model.ThemableColor(cell.BorderTop.Color.ToColor()));
			}
			if (cell.BorderBottom != null)
			{
				cellBorders.Bottom = new global::Telerik.Windows.Documents.Spreadsheet.Model.CellBorder(global::Telerik.Windows.Documents.Spreadsheet.Model.CellBorderStyle.Thin, new global::Telerik.Windows.Documents.Spreadsheet.Model.ThemableColor(cell.BorderBottom.Color.ToColor()));
			}
			if (cell.BorderLeft != null)
			{
				cellBorders.Left = new global::Telerik.Windows.Documents.Spreadsheet.Model.CellBorder(global::Telerik.Windows.Documents.Spreadsheet.Model.CellBorderStyle.Thin, new global::Telerik.Windows.Documents.Spreadsheet.Model.ThemableColor(cell.BorderLeft.Color.ToColor()));
			}
			if (cell.BorderRight != null)
			{
				cellBorders.Right = new global::Telerik.Windows.Documents.Spreadsheet.Model.CellBorder(global::Telerik.Windows.Documents.Spreadsheet.Model.CellBorderStyle.Thin, new global::Telerik.Windows.Documents.Spreadsheet.Model.ThemableColor(cell.BorderRight.Color.ToColor()));
			}
			return cellBorders;
		}

		private global::Telerik.Windows.Documents.Spreadsheet.Model.RadHorizontalAlignment ConvertToHorizontalAlignment(string alignment)
		{
			if (alignment != null)
			{
				if (alignment == "left")
				{
					return global::Telerik.Windows.Documents.Spreadsheet.Model.RadHorizontalAlignment.Left;
				}
				if (alignment == "center")
				{
					return global::Telerik.Windows.Documents.Spreadsheet.Model.RadHorizontalAlignment.Center;
				}
				if (alignment == "right")
				{
					return global::Telerik.Windows.Documents.Spreadsheet.Model.RadHorizontalAlignment.Right;
				}
				if (alignment == "justify")
				{
					return global::Telerik.Windows.Documents.Spreadsheet.Model.RadHorizontalAlignment.Justify;
				}
			}
			return global::Telerik.Windows.Documents.Spreadsheet.Model.RadHorizontalAlignment.General;
		}

		private void SetSortState(global::Telerik.Windows.Documents.Spreadsheet.Model.Worksheet documentWorksheet, global::Telerik.Web.Spreadsheet.Sort sort)
		{
			if (sort == null)
			{
				return;
			}
			global::Telerik.Windows.Documents.Spreadsheet.Model.Sorting.ValuesSortCondition[] array = sort.Columns.Select((global::Telerik.Web.Spreadsheet.SortColumn column) => new global::Telerik.Windows.Documents.Spreadsheet.Model.Sorting.ValuesSortCondition((int)column.Index.Value, column.Ascending.Value ? global::Telerik.Windows.Documents.Spreadsheet.Model.Sorting.SortOrder.Ascending : global::Telerik.Windows.Documents.Spreadsheet.Model.Sorting.SortOrder.Descending)).ToArray<global::Telerik.Windows.Documents.Spreadsheet.Model.Sorting.ValuesSortCondition>();
			global::Telerik.Windows.Documents.Spreadsheet.Model.CellRange cellRange = sort.Ref.ToCellRange().First<global::Telerik.Windows.Documents.Spreadsheet.Model.CellRange>();
			global::Telerik.Windows.Documents.Spreadsheet.Model.Sorting.SortState sortState = documentWorksheet.SortState;
			global::Telerik.Windows.Documents.Spreadsheet.Model.CellRange sortRange = cellRange;
			global::Telerik.Windows.Documents.Spreadsheet.Model.Sorting.ISortCondition[] sortConditions = array;
			sortState.Set(sortRange, sortConditions);
		}

		private T ToEnum<T>(string value)
		{
			return (T)((object)global::System.Enum.Parse(typeof(T), value));
		}

		private void SetFilterState(global::Telerik.Windows.Documents.Spreadsheet.Model.Worksheet documentWorksheet, global::Telerik.Web.Spreadsheet.Filter filter)
		{
			if (filter == null)
			{
				return;
			}
			documentWorksheet.Filter.FilterRange = filter.Ref.ToCellRange().First<global::Telerik.Windows.Documents.Spreadsheet.Model.CellRange>();
			documentWorksheet.Filter.SetFilters(filter.Columns.Select(delegate(global::Telerik.Web.Spreadsheet.FilterColumn column)
			{
				global::Telerik.Windows.Documents.Spreadsheet.Model.Filtering.IFilter result = null;
				if (column.Filter == "top")
				{
					result = new global::Telerik.Windows.Documents.Spreadsheet.Model.Filtering.TopFilter((int)column.Index.Value, column.Type.ToEnum(global::Telerik.Windows.Documents.Spreadsheet.Model.Filtering.TopFilterType.TopNumber), column.Value.GetValueOrDefault());
				}
				else if (column.Filter == "dynamic")
				{
					result = new global::Telerik.Windows.Documents.Spreadsheet.Model.Filtering.DynamicFilter((int)column.Index.Value, column.Type.ToEnum(global::Telerik.Windows.Documents.Spreadsheet.Model.Filtering.DynamicFilterType.AboveAverage));
				}
				else if (column.Filter == "value")
				{
					result = new global::Telerik.Windows.Documents.Spreadsheet.Model.Filtering.ValuesCollectionFilter((int)column.Index.Value, column.Values.GetOrDefault<object>().Select((object value) => global::System.Convert.ToString(value, global::System.Globalization.CultureInfo.InvariantCulture)), column.Dates.GetOrDefault<global::Telerik.Web.Spreadsheet.ValueFilterDate>().Select((global::Telerik.Web.Spreadsheet.ValueFilterDate item) => new global::Telerik.Windows.Documents.Spreadsheet.Model.Filtering.DateGroupItem(item.Year, item.Month + 1, item.Day, item.Hours, item.Minutes, item.Seconds)), column.Blanks.GetValueOrDefault());
				}
				else if (column.Filter == "custom")
				{
					global::System.Collections.Generic.IEnumerable<global::Telerik.Windows.Documents.Spreadsheet.Model.Filtering.CustomFilterCriteria> source = column.Criteria.GetOrDefault<global::Telerik.Web.Spreadsheet.Criteria>().Select((global::Telerik.Web.Spreadsheet.Criteria criteria) => this.FromCriteria(criteria));
					global::Telerik.Windows.Documents.Spreadsheet.Model.Filtering.CustomFilterCriteria criteria3 = ((source.Count<global::Telerik.Windows.Documents.Spreadsheet.Model.Filtering.CustomFilterCriteria>() > 0) ? source.First<global::Telerik.Windows.Documents.Spreadsheet.Model.Filtering.CustomFilterCriteria>() : null);
					global::Telerik.Windows.Documents.Spreadsheet.Model.Filtering.CustomFilterCriteria criteria2 = ((source.Count<global::Telerik.Windows.Documents.Spreadsheet.Model.Filtering.CustomFilterCriteria>() > 1) ? source.Last<global::Telerik.Windows.Documents.Spreadsheet.Model.Filtering.CustomFilterCriteria>() : null);
					result = new global::Telerik.Windows.Documents.Spreadsheet.Model.Filtering.CustomFilter((int)column.Index.Value, criteria3, column.Logic.ToEnum(global::Telerik.Windows.Documents.Spreadsheet.Model.Filtering.LogicalOperator.Or), criteria2);
				}
				return result;
			}).SkipWhile((global::Telerik.Windows.Documents.Spreadsheet.Model.Filtering.IFilter item) => item == null));
		}

		private global::Telerik.Windows.Documents.Spreadsheet.Model.Filtering.CustomFilterCriteria FromCriteria(global::Telerik.Web.Spreadsheet.Criteria criteria)
		{
			string text = criteria.Operator;
			string text2 = global::System.Convert.ToString(criteria.Value, global::System.Globalization.CultureInfo.InvariantCulture);
			if (criteria.Value is string && text != null)
			{
				if (!(text == "startswith"))
				{
					if (!(text == "endswith"))
					{
						if (!(text == "contains"))
						{
							if (text == "doesnotcontain")
							{
								text = "neq";
								text2 = "*" + text2 + "*";
							}
						}
						else
						{
							text = "eq";
							text2 = "*" + text2 + "*";
						}
					}
					else
					{
						text = "eq";
						text2 = "*" + text2;
					}
				}
				else
				{
					text = "eq";
					text2 += "*";
				}
			}
			text = (this.ComparisonOperators.ContainsKey(text) ? this.ComparisonOperators[text] : "eq");
			return new global::Telerik.Windows.Documents.Spreadsheet.Model.Filtering.CustomFilterCriteria(text.ToEnum(global::Telerik.Windows.Documents.Spreadsheet.Model.ComparisonOperator.EqualsTo), text2);
		}

		static Workbook()
		{
			global::Telerik.Windows.Documents.Spreadsheet.FormatProviders.WorkbookFormatProvidersManager.RegisterFormatProvider(new global::Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.XlsxFormatProvider());
			global::Telerik.Windows.Documents.Spreadsheet.FormatProviders.WorkbookFormatProvidersManager.RegisterFormatProvider(new global::Telerik.Web.Spreadsheet.JsonFormatProvider());
		}

		public static global::Telerik.Web.Spreadsheet.Workbook Load(string path)
		{
			global::Telerik.Windows.Documents.Spreadsheet.Model.Workbook document;
			using (global::System.IO.FileStream fileStream = global::System.IO.File.Open(path, global::System.IO.FileMode.Open, global::System.IO.FileAccess.Read, global::System.IO.FileShare.ReadWrite))
			{
				document = global::Telerik.Windows.Documents.Spreadsheet.FormatProviders.WorkbookFormatProvidersManager.Import(global::System.IO.Path.GetExtension(path), fileStream);
			}
			return global::Telerik.Web.Spreadsheet.Workbook.FromDocument(document);
		}

		public static global::Telerik.Web.Spreadsheet.Workbook Load(global::System.IO.Stream input, string extension)
		{
			return global::Telerik.Web.Spreadsheet.Workbook.FromDocument(global::Telerik.Windows.Documents.Spreadsheet.FormatProviders.WorkbookFormatProvidersManager.Import(extension, input));
		}

		public void Save(string path)
		{
			global::Telerik.Windows.Documents.Spreadsheet.Model.Workbook workbook = this.ToDocument();
			using (global::System.IO.FileStream fileStream = global::System.IO.File.OpenWrite(path))
			{
				string extension = global::System.IO.Path.GetExtension(path);
				global::Telerik.Windows.Documents.Spreadsheet.FormatProviders.WorkbookFormatProvidersManager.Export(workbook, extension, fileStream);
			}
		}

		public void Save(global::System.IO.Stream output, string extension)
		{
			global::Telerik.Windows.Documents.Spreadsheet.FormatProviders.WorkbookFormatProvidersManager.Export(this.ToDocument(), extension, output);
		}

		[global::System.Runtime.Serialization.DataMember(Name = "names", EmitDefaultValue = false)]
		public global::System.Collections.Generic.List<global::Telerik.Web.Spreadsheet.NamedRange> NamedRanges { get; private set; }

		public Workbook()
		{
			this.NamedRanges = new global::System.Collections.Generic.List<global::Telerik.Web.Spreadsheet.NamedRange>();
		}

		public global::Telerik.Web.Spreadsheet.Worksheet AddSheet()
		{
			if (this.Sheets == null)
			{
				this.Sheets = new global::System.Collections.Generic.List<global::Telerik.Web.Spreadsheet.Worksheet>();
			}
			global::Telerik.Web.Spreadsheet.Worksheet worksheet = new global::Telerik.Web.Spreadsheet.Worksheet();
			this.Sheets.Add(worksheet);
			return worksheet;
		}

		internal global::System.Collections.Generic.Dictionary<string, object> Serialize()
		{
			global::System.Collections.Generic.Dictionary<string, object> dictionary = this.SerializeSettings();
			if (this.NamedRanges != null)
			{
				dictionary["names"] = this.NamedRanges.Select((global::Telerik.Web.Spreadsheet.NamedRange item) => item.Serialize());
			}
			return dictionary;
		}

		public static implicit operator global::System.Collections.Generic.Dictionary<string, object>(global::Telerik.Web.Spreadsheet.Workbook instance)
		{
			return instance.Serialize();
		}

		[global::System.Runtime.Serialization.DataMember(Name = "activeSheet", EmitDefaultValue = false)]
		public string ActiveSheet { get; set; }

		[global::System.Runtime.Serialization.DataMember(Name = "pdf", EmitDefaultValue = false)]
		public global::Telerik.Web.Spreadsheet.Pdf Pdf { get; set; }

		[global::System.Runtime.Serialization.DataMember(Name = "sheets", EmitDefaultValue = false)]
		public global::System.Collections.Generic.List<global::Telerik.Web.Spreadsheet.Worksheet> Sheets { get; set; }

		protected global::System.Collections.Generic.Dictionary<string, object> SerializeSettings()
		{
			global::System.Collections.Generic.Dictionary<string, object> dictionary = new global::System.Collections.Generic.Dictionary<string, object>();
			if (this.ActiveSheet != null)
			{
				dictionary["activeSheet"] = this.ActiveSheet;
			}
			if (this.Pdf != null)
			{
				dictionary["pdf"] = this.Pdf.Serialize();
			}
			if (this.Sheets != null)
			{
				dictionary["sheets"] = this.Sheets.Select((global::Telerik.Web.Spreadsheet.Worksheet item) => item.Serialize());
			}
			return dictionary;
		}

		private readonly global::System.Collections.Generic.Dictionary<string, string> ComparisonOperators = new global::System.Collections.Generic.Dictionary<string, string>
		{
			{ "eq", "equalsto" },
			{ "neq", "notequalsto" },
			{ "lt", "lessthan" },
			{ "gt", "greaterthan" },
			{ "gte", "greaterthanorequalsto" },
			{ "lte", "lessthanorequalsto" }
		};

		private readonly global::System.Collections.Generic.Dictionary<string, global::Telerik.Windows.Documents.Spreadsheet.Model.ComparisonOperator> ValidationComparisonOperators = new global::System.Collections.Generic.Dictionary<string, global::Telerik.Windows.Documents.Spreadsheet.Model.ComparisonOperator>
		{
			{
				"equalTo",
				global::Telerik.Windows.Documents.Spreadsheet.Model.ComparisonOperator.EqualsTo
			},
			{
				"between",
				global::Telerik.Windows.Documents.Spreadsheet.Model.ComparisonOperator.Between
			},
			{
				"greaterThan",
				global::Telerik.Windows.Documents.Spreadsheet.Model.ComparisonOperator.GreaterThan
			},
			{
				"greaterThanOrEqualTo",
				global::Telerik.Windows.Documents.Spreadsheet.Model.ComparisonOperator.GreaterThanOrEqualsTo
			},
			{
				"lessThan",
				global::Telerik.Windows.Documents.Spreadsheet.Model.ComparisonOperator.LessThan
			},
			{
				"lessThanOrEqualTo",
				global::Telerik.Windows.Documents.Spreadsheet.Model.ComparisonOperator.LessThanOrEqualsTo
			},
			{
				"notBetween",
				global::Telerik.Windows.Documents.Spreadsheet.Model.ComparisonOperator.NotBetween
			},
			{
				"notEqualTo",
				global::Telerik.Windows.Documents.Spreadsheet.Model.ComparisonOperator.NotEqualsTo
			}
		};
	}
}
