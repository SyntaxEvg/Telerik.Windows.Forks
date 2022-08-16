using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Flow.Model.Styles.Core;
using Telerik.Windows.Documents.Primitives;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Flow.Model.Styles
{
	public sealed class TableCellProperties : DocumentElementPropertiesBase, IPropertiesWithShading, IPropertiesWithPadding
	{
		internal TableCellProperties(Style style)
			: base(style)
		{
			this.InitProperties();
		}

		internal TableCellProperties(TableCell cell)
			: base(cell, false)
		{
			this.InitProperties();
		}

		public IStyleProperty<TableCellBorders> Borders
		{
			get
			{
				return this.bordersStyleProperty;
			}
		}

		public IStyleProperty<ThemableColor> BackgroundColor
		{
			get
			{
				return this.backgroundColorStyleProperty;
			}
		}

		public IStyleProperty<ThemableColor> ShadingPatternColor
		{
			get
			{
				return this.shadingPatternColorStyleProperty;
			}
		}

		public IStyleProperty<ShadingPattern?> ShadingPattern
		{
			get
			{
				return this.shadingPatternStyleProperty;
			}
		}

		public IStyleProperty<Padding> Padding
		{
			get
			{
				return this.paddingStyleProperty;
			}
		}

		public IStyleProperty<int?> ColumnSpan
		{
			get
			{
				return this.columnSpanStyleProperty;
			}
		}

		public IStyleProperty<int?> RowSpan
		{
			get
			{
				return this.rowSpanStyleProperty;
			}
		}

		public IStyleProperty<bool?> IgnoreCellMarkerInRowHeightCalculation
		{
			get
			{
				return this.ignoreCellMarkerInRowHeightCalculationStyleProperty;
			}
		}

		public IStyleProperty<bool?> CanWrapContent
		{
			get
			{
				return this.canWrapContentStyleProperty;
			}
		}

		public IStyleProperty<TableWidthUnit> PreferredWidth
		{
			get
			{
				return this.preferredWidthStyleProperty;
			}
		}

		public IStyleProperty<VerticalAlignment?> VerticalAlignment
		{
			get
			{
				return this.verticalAlignmentStyleProperty;
			}
		}

		public IStyleProperty<TextDirection> TextDirection
		{
			get
			{
				return this.textDirectionStyleProperty;
			}
		}

		RadFlowDocument IPropertiesWithShading.Document
		{
			get
			{
				return base.Document;
			}
		}

		protected override IEnumerable<IStyleProperty> EnumerateStyleProperties()
		{
			yield return this.Borders;
			yield return this.BackgroundColor;
			yield return this.ShadingPatternColor;
			yield return this.ShadingPattern;
			yield return this.Padding;
			yield return this.ColumnSpan;
			yield return this.RowSpan;
			yield return this.IgnoreCellMarkerInRowHeightCalculation;
			yield return this.CanWrapContent;
			yield return this.PreferredWidth;
			yield return this.VerticalAlignment;
			yield return this.TextDirection;
			yield break;
		}

		protected override IStyleProperty GetStylePropertyOverride(IStylePropertyDefinition propertyDefinition)
		{
			if (propertyDefinition != DocumentElementPropertiesBase.StyleIdPropertyDefinition && propertyDefinition.StylePropertyType != StylePropertyType.TableCell)
			{
				return null;
			}
			return TableCellProperties.stylePropertyGetters[propertyDefinition.GlobalPropertyIndex](this);
		}

		static void InitStylePropertyGetters()
		{
			if (TableCellProperties.stylePropertyGetters != null)
			{
				return;
			}
			TableCellProperties.stylePropertyGetters = new Func<TableCellProperties, IStyleProperty>[13];
			TableCellProperties.stylePropertyGetters[DocumentElementPropertiesBase.StyleIdPropertyDefinition.GlobalPropertyIndex] = (TableCellProperties prop) => prop.StyleIdProperty;
			TableCellProperties.stylePropertyGetters[TableCell.BackgroundColorPropertyDefinition.GlobalPropertyIndex] = (TableCellProperties prop) => prop.backgroundColorStyleProperty;
			TableCellProperties.stylePropertyGetters[TableCell.ShadingPatternColorPropertyDefinition.GlobalPropertyIndex] = (TableCellProperties prop) => prop.shadingPatternColorStyleProperty;
			TableCellProperties.stylePropertyGetters[TableCell.ShadingPatternPropertyDefinition.GlobalPropertyIndex] = (TableCellProperties prop) => prop.shadingPatternStyleProperty;
			TableCellProperties.stylePropertyGetters[TableCell.BordersPropertyDefinition.GlobalPropertyIndex] = (TableCellProperties prop) => prop.bordersStyleProperty;
			TableCellProperties.stylePropertyGetters[TableCell.PaddingPropertyDefinition.GlobalPropertyIndex] = (TableCellProperties prop) => prop.paddingStyleProperty;
			TableCellProperties.stylePropertyGetters[TableCell.ColumnSpanPropertyDefinition.GlobalPropertyIndex] = (TableCellProperties prop) => prop.columnSpanStyleProperty;
			TableCellProperties.stylePropertyGetters[TableCell.RowSpanPropertyDefinition.GlobalPropertyIndex] = (TableCellProperties prop) => prop.rowSpanStyleProperty;
			TableCellProperties.stylePropertyGetters[TableCell.IgnoreCellMarkerInRowHeightCalculationPropertyDefinition.GlobalPropertyIndex] = (TableCellProperties prop) => prop.ignoreCellMarkerInRowHeightCalculationStyleProperty;
			TableCellProperties.stylePropertyGetters[TableCell.CanWrapContentPropertyDefinition.GlobalPropertyIndex] = (TableCellProperties prop) => prop.canWrapContentStyleProperty;
			TableCellProperties.stylePropertyGetters[TableCell.PreferredWidthPropertyDefinition.GlobalPropertyIndex] = (TableCellProperties prop) => prop.preferredWidthStyleProperty;
			TableCellProperties.stylePropertyGetters[TableCell.VerticalAlignmentPropertyDefinition.GlobalPropertyIndex] = (TableCellProperties prop) => prop.verticalAlignmentStyleProperty;
			TableCellProperties.stylePropertyGetters[TableCell.TextDirectionPropertyDefinition.GlobalPropertyIndex] = (TableCellProperties prop) => prop.textDirectionStyleProperty;
		}

		void InitProperties()
		{
			this.backgroundColorStyleProperty = new StyleProperty<ThemableColor>(TableCell.BackgroundColorPropertyDefinition, this);
			this.shadingPatternColorStyleProperty = new StyleProperty<ThemableColor>(TableCell.ShadingPatternColorPropertyDefinition, this);
			this.shadingPatternStyleProperty = new StyleProperty<ShadingPattern?>(TableCell.ShadingPatternPropertyDefinition, this);
			this.bordersStyleProperty = new StyleProperty<TableCellBorders>(TableCell.BordersPropertyDefinition, this);
			this.paddingStyleProperty = new StyleProperty<Padding>(TableCell.PaddingPropertyDefinition, this);
			this.columnSpanStyleProperty = new LocalProperty<int?>(TableCell.ColumnSpanPropertyDefinition, this);
			this.rowSpanStyleProperty = new LocalProperty<int?>(TableCell.RowSpanPropertyDefinition, this);
			this.ignoreCellMarkerInRowHeightCalculationStyleProperty = new LocalProperty<bool?>(TableCell.IgnoreCellMarkerInRowHeightCalculationPropertyDefinition, this);
			this.canWrapContentStyleProperty = new LocalProperty<bool?>(TableCell.CanWrapContentPropertyDefinition, this);
			this.preferredWidthStyleProperty = new LocalProperty<TableWidthUnit>(TableCell.PreferredWidthPropertyDefinition, this);
			this.verticalAlignmentStyleProperty = new LocalProperty<VerticalAlignment?>(TableCell.VerticalAlignmentPropertyDefinition, this);
			this.textDirectionStyleProperty = new LocalProperty<TextDirection>(TableCell.TextDirectionPropertyDefinition, this);
			TableCellProperties.InitStylePropertyGetters();
		}

		static Func<TableCellProperties, IStyleProperty>[] stylePropertyGetters;

		StyleProperty<ThemableColor> backgroundColorStyleProperty;

		StyleProperty<ThemableColor> shadingPatternColorStyleProperty;

		StyleProperty<ShadingPattern?> shadingPatternStyleProperty;

		StyleProperty<TableCellBorders> bordersStyleProperty;

		StyleProperty<Padding> paddingStyleProperty;

		LocalProperty<int?> columnSpanStyleProperty;

		LocalProperty<int?> rowSpanStyleProperty;

		LocalProperty<bool?> ignoreCellMarkerInRowHeightCalculationStyleProperty;

		LocalProperty<bool?> canWrapContentStyleProperty;

		LocalProperty<TableWidthUnit> preferredWidthStyleProperty;

		LocalProperty<VerticalAlignment?> verticalAlignmentStyleProperty;

		LocalProperty<TextDirection> textDirectionStyleProperty;
	}
}
