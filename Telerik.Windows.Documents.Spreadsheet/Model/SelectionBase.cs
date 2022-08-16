using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands;
using Telerik.Windows.Documents.Spreadsheet.Core;
using Telerik.Windows.Documents.Spreadsheet.Formatting;
using Telerik.Windows.Documents.Spreadsheet.Layout;
using Telerik.Windows.Documents.Spreadsheet.PropertySystem;
using Telerik.Windows.Documents.Spreadsheet.Theming;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model
{
	public abstract class SelectionBase
	{
		public IEnumerable<CellRange> CellRanges
		{
			get
			{
				return this.cellRanges;
			}
		}

		public Worksheet Worksheet
		{
			get
			{
				return this.worksheet;
			}
		}

		internal abstract WorksheetEntityBase WorksheetEntity { get; }

		internal SelectionBase(Worksheet worksheet, IEnumerable<CellRange> cellRanges)
		{
			Guard.ThrowExceptionIfNull<Worksheet>(worksheet, "worksheet");
			Guard.ThrowExceptionIfNull<IEnumerable<CellRange>>(cellRanges, "cellRanges");
			Guard.ThrowExceptionIfContainsNull<CellRange>(cellRanges, "cellRanges");
			this.worksheet = worksheet;
			this.cellRanges = cellRanges;
		}

		protected void BeginUpdate()
		{
			if (this.beginUpdateCount == 0)
			{
				this.Worksheet.Workbook.History.BeginUndoGroup();
				this.Worksheet.Workbook.SuspendLayoutUpdate();
			}
			this.beginUpdateCount++;
		}

		protected void EndUpdate()
		{
			if (this.beginUpdateCount == 0)
			{
				throw new InvalidOperationException("There is no active update to end.");
			}
			this.beginUpdateCount--;
			if (this.beginUpdateCount == 0)
			{
				this.Worksheet.Workbook.ResumeLayoutUpdate();
				this.Worksheet.Workbook.History.EndUndoGroup();
			}
		}

		internal void ExecuteAndExpandToFitNumberValuesWidth(Action action)
		{
			using (new UpdateScope(new Action(this.BeginUpdate), new Action(this.EndUpdate)))
			{
				action();
				this.Worksheet.Columns[this.CellRanges].ExpandToFitNumberValuesWidth();
			}
		}

		protected void ExecuteForEachRangeInsideBeginEndUpdate(Action<CellRange> action)
		{
			this.ExecuteForEachRangeInsideBeginEndUpdate(this.CellRanges, action, null);
		}

		protected bool ExecuteForEachRangeInsideBeginEndUpdate(IEnumerable<CellRange> cellRanges, Action<CellRange> action, Predicate<CellRange> canExecute = null)
		{
			bool result;
			using (new UpdateScope(new Action(this.BeginUpdate), new Action(this.EndUpdate)))
			{
				bool flag = this.ExecuteForEachRange(cellRanges, action, canExecute);
				result = flag;
			}
			return result;
		}

		protected void ExecuteForEachRange(Action<CellRange> action)
		{
			this.ExecuteForEachRange(this.CellRanges, action, null);
		}

		protected bool ExecuteForEachRange(IEnumerable<CellRange> cellRanges, Action<CellRange> action, Predicate<CellRange> canExecute = null)
		{
			if (canExecute != null)
			{
				foreach (CellRange obj in cellRanges)
				{
					if (!canExecute(obj))
					{
						return false;
					}
				}
			}
			foreach (CellRange obj2 in cellRanges)
			{
				action(obj2);
			}
			return true;
		}

		internal RangePropertyValue<T> GetPropertyValue<T>(IProperty<T> property)
		{
			return this.GetPropertyValue<T>(property, this.CellRanges);
		}

		internal RangePropertyValue<T> GetPropertyValue<T>(IProperty<T> property, CellRange cellRange)
		{
			return this.GetPropertyValue<T>(property, new CellRange[] { cellRange });
		}

		internal RangePropertyValue<T> GetPropertyValue<T>(IProperty<T> property, IEnumerable<CellRange> cellRanges)
		{
			return this.GetPropertyValue<T>(new Func<CellRange, RangePropertyValue<T>>(property.GetValue), cellRanges, property.GetDefaultValue());
		}

		protected RangePropertyValue<T> GetPropertyValue<T>(Func<CellRange, RangePropertyValue<T>> getRangePropertyValue, IEnumerable<CellRange> cellRanges, T defaultValue)
		{
			T t = default(T);
			bool flag = false;
			bool flag2 = true;
			foreach (CellRange arg in cellRanges)
			{
				RangePropertyValue<T> rangePropertyValue = getRangePropertyValue(arg);
				if (rangePropertyValue.IsIndeterminate)
				{
					flag = true;
					break;
				}
				if (flag2)
				{
					t = rangePropertyValue.Value;
					flag2 = false;
				}
				else if (!TelerikHelper.EqualsOfT<T>(rangePropertyValue.Value, t))
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				t = defaultValue;
			}
			return new RangePropertyValue<T>(flag, t);
		}

		internal void SetPropertyValue<T>(IProperty<T> property, T value)
		{
			this.ExecuteForEachRangeInsideBeginEndUpdate(delegate(CellRange cellRange)
			{
				this.ClearCellPropertiesIfNeeded<T>(cellRange, property);
				this.PreserveOldStylePropertiesAsLocalIfNeeded<T>(cellRange, property, value);
				this.PreserveOldRowColumnPropertyAsLocalIfNeeded<T>(cellRange, property, value);
				this.PreserveAsLocalPropertyIfStyleIsApplied<T>(cellRange, property, value);
				property.SetValue(cellRange, value);
			});
		}

		internal virtual void PreserveAsLocalPropertyIfStyleIsApplied<T>(CellRange cellRange, IProperty<T> property, T value)
		{
		}

		internal virtual void ClearCellPropertiesIfNeeded<T>(CellRange cellRange, IProperty<T> property)
		{
		}

		internal virtual void PreserveOldRowColumnPropertyAsLocalIfNeeded<T>(CellRange cellRange, IProperty<T> property, T value)
		{
		}

		internal abstract void PreserveOldStylePropertiesAsLocalIfNeeded<T>(CellRange cellRange, IProperty<T> property, T value);

		internal void PreserveAllOldStylePropertiesAsLocalIfNeeded(SelectionBase selection, CellStyle newStyle)
		{
			this.PreserveOldStylePropertiyAsLocalIfNeeded<CellBorder>(newStyle, selection, selection.WorksheetEntity.BottomBorderProperty, CellPropertyDefinitions.BottomBorderProperty);
			this.PreserveOldStylePropertiyAsLocalIfNeeded<CellBorder>(newStyle, selection, selection.WorksheetEntity.DiagonalDownBorderProperty, CellPropertyDefinitions.DiagonalDownBorderProperty);
			this.PreserveOldStylePropertiyAsLocalIfNeeded<CellBorder>(newStyle, selection, selection.WorksheetEntity.DiagonalUpBorderProperty, CellPropertyDefinitions.DiagonalUpBorderProperty);
			this.PreserveOldStylePropertiyAsLocalIfNeeded<IFill>(newStyle, selection, selection.WorksheetEntity.FillProperty, CellPropertyDefinitions.FillProperty);
			this.PreserveOldStylePropertiyAsLocalIfNeeded<ThemableFontFamily>(newStyle, selection, selection.WorksheetEntity.FontFamilyProperty, CellPropertyDefinitions.FontFamilyProperty);
			this.PreserveOldStylePropertiyAsLocalIfNeeded<ThemableFontFamily>(newStyle, selection, selection.WorksheetEntity.FontFamilyProperty, CellPropertyDefinitions.FontFamilyProperty);
			this.PreserveOldStylePropertiyAsLocalIfNeeded<double>(newStyle, selection, selection.WorksheetEntity.FontSizeProperty, CellPropertyDefinitions.FontSizeProperty);
			this.PreserveOldStylePropertiyAsLocalIfNeeded<ThemableColor>(newStyle, selection, selection.WorksheetEntity.ForeColorProperty, CellPropertyDefinitions.ForeColorProperty);
			this.PreserveOldStylePropertiyAsLocalIfNeeded<CellValueFormat>(newStyle, selection, selection.WorksheetEntity.FormatProperty, CellPropertyDefinitions.FormatProperty);
			this.PreserveOldStylePropertiyAsLocalIfNeeded<RadHorizontalAlignment>(newStyle, selection, selection.WorksheetEntity.HorizontalAlignmentProperty, CellPropertyDefinitions.HorizontalAlignmentProperty);
			this.PreserveOldStylePropertiyAsLocalIfNeeded<int>(newStyle, selection, selection.WorksheetEntity.IndentProperty, CellPropertyDefinitions.IndentProperty);
			this.PreserveOldStylePropertiyAsLocalIfNeeded<bool>(newStyle, selection, selection.WorksheetEntity.IsBoldProperty, CellPropertyDefinitions.IsBoldProperty);
			this.PreserveOldStylePropertiyAsLocalIfNeeded<bool>(newStyle, selection, selection.WorksheetEntity.IsItalicProperty, CellPropertyDefinitions.IsItalicProperty);
			this.PreserveOldStylePropertiyAsLocalIfNeeded<bool>(newStyle, selection, selection.WorksheetEntity.IsWrappedProperty, CellPropertyDefinitions.IsWrappedProperty);
			this.PreserveOldStylePropertiyAsLocalIfNeeded<CellBorder>(newStyle, selection, selection.WorksheetEntity.LeftBorderProperty, CellPropertyDefinitions.LeftBorderProperty);
			this.PreserveOldStylePropertiyAsLocalIfNeeded<CellBorder>(newStyle, selection, selection.WorksheetEntity.RightBorderProperty, CellPropertyDefinitions.RightBorderProperty);
			this.PreserveOldStylePropertiyAsLocalIfNeeded<CellBorder>(newStyle, selection, selection.WorksheetEntity.TopBorderProperty, CellPropertyDefinitions.TopBorderProperty);
			this.PreserveOldStylePropertiyAsLocalIfNeeded<UnderlineType>(newStyle, selection, selection.WorksheetEntity.UnderlineProperty, CellPropertyDefinitions.UnderlineProperty);
			this.PreserveOldStylePropertiyAsLocalIfNeeded<RadVerticalAlignment>(newStyle, selection, selection.WorksheetEntity.VerticalAlignmentProperty, CellPropertyDefinitions.VerticalAlignmentProperty);
		}

		internal void PreserveOldStylePropertiyAsLocalIfNeeded<T>(CellStyle newStyle, SelectionBase selection, IProperty<T> property, IPropertyDefinition propertyDefinition)
		{
			if (!newStyle.GetIsPropertyIncluded(propertyDefinition))
			{
				RangePropertyValue<T> propertyValue = selection.GetPropertyValue<T>(property);
				if (!propertyValue.IsIndeterminate)
				{
					T value = propertyValue.Value;
					if (!value.Equals(property.GetDefaultValue()))
					{
						selection.SetPropertyValue<T>(property, propertyValue.Value);
					}
				}
			}
		}

		internal void ClearPropertyValue(IProperty property, bool shouldSetDefaultValuesIfIntersectsWithRowColumn = true)
		{
			this.ExecuteForEachRangeInsideBeginEndUpdate(delegate(CellRange cellRange)
			{
				property.ClearValue(cellRange);
				if (shouldSetDefaultValuesIfIntersectsWithRowColumn)
				{
					this.ClearBySettingDefaultValuesOnRowColumnPropertyIntersection(cellRange, property);
				}
			});
		}

		internal virtual void ClearBySettingDefaultValuesOnRowColumnPropertyIntersection(CellRange cellRange, IProperty property)
		{
		}

		void UpdateValueRespectFormat(CellValueFormat format)
		{
			this.ExecuteForEachRangeInsideBeginEndUpdate(delegate(CellRange cellRange)
			{
				UpdateCellPropertyCommandContext<ICellValue> context = new UpdateCellPropertyCommandContext<ICellValue>(this.Worksheet, CellPropertyDefinitions.ValueProperty, cellRange, (ICellValue value) => FormatHelper.GetCellValueRespectFormat(format, value));
				this.Worksheet.ExecuteCommand<UpdateCellPropertyCommandContext<ICellValue>>(WorkbookCommands.UpdateCellValue, context);
			});
		}

		protected CellIndex GetTopLeftCellIndex()
		{
			int num = int.MaxValue;
			int num2 = int.MaxValue;
			foreach (CellRange cellRange in this.CellRanges)
			{
				if (num > cellRange.FromIndex.RowIndex)
				{
					num = cellRange.FromIndex.RowIndex;
				}
				if (num2 > cellRange.FromIndex.ColumnIndex)
				{
					num2 = cellRange.FromIndex.ColumnIndex;
				}
			}
			return new CellIndex(num, num2);
		}

		public void Clear(ClearType type)
		{
			this.Clear(type, true);
		}

		internal abstract void Clear(ClearType type, bool clearIsLocked);

		internal void SetFormatAndExpandToFitNumberValuesWidth(CellValueFormat value)
		{
			this.ExecuteAndExpandToFitNumberValuesWidth(delegate
			{
				this.SetFormat(value);
			});
		}

		public RangePropertyValue<CellValueFormat> GetFormat()
		{
			return this.GetPropertyValue<CellValueFormat>(this.WorksheetEntity.FormatProperty);
		}

		public void SetFormat(CellValueFormat value)
		{
			using (new UpdateScope(new Action(this.BeginUpdate), new Action(this.EndUpdate)))
			{
				this.UpdateValueRespectFormat(value);
				this.SetPropertyValue<CellValueFormat>(this.WorksheetEntity.FormatProperty, value);
			}
		}

		public void ClearFormat()
		{
			this.ClearPropertyValue(this.WorksheetEntity.FormatProperty, true);
		}

		public RangePropertyValue<string> GetStyleName()
		{
			return this.GetPropertyValue<string>(this.WorksheetEntity.StyleNameProperty);
		}

		public void SetStyleName(string value)
		{
			if (!this.Worksheet.Workbook.Styles.Contains(value))
			{
				throw new ArgumentException("This style does not exist in the styles collection of the workbook.");
			}
			using (new UpdateScope(new Action(this.BeginUpdate), new Action(this.EndUpdate)))
			{
				this.ClearLocallySetPropertiesDefinedByStyle(value);
				this.SetPropertyValue<string>(this.WorksheetEntity.StyleNameProperty, value);
			}
		}

		internal void SetStyleNameAndExpandToFitNumberValuesWidth(string value)
		{
			this.ExecuteAndExpandToFitNumberValuesWidth(delegate
			{
				this.SetStyleName(value);
			});
		}

		void ClearLocallySetPropertiesDefinedByStyle(string styleName)
		{
			CellStyle cellStyle = this.Worksheet.Workbook.Styles[styleName];
			if (cellStyle == null)
			{
				return;
			}
			WorksheetEntityBase[] array = new WorksheetEntityBase[]
			{
				this.Worksheet.Cells,
				this.Worksheet.Rows,
				this.Worksheet.Columns
			};
			foreach (WorksheetEntityBase worksheetEntityBase in array)
			{
				foreach (IProperty property in worksheetEntityBase.Properties)
				{
					if (cellStyle.GetIsPropertyIncluded(property.PropertyDefinition))
					{
						if (property.PropertyDefinition == CellPropertyDefinitions.IsLockedProperty)
						{
							this.SetIsLockedLocalValueIfSheetIsProtected();
						}
						else
						{
							this.ClearPropertyValue(property, false);
						}
					}
				}
			}
		}

		void SetIsLockedLocalValueIfSheetIsProtected()
		{
			if (this.Worksheet.IsProtected)
			{
				bool value = this.GetIsLocked().Value;
				this.SetIsLocked(value);
			}
		}

		public void ClearStyleName()
		{
			this.ClearPropertyValue(this.WorksheetEntity.StyleNameProperty, true);
		}

		public RangePropertyValue<IFill> GetFill()
		{
			return this.GetPropertyValue<IFill>(this.WorksheetEntity.FillProperty);
		}

		public void SetFill(IFill value)
		{
			this.SetPropertyValue<IFill>(this.WorksheetEntity.FillProperty, value);
		}

		public void ClearFill()
		{
			this.ClearPropertyValue(this.WorksheetEntity.FillProperty, true);
		}

		public RangePropertyValue<ThemableFontFamily> GetFontFamily()
		{
			return this.GetPropertyValue<ThemableFontFamily>(this.WorksheetEntity.FontFamilyProperty);
		}

		public void SetFontFamily(ThemableFontFamily value)
		{
			this.SetPropertyValue<ThemableFontFamily>(this.WorksheetEntity.FontFamilyProperty, value);
		}

		internal void SetFontFamilyAndExpandToFitNumberValuesWidth(ThemableFontFamily value)
		{
			this.ExecuteAndExpandToFitNumberValuesWidth(delegate
			{
				this.SetFontFamily(value);
			});
		}

		public void ClearFontFamily()
		{
			this.ClearPropertyValue(this.WorksheetEntity.FontFamilyProperty, true);
		}

		public RangePropertyValue<double> GetFontSize()
		{
			return this.GetPropertyValue<double>(this.WorksheetEntity.FontSizeProperty);
		}

		public void SetFontSize(double value)
		{
			Guard.ThrowExceptionIfOutOfRange<double>(UnitHelper.PointToDip(1.0), UnitHelper.PointToDip(409.0), value, "value");
			this.SetPropertyValue<double>(this.WorksheetEntity.FontSizeProperty, value);
		}

		internal void SetFontSizeAndExpandToFitNumberValuesWidth(double value)
		{
			this.ExecuteAndExpandToFitNumberValuesWidth(delegate
			{
				this.SetFontSize(value);
			});
		}

		public void ClearFontSize()
		{
			this.ClearPropertyValue(this.WorksheetEntity.FontSizeProperty, true);
		}

		public RangePropertyValue<bool> GetIsBold()
		{
			return this.GetPropertyValue<bool>(this.WorksheetEntity.IsBoldProperty);
		}

		public void SetIsBold(bool value)
		{
			this.SetPropertyValue<bool>(this.WorksheetEntity.IsBoldProperty, value);
		}

		internal void SetIsBoldAndExpandToFitNumberValuesWidth(bool value)
		{
			this.ExecuteAndExpandToFitNumberValuesWidth(delegate
			{
				this.SetIsBold(value);
			});
		}

		public void ClearIsBold()
		{
			this.ClearPropertyValue(this.WorksheetEntity.IsBoldProperty, true);
		}

		public RangePropertyValue<bool> GetIsItalic()
		{
			return this.GetPropertyValue<bool>(this.WorksheetEntity.IsItalicProperty);
		}

		public void SetIsItalic(bool value)
		{
			this.SetPropertyValue<bool>(this.WorksheetEntity.IsItalicProperty, value);
		}

		internal void SetIsItalicAndExpandToFitNumberValuesWidth(bool value)
		{
			this.ExecuteAndExpandToFitNumberValuesWidth(delegate
			{
				this.SetIsItalic(value);
			});
		}

		public void ClearIsItalic()
		{
			this.ClearPropertyValue(this.WorksheetEntity.IsItalicProperty, true);
		}

		public RangePropertyValue<UnderlineType> GetUnderline()
		{
			return this.GetPropertyValue<UnderlineType>(this.WorksheetEntity.UnderlineProperty);
		}

		public void SetUnderline(UnderlineType value)
		{
			this.SetPropertyValue<UnderlineType>(this.WorksheetEntity.UnderlineProperty, value);
		}

		internal void SetUnderlineAndExpandToFitNumberValuesWidth(UnderlineType value)
		{
			this.ExecuteAndExpandToFitNumberValuesWidth(delegate
			{
				this.SetUnderline(value);
			});
		}

		public void ClearUnderline()
		{
			this.ClearPropertyValue(this.WorksheetEntity.UnderlineProperty, true);
		}

		public RangePropertyValue<ThemableColor> GetForeColor()
		{
			return this.GetPropertyValue<ThemableColor>(this.WorksheetEntity.ForeColorProperty);
		}

		public void SetForeColor(ThemableColor value)
		{
			this.SetPropertyValue<ThemableColor>(this.WorksheetEntity.ForeColorProperty, value);
		}

		public void ClearForeColor()
		{
			this.ClearPropertyValue(this.WorksheetEntity.ForeColorProperty, true);
		}

		public RangePropertyValue<RadHorizontalAlignment> GetHorizontalAlignment()
		{
			return this.GetPropertyValue<RadHorizontalAlignment>(this.WorksheetEntity.HorizontalAlignmentProperty);
		}

		public void SetHorizontalAlignment(RadHorizontalAlignment value)
		{
			using (new UpdateScope(new Action(this.BeginUpdate), new Action(this.EndUpdate)))
			{
				this.EnsureIndentThatRespectsHorizontalAlignment(value);
				this.SetPropertyValue<RadHorizontalAlignment>(this.WorksheetEntity.HorizontalAlignmentProperty, value);
			}
		}

		void EnsureIndentThatRespectsHorizontalAlignment(RadHorizontalAlignment value)
		{
			if (value != RadHorizontalAlignment.Left && value != RadHorizontalAlignment.Right && value != RadHorizontalAlignment.Distributed)
			{
				this.ClearIndent();
			}
		}

		public void ClearHorizontalAlignment()
		{
			this.ClearPropertyValue(this.WorksheetEntity.HorizontalAlignmentProperty, true);
		}

		public RangePropertyValue<RadVerticalAlignment> GetVerticalAlignment()
		{
			return this.GetPropertyValue<RadVerticalAlignment>(this.WorksheetEntity.VerticalAlignmentProperty);
		}

		public void SetVerticalAlignment(RadVerticalAlignment value)
		{
			this.SetPropertyValue<RadVerticalAlignment>(this.WorksheetEntity.VerticalAlignmentProperty, value);
		}

		public void ClearVerticalAlignment()
		{
			this.ClearPropertyValue(this.WorksheetEntity.VerticalAlignmentProperty, true);
		}

		public RangePropertyValue<int> GetIndent()
		{
			return this.GetPropertyValue<int>(this.WorksheetEntity.IndentProperty);
		}

		public void SetIndent(int value)
		{
			Guard.ThrowExceptionIfOutOfRange<int>(0, 250, value, "value");
			using (new UpdateScope(new Action(this.BeginUpdate), new Action(this.EndUpdate)))
			{
				this.EnsureHorizontalAlignmentThatRespectsIndent();
				this.SetPropertyValue<int>(this.WorksheetEntity.IndentProperty, value);
			}
		}

		internal void SetIndentAndExpandToFitNumberValuesWidth(int value)
		{
			this.ExecuteAndExpandToFitNumberValuesWidth(delegate
			{
				this.SetIndent(value);
			});
		}

		public void ClearIndent()
		{
			this.ClearPropertyValue(this.WorksheetEntity.IndentProperty, true);
		}

		public void IncreaseIndent()
		{
			using (new UpdateScope(new Action(this.BeginUpdate), new Action(this.EndUpdate)))
			{
				this.EnsureHorizontalAlignmentThatRespectsIndent();
				this.ExecuteAndExpandToFitNumberValuesWidth(delegate
				{
					this.ExecuteForEachRangeInsideBeginEndUpdate(delegate(CellRange cellRange)
					{
						UpdateCellPropertyCommandContext<int> context = new UpdateCellPropertyCommandContext<int>(this.Worksheet, CellPropertyDefinitions.IndentProperty, cellRange, delegate(int i)
						{
							i++;
							return Math.Min(i, SpreadsheetDefaultValues.MaxIndent);
						});
						this.Worksheet.ExecuteCommand<UpdateCellPropertyCommandContext<int>>(WorkbookCommands.UpdateIndent, context);
					});
				});
			}
		}

		public void DecreaseIndent()
		{
			using (new UpdateScope(new Action(this.BeginUpdate), new Action(this.EndUpdate)))
			{
				this.EnsureHorizontalAlignmentThatRespectsIndent();
				this.ExecuteAndExpandToFitNumberValuesWidth(delegate
				{
					this.ExecuteForEachRangeInsideBeginEndUpdate(delegate(CellRange cellRange)
					{
						UpdateCellPropertyCommandContext<int> context = new UpdateCellPropertyCommandContext<int>(this.Worksheet, CellPropertyDefinitions.IndentProperty, cellRange, delegate(int i)
						{
							i--;
							return Math.Max(i, 0);
						});
						this.Worksheet.ExecuteCommand<UpdateCellPropertyCommandContext<int>>(WorkbookCommands.UpdateIndent, context);
					});
				});
			}
		}

		void EnsureHorizontalAlignmentThatRespectsIndent()
		{
			RangePropertyValue<RadHorizontalAlignment> horizontalAlignment = this.GetHorizontalAlignment();
			if (horizontalAlignment.IsIndeterminate || !LayoutHelper.ShouldRespectIndent(horizontalAlignment.Value))
			{
				this.SetHorizontalAlignment(RadHorizontalAlignment.Left);
			}
		}

		public RangePropertyValue<bool> GetIsWrapped()
		{
			return this.GetPropertyValue<bool>(this.WorksheetEntity.IsWrappedProperty);
		}

		public void SetIsWrapped(bool value)
		{
			this.SetPropertyValue<bool>(this.WorksheetEntity.IsWrappedProperty, value);
		}

		internal void SetIsWrappedAndExpandToFitNumberValuesWidth(bool value)
		{
			this.ExecuteAndExpandToFitNumberValuesWidth(delegate
			{
				this.SetIsWrapped(value);
			});
		}

		public void ClearIsWrapped()
		{
			this.ClearPropertyValue(this.WorksheetEntity.IsWrappedProperty, true);
		}

		internal RangePropertyValue<CellBorder> GetLeftBorder()
		{
			return this.GetPropertyValue<CellBorder>(this.WorksheetEntity.LeftBorderProperty);
		}

		internal RangePropertyValue<CellBorder> GetRightBorder()
		{
			return this.GetPropertyValue<CellBorder>(this.WorksheetEntity.RightBorderProperty);
		}

		internal RangePropertyValue<CellBorder> GetTopBorder()
		{
			return this.GetPropertyValue<CellBorder>(this.WorksheetEntity.TopBorderProperty);
		}

		internal RangePropertyValue<CellBorder> GetBottomBorder()
		{
			return this.GetPropertyValue<CellBorder>(this.WorksheetEntity.BottomBorderProperty);
		}

		internal RangePropertyValue<CellBorder> GetDiagonalUpBorder()
		{
			return this.GetPropertyValue<CellBorder>(this.WorksheetEntity.DiagonalUpBorderProperty);
		}

		internal RangePropertyValue<CellBorder> GetDiagonalDownBorder()
		{
			return this.GetPropertyValue<CellBorder>(this.WorksheetEntity.DiagonalDownBorderProperty);
		}

		static CellBorder GetActualBorderByPriority(RangePropertyValue<CellBorder> firstBorder, RangePropertyValue<CellBorder> secondBorder, ThemeColorScheme colorScheme)
		{
			if (firstBorder == null)
			{
				if (!secondBorder.IsIndeterminate)
				{
					return secondBorder.Value;
				}
				return null;
			}
			else if (secondBorder == null)
			{
				if (!firstBorder.IsIndeterminate)
				{
					return firstBorder.Value;
				}
				return null;
			}
			else
			{
				if (firstBorder.IsIndeterminate || secondBorder.IsIndeterminate)
				{
					return null;
				}
				return CellBorder.GetWithMaxPriority(firstBorder.Value, secondBorder.Value, colorScheme);
			}
		}

		static CellBorder GetActualBorder(RangePropertyValue<CellBorder> border, ThemeColorScheme colorScheme)
		{
			return SelectionBase.GetActualBorderByPriority(border, border, colorScheme);
		}

		CellBorder GetRowHorizontalBorder(CellRange currentRow, bool isBottom = false)
		{
			Guard.ThrowExceptionIfGreaterThan<int>(1, currentRow.RowCount, "RowCount");
			CellRange cellRange;
			if (isBottom)
			{
				cellRange = currentRow;
				currentRow = currentRow.OffsetFromRow(1);
			}
			else
			{
				cellRange = currentRow.OffsetFromRow(-1);
			}
			RangePropertyValue<CellBorder> firstBorder = null;
			if (currentRow != null)
			{
				firstBorder = this.GetPropertyValue<CellBorder>(this.WorksheetEntity.TopBorderProperty, currentRow);
			}
			RangePropertyValue<CellBorder> secondBorder = null;
			if (cellRange != null)
			{
				secondBorder = this.GetPropertyValue<CellBorder>(this.WorksheetEntity.BottomBorderProperty, cellRange);
			}
			return SelectionBase.GetActualBorderByPriority(firstBorder, secondBorder, this.Worksheet.Workbook.Theme.ColorScheme);
		}

		CellBorder GetColumnVerticalBorder(CellRange currentColumn, bool isRight = false)
		{
			Guard.ThrowExceptionIfGreaterThan<int>(1, currentColumn.ColumnCount, "ColumnCount");
			CellRange cellRange;
			if (isRight)
			{
				cellRange = currentColumn;
				currentColumn = currentColumn.OffsetFromColumn(1);
			}
			else
			{
				cellRange = currentColumn.OffsetFromColumn(-1);
			}
			RangePropertyValue<CellBorder> firstBorder = null;
			if (currentColumn != null)
			{
				firstBorder = this.GetPropertyValue<CellBorder>(this.WorksheetEntity.LeftBorderProperty, currentColumn);
			}
			RangePropertyValue<CellBorder> secondBorder = null;
			if (cellRange != null)
			{
				secondBorder = this.GetPropertyValue<CellBorder>(this.WorksheetEntity.RightBorderProperty, cellRange);
			}
			return SelectionBase.GetActualBorderByPriority(firstBorder, secondBorder, this.Worksheet.Workbook.Theme.ColorScheme);
		}

		CellBorder GetInsideHorizontalBorder(CellRange cellRange)
		{
			if (cellRange.RowCount == 1)
			{
				return null;
			}
			CellRange cellRange2 = new CellRange(cellRange.FromIndex.RowIndex, cellRange.FromIndex.ColumnIndex, cellRange.ToIndex.RowIndex - 1, cellRange.ToIndex.ColumnIndex);
			RangePropertyValue<CellBorder> propertyValue = this.GetPropertyValue<CellBorder>(this.WorksheetEntity.BottomBorderProperty, cellRange2);
			RangePropertyValue<CellBorder> secondBorder = null;
			if (TelerikHelper.IsValidRowIndex(cellRange.FromIndex.RowIndex + 1))
			{
				CellRange cellRange3 = new CellRange(cellRange.FromIndex.RowIndex + 1, cellRange.FromIndex.ColumnIndex, cellRange.ToIndex.RowIndex, cellRange.ToIndex.ColumnIndex);
				secondBorder = this.GetPropertyValue<CellBorder>(this.WorksheetEntity.TopBorderProperty, cellRange3);
			}
			return SelectionBase.GetActualBorderByPriority(propertyValue, secondBorder, this.Worksheet.Workbook.Theme.ColorScheme);
		}

		CellBorder GetInsideVerticalBorder(CellRange cellRange)
		{
			if (cellRange.ColumnCount == 1)
			{
				return null;
			}
			CellRange cellRange2 = new CellRange(cellRange.FromIndex.RowIndex, cellRange.FromIndex.ColumnIndex, cellRange.ToIndex.RowIndex, cellRange.ToIndex.ColumnIndex - 1);
			RangePropertyValue<CellBorder> propertyValue = this.GetPropertyValue<CellBorder>(this.WorksheetEntity.RightBorderProperty, cellRange2);
			RangePropertyValue<CellBorder> secondBorder = null;
			if (TelerikHelper.IsValidColumnIndex(cellRange.FromIndex.ColumnIndex + 1))
			{
				CellRange cellRange3 = new CellRange(cellRange.FromIndex.RowIndex, cellRange.FromIndex.ColumnIndex + 1, cellRange.ToIndex.RowIndex, cellRange.ToIndex.ColumnIndex);
				secondBorder = this.GetPropertyValue<CellBorder>(this.WorksheetEntity.LeftBorderProperty, cellRange3);
			}
			return SelectionBase.GetActualBorderByPriority(propertyValue, secondBorder, this.Worksheet.Workbook.Theme.ColorScheme);
		}

		CellBorder GetDiagonalUpBorder(CellRange cellRange)
		{
			return SelectionBase.GetActualBorder(this.GetPropertyValue<CellBorder>(this.WorksheetEntity.DiagonalUpBorderProperty, cellRange), this.Worksheet.Workbook.Theme.ColorScheme);
		}

		CellBorder GetDiagonalDownBorder(CellRange cellRange)
		{
			return SelectionBase.GetActualBorder(this.GetPropertyValue<CellBorder>(this.WorksheetEntity.DiagonalDownBorderProperty, cellRange), this.Worksheet.Workbook.Theme.ColorScheme);
		}

		CellBorders GetCellBorders(CellRange cellRange)
		{
			Guard.ThrowExceptionIfNull<CellRange>(cellRange, "cellRange");
			CellBorder columnVerticalBorder = this.GetColumnVerticalBorder(cellRange.GetFirstColumn(), false);
			CellBorder rowHorizontalBorder = this.GetRowHorizontalBorder(cellRange.GetFirstRow(), false);
			CellBorder columnVerticalBorder2 = this.GetColumnVerticalBorder(cellRange.GetLastColumn(), true);
			CellBorder rowHorizontalBorder2 = this.GetRowHorizontalBorder(cellRange.GetLastRow(), true);
			CellBorder insideHorizontalBorder = this.GetInsideHorizontalBorder(cellRange);
			CellBorder insideVerticalBorder = this.GetInsideVerticalBorder(cellRange);
			CellBorder diagonalUpBorder = this.GetDiagonalUpBorder(cellRange);
			CellBorder diagonalDownBorder = this.GetDiagonalDownBorder(cellRange);
			return new CellBorders(columnVerticalBorder, rowHorizontalBorder, columnVerticalBorder2, rowHorizontalBorder2, insideHorizontalBorder, insideVerticalBorder, diagonalUpBorder, diagonalDownBorder);
		}

		public CellBorders GetBorders()
		{
			CellBorders cellBorders = null;
			foreach (CellRange cellRange in this.CellRanges)
			{
				CellBorders cellBorders2 = this.GetCellBorders(cellRange);
				if (cellBorders == null)
				{
					cellBorders = cellBorders2;
				}
				else if (cellBorders != cellBorders2)
				{
					cellBorders.MergeWith(cellBorders2);
				}
			}
			return cellBorders;
		}

		public virtual void SetBorders(CellBorders value)
		{
		}

		public void ClearBorders()
		{
			this.ClearPropertyValue(this.WorksheetEntity.LeftBorderProperty, true);
			this.ClearPropertyValue(this.WorksheetEntity.TopBorderProperty, true);
			this.ClearPropertyValue(this.WorksheetEntity.RightBorderProperty, true);
			this.ClearPropertyValue(this.WorksheetEntity.BottomBorderProperty, true);
			this.ClearPropertyValue(this.WorksheetEntity.DiagonalUpBorderProperty, true);
			this.ClearPropertyValue(this.WorksheetEntity.DiagonalDownBorderProperty, true);
		}

		public RangePropertyValue<bool> GetIsLocked()
		{
			return this.GetPropertyValue<bool>(this.WorksheetEntity.IsLockedProperty);
		}

		public void SetIsLocked(bool value)
		{
			this.SetPropertyValue<bool>(this.WorksheetEntity.IsLockedProperty, value);
		}

		public void ClearIsLocked()
		{
			this.ClearPropertyValue(this.WorksheetEntity.IsLockedProperty, true);
		}

		readonly Worksheet worksheet;

		readonly IEnumerable<CellRange> cellRanges;

		int beginUpdateCount;
	}
}
