using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Copying;
using Telerik.Windows.Documents.Spreadsheet.Core.DataStructures;
using Telerik.Windows.Documents.Spreadsheet.PropertySystem;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model
{
	public abstract class WorksheetEntityBase
	{
		internal abstract PropertyBagBase PropertyBagBase { get; }

		public Worksheet Worksheet
		{
			get
			{
				return this.worksheet;
			}
		}

		internal IEnumerable<IProperty> Properties
		{
			get
			{
				return this.properties.Values;
			}
		}

		internal IProperty<string> StyleNameProperty
		{
			get
			{
				return this.styleNameProperty;
			}
		}

		internal IProperty<CellValueFormat> FormatProperty
		{
			get
			{
				return this.formatProperty;
			}
		}

		internal IProperty<IFill> FillProperty
		{
			get
			{
				return this.fillProperty;
			}
		}

		internal IProperty<ThemableFontFamily> FontFamilyProperty
		{
			get
			{
				return this.fontFamilyProperty;
			}
		}

		internal IProperty<double> FontSizeProperty
		{
			get
			{
				return this.fontSizeProperty;
			}
		}

		internal IProperty<bool> IsBoldProperty
		{
			get
			{
				return this.isBoldProperty;
			}
		}

		internal IProperty<bool> IsItalicProperty
		{
			get
			{
				return this.isItalicProperty;
			}
		}

		internal IProperty<UnderlineType> UnderlineProperty
		{
			get
			{
				return this.underlineTypeProperty;
			}
		}

		internal IProperty<ThemableColor> ForeColorProperty
		{
			get
			{
				return this.foreColorProperty;
			}
		}

		internal IProperty<RadHorizontalAlignment> HorizontalAlignmentProperty
		{
			get
			{
				return this.horizontalAlignmentProperty;
			}
		}

		internal IProperty<RadVerticalAlignment> VerticalAlignmentProperty
		{
			get
			{
				return this.verticalAlignmentProperty;
			}
		}

		internal IProperty<int> IndentProperty
		{
			get
			{
				return this.indentProperty;
			}
		}

		internal IProperty<bool> IsWrappedProperty
		{
			get
			{
				return this.isWrappedProperty;
			}
		}

		internal IProperty<CellBorder> LeftBorderProperty
		{
			get
			{
				return this.leftBorderProperty;
			}
		}

		internal IProperty<CellBorder> RightBorderProperty
		{
			get
			{
				return this.rightBorderProperty;
			}
		}

		internal IProperty<CellBorder> TopBorderProperty
		{
			get
			{
				return this.topBorderProperty;
			}
		}

		internal IProperty<CellBorder> BottomBorderProperty
		{
			get
			{
				return this.bottomBorderProperty;
			}
		}

		internal IProperty<CellBorder> DiagonalUpBorderProperty
		{
			get
			{
				return this.diagonalUpBorderProperty;
			}
		}

		internal IProperty<CellBorder> DiagonalDownBorderProperty
		{
			get
			{
				return this.diagonalDownBorderProperty;
			}
		}

		internal IProperty<bool> IsLockedProperty
		{
			get
			{
				return this.isLockedProperty;
			}
		}

		protected WorksheetEntityBase(Worksheet worksheet)
		{
			Guard.ThrowExceptionIfNull<Worksheet>(worksheet, "worksheet");
			this.worksheet = worksheet;
			this.properties = new Dictionary<IPropertyDefinition, IProperty>();
			this.formatProperty = this.CreateProperty<CellValueFormat>(CellPropertyDefinitions.FormatProperty);
			this.styleNameProperty = this.CreateProperty<string>(CellPropertyDefinitions.StyleNameProperty);
			this.fillProperty = this.CreateProperty<IFill>(CellPropertyDefinitions.FillProperty);
			this.fontFamilyProperty = this.CreateProperty<ThemableFontFamily>(CellPropertyDefinitions.FontFamilyProperty);
			this.fontSizeProperty = this.CreateProperty<double>(CellPropertyDefinitions.FontSizeProperty);
			this.isBoldProperty = this.CreateProperty<bool>(CellPropertyDefinitions.IsBoldProperty);
			this.isItalicProperty = this.CreateProperty<bool>(CellPropertyDefinitions.IsItalicProperty);
			this.underlineTypeProperty = this.CreateProperty<UnderlineType>(CellPropertyDefinitions.UnderlineProperty);
			this.foreColorProperty = this.CreateProperty<ThemableColor>(CellPropertyDefinitions.ForeColorProperty);
			this.horizontalAlignmentProperty = this.CreateProperty<RadHorizontalAlignment>(CellPropertyDefinitions.HorizontalAlignmentProperty);
			this.verticalAlignmentProperty = this.CreateProperty<RadVerticalAlignment>(CellPropertyDefinitions.VerticalAlignmentProperty);
			this.indentProperty = this.CreateProperty<int>(CellPropertyDefinitions.IndentProperty);
			this.isWrappedProperty = this.CreateProperty<bool>(CellPropertyDefinitions.IsWrappedProperty);
			this.isLockedProperty = this.CreateProperty<bool>(CellPropertyDefinitions.IsLockedProperty);
			this.leftBorderProperty = this.CreateProperty<CellBorder>(CellPropertyDefinitions.LeftBorderProperty);
			this.rightBorderProperty = this.CreateProperty<CellBorder>(CellPropertyDefinitions.RightBorderProperty);
			this.topBorderProperty = this.CreateProperty<CellBorder>(CellPropertyDefinitions.TopBorderProperty);
			this.bottomBorderProperty = this.CreateProperty<CellBorder>(CellPropertyDefinitions.BottomBorderProperty);
			this.diagonalUpBorderProperty = this.CreateProperty<CellBorder>(CellPropertyDefinitions.DiagonalUpBorderProperty);
			this.diagonalDownBorderProperty = this.CreateProperty<CellBorder>(CellPropertyDefinitions.DiagonalDownBorderProperty);
		}

		internal IProperty<T> CreateProperty<T>(IPropertyDefinition<T> propertyDefinition)
		{
			Guard.ThrowExceptionIfNull<IPropertyDefinition<T>>(propertyDefinition, "propertyDefinition");
			IProperty<T> property = this.CreatePropertyOverride<T>(propertyDefinition);
			this.AddToProperties(property);
			return property;
		}

		internal abstract IProperty<T> CreatePropertyOverride<T>(IPropertyDefinition<T> propertyDefinition);

		void AddToProperties(IProperty property)
		{
			this.properties.Add(property.PropertyDefinition, property);
		}

		internal bool TryGetProperyFromPropertyDefinition<T>(IPropertyDefinition propertyDefinition, out IProperty<T> property)
		{
			IProperty property2;
			bool result = this.TryGetProperyFromPropertyDefinition(propertyDefinition, out property2);
			property = property2 as IProperty<T>;
			return result;
		}

		internal bool TryGetProperyFromPropertyDefinition(IPropertyDefinition propertyDefinition, out IProperty property)
		{
			return this.properties.TryGetValue(propertyDefinition, out property);
		}

		internal bool ContainsPropertyDefinition(IPropertyDefinition property)
		{
			return this.properties.ContainsKey(property);
		}

		internal void CopyFrom(WorksheetEntityBase fromWorksheetEntity, CopyContext cloneContext)
		{
			this.CopyFromOverride(fromWorksheetEntity, cloneContext, this.Properties);
		}

		internal virtual void CopyFromOverride(WorksheetEntityBase fromWorksheetEntity, CopyContext cloneContext, IEnumerable<IProperty> properties)
		{
			foreach (IProperty property in properties)
			{
				ICompressedList propertyValueCollection = fromWorksheetEntity.PropertyBagBase.GetPropertyValueCollection(property.PropertyDefinition);
				ICompressedList propertyValueCollection2 = this.PropertyBagBase.GetPropertyValueCollection(property.PropertyDefinition);
				foreach (LongRange longRange in propertyValueCollection.GetRanges(false))
				{
					ICompressedList value = propertyValueCollection.GetValue(longRange.Start, longRange.End);
					propertyValueCollection2.SetValue(value);
				}
			}
		}

		internal virtual void Clear()
		{
			foreach (IProperty property in this.Properties)
			{
				this.PropertyBagBase.ClearPropertyValue(property.PropertyDefinition);
			}
		}

		readonly Worksheet worksheet;

		readonly Dictionary<IPropertyDefinition, IProperty> properties;

		readonly IProperty<string> styleNameProperty;

		readonly IProperty<CellValueFormat> formatProperty;

		readonly IProperty<IFill> fillProperty;

		readonly IProperty<ThemableFontFamily> fontFamilyProperty;

		readonly IProperty<double> fontSizeProperty;

		readonly IProperty<bool> isBoldProperty;

		readonly IProperty<bool> isItalicProperty;

		readonly IProperty<UnderlineType> underlineTypeProperty;

		readonly IProperty<ThemableColor> foreColorProperty;

		readonly IProperty<RadHorizontalAlignment> horizontalAlignmentProperty;

		readonly IProperty<RadVerticalAlignment> verticalAlignmentProperty;

		readonly IProperty<int> indentProperty;

		readonly IProperty<bool> isWrappedProperty;

		readonly IProperty<CellBorder> leftBorderProperty;

		readonly IProperty<CellBorder> rightBorderProperty;

		readonly IProperty<CellBorder> topBorderProperty;

		readonly IProperty<CellBorder> bottomBorderProperty;

		readonly IProperty<CellBorder> diagonalUpBorderProperty;

		readonly IProperty<CellBorder> diagonalDownBorderProperty;

		readonly IProperty<bool> isLockedProperty;
	}
}
