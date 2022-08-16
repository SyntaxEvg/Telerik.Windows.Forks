using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Core.Data;
using Telerik.Windows.Documents.Flow.Model.Cloning;
using Telerik.Windows.Documents.Flow.Model.Collections;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.Flow.Model.Styles.Core;
using Telerik.Windows.Documents.Primitives;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.Model
{
	public sealed class Table : BlockBase, IElementWithStyle, IElementWithProperties
	{
		public Table(RadFlowDocument document)
			: this(document, 0, 0)
		{
		}

		public Table(RadFlowDocument document, int rows, int columns)
			: base(document)
		{
			Guard.ThrowExceptionIfOutOfRange<int>(0, int.MaxValue, rows, "rows");
			Guard.ThrowExceptionIfOutOfRange<int>(0, int.MaxValue, columns, "columns");
			this.rows = new TableRowCollection(this);
			this.properties = new TableProperties(this);
			this.Shading = new Shading(this.Properties);
			for (int i = 0; i < rows; i++)
			{
				TableRow tableRow = this.Rows.AddTableRow();
				for (int j = 0; j < columns; j++)
				{
					tableRow.Cells.AddTableCell();
				}
			}
			this.AssureTableGrid();
		}

		DocumentElementPropertiesBase IElementWithProperties.Properties
		{
			get
			{
				return this.properties;
			}
		}

		public TableProperties Properties
		{
			get
			{
				return this.properties;
			}
		}

		public TableRowCollection Rows
		{
			get
			{
				return this.rows;
			}
		}

		public string StyleId
		{
			get
			{
				return this.Properties.StyleId;
			}
			set
			{
				this.Properties.StyleId = value;
			}
		}

		public Alignment Alignment
		{
			get
			{
				return this.Properties.Alignment.GetActualValue().Value;
			}
			set
			{
				this.Properties.Alignment.LocalValue = new Alignment?(value);
			}
		}

		public TableBorders Borders
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

		public int GridColumnsCount
		{
			get
			{
				this.AssureTableGrid();
				return this.gridColumnsCount;
			}
		}

		public int GridRowsCount
		{
			get
			{
				this.AssureTableGrid();
				return this.gridRowsCount;
			}
		}

		public double TableCellSpacing
		{
			get
			{
				return this.Properties.TableCellSpacing.GetActualValue().Value;
			}
			set
			{
				this.Properties.TableCellSpacing.LocalValue = new double?(value);
			}
		}

		public bool HasCellSpacing
		{
			get
			{
				return this.TableCellSpacing > 0.0;
			}
		}

		public Padding TableCellPadding
		{
			get
			{
				return this.Properties.TableCellPadding.GetActualValue();
			}
			set
			{
				this.Properties.TableCellPadding.LocalValue = value;
			}
		}

		public double Indent
		{
			get
			{
				return this.Properties.Indent.GetActualValue().Value;
			}
			set
			{
				this.Properties.Indent.LocalValue = new double?(value);
			}
		}

		public FlowDirection FlowDirection
		{
			get
			{
				return this.Properties.FlowDirection.GetActualValue().Value;
			}
			set
			{
				this.Properties.FlowDirection.LocalValue = new FlowDirection?(value);
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

		public TableLooks Looks
		{
			get
			{
				return this.Properties.Looks.GetActualValue().Value;
			}
			set
			{
				this.Properties.Looks.LocalValue = new TableLooks?(value);
			}
		}

		public TableLayoutType LayoutType
		{
			get
			{
				return this.Properties.LayoutType.GetActualValue().Value;
			}
			set
			{
				this.Properties.LayoutType.LocalValue = new TableLayoutType?(value);
			}
		}

		public bool Overlap
		{
			get
			{
				return this.Properties.Overlap.GetActualValue().Value;
			}
			set
			{
				this.Properties.Overlap.LocalValue = new bool?(value);
			}
		}

		internal override IEnumerable<DocumentElementBase> Children
		{
			get
			{
				return this.rows;
			}
		}

		internal override DocumentElementType Type
		{
			get
			{
				return DocumentElementType.Table;
			}
		}

		public Table Clone()
		{
			return this.CloneInternal(null);
		}

		public Table Clone(RadFlowDocument document)
		{
			Guard.ThrowExceptionIfNull<RadFlowDocument>(document, "document");
			return this.CloneInternal(document);
		}

		internal override DocumentElementBase CloneCore(CloneContext cloneContext)
		{
			Guard.ThrowExceptionIfNull<CloneContext>(cloneContext, "cloneContext");
			Table table = new Table(cloneContext.Document);
			table.Properties.CopyPropertiesFrom(this.Properties);
			if (cloneContext.RenamedStyles.ContainsKey(table.StyleId))
			{
				table.StyleId = cloneContext.RenamedStyles[table.StyleId];
			}
			table.Rows.AddClonedChildrenFrom(this.Rows, cloneContext);
			return table;
		}

		internal void InvalidateTableGrid()
		{
			this.isTableGridValid = false;
		}

		internal void AssureTableGrid()
		{
			if (!this.isTableGridValid)
			{
				this.RecalculateGridIndexes();
				this.isTableGridValid = true;
			}
		}

		protected override void OnChildAdded(DocumentElementBase child)
		{
			base.OnChildAdded(child);
			this.InvalidateTableGrid();
		}

		protected override void OnChildRemoved(DocumentElementBase child)
		{
			base.OnChildRemoved(child);
			this.InvalidateTableGrid();
		}

		void RecalculateGridIndexes()
		{
			ExpandoList<ExpandoList<bool>> expandoList = new ExpandoList<ExpandoList<bool>>();
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			foreach (TableRow tableRow in this.Rows)
			{
				int num4 = 0;
				foreach (TableCell tableCell in tableRow.Cells)
				{
					if (expandoList[num] == null)
					{
						expandoList[num] = new ExpandoList<bool>();
					}
					while (expandoList[num][num4])
					{
						num4++;
					}
					for (int i = 0; i < tableCell.ColumnSpan; i++)
					{
						for (int j = 0; j < tableCell.RowSpan; j++)
						{
							if (expandoList[num + j] == null)
							{
								expandoList[num + j] = new ExpandoList<bool>();
							}
							expandoList[num + j][num4 + i] = true;
						}
					}
					tableCell.GridRowIndex = num;
					tableCell.GridColumnIndex = num4;
					num2 = Math.Max(num2, num4 + tableCell.ColumnSpan - 1);
					num3 = Math.Max(num3, num + tableCell.RowSpan - 1);
					num4++;
				}
				tableRow.GridRowIndex = num;
				num++;
			}
			this.gridColumnsCount = num2 + 1;
			this.gridRowsCount = num3 + 1;
		}

		Table CloneInternal(RadFlowDocument document = null)
		{
			return (Table)this.CloneCore(new CloneContext(document ?? this.Document));
		}

		public static readonly StylePropertyDefinition<Alignment?> AlignmentPropertyDefinition = new StylePropertyDefinition<Alignment?>("Alignment", new Alignment?(DocumentDefaultStyleSettings.TableAlignment), StylePropertyType.Table);

		public static readonly StylePropertyDefinition<Padding> TableCellPaddingPropertyDefinition = new StylePropertyDefinition<Padding>("TableCellPadding", DocumentDefaultStyleSettings.TableTableCellPadding, StylePropertyType.Table);

		public static readonly StylePropertyDefinition<double?> TableCellSpacingPropertyDefinition = new StylePropertyDefinition<double?>("TableCellSpacing", new double?(DocumentDefaultStyleSettings.TableTableCellSpacing), StylePropertyType.Table);

		public static readonly StylePropertyDefinition<double?> IndentPropertyDefinition = new StylePropertyDefinition<double?>("Indent", new double?(DocumentDefaultStyleSettings.TableIndent), StylePropertyType.Table);

		public static readonly StylePropertyDefinition<int?> RowBandingPropertyDefinition = new StylePropertyDefinition<int?>("RowBanding", new int?(DocumentDefaultStyleSettings.RowBanding), StylePropertyType.Table);

		public static readonly StylePropertyDefinition<int?> ColumnBandingPropertyDefinition = new StylePropertyDefinition<int?>("ColumnBanding", new int?(DocumentDefaultStyleSettings.ColumnBanding), StylePropertyType.Table);

		public static readonly StylePropertyDefinition<TableBorders> TableBordersPropertyDefinition = new StylePropertyDefinition<TableBorders>("Borders", DocumentDefaultStyleSettings.TableBorders, StylePropertyType.Table);

		public static readonly StylePropertyDefinition<ThemableColor> BackgroundColorPropertyDefinition = new StylePropertyDefinition<ThemableColor>("BackgroundColor", DocumentDefaultStyleSettings.TableBackgroundColor, StylePropertyType.Table);

		public static readonly StylePropertyDefinition<ThemableColor> ShadingPatternColorPropertyDefinition = new StylePropertyDefinition<ThemableColor>("ShadingPatternColor", DocumentDefaultStyleSettings.TableShadingPatternColor, StylePropertyType.Table);

		public static readonly StylePropertyDefinition<ShadingPattern?> ShadingPatternPropertyDefinition = new StylePropertyDefinition<ShadingPattern?>("ShadingPattern", new ShadingPattern?(DocumentDefaultStyleSettings.TableShadingPattern), StylePropertyType.Table);

		public static readonly StylePropertyDefinition<FlowDirection?> FlowDirectionPropertyDefinition = new StylePropertyDefinition<FlowDirection?>("FlowDirection", new FlowDirection?(DocumentDefaultStyleSettings.TableFlowDirection), StylePropertyType.Table);

		public static readonly StylePropertyDefinition<TableWidthUnit> PreferredWidthPropertyDefinition = new StylePropertyDefinition<TableWidthUnit>("PreferredWidth", DocumentDefaultStyleSettings.TablePreferredWidth, StylePropertyType.Table);

		public static readonly StylePropertyDefinition<TableLooks?> LooksPropertyDefinition = new StylePropertyDefinition<TableLooks?>("Looks", new TableLooks?(DocumentDefaultStyleSettings.TableLooks), StylePropertyType.Table);

		public static readonly StylePropertyDefinition<TableLayoutType?> LayoutTypePropertyDefinition = new StylePropertyDefinition<TableLayoutType?>("LayoutType", new TableLayoutType?(DocumentDefaultStyleSettings.TableLayoutType), StylePropertyType.Table);

		public static readonly StylePropertyDefinition<bool?> OverlapPropertyDefinition = new StylePropertyDefinition<bool?>("Overlap", new bool?(DocumentDefaultStyleSettings.TableOverlap), StylePropertyType.Table);

		readonly TableRowCollection rows;

		readonly TableProperties properties;

		bool isTableGridValid;

		int gridColumnsCount;

		int gridRowsCount;
	}
}
