using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands;
using Telerik.Windows.Documents.Spreadsheet.Core;
using Telerik.Windows.Documents.Spreadsheet.Core.DataStructures;
using Telerik.Windows.Documents.Spreadsheet.DataSeries;
using Telerik.Windows.Documents.Spreadsheet.DataSeries.AutoFill;
using Telerik.Windows.Documents.Spreadsheet.Expressions;
using Telerik.Windows.Documents.Spreadsheet.Formatting;
using Telerik.Windows.Documents.Spreadsheet.Formatting.FormatStrings;
using Telerik.Windows.Documents.Spreadsheet.History;
using Telerik.Windows.Documents.Spreadsheet.Model.DataValidation;
using Telerik.Windows.Documents.Spreadsheet.Model.Filtering;
using Telerik.Windows.Documents.Spreadsheet.Model.Shapes;
using Telerik.Windows.Documents.Spreadsheet.Model.Sorting;
using Telerik.Windows.Documents.Spreadsheet.PropertySystem;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model
{
	public class CellSelection : SelectionBase
	{
		internal override WorksheetEntityBase WorksheetEntity
		{
			get
			{
				return this.Cells;
			}
		}

		Cells Cells
		{
			get
			{
				return base.Worksheet.Cells;
			}
		}

		internal CellSelection(Worksheet worksheet, IEnumerable<CellRange> cellRanges)
			: base(worksheet, cellRanges)
		{
		}

		public override void SetBorders(CellBorders value)
		{
			base.ExecuteForEachRangeInsideBeginEndUpdate(delegate(CellRange cellRange)
			{
				SetCellBordersCommandContext context = new SetCellBordersCommandContext(this.Worksheet, cellRange, value);
				this.Worksheet.ExecuteCommand<SetCellBordersCommandContext>(WorkbookCommands.SetCellBorders, context);
			});
		}

		internal override void PreserveOldStylePropertiesAsLocalIfNeeded<T>(CellRange cellRange, IProperty<T> property, T value)
		{
			if (property.PropertyDefinition == CellPropertyDefinitions.StyleNameProperty)
			{
				ISheet worksheet = base.Worksheet;
				using (new UpdateScope(new Action(worksheet.SuspendLayoutUpdate), new Action(worksheet.ResumeLayoutUpdate)))
				{
					CellStyle newStyle = base.Worksheet.Workbook.Styles.GetByName(value as string);
					Action<CellSelection> action = delegate(CellSelection selection)
					{
						this.PreserveOldStylePropertiyAsLocalIfNeeded<CellBorder>(newStyle, selection, selection.Cells.BottomBorderProperty, CellPropertyDefinitions.BottomBorderProperty);
						this.PreserveOldStylePropertiyAsLocalIfNeeded<CellBorder>(newStyle, selection, selection.Cells.DiagonalDownBorderProperty, CellPropertyDefinitions.DiagonalDownBorderProperty);
						this.PreserveOldStylePropertiyAsLocalIfNeeded<CellBorder>(newStyle, selection, selection.Cells.DiagonalUpBorderProperty, CellPropertyDefinitions.DiagonalUpBorderProperty);
						this.PreserveOldStylePropertiyAsLocalIfNeeded<IFill>(newStyle, selection, selection.Cells.FillProperty, CellPropertyDefinitions.FillProperty);
						this.PreserveOldStylePropertiyAsLocalIfNeeded<ThemableFontFamily>(newStyle, selection, selection.Cells.FontFamilyProperty, CellPropertyDefinitions.FontFamilyProperty);
						this.PreserveOldStylePropertiyAsLocalIfNeeded<ThemableFontFamily>(newStyle, selection, selection.Cells.FontFamilyProperty, CellPropertyDefinitions.FontFamilyProperty);
						this.PreserveOldStylePropertiyAsLocalIfNeeded<double>(newStyle, selection, selection.Cells.FontSizeProperty, CellPropertyDefinitions.FontSizeProperty);
						this.PreserveOldStylePropertiyAsLocalIfNeeded<ThemableColor>(newStyle, selection, selection.Cells.ForeColorProperty, CellPropertyDefinitions.ForeColorProperty);
						this.PreserveOldStylePropertiyAsLocalIfNeeded<CellValueFormat>(newStyle, selection, selection.Cells.FormatProperty, CellPropertyDefinitions.FormatProperty);
						this.PreserveOldStylePropertiyAsLocalIfNeeded<RadHorizontalAlignment>(newStyle, selection, selection.Cells.HorizontalAlignmentProperty, CellPropertyDefinitions.HorizontalAlignmentProperty);
						this.PreserveOldStylePropertiyAsLocalIfNeeded<int>(newStyle, selection, selection.Cells.IndentProperty, CellPropertyDefinitions.IndentProperty);
						this.PreserveOldStylePropertiyAsLocalIfNeeded<bool>(newStyle, selection, selection.Cells.IsBoldProperty, CellPropertyDefinitions.IsBoldProperty);
						this.PreserveOldStylePropertiyAsLocalIfNeeded<bool>(newStyle, selection, selection.Cells.IsItalicProperty, CellPropertyDefinitions.IsItalicProperty);
						this.PreserveOldStylePropertiyAsLocalIfNeeded<bool>(newStyle, selection, selection.Cells.IsWrappedProperty, CellPropertyDefinitions.IsWrappedProperty);
						this.PreserveOldStylePropertiyAsLocalIfNeeded<CellBorder>(newStyle, selection, selection.Cells.LeftBorderProperty, CellPropertyDefinitions.LeftBorderProperty);
						this.PreserveOldStylePropertiyAsLocalIfNeeded<CellBorder>(newStyle, selection, selection.Cells.RightBorderProperty, CellPropertyDefinitions.RightBorderProperty);
						this.PreserveOldStylePropertiyAsLocalIfNeeded<CellBorder>(newStyle, selection, selection.Cells.TopBorderProperty, CellPropertyDefinitions.TopBorderProperty);
						this.PreserveOldStylePropertiyAsLocalIfNeeded<UnderlineType>(newStyle, selection, selection.Cells.UnderlineProperty, CellPropertyDefinitions.UnderlineProperty);
						this.PreserveOldStylePropertiyAsLocalIfNeeded<RadVerticalAlignment>(newStyle, selection, selection.Cells.VerticalAlignmentProperty, CellPropertyDefinitions.VerticalAlignmentProperty);
					};
					RangePropertyValue<string> styleName = base.GetStyleName();
					if (styleName.IsIndeterminate)
					{
						ICompressedList<string> propertyValueCollection = this.Cells[cellRange].Cells.PropertyBag.GetPropertyValueCollection<string>(CellPropertyDefinitions.StyleNameProperty);
						using (IEnumerator<Range<long, string>> enumerator = propertyValueCollection.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								Range<long, string> range = enumerator.Current;
								CellRange cellRange2 = WorksheetPropertyBagBase.ConvertLongCellRangeToCellRange(range.Start, range.End);
								if (cellRange.IntersectsWith(cellRange2))
								{
									cellRange2 = cellRange.Intersect(cellRange2);
									action(base.Worksheet.Cells[cellRange2]);
								}
							}
							goto IL_121;
						}
					}
					action(this);
					IL_121:;
				}
			}
		}

		bool ValidateSelection(IEnumerable<CellRange> cellRanges)
		{
			int rowCount = base.CellRanges.First<CellRange>().RowCount;
			int rowIndex = base.CellRanges.First<CellRange>().GetFirstRow().FromIndex.RowIndex;
			int columnCount = base.CellRanges.First<CellRange>().ColumnCount;
			int columnIndex = base.CellRanges.First<CellRange>().GetFirstColumn().FromIndex.ColumnIndex;
			bool flag = true;
			bool flag2 = true;
			if (cellRanges.First<CellRange>().ColumnCount != cellRanges.Last<CellRange>().ColumnCount)
			{
				flag2 = false;
			}
			if (cellRanges.First<CellRange>().RowCount != cellRanges.Last<CellRange>().RowCount)
			{
				flag = false;
			}
			if (!flag2 && !flag)
			{
				return false;
			}
			foreach (CellRange cellRange in base.CellRanges)
			{
				if (flag2 && (cellRange.ColumnCount != columnCount || cellRange.GetFirstColumn().FromIndex.ColumnIndex != columnIndex))
				{
					return false;
				}
				if (flag && (cellRange.RowCount != rowCount || cellRange.GetFirstRow().FromIndex.RowIndex != rowIndex))
				{
					return false;
				}
			}
			return true;
		}

		public RangePropertyValue<ICellValue> GetValue()
		{
			return base.GetPropertyValue<ICellValue>(this.Cells.ValueProperty);
		}

		public void SetValue(DateTime value)
		{
			this.SetValue(value.ToCellValue());
			CellValueFormat value2 = base.GetFormat().Value;
			bool flag = value2 == CellPropertyDefinitions.FormatProperty.DefaultValue;
			if (flag)
			{
				base.SetFormat(CellSelection.shortDateFormat);
			}
		}

		internal void SetValueAndExpandToFitNumberValuesWidth(DateTime value)
		{
			base.ExecuteAndExpandToFitNumberValuesWidth(delegate
			{
				this.SetValue(value);
			});
		}

		public void SetValue(double value)
		{
			this.SetValue(value.ToCellValue());
		}

		internal void SetValueAndExpandToFitNumberValuesWidth(double value)
		{
			base.ExecuteAndExpandToFitNumberValuesWidth(delegate
			{
				this.SetValue(value);
			});
		}

		public void SetValue(bool value)
		{
			this.SetValue(value.ToCellValue());
		}

		internal void SetValueAndExpandToFitNumberValuesWidth(bool value)
		{
			base.ExecuteAndExpandToFitNumberValuesWidth(delegate
			{
				this.SetValue(value);
			});
		}

		public void SetValue(string value)
		{
			ICellValue value2;
			CellValueFormat format;
			this.CreateValue(value, out value2, out format);
			this.SetCellValueInternal(value2, format);
		}

		internal void SetValueAndExpandToFitNumberValuesWidth(string value)
		{
			base.ExecuteAndExpandToFitNumberValuesWidth(delegate
			{
				this.SetValue(value);
			});
		}

		internal void SetValueIgnoreErrorsInternal(string value, bool shouldExpandToFitNumberValuesWidth)
		{
			ICellValue value2;
			CellValueFormat format;
			this.CreateValueIgnoreErrors(value, out value2, out format);
			this.SetCellValueInternal(value2, format);
			if (shouldExpandToFitNumberValuesWidth)
			{
				base.Worksheet.Columns[base.CellRanges].ExpandToFitNumberValuesWidth();
			}
		}

		public void SetValueAsText(string value)
		{
			Guard.ThrowExceptionIfNull<string>(value, "value");
			this.SetValue(value.ToTextCellValue());
		}

		public void SetValueAsFormula(string value)
		{
			Guard.ThrowExceptionIfNullOrEmpty(value, "value");
			CellIndex topLeftCellIndex = base.GetTopLeftCellIndex();
			this.SetValue(value.ToFormulaCellValue(base.Worksheet, topLeftCellIndex.RowIndex, topLeftCellIndex.ColumnIndex));
		}

		public void SetValue(ICellValue value)
		{
			Guard.ThrowExceptionIfNull<ICellValue>(value, "value");
			this.SetCellValueInternal(value, CellValueFormat.GeneralFormat);
		}

		internal void SetValueAndExpandToFitNumberValuesWidth(ICellValue value)
		{
			base.ExecuteAndExpandToFitNumberValuesWidth(delegate
			{
				this.SetValue(value);
			});
		}

		void SetCellValueInternal(ICellValue value, CellValueFormat format)
		{
			Guard.ThrowExceptionIfNull<ICellValue>(value, "value");
			FormulaCellValue formulaCellValue = value as FormulaCellValue;
			if (formulaCellValue != null)
			{
				this.SetFormulaValueInternal(formulaCellValue);
				return;
			}
			this.SetValueInternal(value, format);
		}

		void SetFormulaValueInternal(FormulaCellValue value)
		{
			FunctionExpression functionExpression = value.Value as FunctionExpression;
			CellValueFormat newFormat = CellValueFormat.GeneralFormat;
			if (functionExpression != null && functionExpression.IsValid)
			{
				newFormat = functionExpression.Function.FunctionInfo.Format;
			}
			base.ExecuteForEachRangeInsideBeginEndUpdate(delegate(CellRange range)
			{
				for (int i = range.FromIndex.RowIndex; i <= range.ToIndex.RowIndex; i++)
				{
					for (int j = range.FromIndex.ColumnIndex; j <= range.ToIndex.ColumnIndex; j++)
					{
						this.Cells[i, j].SetValueInternal(value.CloneAndTranslate(this.Worksheet, i, j, false), newFormat);
					}
				}
			});
		}

		internal void SetValueInternal(ICellValue cellValue, CellValueFormat newFormat)
		{
			using (new UpdateScope(new Action(base.BeginUpdate), new Action(base.EndUpdate)))
			{
				if (SpreadsheetHelper.IsHyperlinkFormula(cellValue))
				{
					base.SetStyleName(SpreadsheetDefaultValues.HyperlinkStyleName);
				}
				if (!TelerikHelper.EqualsOfT<CellValueFormat>(newFormat, CellValueFormat.GeneralFormat))
				{
					RangePropertyValue<CellValueFormat> format = base.GetFormat();
					CellValueFormat value = format.Value;
					if (TelerikHelper.EqualsOfT<CellValueFormat>(value, CellValueFormat.GeneralFormat) && !format.IsIndeterminate)
					{
						base.SetFormat(newFormat);
					}
					else
					{
						this.UpdateCellsFormat(newFormat);
					}
				}
				base.SetPropertyValue<ICellValue>(this.Cells.ValueProperty, cellValue);
			}
		}

		void UpdateCellsFormat(CellValueFormat newFormat)
		{
			using (new UpdateScope(new Action(base.BeginUpdate), new Action(base.EndUpdate)))
			{
				base.ExecuteForEachRangeInsideBeginEndUpdate(delegate(CellRange cellRange)
				{
					UpdateCellPropertyCommandContext<CellValueFormat> context = new UpdateCellPropertyCommandContext<CellValueFormat>(this.Worksheet, CellPropertyDefinitions.FormatProperty, cellRange, delegate(CellValueFormat value)
					{
						bool flag = value == CellValueFormat.GeneralFormat;
						bool flag2 = value.FormatStringInfo.Category == FormatStringCategory.Text && newFormat.FormatStringInfo.Category != FormatStringCategory.General && newFormat.FormatStringInfo.Category != FormatStringCategory.Number;
						if (flag || flag2)
						{
							return newFormat;
						}
						return value;
					});
					this.Worksheet.ExecuteCommand<UpdateCellPropertyCommandContext<CellValueFormat>>(WorkbookCommands.UpdateCellFormat, context);
				});
			}
		}

		void CreateValue(string value, out ICellValue cellValue, out CellValueFormat newFormat)
		{
			CellValueFormat value2 = base.GetFormat().Value;
			CellIndex topLeftCellIndex = base.GetTopLeftCellIndex();
			CellValueFactory.Create(value, base.Worksheet, topLeftCellIndex.RowIndex, topLeftCellIndex.ColumnIndex, value2, out cellValue, out newFormat);
		}

		internal void CreateValueIgnoreErrors(string value, out ICellValue cellValue, out CellValueFormat newFormat)
		{
			CellValueFormat value2 = base.GetFormat().Value;
			CellIndex topLeftCellIndex = base.GetTopLeftCellIndex();
			CellValueFactory.CreateIgnoreErrors(value, base.Worksheet, topLeftCellIndex.RowIndex, topLeftCellIndex.ColumnIndex, value2, out cellValue, out newFormat);
		}

		void CreateValueIgnoreErrorsAndFormat(string value, out ICellValue cellValue, out CellValueFormat newFormat)
		{
			CellIndex topLeftCellIndex = base.GetTopLeftCellIndex();
			CellValueFactory.CreateIgnoreErrors(value, base.Worksheet, topLeftCellIndex.RowIndex, topLeftCellIndex.ColumnIndex, CellValueFormat.GeneralFormat, out cellValue, out newFormat);
		}

		internal void CreateValueIgnoreFormat(string value, out ICellValue cellValue, out CellValueFormat newFormat)
		{
			CellValueFormat generalFormat = CellValueFormat.GeneralFormat;
			CellIndex topLeftCellIndex = base.GetTopLeftCellIndex();
			CellValueFactory.Create(value, base.Worksheet, topLeftCellIndex.RowIndex, topLeftCellIndex.ColumnIndex, generalFormat, out cellValue, out newFormat);
		}

		public void ClearValue()
		{
			base.ClearPropertyValue(this.Cells.ValueProperty, true);
		}

		internal override void Clear(ClearType type, bool clearIsLocked)
		{
			using (new UpdateScope(new Action(base.BeginUpdate), new Action(base.EndUpdate)))
			{
				if (type == ClearType.All)
				{
					foreach (IProperty property in this.Cells.Properties)
					{
						if (property.PropertyDefinition != CellPropertyDefinitions.IsLockedProperty || clearIsLocked)
						{
							foreach (CellRange cellRange in base.CellRanges)
							{
								IProperty property2;
								if (cellRange.ColumnCount == SpreadsheetDefaultValues.ColumnCount && base.Worksheet.Rows.TryGetProperyFromPropertyDefinition(property.PropertyDefinition, out property2))
								{
									base.Worksheet.Rows[cellRange.FromIndex.RowIndex, cellRange.ToIndex.RowIndex].ClearPropertyValue(property2, true);
								}
								IProperty property3;
								if (cellRange.RowCount == SpreadsheetDefaultValues.RowCount && base.Worksheet.Columns.TryGetProperyFromPropertyDefinition(property.PropertyDefinition, out property3))
								{
									base.Worksheet.Columns[cellRange.FromIndex.ColumnIndex, cellRange.ToIndex.ColumnIndex].ClearPropertyValue(property3, true);
								}
							}
							base.ClearPropertyValue(property, true);
						}
					}
					this.ClearHyperlinks();
				}
				else
				{
					if (type == ClearType.Formats)
					{
						using (IEnumerator<IProperty> enumerator3 = this.Cells.Properties.GetEnumerator())
						{
							while (enumerator3.MoveNext())
							{
								IProperty property4 = enumerator3.Current;
								if (property4 != this.Cells.ValueProperty)
								{
									base.ClearPropertyValue(property4, true);
								}
							}
							goto IL_1D1;
						}
					}
					if (type == ClearType.Contents)
					{
						this.ClearValue();
						this.ClearHyperlinks();
					}
					else
					{
						if (type != ClearType.Hyperlinks)
						{
							throw new InvalidOperationException();
						}
						this.ClearHyperlinks();
					}
				}
				IL_1D1:;
			}
		}

		internal override void ClearBySettingDefaultValuesOnRowColumnPropertyIntersection(CellRange cellRange, IProperty property)
		{
			Action<RowsColumnsBase, PropertyBagBase, Func<int, int, CellRange>> action = delegate(RowsColumnsBase rowsColumns, PropertyBagBase propertyBag, Func<int, int, CellRange> createRowColumnRangeAction)
			{
				IProperty property2;
				if (rowsColumns.TryGetProperyFromPropertyDefinition(property.PropertyDefinition, out property2))
				{
					ICompressedList propertyValueCollection = propertyBag.GetPropertyValueCollection(property2.PropertyDefinition);
					foreach (LongRange longRange in propertyValueCollection.GetRanges(false))
					{
						CellRange other = createRowColumnRangeAction((int)longRange.Start, (int)longRange.End);
						if (cellRange.IntersectsWith(other))
						{
							CellRange defaultValue = cellRange.Intersect(other);
							property.SetDefaultValue(defaultValue);
						}
					}
				}
			};
			action(base.Worksheet.Rows, base.Worksheet.Rows.PropertyBag, new Func<int, int, CellRange>(CellRange.FromRowRange));
			action(base.Worksheet.Columns, base.Worksheet.Columns.PropertyBag, new Func<int, int, CellRange>(CellRange.FromColumnRange));
		}

		void ClearHyperlinks()
		{
			base.Worksheet.Hyperlinks.RemoveRange(base.Worksheet.Hyperlinks.GetContainingHyperlinks(base.CellRanges), false);
		}

		public bool Merge()
		{
			if (base.CellRanges.ContainsOverlappingRanges())
			{
				return false;
			}
			using (new UpdateScope(new Action(this.Cells.MergedCellRanges.BeginUpdate), new Action(this.Cells.MergedCellRanges.EndUpdate)))
			{
				base.ExecuteForEachRangeInsideBeginEndUpdate(delegate(CellRange cellRange)
				{
					this.MergeInternal(cellRange);
				});
			}
			return true;
		}

		void MergeInternal(CellRange cellRange)
		{
			this.PrepareForMerge(cellRange);
			MergeCellsCommandContext context = new MergeCellsCommandContext(base.Worksheet, cellRange);
			base.Worksheet.ExecuteCommand<MergeCellsCommandContext>(WorkbookCommands.MergeCells, context);
		}

		void PrepareForMerge(CellRange cellRange)
		{
			CellSelection sourceCells = this.Cells[cellRange.FromIndex];
			CellSelection.CellPropertiesValues formatProperties = this.GetFormatProperties(sourceCells);
			foreach (CellRange cellRange2 in cellRange.SplitToRangesSurroundingRange(cellRange.FromIndex.ToCellRange()))
			{
				CellSelection cellSelection = this.Cells[cellRange2];
				using (new UpdateScope(new Action(cellSelection.BeginUpdate), new Action(cellSelection.EndUpdate)))
				{
					cellSelection.Clear(ClearType.Contents);
					this.SetFormatProperties(formatProperties, cellSelection);
				}
			}
		}

		CellSelection.CellPropertiesValues GetFormatProperties(CellSelection sourceCells)
		{
			return new CellSelection.CellPropertiesValues
			{
				Borders = sourceCells.GetBorders(),
				Fill = sourceCells.GetFill().Value,
				FontFamily = sourceCells.GetFontFamily().Value,
				FontSize = sourceCells.GetFontSize().Value,
				ForeColor = sourceCells.GetForeColor().Value,
				Format = sourceCells.GetFormat().Value,
				HorizontalAlignment = sourceCells.GetHorizontalAlignment().Value,
				Indent = sourceCells.GetIndent().Value,
				IsBold = sourceCells.GetIsBold().Value,
				IsItalic = sourceCells.GetIsItalic().Value,
				IsWrapped = sourceCells.GetIsWrapped().Value,
				StyleName = sourceCells.GetStyleName().Value,
				Underline = sourceCells.GetUnderline().Value,
				VerticalAlignment = sourceCells.GetVerticalAlignment().Value
			};
		}

		void SetFormatProperties(CellSelection.CellPropertiesValues values, CellSelection destinationCells)
		{
			this.SetFormatPropertyIfNeeded<string>(destinationCells, CellPropertyDefinitions.StyleNameProperty, values.StyleName);
			this.SetFormatPropertyIfNeeded<CellBorder>(destinationCells, CellPropertyDefinitions.BottomBorderProperty, values.Borders.Bottom);
			this.SetFormatPropertyIfNeeded<CellBorder>(destinationCells, CellPropertyDefinitions.DiagonalDownBorderProperty, values.Borders.DiagonalDown);
			this.SetFormatPropertyIfNeeded<CellBorder>(destinationCells, CellPropertyDefinitions.DiagonalUpBorderProperty, values.Borders.DiagonalUp);
			this.SetFormatPropertyIfNeeded<CellBorder>(destinationCells, CellPropertyDefinitions.LeftBorderProperty, values.Borders.Left);
			this.SetFormatPropertyIfNeeded<CellBorder>(destinationCells, CellPropertyDefinitions.RightBorderProperty, values.Borders.Right);
			this.SetFormatPropertyIfNeeded<CellBorder>(destinationCells, CellPropertyDefinitions.TopBorderProperty, values.Borders.Top);
			this.SetFormatPropertyIfNeeded<IFill>(destinationCells, CellPropertyDefinitions.FillProperty, values.Fill);
			this.SetFormatPropertyIfNeeded<ThemableFontFamily>(destinationCells, CellPropertyDefinitions.FontFamilyProperty, values.FontFamily);
			this.SetFormatPropertyIfNeeded<double>(destinationCells, CellPropertyDefinitions.FontSizeProperty, values.FontSize);
			this.SetFormatPropertyIfNeeded<ThemableColor>(destinationCells, CellPropertyDefinitions.ForeColorProperty, values.ForeColor);
			this.SetFormatPropertyIfNeeded<CellValueFormat>(destinationCells, CellPropertyDefinitions.FormatProperty, values.Format);
			this.SetFormatPropertyIfNeeded<int>(destinationCells, CellPropertyDefinitions.IndentProperty, values.Indent);
			this.SetFormatPropertyIfNeeded<RadHorizontalAlignment>(destinationCells, CellPropertyDefinitions.HorizontalAlignmentProperty, values.HorizontalAlignment);
			this.SetFormatPropertyIfNeeded<bool>(destinationCells, CellPropertyDefinitions.IsBoldProperty, values.IsBold);
			this.SetFormatPropertyIfNeeded<bool>(destinationCells, CellPropertyDefinitions.IsItalicProperty, values.IsItalic);
			this.SetFormatPropertyIfNeeded<bool>(destinationCells, CellPropertyDefinitions.IsWrappedProperty, values.IsWrapped);
			this.SetFormatPropertyIfNeeded<UnderlineType>(destinationCells, CellPropertyDefinitions.UnderlineProperty, values.Underline);
			this.SetFormatPropertyIfNeeded<RadVerticalAlignment>(destinationCells, CellPropertyDefinitions.VerticalAlignmentProperty, values.VerticalAlignment);
			destinationCells.Worksheet.Columns[destinationCells.CellRanges].ExpandToFitNumberValuesWidth();
		}

		void SetFormatPropertyIfNeeded<T>(CellSelection destinationCells, IPropertyDefinition<T> propertyDefinition, T value)
		{
			IProperty<T> property;
			this.Cells.TryGetProperyFromPropertyDefinition<T>(propertyDefinition, out property);
			if (TelerikHelper.EqualsOfT<T>(value, propertyDefinition.DefaultValue))
			{
				ICompressedList<T> propertyValueCollection = this.Cells.PropertyBag.GetPropertyValueCollection<T>(propertyDefinition);
				using (IEnumerator<CellRange> enumerator = destinationCells.CellRanges.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						CellRange cellRange = enumerator.Current;
						foreach (LongRange longRange in WorksheetPropertyBagBase.ConvertCellRangeToLongRanges(cellRange))
						{
							ICompressedList<T> value2 = propertyValueCollection.GetValue(longRange.Start, longRange.End);
							foreach (Range<long, T> range in value2.GetNonDefaultRanges())
							{
								CellRange cellRange2 = WorksheetPropertyBagBase.ConvertLongCellRangeToCellRange(range.Start, range.End);
								property.ClearValue(cellRange2);
							}
						}
					}
					return;
				}
			}
			destinationCells.SetPropertyValue<T>(property, value);
		}

		public bool MergeAcross()
		{
			if (base.CellRanges.ContainsOverlappingRanges())
			{
				return false;
			}
			base.ExecuteForEachRangeInsideBeginEndUpdate(delegate(CellRange cellRange)
			{
				for (int i = cellRange.FromIndex.RowIndex; i <= cellRange.ToIndex.RowIndex; i++)
				{
					CellRange cellRange2 = new CellRange(i, cellRange.FromIndex.ColumnIndex, i, cellRange.ToIndex.ColumnIndex);
					this.PrepareForMerge(cellRange2);
					MergeCellsCommandContext context = new MergeCellsCommandContext(base.Worksheet, cellRange2);
					base.Worksheet.ExecuteCommand<MergeCellsCommandContext>(WorkbookCommands.MergeCells, context);
				}
			});
			return true;
		}

		public void Unmerge()
		{
			base.ExecuteForEachRangeInsideBeginEndUpdate(delegate(CellRange cellRange)
			{
				this.UnmergeInternal(cellRange);
			});
		}

		void UnmergeInternal(CellRange cellRange)
		{
			UnmergeCellsCommandContext context = new UnmergeCellsCommandContext(base.Worksheet, cellRange);
			base.Worksheet.ExecuteCommand<UnmergeCellsCommandContext>(WorkbookCommands.UnmergeCells, context);
		}

		public WorksheetFragment Copy()
		{
			return this.CopyInternal().Fragment;
		}

		internal CopyResult CopyInternal()
		{
			bool flag = true;
			string errorMessage = string.Empty;
			string errorMessageLocalizationKey = string.Empty;
			WorksheetFragment fragment = null;
			if (base.CellRanges.Count<CellRange>() > 1)
			{
				flag = false;
				errorMessage = "That command cannot be used on multiple selections.";
				errorMessageLocalizationKey = "Spreadsheet_MessageBox_CommandCannotBeUsed";
				fragment = null;
			}
			if (this.Cells.MergedCellRanges.MergedCellIsSplitByHiddenRowColumn(base.CellRanges.First<CellRange>()))
			{
				flag = false;
				errorMessage = "This cannot be done to a merged cell.";
				fragment = null;
			}
			if (flag)
			{
				CellRange cellRange = this.Cells.MergedCellRanges.ExpandRangeToNotIntersectMergedCells(base.CellRanges.First<CellRange>());
				fragment = new WorksheetFragment(cellRange, null, base.Worksheet, false, true);
			}
			return new CopyResult(flag, fragment, errorMessage, errorMessageLocalizationKey);
		}

		public bool CanCopy(WorksheetFragment fragment)
		{
			return fragment != null;
		}

		public bool Paste(WorksheetFragment fragment, PasteOptions pasteOptions)
		{
			IEnumerable<CellRange> enumerable;
			IEnumerable<FloatingShapeBase> enumerable2;
			return this.Paste(fragment, pasteOptions, out enumerable, out enumerable2);
		}

		public bool Paste(WorksheetFragment fragment, PasteOptions pasteOptions, out IEnumerable<FloatingShapeBase> newShapes)
		{
			IEnumerable<CellRange> enumerable;
			return this.Paste(fragment, pasteOptions, out enumerable, out newShapes);
		}

		public PasteResult CanPaste(WorksheetFragment fragment)
		{
			bool success = true;
			string errorMessage = string.Empty;
			string errorMessageLocalizationKey = string.Empty;
			bool shouldDisableHistory = false;
			if (fragment == null)
			{
				success = false;
				errorMessage = "The information cannot be pasted.";
				errorMessageLocalizationKey = "Spreadsheet_ErrorExpressions_CannotPasteGeneral";
			}
			else if (fragment.IsCellCopyPaste)
			{
				CellRange pasteCellRange = this.GetPasteCellRange(fragment);
				if (pasteCellRange == null)
				{
					success = false;
					errorMessage = "The information cannot be pasted beacause the Copy area and \r\n the paste area are not the same size and shape.";
					errorMessageLocalizationKey = "Spreadsheet_ErrorExpressions_CannotPasteDueToSize";
				}
				else if (!this.CanChangeCellContent(pasteCellRange))
				{
					success = false;
					errorMessage = "The cell you are trying to change is on a protected sheet. To make changes, unprotect the sheet.";
					errorMessageLocalizationKey = "Spreadsheet_ProtectedWorksheet_Error";
				}
				else if (this.Cells.MergedCellRanges.GetIntersectingMergedRanges(pasteCellRange).Count > 0)
				{
					success = false;
					errorMessage = "Cannot change part of a merged cell.";
					errorMessageLocalizationKey = "Spreadsheet_ErrorExpressions_CannotChangeMergedCell";
				}
				else if (WorksheetFragment.GetPasteCellRangesCount(fragment.FromCellRanges, pasteCellRange) > (long)WorksheetFragment.MaxAllowedAffectedRanges)
				{
					success = false;
					errorMessage = "The operation you requested affects a large number of cells and may not be possible with the available resources.";
				}
				else if (WorksheetFragment.GetPasteCellRangesCount(fragment.FromCellRanges, pasteCellRange) > (long)WorksheetFragment.MaxAllowedAffectedRangesWithUndo)
				{
					success = false;
					shouldDisableHistory = true;
					errorMessage = "The operation you requested affects a large number of cells and may not be possible with the available resources. Would you like to continue without undo?";
				}
			}
			return new PasteResult(success, errorMessage, errorMessageLocalizationKey, shouldDisableHistory);
		}

		internal CellRange GetPasteCellRange(WorksheetFragment fragment)
		{
			Guard.ThrowExceptionIfNull<WorksheetFragment>(fragment, "fragment");
			if (base.CellRanges.Count<CellRange>() > 1)
			{
				return null;
			}
			CellRange toCellRange = base.CellRanges.First<CellRange>();
			return WorksheetFragment.EnsureProportionalPasteCellRange(fragment.FromCellRanges, toCellRange);
		}

		public bool Paste(WorksheetFragment fragment, PasteOptions pasteOptions, out IEnumerable<CellRange> affectedCellRanges, out IEnumerable<FloatingShapeBase> newShapes)
		{
			Guard.ThrowExceptionIfNull<WorksheetFragment>(fragment, "fragment");
			List<CellRange> list;
			bool result = fragment.Paste(base.Worksheet, base.CellRanges.First<CellRange>(), null, pasteOptions, out list, out newShapes, fragment.IsCellCopyPaste);
			affectedCellRanges = list;
			return result;
		}

		internal bool CanChangeCellContent(CellRange cellRange)
		{
			RangePropertyValue<bool> isLocked = this.Cells[cellRange].GetIsLocked();
			return !base.Worksheet.IsProtected || (!isLocked.IsIndeterminate && !isLocked.Value);
		}

		internal bool CanChangeCellContent()
		{
			return !base.Worksheet.IsProtected || !this.SelectedCellRangesContainsLockedCell();
		}

		bool SelectedCellRangesContainsLockedCell()
		{
			foreach (CellRange cellRange in base.CellRanges)
			{
				if (this.Cells[cellRange].GetIsLocked().Value)
				{
					return true;
				}
			}
			return false;
		}

		public void Insert(InsertShiftType shiftType)
		{
			WorkbookHistory history = this.Cells.Worksheet.Workbook.History;
			using (new UpdateScope(new Action(history.BeginUndoGroup), new Action(history.EndUndoGroup)))
			{
				base.ExecuteForEachRangeInsideBeginEndUpdate(delegate(CellRange cellRange)
				{
					this.Worksheet.Cells.Insert(cellRange, shiftType);
				});
			}
		}

		public void Remove(RemoveShiftType shiftType)
		{
			WorkbookHistory history = this.Cells.Worksheet.Workbook.History;
			using (new UpdateScope(new Action(history.BeginUndoGroup), new Action(history.EndUndoGroup)))
			{
				base.ExecuteForEachRangeInsideBeginEndUpdate(delegate(CellRange cellRange)
				{
					this.Worksheet.Cells.Remove(cellRange, shiftType);
				});
			}
		}

		public bool CanInsertOrRemove(CellRange selectedRange, ShiftType shiftType)
		{
			CanInsertRemoveResult canInsertRemoveResult = this.CanInsertOrRemoveInternal(selectedRange, shiftType);
			return canInsertRemoveResult == CanInsertRemoveResult.Success;
		}

		internal CanInsertRemoveResult CanInsertOrRemoveInternal(CellRange selectedRange, ShiftType shiftType)
		{
			CanInsertRemoveResult result = CanInsertRemoveResult.Success;
			InsertShiftType shiftType2;
			if (shiftType.TryGetInsertShiftType(out shiftType2))
			{
				result = base.Worksheet.Cells.CanInsertInternal(selectedRange, shiftType2);
			}
			RemoveShiftType shiftType3;
			if (shiftType.TryGetRemoveShiftType(out shiftType3))
			{
				result = base.Worksheet.Cells.CanRemoveInternal(selectedRange, shiftType3);
			}
			return result;
		}

		int GetInitialValuesCount(ICellValue[] initialValues, CellIndex[] indexes)
		{
			for (int i = initialValues.Length - 1; i >= 0; i--)
			{
				CellRange cellRange;
				base.Worksheet.Cells.TryGetContainingMergedRange(indexes[i], out cellRange);
				if (cellRange != null && indexes.Contains(cellRange.FromIndex))
				{
					int num = Array.IndexOf<CellIndex>(indexes, cellRange.FromIndex);
					if (initialValues[num].ValueType != CellValueType.Empty)
					{
						return i + 1;
					}
				}
				if (initialValues[i].ValueType != CellValueType.Empty)
				{
					return i + 1;
				}
			}
			return initialValues.Length;
		}

		void RepeatFormatProperties(CellIndex[] indexes, int initialValuesCount)
		{
			if (indexes.Length == 0)
			{
				return;
			}
			for (int i = 0; i < initialValuesCount; i++)
			{
				CellSelection sourceCells = base.Worksheet.Cells[indexes[i]];
				CellSelection.CellPropertiesValues formatProperties = this.GetFormatProperties(sourceCells);
				for (int j = i + initialValuesCount; j < indexes.Length; j += initialValuesCount)
				{
					CellSelection destinationCells = base.Worksheet.Cells[indexes[j]];
					this.SetFormatProperties(formatProperties, destinationCells);
				}
			}
		}

		void FillDataSeries(CellRange cellRange, CellOrientation seriesOrientation, Action<CellIndex[], int> fillGenerator, int? initialValuesCount = null, bool isDirectionReversed = false)
		{
			this.FillDataSeries(null, cellRange, seriesOrientation, fillGenerator, initialValuesCount, isDirectionReversed);
		}

		void FillDataSeries(ValuesInfo valuesInfo, CellRange cellRange, CellOrientation seriesOrientation, Action<CellIndex[], int> fillGenerator, int? initialValuesCount = null, bool isDirectionReversed = false)
		{
			if (valuesInfo == null)
			{
				valuesInfo = this.GetValuesInfo(cellRange, seriesOrientation, isDirectionReversed, initialValuesCount);
			}
			for (int i = 0; i < valuesInfo.CellIndexLists.Count; i++)
			{
				fillGenerator(valuesInfo.CellIndexLists[i], valuesInfo.InitialValuesCounts[i]);
			}
		}

		ValuesInfo GetValuesInfo(CellRange cellRange, CellOrientation seriesOrientation, bool isDirectionReversed, int? initialValuesCount)
		{
			CellRangeEnumerator cellRangeEnumerator = new CellRangeEnumerator(cellRange, seriesOrientation, isDirectionReversed);
			ValuesInfo valuesInfo = new ValuesInfo();
			List<CellIndex> list = null;
			while (cellRangeEnumerator.MoveNext())
			{
				if (cellRangeEnumerator.IsAtRowColumnStart)
				{
					if (list != null)
					{
						valuesInfo.CellIndexLists.Add(list.ToArray());
					}
					list = new List<CellIndex>();
				}
				list.Add(cellRangeEnumerator.Current);
			}
			valuesInfo.CellIndexLists.Add(list.ToArray());
			foreach (CellIndex[] array in valuesInfo.CellIndexLists)
			{
				ICellValue[] array2 = new ICellValue[array.Length];
				for (int i = 0; i < array.Length; i++)
				{
					CellIndex cellIndex = array[i];
					ICellValue value = base.Worksheet.Cells[cellIndex].GetValue().Value;
					array2[i] = value;
					int num = i;
					if (num == initialValuesCount - 1)
					{
						break;
					}
				}
				valuesInfo.InitialValuesList.Add(array2);
				if (initialValuesCount == null)
				{
					valuesInfo.InitialValuesCounts.Add(this.GetInitialValuesCount(array2, array));
				}
				else
				{
					valuesInfo.InitialValuesCounts.Add(initialValuesCount.Value);
				}
			}
			return valuesInfo;
		}

		CellIndex[] RebuildCellIdexesRespectingMergedCells(CellIndex[] indexes)
		{
			List<CellIndex> list = new List<CellIndex>();
			for (int i = 0; i < indexes.Length; i++)
			{
				CellRange cellRange;
				CellRange other;
				if (list.Count == 0)
				{
					list.Add(indexes[i]);
				}
				else if (!base.Worksheet.Cells.TryGetContainingMergedRange(indexes[i], out cellRange) || !base.Worksheet.Cells.TryGetContainingMergedRange(indexes[i - 1], out other) || !cellRange.IntersectsWith(other))
				{
					list.Add(indexes[i]);
				}
			}
			return list.ToArray();
		}

		void ApplyChanges(double[] doubleResult, CellIndex[] indexes)
		{
			if (doubleResult == null)
			{
				return;
			}
			for (int i = 0; i < doubleResult.Length; i++)
			{
				base.Worksheet.Cells[indexes[i]].SetValueAndExpandToFitNumberValuesWidth(doubleResult[i]);
			}
		}

		bool InitializeBeforeFill(int initialIndexesCount, ref CellIndex[] indexes, out double?[] initialDoubleValues)
		{
			initialDoubleValues = new double?[initialIndexesCount];
			if (indexes[0].ToCellValue(base.Worksheet).GetAsDoubleOrNull() == null)
			{
				return true;
			}
			CellRange cellRange = null;
			base.Worksheet.Cells.TryGetContainingMergedRange(indexes[0], out cellRange);
			if (cellRange != null)
			{
				FillDataSeriesHelper.RepeatMergedRanges(base.Worksheet, new CellRange[] { cellRange }, indexes, 1);
			}
			indexes = this.RebuildCellIdexesRespectingMergedCells(indexes);
			int num = 0;
			if (indexes.Length > initialIndexesCount)
			{
				for (int i = 0; i < initialIndexesCount; i++)
				{
					double? asDoubleOrNull = indexes[i].ToCellValue(base.Worksheet).GetAsDoubleOrNull();
					initialDoubleValues[i] = asDoubleOrNull;
					num++;
				}
			}
			return num == 0;
		}

		void FillRespectingMergedCells(int initialIndexesCount, double stepValue, double? stopValue, CellIndex[] indexes, Func<double, int, double, double?, double[]> generator)
		{
			double?[] array;
			if (this.InitializeBeforeFill(initialIndexesCount, ref indexes, out array))
			{
				return;
			}
			double[] doubleResult = generator(array[0].Value, indexes.Length, stepValue, stopValue);
			this.ApplyChanges(doubleResult, indexes);
		}

		void FillRespectingMergedCells(int initialIndexesCount, double stepValue, double? stopValue, CellIndex[] indexes, DateUnitType dateUnitType, Func<double, DateUnitType, int, double, double?, double[]> generator)
		{
			double?[] array;
			if (this.InitializeBeforeFill(initialIndexesCount, ref indexes, out array))
			{
				return;
			}
			double[] doubleResult = generator(array[0].Value, dateUnitType, indexes.Length, stepValue, stopValue);
			this.ApplyChanges(doubleResult, indexes);
		}

		void FillRespectingMergedCells(int initialIndexesCount, CellIndex[] indexes, Func<double?[], int, double[]> generator)
		{
			double?[] arg;
			if (this.InitializeBeforeFill(initialIndexesCount, ref indexes, out arg))
			{
				return;
			}
			double[] doubleResult = generator(arg, indexes.Length);
			this.ApplyChanges(doubleResult, indexes);
		}

		internal CellRange GetRangeAffectedByFillData(CellRange newSelection, FillDirection direction, int initialValuesCount)
		{
			int num = newSelection.FromIndex.RowIndex;
			int num2 = newSelection.FromIndex.ColumnIndex;
			int num3 = newSelection.ToIndex.RowIndex;
			int num4 = newSelection.ToIndex.ColumnIndex;
			switch (direction)
			{
			case FillDirection.Left:
				num4 -= initialValuesCount;
				break;
			case FillDirection.Up:
				num3 -= initialValuesCount;
				break;
			case FillDirection.Right:
				num2 += initialValuesCount;
				break;
			case FillDirection.Down:
				num += initialValuesCount;
				break;
			}
			return new CellRange(num, num2, num3, num4);
		}

		public void FillDataSeriesLinear(CellOrientation seriesOrientation, double stepValue, double? stopValue = null)
		{
			Action<CellRange> fillRangeAction = delegate(CellRange cellRange)
			{
				this.FillDataSeries(cellRange, seriesOrientation, delegate(CellIndex[] indexes, int initialIndexesCount)
				{
					this.FillRespectingMergedCells(initialIndexesCount, stepValue, stopValue, indexes, new Func<double, int, double, double?, double[]>(FillDataSeriesHelper.FillLinear));
					CellIndex[] indexes2 = this.RebuildCellIdexesRespectingMergedCells(indexes);
					this.RepeatFormatProperties(indexes2, 1);
				}, new int?(1), false);
			};
			base.ExecuteAndExpandToFitNumberValuesWidth(delegate
			{
				this.ExecuteForEachRangeInsideBeginEndUpdate(fillRangeAction);
			});
		}

		public void FillDataSeriesLinearTrend(CellOrientation seriesOrientation)
		{
			Action<CellRange> fillRangeAction = delegate(CellRange cellRange)
			{
				this.FillDataSeries(cellRange, seriesOrientation, delegate(CellIndex[] indexes, int initialIndexesCount)
				{
					this.FillRespectingMergedCells(initialIndexesCount, indexes, new Func<double?[], int, double[]>(FillDataSeriesHelper.FillLinearTrend));
					CellIndex[] indexes2 = this.RebuildCellIdexesRespectingMergedCells(indexes);
					this.RepeatFormatProperties(indexes2, 1);
				}, null, false);
			};
			base.ExecuteAndExpandToFitNumberValuesWidth(delegate
			{
				this.ExecuteForEachRangeInsideBeginEndUpdate(fillRangeAction);
			});
		}

		public void FillDataSeriesExponential(CellOrientation seriesOrientation, double stepValue, double? stopValue = null)
		{
			Action<CellRange> fillRangeAction = delegate(CellRange cellRange)
			{
				this.FillDataSeries(cellRange, seriesOrientation, delegate(CellIndex[] indexes, int initialIndexesCount)
				{
					this.FillRespectingMergedCells(initialIndexesCount, stepValue, stopValue, indexes, new Func<double, int, double, double?, double[]>(FillDataSeriesHelper.FillExponential));
					CellIndex[] indexes2 = this.RebuildCellIdexesRespectingMergedCells(indexes);
					this.RepeatFormatProperties(indexes2, 1);
				}, new int?(1), false);
			};
			base.ExecuteAndExpandToFitNumberValuesWidth(delegate
			{
				this.ExecuteForEachRangeInsideBeginEndUpdate(fillRangeAction);
			});
		}

		public void FillDataSeriesExponentialTrend(CellOrientation seriesOrientation)
		{
			Action<CellRange> fillRangeAction = delegate(CellRange cellRange)
			{
				this.FillDataSeries(cellRange, seriesOrientation, delegate(CellIndex[] indexes, int initialIndexesCount)
				{
					this.FillRespectingMergedCells(initialIndexesCount, indexes, new Func<double?[], int, double[]>(FillDataSeriesHelper.FillExponentialTrend));
					CellIndex[] indexes2 = this.RebuildCellIdexesRespectingMergedCells(indexes);
					this.RepeatFormatProperties(indexes2, 1);
				}, null, false);
			};
			base.ExecuteAndExpandToFitNumberValuesWidth(delegate
			{
				this.ExecuteForEachRangeInsideBeginEndUpdate(fillRangeAction);
			});
		}

		public void FillDataSeriesDate(CellOrientation seriesOrientation, DateUnitType dateUnitType, double stepValue, double? stopValue = null)
		{
			Action<CellRange> fillRangeAction = delegate(CellRange cellRange)
			{
				this.FillDataSeries(cellRange, seriesOrientation, delegate(CellIndex[] indexes, int initialIndexesCount)
				{
					this.FillRespectingMergedCells(initialIndexesCount, stepValue, stopValue, indexes, dateUnitType, new Func<double, DateUnitType, int, double, double?, double[]>(FillDataSeriesHelper.FillDate));
					CellIndex[] indexes2 = this.RebuildCellIdexesRespectingMergedCells(indexes);
					this.RepeatFormatProperties(indexes2, 1);
				}, new int?(1), false);
			};
			base.ExecuteAndExpandToFitNumberValuesWidth(delegate
			{
				this.ExecuteForEachRangeInsideBeginEndUpdate(fillRangeAction);
			});
		}

		public void FillDataSeriesAuto(CellOrientation seriesOrientation, bool respectRangesDirection, int? initialValueCount = null)
		{
			int maxInitialValuesCount = 0;
			ValuesInfo valuesInfo = new ValuesInfo();
			Dictionary<CellRange, HashSet<CellRange>> mergedRanges = new Dictionary<CellRange, HashSet<CellRange>>();
			Func<CellRange, bool> getIsDirectionReserved = (CellRange cellRange) => respectRangesDirection && ((seriesOrientation == CellOrientation.Horizontal && !cellRange.IsLeftToRight) || (seriesOrientation == CellOrientation.Vertical && !cellRange.IsTopToBottom));
			Action<CellRange> fillRangeAction = delegate(CellRange cellRange)
			{
				HashSet<CellRange> hashSet = new HashSet<CellRange>();
				mergedRanges.Add(cellRange, hashSet);
				bool isDirectionReversed = getIsDirectionReserved(cellRange);
				valuesInfo = this.GetValuesInfo(cellRange, seriesOrientation, isDirectionReversed, initialValueCount);
				maxInitialValuesCount = valuesInfo.MaxInitialValuesCount;
				foreach (CellIndex[] indexes2 in valuesInfo.CellIndexLists)
				{
					CellRange[] array = FillDataSeriesHelper.ExtractMergedRanges(this.Worksheet, indexes2, maxInitialValuesCount);
					if (array.Length > 0)
					{
						FillDataSeriesHelper.UnmergeMergedRanges(this.Worksheet, array);
						foreach (CellRange item in array)
						{
							if (!hashSet.Contains(item))
							{
								hashSet.Add(item);
							}
						}
					}
				}
				this.Worksheet.Cells[cellRange].Unmerge();
				this.FillDataSeries(valuesInfo, cellRange, seriesOrientation, delegate(CellIndex[] indexes, int initialIndexesCount)
				{
					AutoFillDataSeriesManager autoFillDataSeriesManager = new AutoFillDataSeriesManager(this.Worksheet);
					autoFillDataSeriesManager.FillAuto(indexes, maxInitialValuesCount);
				}, null, isDirectionReversed);
			};
			Action<CellRange> mergeAction = delegate(CellRange cellRange)
			{
				bool isDirectionReversed = getIsDirectionReserved(cellRange);
				if (mergedRanges.ContainsKey(cellRange))
				{
					CellRange[] mergedCellRanges = mergedRanges[cellRange].ToArray<CellRange>();
					FillDataSeriesHelper.RepeatMergedRanges(this.Worksheet, mergedCellRanges, cellRange, maxInitialValuesCount, seriesOrientation, isDirectionReversed);
				}
			};
			base.ExecuteAndExpandToFitNumberValuesWidth(delegate
			{
				this.ExecuteForEachRange(fillRangeAction);
				foreach (CellIndex[] indexes in valuesInfo.CellIndexLists)
				{
					this.RepeatFormatProperties(indexes, valuesInfo.MaxInitialValuesCount);
				}
				this.ExecuteForEachRange(mergeAction);
			});
		}

		public void FillData(FillDirection direction)
		{
			bool isDirectionReversed = false;
			CellOrientation orientation;
			switch (direction)
			{
			case FillDirection.Left:
				orientation = CellOrientation.Horizontal;
				isDirectionReversed = true;
				break;
			case FillDirection.Up:
				orientation = CellOrientation.Vertical;
				isDirectionReversed = true;
				break;
			case FillDirection.Right:
				orientation = CellOrientation.Horizontal;
				break;
			case FillDirection.Down:
				orientation = CellOrientation.Vertical;
				break;
			default:
				throw new InvalidOperationException();
			}
			Action<CellRange> fillRangeAction = delegate(CellRange cellRange)
			{
				int usedMergedCellRangesIndexer = 0;
				bool shouldGetNextMergedCellRange = false;
				CellRange mergedCells = null;
				CellRange[] mergedCellRanges = null;
				CellIndex cellIndex = (isDirectionReversed ? cellRange.ToIndex : cellRange.FromIndex);
				if (isDirectionReversed && orientation != CellOrientation.Vertical)
				{
					this.TryGetMergedCellRangesInReversedSelection(cellRange, out mergedCells, out mergedCellRanges);
				}
				else
				{
					this.Worksheet.Cells.TryGetContainingMergedRange(cellIndex, out mergedCells);
				}
				if ((cellRange.ColumnCount == 1 && (direction == FillDirection.Left || direction == FillDirection.Right)) || (cellRange.RowCount == 1 && (direction == FillDirection.Down || direction == FillDirection.Up)) || (mergedCells != null && mergedCells.Equals(cellRange)))
				{
					WorksheetFragment worksheetFragment = null;
					CellRange[] mergedRanges = this.PreserveMergedRanges(cellRange);
					this.Worksheet.Cells[cellRange].Unmerge();
					switch (direction)
					{
					case FillDirection.Left:
						if (cellRange.FromIndex.ColumnIndex + 1 < SpreadsheetDefaultValues.ColumnCount)
						{
							CellRange sourceRange = this.GetSourceRange(cellRange.FromIndex.RowIndex, cellRange.FromIndex.ColumnIndex + 1, cellRange.ToIndex.RowIndex, cellRange.ToIndex.ColumnIndex + 1);
							CellSelection cellSelection = this.Worksheet.Cells[sourceRange];
							worksheetFragment = cellSelection.Copy();
						}
						break;
					case FillDirection.Up:
						if (cellRange.FromIndex.RowIndex + 1 < SpreadsheetDefaultValues.RowCount)
						{
							CellRange sourceRange2 = this.GetSourceRange(cellRange.FromIndex.RowIndex + 1, cellRange.FromIndex.ColumnIndex, cellRange.ToIndex.RowIndex + 1, cellRange.ToIndex.ColumnIndex);
							CellSelection cellSelection2 = this.Worksheet.Cells[sourceRange2];
							worksheetFragment = cellSelection2.Copy();
						}
						break;
					case FillDirection.Right:
						if (cellRange.FromIndex.ColumnIndex - 1 >= 0)
						{
							CellRange sourceRange3 = this.GetSourceRange(cellRange.FromIndex.RowIndex, cellRange.FromIndex.ColumnIndex - 1, cellRange.ToIndex.RowIndex, cellRange.ToIndex.ColumnIndex - 1);
							CellSelection cellSelection3 = this.Worksheet.Cells[sourceRange3];
							worksheetFragment = cellSelection3.Copy();
						}
						break;
					case FillDirection.Down:
						if (cellRange.FromIndex.RowIndex - 1 >= 0)
						{
							CellRange sourceRange4 = this.GetSourceRange(cellRange.FromIndex.RowIndex - 1, cellRange.FromIndex.ColumnIndex, cellRange.ToIndex.RowIndex - 1, cellRange.ToIndex.ColumnIndex);
							CellSelection cellSelection4 = this.Worksheet.Cells[sourceRange4];
							worksheetFragment = cellSelection4.Copy();
						}
						break;
					default:
						throw new InvalidOperationException();
					}
					if (worksheetFragment != null)
					{
						this.Worksheet.Cells[cellRange].Paste(worksheetFragment, new PasteOptions(PasteType.All, false, true));
					}
					this.RestoreMergedRanges(mergedRanges);
					return;
				}
				this.FillDataSeries(cellRange, orientation, delegate(CellIndex[] indexes, int initialIndexesCount)
				{
					if (mergedCellRanges != null && shouldGetNextMergedCellRange)
					{
						mergedCells = ((mergedCellRanges.Length > usedMergedCellRangesIndexer) ? mergedCellRanges[usedMergedCellRangesIndexer] : null);
						shouldGetNextMergedCellRange = false;
					}
					CellIndex cellIndex2 = ((mergedCells != null && isDirectionReversed && (indexes.Contains(mergedCells.FromIndex) || indexes.Contains(mergedCells.ToIndex))) ? mergedCells.FromIndex : indexes[0]);
					if (mergedCells != null && indexes.Contains(mergedCells.ToIndex) && !indexes.Contains(mergedCells.FromIndex))
					{
						cellIndex2 = mergedCells.ToIndex;
						shouldGetNextMergedCellRange = true;
					}
				    cellRange = new CellRange(cellIndex2, indexes[0]);
					CellMergeState mergeState = this.Worksheet.Cells.GetMergeState(cellIndex2);
					if (mergeState == CellMergeState.NonTopLeftCellInMergedRange)
					{
						return;
					}
					CellRange[] array = null;
					CellRange cellRange2;
					if (this.Worksheet.Cells.TryGetContainingMergedRange(cellIndex2, out cellRange2))
					{
						if (mergedCellRanges != null && mergedCellRanges.Contains(cellRange2))
						{
							shouldGetNextMergedCellRange = true;
							usedMergedCellRangesIndexer++;
						}
						cellRange = cellRange2;
						CellRange[] mergedRanges2 = new CellRange[] { cellRange2 };
						array = FillDataSeriesHelper.RepeatMergedRanges(mergedRanges2, indexes, initialIndexesCount, isDirectionReversed);
					}
					CellRange cellRange3;
					if (array != null)
					{
						int num = ((array.Length > 1) ? 1 : 0);
						cellRange3 = ((!isDirectionReversed) ? new CellRange(array[num].FromIndex, array[array.Length - 1].ToIndex) : new CellRange(array[array.Length - 1].FromIndex, array[num].ToIndex));
					}
					else
					{
						cellRange3 = new CellRange(indexes[1], new CellIndex(indexes[indexes.Length - 1].RowIndex + cellRange.RowCount - 1, indexes[indexes.Length - 1].ColumnIndex + cellRange.ColumnCount - 1));
					}
					WorksheetFragment fragment = this.Worksheet.Cells[cellRange].Copy();
					this.Worksheet.Cells[cellRange3].Unmerge();
					this.Worksheet.Cells[cellRange3].Paste(fragment, new PasteOptions(PasteType.All, false));
				}, new int?(1), isDirectionReversed);
			};
			base.ExecuteAndExpandToFitNumberValuesWidth(delegate
			{
				this.ExecuteForEachRangeInsideBeginEndUpdate(fillRangeAction);
			});
		}

		bool TryGetMergedCellRangesInReversedSelection(CellRange cellRange, out CellRange mergedCellRange, out CellRange[] mergedCellRanges)
		{
			mergedCellRange = null;
			mergedCellRanges = base.Worksheet.Cells.MergedCellRanges.GetIntersectingMergedRanges(cellRange).ToArray<CellRange>();
			if (mergedCellRanges != null && mergedCellRanges.Length > 0)
			{
				mergedCellRange = mergedCellRanges[0];
			}
			return mergedCellRange != null;
		}

		CellRange[] PreserveMergedRanges(CellRange cellRange)
		{
			List<CellRange> list = new List<CellRange>();
			foreach (CellRange cellRange2 in base.Worksheet.Cells.MergedCellRanges.Ranges)
			{
				if (cellRange2.IntersectsWith(cellRange))
				{
					list.Add(cellRange2);
				}
			}
			return list.ToArray();
		}

		void RestoreMergedRanges(CellRange[] mergedRanges)
		{
			foreach (CellRange cellRange in mergedRanges)
			{
				base.Worksheet.Cells[cellRange].Merge();
			}
		}

		CellRange GetSourceRange(int fromRowIndex, int fromColumnIndex, int toRowIndex, int toColumnIndex)
		{
			CellIndex cellIndex = new CellIndex(fromRowIndex, fromColumnIndex);
			CellIndex cellIndex2 = new CellIndex(toRowIndex, toColumnIndex);
			ISet<CellRange> intersectingMergedRanges = base.Worksheet.Cells.MergedCellRanges.GetIntersectingMergedRanges(new CellRange(cellIndex, cellIndex2));
			if (intersectingMergedRanges.Count > 0)
			{
				CellRange cellRange = intersectingMergedRanges.First<CellRange>();
				cellIndex2 = cellIndex2.Offset(cellRange.FromIndex.RowIndex - cellIndex.RowIndex, cellRange.FromIndex.ColumnIndex - cellIndex.ColumnIndex);
				cellIndex = cellRange.FromIndex;
			}
			return new CellRange(cellIndex, cellIndex2);
		}

		public RangePropertyValue<IDataValidationRule> GetDataValidationRule()
		{
			return base.GetPropertyValue<IDataValidationRule>(this.Cells.DataValidationRuleProperty);
		}

		public void SetDataValidationRule(IDataValidationRule value)
		{
			base.SetPropertyValue<IDataValidationRule>(this.Cells.DataValidationRuleProperty, value);
		}

		public void ClearDataValidationRule()
		{
			base.ClearPropertyValue(this.Cells.DataValidationRuleProperty, true);
		}

		public void Sort(params ISortCondition[] sortConditions)
		{
			CellRange cellRange = base.CellRanges.First<CellRange>();
			if (!SortAndFilterHelper.IsValidSelection(base.CellRanges))
			{
				throw new SortingException("Wrong range selected.", "Spreadsheet_Sorting_WrongSelection");
			}
			if (SortManager.AreSortConditionsDuplicating(sortConditions))
			{
				throw new SortingException("Duplicated sort conditions.", "Spreadsheet_Sorting_DuplicatedSortConditions");
			}
			int num = this.Cells.GetContainingMergedRanges(cellRange).Count<CellRange>();
			if (num > 0)
			{
				throw new SortingException("Cannot sort a range containing merged cells.", "Spreadsheet_Sorting_MergedCells");
			}
			CellSelection selection = this.Cells[cellRange];
			if (SortAndFilterHelper.IsEmptySortRange(selection))
			{
				throw new SortingException("Cannot sort empty range!", "Spreadsheet_Sorting_NoData");
			}
			foreach (ISortCondition sortCondition in sortConditions)
			{
				Guard.ThrowExceptionIfOutOfRange<int>(0, cellRange.ColumnCount - 1, sortCondition.RelativeIndex, "RelativeIndex");
			}
			using (new UpdateScope(new Action(base.Worksheet.BeginUndoGroup), new Action(base.Worksheet.EndUndoGroup)))
			{
				base.Worksheet.SortState.ClearInternal();
				base.Worksheet.SortState.SortRange = cellRange;
				sortConditions = sortConditions.Reverse<ISortCondition>().ToArray<ISortCondition>();
				SortManager.Sort(this.Cells, cellRange, sortConditions);
			}
		}

		public void Filter(params IFilter[] filters)
		{
			if (base.CellRanges.Count<CellRange>() > 1)
			{
				throw new LocalizableException("This action cannot be performed on multiple ranges");
			}
			CellRange filterRange = base.CellRanges.First<CellRange>();
			using (new UpdateScope(new Action(base.Worksheet.BeginUndoGroup), new Action(base.Worksheet.EndUndoGroup)))
			{
				base.Worksheet.Filter.FilterRange = filterRange;
				base.Worksheet.Filter.SetFilters(filters);
			}
		}

		static readonly CellValueFormat shortDateFormat = new CellValueFormat(FormatHelper.ShortDatePattern);

		struct CellPropertiesValues
		{
			public CellBorders Borders { get; set; }

			public IFill Fill { get; set; }

			public ThemableFontFamily FontFamily { get; set; }

			public double FontSize { get; set; }

			public ThemableColor ForeColor { get; set; }

			public CellValueFormat Format { get; set; }

			public RadHorizontalAlignment HorizontalAlignment { get; set; }

			public int Indent { get; set; }

			public bool IsBold { get; set; }

			public bool IsItalic { get; set; }

			public bool IsWrapped { get; set; }

			public string StyleName { get; set; }

			public UnderlineType Underline { get; set; }

			public RadVerticalAlignment VerticalAlignment { get; set; }
		}
	}
}
