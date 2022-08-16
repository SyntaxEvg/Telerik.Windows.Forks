using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Flow.Model.Styles.Core;
using Telerik.Windows.Documents.Primitives;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Flow.Model.Styles
{
	public sealed class TableProperties : DocumentElementPropertiesBase, IPropertiesWithShading, IPropertiesWithPadding
	{
		internal TableProperties(Style style)
			: base(style)
		{
			this.InitProperties();
		}

		internal TableProperties(Table table)
			: base(table, false)
		{
			this.InitProperties();
		}

		public IStyleProperty<Alignment?> Alignment
		{
			get
			{
				return this.alignmentStyleProperty;
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

		public IStyleProperty<TableBorders> Borders
		{
			get
			{
				return this.tableBordersStyleProperty;
			}
		}

		public IStyleProperty<Padding> TableCellPadding
		{
			get
			{
				return this.tableCellPaddingStyleProperty;
			}
		}

		public IStyleProperty<double?> TableCellSpacing
		{
			get
			{
				return this.tableCellSpacingStyleProperty;
			}
		}

		public IStyleProperty<double?> Indent
		{
			get
			{
				return this.indentStyleProperty;
			}
		}

		public IStyleProperty<int?> RowBanding
		{
			get
			{
				return this.rowBandingStyleProperty;
			}
		}

		public IStyleProperty<int?> ColumnBanding
		{
			get
			{
				return this.columnBandingStyleProperty;
			}
		}

		public IStyleProperty<FlowDirection?> FlowDirection
		{
			get
			{
				return this.flowDirectionStyleProperty;
			}
		}

		public IStyleProperty<TableWidthUnit> PreferredWidth
		{
			get
			{
				return this.preferredWidthStyleProperty;
			}
		}

		public IStyleProperty<TableLooks?> Looks
		{
			get
			{
				return this.looksStyleProperty;
			}
		}

		public IStyleProperty<TableLayoutType?> LayoutType
		{
			get
			{
				return this.layoutTypeStyleProperty;
			}
		}

		public IStyleProperty<bool?> Overlap
		{
			get
			{
				return this.overlapStyleProperty;
			}
		}

		RadFlowDocument IPropertiesWithShading.Document
		{
			get
			{
				return base.Document;
			}
		}

		IStyleProperty<Padding> IPropertiesWithPadding.Padding
		{
			get
			{
				return this.TableCellPadding;
			}
		}

		protected override IEnumerable<IStyleProperty> EnumerateStyleProperties()
		{
			yield return this.Alignment;
			yield return this.BackgroundColor;
			yield return this.ShadingPatternColor;
			yield return this.ShadingPattern;
			yield return this.Borders;
			yield return this.TableCellPadding;
			yield return this.TableCellSpacing;
			yield return this.Indent;
			yield return this.RowBanding;
			yield return this.ColumnBanding;
			yield return this.FlowDirection;
			yield return this.PreferredWidth;
			yield return this.Looks;
			yield return this.LayoutType;
			yield return this.Overlap;
			yield break;
		}

		protected override IStyleProperty GetStylePropertyOverride(IStylePropertyDefinition propertyDefinition)
		{
			if (propertyDefinition != DocumentElementPropertiesBase.StyleIdPropertyDefinition && propertyDefinition.StylePropertyType != StylePropertyType.Table)
			{
				return null;
			}
			return TableProperties.stylePropertyGetters[propertyDefinition.GlobalPropertyIndex](this);
		}

		static void InitStylePropertyGetters()
		{
			if (TableProperties.stylePropertyGetters != null)
			{
				return;
			}
			TableProperties.stylePropertyGetters = new Func<TableProperties, IStyleProperty>[16];
			TableProperties.stylePropertyGetters[DocumentElementPropertiesBase.StyleIdPropertyDefinition.GlobalPropertyIndex] = (TableProperties prop) => prop.StyleIdProperty;
			TableProperties.stylePropertyGetters[Table.AlignmentPropertyDefinition.GlobalPropertyIndex] = (TableProperties prop) => prop.alignmentStyleProperty;
			TableProperties.stylePropertyGetters[Table.TableCellPaddingPropertyDefinition.GlobalPropertyIndex] = (TableProperties prop) => prop.tableCellPaddingStyleProperty;
			TableProperties.stylePropertyGetters[Table.TableCellSpacingPropertyDefinition.GlobalPropertyIndex] = (TableProperties prop) => prop.tableCellSpacingStyleProperty;
			TableProperties.stylePropertyGetters[Table.IndentPropertyDefinition.GlobalPropertyIndex] = (TableProperties prop) => prop.indentStyleProperty;
			TableProperties.stylePropertyGetters[Table.RowBandingPropertyDefinition.GlobalPropertyIndex] = (TableProperties prop) => prop.rowBandingStyleProperty;
			TableProperties.stylePropertyGetters[Table.ColumnBandingPropertyDefinition.GlobalPropertyIndex] = (TableProperties prop) => prop.columnBandingStyleProperty;
			TableProperties.stylePropertyGetters[Table.TableBordersPropertyDefinition.GlobalPropertyIndex] = (TableProperties prop) => prop.tableBordersStyleProperty;
			TableProperties.stylePropertyGetters[Table.BackgroundColorPropertyDefinition.GlobalPropertyIndex] = (TableProperties prop) => prop.backgroundColorStyleProperty;
			TableProperties.stylePropertyGetters[Table.ShadingPatternColorPropertyDefinition.GlobalPropertyIndex] = (TableProperties prop) => prop.shadingPatternColorStyleProperty;
			TableProperties.stylePropertyGetters[Table.ShadingPatternPropertyDefinition.GlobalPropertyIndex] = (TableProperties prop) => prop.shadingPatternStyleProperty;
			TableProperties.stylePropertyGetters[Table.FlowDirectionPropertyDefinition.GlobalPropertyIndex] = (TableProperties prop) => prop.flowDirectionStyleProperty;
			TableProperties.stylePropertyGetters[Table.PreferredWidthPropertyDefinition.GlobalPropertyIndex] = (TableProperties prop) => prop.preferredWidthStyleProperty;
			TableProperties.stylePropertyGetters[Table.LooksPropertyDefinition.GlobalPropertyIndex] = (TableProperties prop) => prop.looksStyleProperty;
			TableProperties.stylePropertyGetters[Table.LayoutTypePropertyDefinition.GlobalPropertyIndex] = (TableProperties prop) => prop.layoutTypeStyleProperty;
			TableProperties.stylePropertyGetters[Table.OverlapPropertyDefinition.GlobalPropertyIndex] = (TableProperties prop) => prop.overlapStyleProperty;
		}

		void InitProperties()
		{
			this.alignmentStyleProperty = new StyleProperty<Alignment?>(Table.AlignmentPropertyDefinition, this);
			this.tableCellPaddingStyleProperty = new StyleProperty<Padding>(Table.TableCellPaddingPropertyDefinition, this);
			this.tableCellSpacingStyleProperty = new StyleProperty<double?>(Table.TableCellSpacingPropertyDefinition, this);
			this.indentStyleProperty = new StyleProperty<double?>(Table.IndentPropertyDefinition, this);
			this.rowBandingStyleProperty = new StyleProperty<int?>(Table.RowBandingPropertyDefinition, this);
			this.columnBandingStyleProperty = new StyleProperty<int?>(Table.ColumnBandingPropertyDefinition, this);
			this.tableBordersStyleProperty = new StyleProperty<TableBorders>(Table.TableBordersPropertyDefinition, this);
			this.backgroundColorStyleProperty = new StyleProperty<ThemableColor>(Table.BackgroundColorPropertyDefinition, this);
			this.shadingPatternColorStyleProperty = new StyleProperty<ThemableColor>(Table.ShadingPatternColorPropertyDefinition, this);
			this.shadingPatternStyleProperty = new StyleProperty<ShadingPattern?>(Table.ShadingPatternPropertyDefinition, this);
			this.flowDirectionStyleProperty = new LocalProperty<FlowDirection?>(Table.FlowDirectionPropertyDefinition, this);
			this.preferredWidthStyleProperty = new LocalProperty<TableWidthUnit>(Table.PreferredWidthPropertyDefinition, this);
			this.looksStyleProperty = new LocalProperty<TableLooks?>(Table.LooksPropertyDefinition, this);
			this.layoutTypeStyleProperty = new LocalProperty<TableLayoutType?>(Table.LayoutTypePropertyDefinition, this);
			this.overlapStyleProperty = new LocalProperty<bool?>(Table.OverlapPropertyDefinition, this);
			TableProperties.InitStylePropertyGetters();
		}

		static Func<TableProperties, IStyleProperty>[] stylePropertyGetters;

		StyleProperty<Alignment?> alignmentStyleProperty;

		StyleProperty<Padding> tableCellPaddingStyleProperty;

		StyleProperty<double?> tableCellSpacingStyleProperty;

		StyleProperty<double?> indentStyleProperty;

		StyleProperty<int?> rowBandingStyleProperty;

		StyleProperty<int?> columnBandingStyleProperty;

		StyleProperty<TableBorders> tableBordersStyleProperty;

		StyleProperty<ThemableColor> backgroundColorStyleProperty;

		StyleProperty<ThemableColor> shadingPatternColorStyleProperty;

		StyleProperty<ShadingPattern?> shadingPatternStyleProperty;

		LocalProperty<FlowDirection?> flowDirectionStyleProperty;

		LocalProperty<TableWidthUnit> preferredWidthStyleProperty;

		LocalProperty<TableLooks?> looksStyleProperty;

		LocalProperty<TableLayoutType?> layoutTypeStyleProperty;

		LocalProperty<bool?> overlapStyleProperty;
	}
}
