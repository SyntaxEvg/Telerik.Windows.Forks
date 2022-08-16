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
	[DataContract]
	public class Workbook
	{
		public static Workbook FromJson(string json)
		{
			return JsonConvert.DeserializeObject<Workbook>(json);
		}

		public string ToJson()
		{
			return JsonConvert.SerializeObject(this);
		}

		public static Workbook FromDocument(Workbook document)
		{
			Workbook workbook = new Workbook();
			workbook.ActiveSheet = document.ActiveSheet.Name;
			workbook.NamedRanges = document.Names.GetNamedRanges();
			foreach (Worksheet worksheet in document.Worksheets)
			{
				Worksheet worksheet2 = workbook.AddSheet();
				worksheet2.Name = worksheet.Name;
				worksheet2.ActiveCell = NameConverter.ConvertCellIndexToName(worksheet.ViewState.SelectionState.ActiveCellIndex);
				CellRange cellRange = worksheet.ViewState.SelectionState.SelectedRanges.First<CellRange>();
				worksheet2.Selection = NameConverter.ConvertCellRangeToName(cellRange.FromIndex, cellRange.ToIndex);
				worksheet2.Columns = worksheet.ImportColumns().ToList<Column>();
				worksheet2.Rows = worksheet.ImportRows().ToList<Row>();
				worksheet2.MergedCells = Workbook.GetMergedCells(worksheet).ToList<string>();
				worksheet2.ShowGridLines = new bool?(worksheet.ViewState.ShowGridLines);
				Pane pane = worksheet.ViewState.Pane;
				if (pane != null && pane.State == PaneState.Frozen)
				{
					worksheet2.FrozenRows = new int?(pane.TopLeftCellIndex.RowIndex);
					worksheet2.FrozenColumns = new int?(pane.TopLeftCellIndex.ColumnIndex);
				}
				worksheet2.Sort = Workbook.GetSorting(worksheet);
				worksheet2.Filter = Workbook.GetFilters(worksheet);
			}
			return workbook;
		}

		static IEnumerable<string> GetMergedCells(Worksheet worksheet)
		{
			foreach (CellRange cellRange in worksheet.Cells.GetMergedCellRanges())
			{
				yield return NameConverter.ConvertCellRangeToName(cellRange.FromIndex, cellRange.ToIndex);
			}
			IEnumerator<CellRange> enumerator = null;
			yield break;
			yield break;
		}

		static Sort GetSorting(Worksheet worksheet)
		{
			CellRange sortRange = worksheet.SortState.SortRange;
			if (sortRange == null)
			{
				return null;
			}
			Sort sort = new Sort();
			sort.Ref = NameConverter.ConvertCellRangeToName(sortRange.FromIndex, sortRange.ToIndex);
			sort.Columns = (from cond in worksheet.SortState.SortConditions.OfType<ValuesSortCondition>()
				select new SortColumn
				{
					Ascending = new bool?(cond.SortOrder == SortOrder.Ascending),
					Index = new double?((double)cond.RelativeIndex)
				}).ToList<SortColumn>();
			return sort;
		}

		static Filter GetFilters(Worksheet worksheet)
		{
			AutoFilter filter = worksheet.Filter;
			CellRange filterRange = filter.FilterRange;
			if (filterRange == null)
			{
				return null;
			}
			Filter filter2 = new Filter();
			filter2.Ref = NameConverter.ConvertCellRangeToName(filterRange.FromIndex, filterRange.ToIndex);
			filter2.Columns = (from item in filter.Filters
				select item.ToFilterColumn()).SkipWhile((FilterColumn column) => column == null).ToList<FilterColumn>();
			return filter2;
		}

		public Workbook ToDocument()
		{
			Workbook workbook = new Workbook();
			workbook.History.IsEnabled = false;
			using (new UpdateScope(new Action(workbook.SuspendLayoutUpdate), new Action(workbook.ResumeLayoutUpdate)))
			{
				foreach (Worksheet worksheet in this.Sheets.GetOrDefault<Worksheet>())
				{
					Worksheet worksheet2 = workbook.Worksheets.Add();
					if (!string.IsNullOrEmpty(worksheet.Name))
					{
						worksheet2.Name = worksheet.Name;
					}
					if (worksheet.Name == this.ActiveSheet)
					{
						workbook.ActiveWorksheet = worksheet2;
					}
					worksheet2.ViewState.SelectionState = this.CreateSelectionState(worksheet, worksheet2);
					foreach (Row row in worksheet.Rows.GetOrDefault<Row>())
					{
						this.SetCells(row, worksheet2);
						if (row.Height != null)
						{
							double? num = row.Height;
							double value = RowsPropertyBag.HeightProperty.DefaultValue.Value;
							if (!((num.GetValueOrDefault() == value) & (num != null)))
							{
								worksheet2.Rows[row.Index.Value].SetHeight(new RowHeight(row.Height.Value, true));
							}
						}
						if (row.Hidden != null)
						{
							worksheet2.Rows[row.Index.Value].SetHidden(true);
							worksheet2.Rows[row.Index.Value].SetHeight(new RowHeight(row.Hidden.Value, true));
						}
					}
					foreach (Column column in worksheet.Columns.GetOrDefault<Column>())
					{
						if (column.Width != null)
						{
							double? num = column.Width;
							double value = ColumnsPropertyBag.WidthProperty.DefaultValue.Value;
							if (!((num.GetValueOrDefault() == value) & (num != null)))
							{
								worksheet2.Columns[column.Index.Value].SetWidth(new ColumnWidth(column.Width.Value, true));
							}
						}
						if (column.Hidden != null)
						{
							worksheet2.Columns[column.Index.Value].SetHidden(true);
							worksheet2.Columns[column.Index.Value].SetWidth(new ColumnWidth(column.Hidden.Value, true));
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
					foreach (NamedRange namedRange in this.NamedRanges)
					{
						workbook.Names.Add(namedRange.Name, string.Format("={0}", namedRange.Value), 0, 0, null, true);
					}
				}
				for (int i = 0; i < this.Sheets.Count; i++)
				{
					foreach (Row srcRow in this.Sheets[i].Rows.GetOrDefault<Row>())
					{
						this.SetCells(srcRow, workbook.Worksheets[i]);
					}
				}
			}
			workbook.History.IsEnabled = true;
			return workbook;
		}

		SelectionState CreateSelectionState(Worksheet sheet, Worksheet documentSheet)
		{
			if (sheet.Selection != null)
			{
				return new SelectionState(sheet.Selection.ToCellRange(), sheet.ActiveCell.ToCellIndex(), documentSheet.ViewState.SelectionState.Pane);
			}
			return new SelectionState();
		}

		void SetCells(Row srcRow, Worksheet documentSheet)
		{
			foreach (Cell cell in srcRow.Cells.GetOrDefault<Cell>())
			{
				string text = ((cell.Value == null) ? null : cell.Value.ToString());
				CellSelection cellSelection = documentSheet.Cells[srcRow.Index.Value, cell.Index.Value];
				CellIndex cellIndex = new CellIndex(srcRow.Index.Value, cell.Index.Value);
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
					cellSelection.SetFormat(new CellValueFormat(cell.Format));
				}
				if (!string.IsNullOrEmpty(cell.Color))
				{
					cellSelection.SetForeColor(new ThemableColor(cell.Color.ToColor()));
				}
				if (!string.IsNullOrEmpty(cell.Background))
				{
					IFill fill = PatternFill.CreateSolidFill(cell.Background.ToColor());
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
						cellSelection.SetUnderline(UnderlineType.Single);
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
					cellSelection.SetFontFamily(new ThemableFontFamily(cell.FontFamily));
				}
				if (cell.FontSize != null)
				{
					cellSelection.SetFontSize(UnitHelper.PointToDip(cell.FontSize.Value));
				}
				if (!string.IsNullOrEmpty(cell.Link))
				{
					HyperlinkInfo hyperlinkInfo = HyperlinkInfo.CreateHyperlink(cell.Link, text);
					documentSheet.Hyperlinks.Add(cellSelection.CellRanges.First<CellRange>(), hyperlinkInfo);
				}
				if (cell.Validation != null)
				{
					if (cell.Validation.DataType == "list")
					{
						cellSelection.SetDataValidationRule(new ListDataValidationRule(new ListDataValidationRuleContext(documentSheet, cellIndex)
						{
							Argument1 = string.Format("={0}", cell.Validation.From),
							ErrorAlertContent = cell.Validation.MessageTemplate,
							ErrorAlertTitle = cell.Validation.TitleTemplate,
							IgnoreBlank = cell.Validation.AllowNulls.Value,
							ErrorStyle = ((cell.Validation.Type == "reject") ? ErrorStyle.Stop : ErrorStyle.Warning)
						}));
					}
					else
					{
						NumberDataValidationRuleContext numberDataValidationRuleContext = new NumberDataValidationRuleContext(documentSheet, cellIndex);
						numberDataValidationRuleContext.Argument1 = string.Format("={0}", cell.Validation.From);
						numberDataValidationRuleContext.Argument2 = string.Format("={0}", cell.Validation.To);
						numberDataValidationRuleContext.ErrorAlertContent = cell.Validation.MessageTemplate;
						numberDataValidationRuleContext.ErrorAlertTitle = cell.Validation.TitleTemplate;
						numberDataValidationRuleContext.IgnoreBlank = cell.Validation.AllowNulls.Value;
						numberDataValidationRuleContext.ErrorStyle = ((cell.Validation.Type == "reject") ? ErrorStyle.Stop : ErrorStyle.Warning);
						string comparerType = cell.Validation.ComparerType;
						ComparisonOperator comparisonOperator = (this.ValidationComparisonOperators.ContainsKey(comparerType) ? this.ValidationComparisonOperators[comparerType] : ComparisonOperator.EqualsTo);
						numberDataValidationRuleContext.ComparisonOperator = comparisonOperator;
						if (cell.Validation.DataType == "date")
						{
							cellSelection.SetDataValidationRule(new DateDataValidationRule(numberDataValidationRuleContext));
						}
						else
						{
							cellSelection.SetDataValidationRule(new DecimalDataValidationRule(numberDataValidationRuleContext));
						}
					}
				}
			}
		}

		CellBorders CreateCellBorders(Cell cell)
		{
			CellBorders cellBorders = new CellBorders();
			if (cell.BorderTop != null)
			{
				cellBorders.Top = new CellBorder(CellBorderStyle.Thin, new ThemableColor(cell.BorderTop.Color.ToColor()));
			}
			if (cell.BorderBottom != null)
			{
				cellBorders.Bottom = new CellBorder(CellBorderStyle.Thin, new ThemableColor(cell.BorderBottom.Color.ToColor()));
			}
			if (cell.BorderLeft != null)
			{
				cellBorders.Left = new CellBorder(CellBorderStyle.Thin, new ThemableColor(cell.BorderLeft.Color.ToColor()));
			}
			if (cell.BorderRight != null)
			{
				cellBorders.Right = new CellBorder(CellBorderStyle.Thin, new ThemableColor(cell.BorderRight.Color.ToColor()));
			}
			return cellBorders;
		}

		RadHorizontalAlignment ConvertToHorizontalAlignment(string alignment)
		{
			if (alignment != null)
			{
				if (alignment == "left")
				{
					return RadHorizontalAlignment.Left;
				}
				if (alignment == "center")
				{
					return RadHorizontalAlignment.Center;
				}
				if (alignment == "right")
				{
					return RadHorizontalAlignment.Right;
				}
				if (alignment == "justify")
				{
					return RadHorizontalAlignment.Justify;
				}
			}
			return RadHorizontalAlignment.General;
		}

		void SetSortState(Worksheet documentWorksheet, Sort sort)
		{
			if (sort == null)
			{
				return;
			}
			ValuesSortCondition[] array = (from column in sort.Columns
				select new ValuesSortCondition((int)column.Index.Value, column.Ascending.Value ? SortOrder.Ascending : SortOrder.Descending)).ToArray<ValuesSortCondition>();
			CellRange cellRange = sort.Ref.ToCellRange().First<CellRange>();
			SortState sortState = documentWorksheet.SortState;
			CellRange sortRange = cellRange;
			ISortCondition[] sortConditions = array;
			sortState.Set(sortRange, sortConditions);
		}

		T ToEnum<T>(string value)
		{
			return (T)((object)Enum.Parse(typeof(T), value));
		}

		void SetFilterState(Worksheet documentWorksheet, Filter filter)
		{
			if (filter == null)
			{
				return;
			}
			documentWorksheet.Filter.FilterRange = filter.Ref.ToCellRange().First<CellRange>();
			documentWorksheet.Filter.SetFilters(filter.Columns.Select(delegate(FilterColumn column)
			{
				IFilter result = null;
				if (column.Filter == "top")
				{
					result = new TopFilter((int)column.Index.Value, column.Type.ToEnum(TopFilterType.TopNumber), column.Value.GetValueOrDefault());
				}
				else if (column.Filter == "dynamic")
				{
					result = new DynamicFilter((int)column.Index.Value, column.Type.ToEnum(DynamicFilterType.AboveAverage));
				}
				else if (column.Filter == "value")
				{
					result = new ValuesCollectionFilter((int)column.Index.Value, from value in column.Values.GetOrDefault<object>()
						select Convert.ToString(value, CultureInfo.InvariantCulture), column.Dates.GetOrDefault<ValueFilterDate>().Select((ValueFilterDate item) => new DateGroupItem(item.Year, item.Month + 1, item.Day, item.Hours, item.Minutes, item.Seconds)), column.Blanks.GetValueOrDefault());
				}
				else if (column.Filter == "custom")
				{
					IEnumerable<CustomFilterCriteria> source = from criteria in column.Criteria.GetOrDefault<Criteria>()
						select this.FromCriteria(criteria);
					CustomFilterCriteria criteria3 = ((source.Count<CustomFilterCriteria>() > 0) ? source.First<CustomFilterCriteria>() : null);
					CustomFilterCriteria criteria2 = ((source.Count<CustomFilterCriteria>() > 1) ? source.Last<CustomFilterCriteria>() : null);
					result = new CustomFilter((int)column.Index.Value, criteria3, column.Logic.ToEnum(LogicalOperator.Or), criteria2);
				}
				return result;
			}).SkipWhile((IFilter item) => item == null));
		}

		CustomFilterCriteria FromCriteria(Criteria criteria)
		{
			string text = criteria.Operator;
			string text2 = Convert.ToString(criteria.Value, CultureInfo.InvariantCulture);
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
			return new CustomFilterCriteria(text.ToEnum(ComparisonOperator.EqualsTo), text2);
		}

		static Workbook()
		{
			WorkbookFormatProvidersManager.RegisterFormatProvider(new XlsxFormatProvider());
			WorkbookFormatProvidersManager.RegisterFormatProvider(new JsonFormatProvider());
		}

		public static Workbook Load(string path)
		{
			Workbook document;
			using (FileStream fileStream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
			{
				document = WorkbookFormatProvidersManager.Import(Path.GetExtension(path), fileStream);
			}
			return Workbook.FromDocument(document);
		}

		public static Workbook Load(Stream input, string extension)
		{
			return Workbook.FromDocument(WorkbookFormatProvidersManager.Import(extension, input));
		}

		public void Save(string path)
		{
			Workbook workbook = this.ToDocument();
			using (FileStream fileStream = File.OpenWrite(path))
			{
				string extension = Path.GetExtension(path);
				WorkbookFormatProvidersManager.Export(workbook, extension, fileStream);
			}
		}

		public void Save(Stream output, string extension)
		{
			WorkbookFormatProvidersManager.Export(this.ToDocument(), extension, output);
		}

		[DataMember(Name = "names", EmitDefaultValue = false)]
		public List<NamedRange> NamedRanges { get; set; }

		public Workbook()
		{
			this.NamedRanges = new List<NamedRange>();
		}

		public Worksheet AddSheet()
		{
			if (this.Sheets == null)
			{
				this.Sheets = new List<Worksheet>();
			}
			Worksheet worksheet = new Worksheet();
			this.Sheets.Add(worksheet);
			return worksheet;
		}

		internal Dictionary<string, object> Serialize()
		{
			Dictionary<string, object> dictionary = this.SerializeSettings();
			if (this.NamedRanges != null)
			{
				dictionary["names"] = from item in this.NamedRanges
					select item.Serialize();
			}
			return dictionary;
		}

		public static implicit operator Dictionary<string, object>(Workbook instance)
		{
			return instance.Serialize();
		}

		[DataMember(Name = "activeSheet", EmitDefaultValue = false)]
		public string ActiveSheet { get; set; }

		[DataMember(Name = "pdf", EmitDefaultValue = false)]
		public Pdf Pdf { get; set; }

		[DataMember(Name = "sheets", EmitDefaultValue = false)]
		public List<Worksheet> Sheets { get; set; }

		protected Dictionary<string, object> SerializeSettings()
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
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
				dictionary["sheets"] = from item in this.Sheets
					select item.Serialize();
			}
			return dictionary;
		}

		readonly Dictionary<string, string> ComparisonOperators = new Dictionary<string, string>
		{
			{ "eq", "equalsto" },
			{ "neq", "notequalsto" },
			{ "lt", "lessthan" },
			{ "gt", "greaterthan" },
			{ "gte", "greaterthanorequalsto" },
			{ "lte", "lessthanorequalsto" }
		};

		readonly Dictionary<string, ComparisonOperator> ValidationComparisonOperators = new Dictionary<string, ComparisonOperator>
		{
			{
				"equalTo",
				ComparisonOperator.EqualsTo
			},
			{
				"between",
				ComparisonOperator.Between
			},
			{
				"greaterThan",
				ComparisonOperator.GreaterThan
			},
			{
				"greaterThanOrEqualTo",
				ComparisonOperator.GreaterThanOrEqualsTo
			},
			{
				"lessThan",
				ComparisonOperator.LessThan
			},
			{
				"lessThanOrEqualTo",
				ComparisonOperator.LessThanOrEqualsTo
			},
			{
				"notBetween",
				ComparisonOperator.NotBetween
			},
			{
				"notEqualTo",
				ComparisonOperator.NotEqualsTo
			}
		};
	}
}
