using System;
using Telerik.Windows.Documents.Flow.Model.Cloning;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.Flow.Model.Styles.Core;
using Telerik.Windows.Documents.Primitives;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.Model
{
	public sealed class TableCell : BlockContainerBase, IElementWithProperties
	{
		static TableCell()
		{
			TableCell.ColumnSpanPropertyDefinition.Validation.AddRule(new ValidationRule<int>((int value) => value > 0));
			TableCell.RowSpanPropertyDefinition.Validation.AddRule(new ValidationRule<int>((int value) => value > 0));
		}

		public TableCell(RadFlowDocument document)
			: base(document)
		{
			this.properties = new TableCellProperties(this);
			this.Shading = new Shading(this.properties);
		}

		DocumentElementPropertiesBase IElementWithProperties.Properties
		{
			get
			{
				return this.properties;
			}
		}

		public TableCellProperties Properties
		{
			get
			{
				return this.properties;
			}
		}

		public TableRow Row
		{
			get
			{
				return (TableRow)base.Parent;
			}
		}

		public Table Table
		{
			get
			{
				if (this.Row == null)
				{
					return null;
				}
				return this.Row.Table;
			}
		}

		public TableCellBorders Borders
		{
			get
			{
				return this.Properties.Borders.GetActualValue();
			}
			set
			{
				this.Properties.Borders.LocalValue = value;
			}
		}

		public Shading Shading { get; set; }

		public Padding Padding
		{
			get
			{
				return this.Properties.Padding.GetActualValue();
			}
			set
			{
				this.Properties.Padding.LocalValue = value;
			}
		}

		public int ColumnSpan
		{
			get
			{
				return this.Properties.ColumnSpan.GetActualValue().Value;
			}
			set
			{
				this.InvalidateTableGrid();
				this.Properties.ColumnSpan.LocalValue = new int?(value);
			}
		}

		public int RowSpan
		{
			get
			{
				return this.Properties.RowSpan.GetActualValue().Value;
			}
			set
			{
				this.InvalidateTableGrid();
				this.Properties.RowSpan.LocalValue = new int?(value);
			}
		}

		public bool IgnoreCellMarkerInRowHeightCalculation
		{
			get
			{
				return this.Properties.IgnoreCellMarkerInRowHeightCalculation.GetActualValue().Value;
			}
			set
			{
				this.Properties.IgnoreCellMarkerInRowHeightCalculation.LocalValue = new bool?(value);
			}
		}

		public bool CanWrapContent
		{
			get
			{
				return this.Properties.CanWrapContent.GetActualValue().Value;
			}
			set
			{
				this.Properties.CanWrapContent.LocalValue = new bool?(value);
			}
		}

		public TableWidthUnit PreferredWidth
		{
			get
			{
				return this.Properties.PreferredWidth.GetActualValue();
			}
			set
			{
				this.Properties.PreferredWidth.LocalValue = value;
			}
		}

		public VerticalAlignment VerticalAlignment
		{
			get
			{
				return this.Properties.VerticalAlignment.GetActualValue().Value;
			}
			set
			{
				this.Properties.VerticalAlignment.LocalValue = new VerticalAlignment?(value);
			}
		}

		public TextDirection TextDirection
		{
			get
			{
				return this.Properties.TextDirection.GetActualValue();
			}
			set
			{
				this.Properties.TextDirection.LocalValue = value;
			}
		}

		public int GridColumnIndex
		{
			get
			{
				this.AssureTableGrid();
				return this.gridColumnIndex;
			}
			internal set
			{
				this.gridColumnIndex = value;
			}
		}

		public int GridRowIndex
		{
			get
			{
				this.AssureTableGrid();
				return this.gridRowIndex;
			}
			internal set
			{
				this.gridRowIndex = value;
			}
		}

		internal override DocumentElementType Type
		{
			get
			{
				return DocumentElementType.TableCell;
			}
		}

		public TableCell Clone()
		{
			return this.CloneInternal(null);
		}

		public TableCell Clone(RadFlowDocument document)
		{
			Guard.ThrowExceptionIfNull<RadFlowDocument>(document, "document");
			return this.CloneInternal(document);
		}

		internal override DocumentElementBase CloneCore(CloneContext cloneContext)
		{
			Guard.ThrowExceptionIfNull<CloneContext>(cloneContext, "cloneContext");
			TableCell tableCell = new TableCell(cloneContext.Document);
			tableCell.Properties.CopyPropertiesFrom(this.Properties);
			tableCell.Blocks.AddClonedChildrenFrom(base.Blocks, cloneContext);
			return tableCell;
		}

		void AssureTableGrid()
		{
			if (this.Table != null)
			{
				this.Table.AssureTableGrid();
			}
		}

		void InvalidateTableGrid()
		{
			if (this.Row != null)
			{
				this.Row.InvalidateTableGrid();
			}
		}

		TableCell CloneInternal(RadFlowDocument document = null)
		{
			return (TableCell)this.CloneCore(new CloneContext(document ?? this.Document));
		}

		public static readonly StylePropertyDefinition<TableCellBorders> BordersPropertyDefinition = new StylePropertyDefinition<TableCellBorders>("Borders", DocumentDefaultStyleSettings.TableCellBorders, StylePropertyType.TableCell);

		public static readonly StylePropertyDefinition<ThemableColor> BackgroundColorPropertyDefinition = new StylePropertyDefinition<ThemableColor>("BackgroundColor", DocumentDefaultStyleSettings.TableCellBackgroundColor, StylePropertyType.TableCell);

		public static readonly StylePropertyDefinition<ThemableColor> ShadingPatternColorPropertyDefinition = new StylePropertyDefinition<ThemableColor>("ShadingPatternColor", DocumentDefaultStyleSettings.TableCellShadingPatternColor, StylePropertyType.TableCell);

		public static readonly StylePropertyDefinition<ShadingPattern?> ShadingPatternPropertyDefinition = new StylePropertyDefinition<ShadingPattern?>("ShadingPattern", new ShadingPattern?(DocumentDefaultStyleSettings.TableCellShadingPattern), StylePropertyType.TableCell);

		public static readonly StylePropertyDefinition<Padding> PaddingPropertyDefinition = new StylePropertyDefinition<Padding>("Padding", DocumentDefaultStyleSettings.TableCellPadding, StylePropertyType.TableCell);

		public static readonly StylePropertyDefinition<int?> ColumnSpanPropertyDefinition = new StylePropertyDefinition<int?>("ColumnSpan", new int?(DocumentDefaultStyleSettings.ColumnSpan), StylePropertyType.TableCell);

		public static readonly StylePropertyDefinition<int?> RowSpanPropertyDefinition = new StylePropertyDefinition<int?>("RowSpan", new int?(DocumentDefaultStyleSettings.RowSpan), StylePropertyType.TableCell);

		public static readonly StylePropertyDefinition<bool?> IgnoreCellMarkerInRowHeightCalculationPropertyDefinition = new StylePropertyDefinition<bool?>("IgnoreCellMarkerInRowHeightCalculation", new bool?(DocumentDefaultStyleSettings.IgnoreCellMarkerInRowHeightCalculation), StylePropertyType.TableCell);

		public static readonly StylePropertyDefinition<bool?> CanWrapContentPropertyDefinition = new StylePropertyDefinition<bool?>("CanWrapContent", new bool?(DocumentDefaultStyleSettings.CanWrapContent), StylePropertyType.TableCell);

		public static readonly StylePropertyDefinition<TableWidthUnit> PreferredWidthPropertyDefinition = new StylePropertyDefinition<TableWidthUnit>("PreferredWidth", DocumentDefaultStyleSettings.TableCellPreferredWidth, StylePropertyType.TableCell);

		public static readonly StylePropertyDefinition<VerticalAlignment?> VerticalAlignmentPropertyDefinition = new StylePropertyDefinition<VerticalAlignment?>("VerticalAlignment", new VerticalAlignment?(DocumentDefaultStyleSettings.TableCellVerticalAlignment), StylePropertyType.TableCell);

		public static readonly StylePropertyDefinition<TextDirection> TextDirectionPropertyDefinition = new StylePropertyDefinition<TextDirection>("TextDirection", DocumentDefaultStyleSettings.TableCellTextDirection, StylePropertyType.TableCell);

		readonly TableCellProperties properties;

		int gridRowIndex;

		int gridColumnIndex;
	}
}
