using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Telerik.Windows.Documents.Spreadsheet.Commands.WorkbookCommands;
using Telerik.Windows.Documents.Spreadsheet.Core;
using Telerik.Windows.Documents.Spreadsheet.Core.DataStructures;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.PropertySystem
{
	public class CellStyle
	{
		public Workbook Workbook
		{
			get
			{
				return this.workbook;
			}
		}

		public string Name
		{
			get
			{
				return this.name;
			}
		}

		public CellStyleCategory Category
		{
			get
			{
				return this.category;
			}
		}

		public bool IsRemovable
		{
			get
			{
				return this.isRemovable;
			}
		}

		public bool IsRemoved
		{
			get
			{
				return this.Workbook == null || this.Workbook.Styles == null || !this.Workbook.Styles.Contains(this);
			}
		}

		public int? BuiltInId
		{
			get
			{
				return this.builtInId;
			}
		}

		public bool IsUpdateInProgress
		{
			get
			{
				return this.beginEndUpdateCount > 0;
			}
		}

		public CellBorder LeftBorder
		{
			get
			{
				return this.leftBorderProperty.GetValue();
			}
			set
			{
				this.SetStyleProperty<CellBorder>(this.leftBorderProperty, value);
			}
		}

		public CellBorder TopBorder
		{
			get
			{
				return this.topBorderProperty.GetValue();
			}
			set
			{
				this.SetStyleProperty<CellBorder>(this.topBorderProperty, value);
			}
		}

		public CellBorder RightBorder
		{
			get
			{
				return this.rightBorderProperty.GetValue();
			}
			set
			{
				this.SetStyleProperty<CellBorder>(this.rightBorderProperty, value);
			}
		}

		public CellBorder BottomBorder
		{
			get
			{
				return this.bottomBorderProperty.GetValue();
			}
			set
			{
				this.SetStyleProperty<CellBorder>(this.bottomBorderProperty, value);
			}
		}

		public CellBorder DiagonalUpBorder
		{
			get
			{
				return this.diagonalUpBorderProperty.GetValue();
			}
			set
			{
				this.SetStyleProperty<CellBorder>(this.diagonalUpBorderProperty, value);
			}
		}

		public CellBorder DiagonalDownBorder
		{
			get
			{
				return this.diagonalDownBorderProperty.GetValue();
			}
			set
			{
				this.SetStyleProperty<CellBorder>(this.diagonalDownBorderProperty, value);
			}
		}

		public IFill Fill
		{
			get
			{
				return this.fillProperty.GetValue();
			}
			set
			{
				this.SetStyleProperty<IFill>(this.fillProperty, value);
			}
		}

		public ThemableFontFamily FontFamily
		{
			get
			{
				return this.fontFamilyProperty.GetValue();
			}
			set
			{
				this.SetStyleProperty<ThemableFontFamily>(this.fontFamilyProperty, value);
			}
		}

		public double FontSize
		{
			get
			{
				return this.fontSizeProperty.GetValue();
			}
			set
			{
				this.SetStyleProperty<double>(this.fontSizeProperty, value);
			}
		}

		public bool IsBold
		{
			get
			{
				return this.isBoldProperty.GetValue();
			}
			set
			{
				this.SetStyleProperty<bool>(this.isBoldProperty, value);
			}
		}

		public bool IsItalic
		{
			get
			{
				return this.isItalicProperty.GetValue();
			}
			set
			{
				this.SetStyleProperty<bool>(this.isItalicProperty, value);
			}
		}

		public UnderlineType Underline
		{
			get
			{
				return this.underlineProperty.GetValue();
			}
			set
			{
				this.SetStyleProperty<UnderlineType>(this.underlineProperty, value);
			}
		}

		public ThemableColor ForeColor
		{
			get
			{
				return this.foreColorProperty.GetValue();
			}
			set
			{
				this.SetStyleProperty<ThemableColor>(this.foreColorProperty, value);
			}
		}

		public RadHorizontalAlignment HorizontalAlignment
		{
			get
			{
				return this.horizontalAlignmentProperty.GetValue();
			}
			set
			{
				this.SetStyleProperty<RadHorizontalAlignment>(this.horizontalAlignmentProperty, value);
			}
		}

		public RadVerticalAlignment VerticalAlignment
		{
			get
			{
				return this.verticalAlignmentProperty.GetValue();
			}
			set
			{
				this.SetStyleProperty<RadVerticalAlignment>(this.verticalAlignmentProperty, value);
			}
		}

		public int Indent
		{
			get
			{
				return this.indentProperty.GetValue();
			}
			set
			{
				this.SetStyleProperty<int>(this.indentProperty, value);
			}
		}

		public bool IsWrapped
		{
			get
			{
				return this.isWrappedProperty.GetValue();
			}
			set
			{
				this.SetStyleProperty<bool>(this.isWrappedProperty, value);
			}
		}

		public CellValueFormat Format
		{
			get
			{
				return this.formatProperty.GetValue();
			}
			set
			{
				this.SetStyleProperty<CellValueFormat>(this.formatProperty, value);
			}
		}

		public bool IsLocked
		{
			get
			{
				return this.isLockedProperty.GetValue();
			}
			set
			{
				this.SetStyleProperty<bool>(this.isLockedProperty, value);
			}
		}

		public bool IncludeNumber
		{
			get
			{
				return this.GetIsStylePropertyGroupIncluded(StylePropertyGroup.Number);
			}
			set
			{
				if (this.GetIsStylePropertyGroupIncluded(StylePropertyGroup.Number) != value)
				{
					this.SetIsStylePropertyGroupIncluded(StylePropertyGroup.Number, value);
				}
			}
		}

		public bool IncludeAlignment
		{
			get
			{
				return this.GetIsStylePropertyGroupIncluded(StylePropertyGroup.Alignment);
			}
			set
			{
				if (this.GetIsStylePropertyGroupIncluded(StylePropertyGroup.Alignment) != value)
				{
					this.SetIsStylePropertyGroupIncluded(StylePropertyGroup.Alignment, value);
				}
			}
		}

		public bool IncludeFont
		{
			get
			{
				return this.GetIsStylePropertyGroupIncluded(StylePropertyGroup.Font);
			}
			set
			{
				if (this.GetIsStylePropertyGroupIncluded(StylePropertyGroup.Font) != value)
				{
					this.SetIsStylePropertyGroupIncluded(StylePropertyGroup.Font, value);
				}
			}
		}

		public bool IncludeBorder
		{
			get
			{
				return this.GetIsStylePropertyGroupIncluded(StylePropertyGroup.Border);
			}
			set
			{
				if (this.GetIsStylePropertyGroupIncluded(StylePropertyGroup.Border) != value)
				{
					this.SetIsStylePropertyGroupIncluded(StylePropertyGroup.Border, value);
				}
			}
		}

		public bool IncludeFill
		{
			get
			{
				return this.GetIsStylePropertyGroupIncluded(StylePropertyGroup.Fill);
			}
			set
			{
				if (this.GetIsStylePropertyGroupIncluded(StylePropertyGroup.Fill) != value)
				{
					this.SetIsStylePropertyGroupIncluded(StylePropertyGroup.Fill, value);
				}
			}
		}

		public bool IncludeProtection
		{
			get
			{
				return this.GetIsStylePropertyGroupIncluded(StylePropertyGroup.Protection);
			}
			set
			{
				if (this.GetIsStylePropertyGroupIncluded(StylePropertyGroup.Protection) != value)
				{
					this.SetIsStylePropertyGroupIncluded(StylePropertyGroup.Protection, value);
				}
			}
		}

		static CellStyle()
		{
			CellStyle.propertyDefinitionToStyleProperty.Add(CellPropertyDefinitions.LeftBorderProperty, (CellStyle style) => style.leftBorderProperty);
			CellStyle.propertyDefinitionToStyleProperty.Add(CellPropertyDefinitions.TopBorderProperty, (CellStyle style) => style.topBorderProperty);
			CellStyle.propertyDefinitionToStyleProperty.Add(CellPropertyDefinitions.RightBorderProperty, (CellStyle style) => style.rightBorderProperty);
			CellStyle.propertyDefinitionToStyleProperty.Add(CellPropertyDefinitions.BottomBorderProperty, (CellStyle style) => style.bottomBorderProperty);
			CellStyle.propertyDefinitionToStyleProperty.Add(CellPropertyDefinitions.DiagonalUpBorderProperty, (CellStyle style) => style.diagonalUpBorderProperty);
			CellStyle.propertyDefinitionToStyleProperty.Add(CellPropertyDefinitions.DiagonalDownBorderProperty, (CellStyle style) => style.diagonalDownBorderProperty);
			CellStyle.propertyDefinitionToStyleProperty.Add(CellPropertyDefinitions.FillProperty, (CellStyle style) => style.fillProperty);
			CellStyle.propertyDefinitionToStyleProperty.Add(CellPropertyDefinitions.FontFamilyProperty, (CellStyle style) => style.fontFamilyProperty);
			CellStyle.propertyDefinitionToStyleProperty.Add(CellPropertyDefinitions.FontSizeProperty, (CellStyle style) => style.fontSizeProperty);
			CellStyle.propertyDefinitionToStyleProperty.Add(CellPropertyDefinitions.IsBoldProperty, (CellStyle style) => style.isBoldProperty);
			CellStyle.propertyDefinitionToStyleProperty.Add(CellPropertyDefinitions.IsItalicProperty, (CellStyle style) => style.isItalicProperty);
			CellStyle.propertyDefinitionToStyleProperty.Add(CellPropertyDefinitions.UnderlineProperty, (CellStyle style) => style.underlineProperty);
			CellStyle.propertyDefinitionToStyleProperty.Add(CellPropertyDefinitions.ForeColorProperty, (CellStyle style) => style.foreColorProperty);
			CellStyle.propertyDefinitionToStyleProperty.Add(CellPropertyDefinitions.HorizontalAlignmentProperty, (CellStyle style) => style.horizontalAlignmentProperty);
			CellStyle.propertyDefinitionToStyleProperty.Add(CellPropertyDefinitions.VerticalAlignmentProperty, (CellStyle style) => style.verticalAlignmentProperty);
			CellStyle.propertyDefinitionToStyleProperty.Add(CellPropertyDefinitions.IndentProperty, (CellStyle style) => style.indentProperty);
			CellStyle.propertyDefinitionToStyleProperty.Add(CellPropertyDefinitions.IsWrappedProperty, (CellStyle style) => style.isWrappedProperty);
			CellStyle.propertyDefinitionToStyleProperty.Add(CellPropertyDefinitions.FormatProperty, (CellStyle style) => style.formatProperty);
			CellStyle.propertyDefinitionToStyleProperty.Add(CellPropertyDefinitions.IsLockedProperty, (CellStyle style) => style.isLockedProperty);
		}

		internal CellStyle(Workbook workbook, string name, CellStyleCategory category, bool isRemoveable = true, int? builtinId = null)
		{
			Guard.ThrowExceptionIfNullOrEmpty(name, "name");
			this.workbook = workbook;
			this.name = name;
			this.category = category;
			this.isRemovable = isRemoveable;
			this.builtInId = builtinId;
			this.changedProperties = new HashSet<IPropertyDefinition>();
			this.leftBorderProperty = new StyleProperty<CellBorder>(this, CellPropertyDefinitions.LeftBorderProperty);
			this.topBorderProperty = new StyleProperty<CellBorder>(this, CellPropertyDefinitions.TopBorderProperty);
			this.rightBorderProperty = new StyleProperty<CellBorder>(this, CellPropertyDefinitions.RightBorderProperty);
			this.bottomBorderProperty = new StyleProperty<CellBorder>(this, CellPropertyDefinitions.BottomBorderProperty);
			this.diagonalUpBorderProperty = new StyleProperty<CellBorder>(this, CellPropertyDefinitions.DiagonalUpBorderProperty);
			this.diagonalDownBorderProperty = new StyleProperty<CellBorder>(this, CellPropertyDefinitions.DiagonalDownBorderProperty);
			this.fillProperty = new StyleProperty<IFill>(this, CellPropertyDefinitions.FillProperty);
			this.fontFamilyProperty = new StyleProperty<ThemableFontFamily>(this, CellPropertyDefinitions.FontFamilyProperty);
			this.fontSizeProperty = new StyleProperty<double>(this, CellPropertyDefinitions.FontSizeProperty);
			this.isBoldProperty = new StyleProperty<bool>(this, CellPropertyDefinitions.IsBoldProperty);
			this.isItalicProperty = new StyleProperty<bool>(this, CellPropertyDefinitions.IsItalicProperty);
			this.underlineProperty = new StyleProperty<UnderlineType>(this, CellPropertyDefinitions.UnderlineProperty);
			this.foreColorProperty = new StyleProperty<ThemableColor>(this, CellPropertyDefinitions.ForeColorProperty);
			this.horizontalAlignmentProperty = new StyleProperty<RadHorizontalAlignment>(this, CellPropertyDefinitions.HorizontalAlignmentProperty);
			this.verticalAlignmentProperty = new StyleProperty<RadVerticalAlignment>(this, CellPropertyDefinitions.VerticalAlignmentProperty);
			this.indentProperty = new StyleProperty<int>(this, CellPropertyDefinitions.IndentProperty);
			this.isWrappedProperty = new StyleProperty<bool>(this, CellPropertyDefinitions.IsWrappedProperty);
			this.formatProperty = new StyleProperty<CellValueFormat>(this, CellPropertyDefinitions.FormatProperty);
			this.isLockedProperty = new StyleProperty<bool>(this, CellPropertyDefinitions.IsLockedProperty);
			this.stylePropertyGroupToIsIncluded = new Dictionary<StylePropertyGroup, bool>();
			foreach (object obj in Enum.GetValues(typeof(StylePropertyGroup)))
			{
				StylePropertyGroup key = (StylePropertyGroup)obj;
				this.stylePropertyGroupToIsIncluded.Add(key, false);
			}
		}

		void SetStyleProperty<T>(StyleProperty<T> property, T value)
		{
			if (this.Workbook != null && this.Workbook.History != null)
			{
				this.Workbook.History.BeginUndoGroup();
			}
			property.SetValue(value);
			this.SetIsStylePropertyGroupIncluded(property.PropertyDefinition.StylePropertyGroup, true);
			if (this.Workbook != null && this.Workbook.History != null)
			{
				this.Workbook.History.EndUndoGroup();
			}
		}

		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static CellStyle CreateTempStyle()
		{
			return new CellStyle(null, Guid.NewGuid().ToString(), CellStyleCategory.Custom, true, null);
		}

		public void CopyPropertiesFrom(CellStyle fromStyle)
		{
			Guard.ThrowExceptionIfNull<CellStyle>(fromStyle, "fromStyle");
			using (new UpdateScope(new Action(this.BeginUpdate), new Action(this.EndUpdate)))
			{
				foreach (KeyValuePair<IPropertyDefinition, Func<CellStyle, StylePropertyBase>> keyValuePair in CellStyle.propertyDefinitionToStyleProperty)
				{
					Func<CellStyle, StylePropertyBase> value = keyValuePair.Value;
					object valueAsObject = value(fromStyle).GetValueAsObject();
					value(this).SetValue(valueAsObject);
				}
				foreach (object obj in Enum.GetValues(typeof(StylePropertyGroup)))
				{
					StylePropertyGroup stylePropertyGroup = (StylePropertyGroup)obj;
					bool isStylePropertyGroupIncluded = fromStyle.GetIsStylePropertyGroupIncluded(stylePropertyGroup);
					this.SetIsStylePropertyGroupIncluded(stylePropertyGroup, isStylePropertyGroupIncluded);
				}
			}
		}

		public bool HasSameProperties(CellStyle otherStyle)
		{
			foreach (KeyValuePair<IPropertyDefinition, Func<CellStyle, StylePropertyBase>> keyValuePair in CellStyle.propertyDefinitionToStyleProperty)
			{
				Func<CellStyle, StylePropertyBase> value = keyValuePair.Value;
				if (!TelerikHelper.EqualsOfT<object>(value(this).GetValueAsObject(), value(otherStyle).GetValueAsObject()))
				{
					return false;
				}
			}
			return true;
		}

		public static IEnumerable<IPropertyDefinition> GetAllProperties()
		{
			return CellStyle.propertyDefinitionToStyleProperty.Keys;
		}

		public void BeginUpdate()
		{
			this.beginEndUpdateCount++;
		}

		public void EndUpdate()
		{
			if (this.beginEndUpdateCount == 0)
			{
				throw new InvalidOperationException("There is no active update to end.");
			}
			this.beginEndUpdateCount--;
			if (this.beginEndUpdateCount == 0 && this.changedProperties.Count > 0)
			{
				this.OnChanged();
				this.changedProperties.Clear();
			}
		}

		internal void SuspendForcingLayoutUpdate()
		{
			this.suspendForcingLayoutCount++;
		}

		internal void ResumeForcingLayoutUpdate()
		{
			if (this.suspendForcingLayoutCount == 0)
			{
				throw new InvalidOperationException("There is no active update to end.");
			}
			this.suspendForcingLayoutCount--;
		}

		public static bool IsSupportedProperty(IPropertyDefinition propertyDefinition)
		{
			Guard.ThrowExceptionIfNull<IPropertyDefinition>(propertyDefinition, "propertyDefinition");
			return CellStyle.propertyDefinitionToStyleProperty.ContainsKey(propertyDefinition);
		}

		public bool IsPropertyValueSet<T>(IPropertyDefinition<T> propertyDefinition)
		{
			Guard.ThrowExceptionIfNull<IPropertyDefinition<T>>(propertyDefinition, "propertyDefinition");
			StylePropertyBase stylePropertyFromPropertyDefinition = this.GetStylePropertyFromPropertyDefinition(propertyDefinition);
			return ((StyleProperty<T>)stylePropertyFromPropertyDefinition).IsValueSet;
		}

		public bool GetIsPropertyIncluded(IPropertyDefinition propertyDefinition)
		{
			return this.stylePropertyGroupToIsIncluded[propertyDefinition.StylePropertyGroup];
		}

		StylePropertyBase GetStylePropertyFromPropertyDefinition(IPropertyDefinition propertyDefinition)
		{
			Func<CellStyle, StylePropertyBase> func;
			if (!CellStyle.propertyDefinitionToStyleProperty.TryGetValue(propertyDefinition, out func))
			{
				throw new InvalidOperationException(string.Format("The {0} property is not supported in styles", propertyDefinition.Name));
			}
			return func(this);
		}

		public bool GetIsStylePropertyGroupIncluded(StylePropertyGroup stylePropertyGroup)
		{
			return this.stylePropertyGroupToIsIncluded[stylePropertyGroup];
		}

		public void SetIsStylePropertyGroupIncluded(StylePropertyGroup stylePropertyGroup, bool value)
		{
			if (!this.IsRemoved && this.suspendForcingLayoutCount == 0)
			{
				using (new UpdateScope(new Action(this.Workbook.History.BeginUndoGroup), new Action(this.Workbook.History.EndUndoGroup)))
				{
					if (!TelerikHelper.EqualsOfT<bool>(this.GetIsStylePropertyGroupIncluded(stylePropertyGroup), value))
					{
						SetStylePropertyGroupIncludedCommandContext context = new SetStylePropertyGroupIncludedCommandContext(this.Workbook, this, stylePropertyGroup, this.GetIsStylePropertyGroupIncluded(stylePropertyGroup), value);
						this.Workbook.ExecuteCommand<SetStylePropertyGroupIncludedCommandContext>(WorkbookCommands.SetStylePropertyIsIncluded, context);
						if (this.Name != SpreadsheetDefaultValues.DefaultStyleName)
						{
							ReapplyCellStyleCommandContext context2 = new ReapplyCellStyleCommandContext(this.Workbook, this.Name);
							this.Workbook.ExecuteCommand<ReapplyCellStyleCommandContext>(WorkbookCommands.ReapplyCellStyleCommand, context2);
						}
						if (!value)
						{
							this.ClearLocalPropertiesThatAreNotNeeded(stylePropertyGroup);
						}
					}
					return;
				}
			}
			this.SetIsStylePropertyGroupIncludedInternal(stylePropertyGroup, value);
		}

		void ClearLocalPropertiesThatAreNotNeeded(StylePropertyGroup stylePropertyGroup)
		{
			if (stylePropertyGroup == StylePropertyGroup.None)
			{
				return;
			}
			using (new UpdateScope(new Action(this.workbook.History.BeginUndoGroup), new Action(this.workbook.History.EndUndoGroup)))
			{
				IEnumerable<IPropertyDefinition> propertyGroupProperties = this.GetPropertyGroupProperties(stylePropertyGroup);
				foreach (Worksheet worksheet in this.workbook.Worksheets)
				{
					ICompressedList<string> propertyValueCollection = worksheet.Cells.PropertyBag.GetPropertyValueCollection<string>(CellPropertyDefinitions.StyleNameProperty);
					IEnumerable<Range<long, string>> enumerable = from x in propertyValueCollection
						where x.Value == this.Name
						select x;
					foreach (Range<long, string> range in enumerable)
					{
						CellRange cellRange = WorksheetPropertyBagBase.ConvertLongCellRangeToCellRange(range.Start, range.End);
						foreach (IPropertyDefinition propertyDefinition in propertyGroupProperties)
						{
							IProperty cellPropertyFromPropertyDefinition = this.GetCellPropertyFromPropertyDefinition(worksheet, propertyDefinition);
							if (cellPropertyFromPropertyDefinition != null)
							{
								worksheet.Cells[cellRange].ClearPropertyValue(cellPropertyFromPropertyDefinition, true);
							}
						}
					}
				}
			}
		}

		IProperty GetCellPropertyFromPropertyDefinition(Worksheet sheet, IPropertyDefinition propertyDefinition)
		{
			return (from p in sheet.Cells.Properties
				where p.PropertyDefinition == propertyDefinition
				select p).FirstOrDefault<IProperty>();
		}

		internal void SetIsStylePropertyGroupIncludedInternal(StylePropertyGroup stylePropertyGroup, bool value)
		{
			if (this.stylePropertyGroupToIsIncluded[stylePropertyGroup] == value)
			{
				return;
			}
			this.stylePropertyGroupToIsIncluded[stylePropertyGroup] = value;
			IEnumerable<IPropertyDefinition> propertyGroupProperties = this.GetPropertyGroupProperties(stylePropertyGroup);
			using (new UpdateScope(new Action(this.BeginUpdate), new Action(this.EndUpdate)))
			{
				foreach (IPropertyDefinition proeprtyDefinition in propertyGroupProperties)
				{
					this.OnPropertyChanged(proeprtyDefinition);
				}
			}
		}

		IEnumerable<IPropertyDefinition> GetPropertyGroupProperties(StylePropertyGroup stylePropertyGroup)
		{
			return from x in CellPropertyDefinitions.AllPropertyDefinitions
				where x.StylePropertyGroup == stylePropertyGroup
				select x;
		}

		public T GetPropertyValue<T>(IPropertyDefinition<T> propertyDefinition)
		{
			Guard.ThrowExceptionIfNull<IPropertyDefinition<T>>(propertyDefinition, "propertyDefinition");
			Func<CellStyle, StylePropertyBase> func;
			if (!CellStyle.propertyDefinitionToStyleProperty.TryGetValue(propertyDefinition, out func))
			{
				throw new InvalidOperationException(string.Format("The {0} property is not supported in styles", propertyDefinition.Name));
			}
			return ((StyleProperty<T>)func(this)).GetValue();
		}

		internal void OnPropertyChanged(IPropertyDefinition proeprtyDefinition)
		{
			using (new UpdateScope(new Action(this.BeginUpdate), new Action(this.EndUpdate)))
			{
				this.changedProperties.Add(proeprtyDefinition);
			}
		}

		public override bool Equals(object obj)
		{
			CellStyle cellStyle = obj as CellStyle;
			return cellStyle != null && (this.Name.Equals(cellStyle.Name) && this.Category.Equals(cellStyle.Category) && this.LeftBorder.Equals(cellStyle.LeftBorder) && this.TopBorder.Equals(cellStyle.TopBorder) && this.RightBorder.Equals(cellStyle.RightBorder) && this.BottomBorder.Equals(cellStyle.BottomBorder) && this.DiagonalUpBorder.Equals(cellStyle.DiagonalUpBorder) && this.DiagonalDownBorder.Equals(cellStyle.DiagonalDownBorder) && this.Fill.Equals(cellStyle.Fill) && this.FontFamily.Equals(cellStyle.FontFamily) && this.FontSize.Equals(cellStyle.FontSize) && this.IsBold.Equals(cellStyle.IsBold) && this.IsItalic.Equals(cellStyle.IsItalic) && this.Underline.Equals(cellStyle.Underline) && this.ForeColor.Equals(cellStyle.ForeColor) && this.HorizontalAlignment.Equals(cellStyle.HorizontalAlignment) && this.VerticalAlignment.Equals(cellStyle.VerticalAlignment) && this.Indent.Equals(cellStyle.Indent) && this.IsWrapped.Equals(cellStyle.IsWrapped) && this.IsLocked.Equals(cellStyle.IsLocked) && this.IncludeNumber.Equals(cellStyle.IncludeNumber) && this.IncludeAlignment.Equals(cellStyle.IncludeAlignment) && this.IncludeFont.Equals(cellStyle.IncludeFont) && this.IncludeBorder.Equals(cellStyle.IncludeBorder) && this.IncludeFill.Equals(cellStyle.IncludeFill)) && this.IncludeProtection.Equals(cellStyle.IncludeProtection);
		}

		public override int GetHashCode()
		{
			return TelerikHelper.CombineHashCodes(TelerikHelper.CombineHashCodes(this.Name.GetHashCode(), this.Category.GetHashCode(), this.LeftBorder.GetHashCode(), this.TopBorder.GetHashCode(), this.RightBorder.GetHashCode(), this.BottomBorder.GetHashCode(), this.DiagonalUpBorder.GetHashCode(), this.DiagonalDownBorder.GetHashCode(), this.Fill.GetHashCode()), TelerikHelper.CombineHashCodes(this.FontFamily.GetHashCode(), this.FontSize.GetHashCode(), this.IsBold.GetHashCode(), this.IsItalic.GetHashCode(), this.Underline.GetHashCode(), this.ForeColor.GetHashCode(), this.HorizontalAlignment.GetHashCode(), this.VerticalAlignment.GetHashCode(), this.Indent.GetHashCode()), TelerikHelper.CombineHashCodes(this.IsWrapped.GetHashCode(), this.IsLocked.GetHashCode(), this.IncludeNumber.GetHashCode(), this.IncludeAlignment.GetHashCode(), this.IncludeFont.GetHashCode(), this.IncludeBorder.GetHashCode(), this.IncludeFill.GetHashCode(), this.IncludeProtection.GetHashCode()));
		}

		public event EventHandler<StyleChangedEventArgs> Changed;

		protected virtual void OnChanged()
		{
			Guard.ThrowExceptionIfTrue(this.IsUpdateInProgress, "IsUpdateInProgress");
			if (this.Changed != null)
			{
				this.Changed(this, new StyleChangedEventArgs(this.changedProperties));
			}
		}

		static readonly Dictionary<IPropertyDefinition, Func<CellStyle, StylePropertyBase>> propertyDefinitionToStyleProperty = new Dictionary<IPropertyDefinition, Func<CellStyle, StylePropertyBase>>();

		readonly Dictionary<StylePropertyGroup, bool> stylePropertyGroupToIsIncluded;

		readonly Workbook workbook;

		readonly string name;

		readonly CellStyleCategory category;

		readonly bool isRemovable;

		readonly int? builtInId;

		int beginEndUpdateCount;

		int suspendForcingLayoutCount;

		readonly HashSet<IPropertyDefinition> changedProperties;

		readonly StyleProperty<CellBorder> leftBorderProperty;

		readonly StyleProperty<CellBorder> topBorderProperty;

		readonly StyleProperty<CellBorder> rightBorderProperty;

		readonly StyleProperty<CellBorder> bottomBorderProperty;

		readonly StyleProperty<CellBorder> diagonalUpBorderProperty;

		readonly StyleProperty<CellBorder> diagonalDownBorderProperty;

		readonly StyleProperty<IFill> fillProperty;

		readonly StyleProperty<ThemableFontFamily> fontFamilyProperty;

		readonly StyleProperty<double> fontSizeProperty;

		readonly StyleProperty<bool> isBoldProperty;

		readonly StyleProperty<bool> isItalicProperty;

		readonly StyleProperty<UnderlineType> underlineProperty;

		readonly StyleProperty<ThemableColor> foreColorProperty;

		readonly StyleProperty<RadHorizontalAlignment> horizontalAlignmentProperty;

		readonly StyleProperty<RadVerticalAlignment> verticalAlignmentProperty;

		readonly StyleProperty<int> indentProperty;

		readonly StyleProperty<bool> isWrappedProperty;

		readonly StyleProperty<CellValueFormat> formatProperty;

		readonly StyleProperty<bool> isLockedProperty;
	}
}
