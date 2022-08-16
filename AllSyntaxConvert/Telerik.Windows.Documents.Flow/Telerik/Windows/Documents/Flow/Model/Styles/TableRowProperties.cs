using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Flow.Model.Styles.Core;

namespace Telerik.Windows.Documents.Flow.Model.Styles
{
	public sealed class TableRowProperties : DocumentElementPropertiesBase
	{
		internal TableRowProperties(Style style)
			: base(style)
		{
			this.InitProperties();
		}

		internal TableRowProperties(TableRow tableRow)
			: base(tableRow, false)
		{
			this.InitProperties();
		}

		public IStyleProperty<double?> TableCellSpacing
		{
			get
			{
				return this.tableCellSpacingStyleProperty;
			}
		}

		public IStyleProperty<bool?> RepeatOnEveryPage
		{
			get
			{
				return this.repeatOnEveryPageStyleProperty;
			}
		}

		public IStyleProperty<bool?> CanSplit
		{
			get
			{
				return this.canSplitStyleProperty;
			}
		}

		public IStyleProperty<TableRowHeight> Height
		{
			get
			{
				return this.heightStyleProperty;
			}
		}

		protected override IEnumerable<IStyleProperty> EnumerateStyleProperties()
		{
			yield return this.TableCellSpacing;
			yield return this.RepeatOnEveryPage;
			yield return this.CanSplit;
			yield return this.Height;
			yield break;
		}

		protected override IStyleProperty GetStylePropertyOverride(IStylePropertyDefinition propertyDefinition)
		{
			if (propertyDefinition != DocumentElementPropertiesBase.StyleIdPropertyDefinition && propertyDefinition.StylePropertyType != StylePropertyType.TableRow)
			{
				return null;
			}
			return TableRowProperties.stylePropertyGetters[propertyDefinition.GlobalPropertyIndex](this);
		}

		static void InitStylePropertyGetters()
		{
			if (TableRowProperties.stylePropertyGetters != null)
			{
				return;
			}
			TableRowProperties.stylePropertyGetters = new Func<TableRowProperties, IStyleProperty>[5];
			TableRowProperties.stylePropertyGetters[DocumentElementPropertiesBase.StyleIdPropertyDefinition.GlobalPropertyIndex] = (TableRowProperties prop) => prop.StyleIdProperty;
			TableRowProperties.stylePropertyGetters[TableRow.TableCellSpacingPropertyDefinition.GlobalPropertyIndex] = (TableRowProperties prop) => prop.tableCellSpacingStyleProperty;
			TableRowProperties.stylePropertyGetters[TableRow.RepeatOnEveryPagePropertyDefinition.GlobalPropertyIndex] = (TableRowProperties prop) => prop.repeatOnEveryPageStyleProperty;
			TableRowProperties.stylePropertyGetters[TableRow.HeightPropertyDefinition.GlobalPropertyIndex] = (TableRowProperties prop) => prop.heightStyleProperty;
			TableRowProperties.stylePropertyGetters[TableRow.CanSplitPropertyDefinition.GlobalPropertyIndex] = (TableRowProperties prop) => prop.canSplitStyleProperty;
		}

		void InitProperties()
		{
			this.tableCellSpacingStyleProperty = new StyleProperty<double?>(TableRow.TableCellSpacingPropertyDefinition, this);
			this.repeatOnEveryPageStyleProperty = new LocalProperty<bool?>(TableRow.RepeatOnEveryPagePropertyDefinition, this);
			this.heightStyleProperty = new LocalProperty<TableRowHeight>(TableRow.HeightPropertyDefinition, this);
			this.canSplitStyleProperty = new LocalProperty<bool?>(TableRow.CanSplitPropertyDefinition, this);
			TableRowProperties.InitStylePropertyGetters();
		}

		static Func<TableRowProperties, IStyleProperty>[] stylePropertyGetters;

		StyleProperty<double?> tableCellSpacingStyleProperty;

		LocalProperty<bool?> repeatOnEveryPageStyleProperty;

		LocalProperty<TableRowHeight> heightStyleProperty;

		LocalProperty<bool?> canSplitStyleProperty;
	}
}
