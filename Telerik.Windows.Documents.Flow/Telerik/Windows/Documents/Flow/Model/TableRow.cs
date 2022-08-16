using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Flow.Model.Cloning;
using Telerik.Windows.Documents.Flow.Model.Collections;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.Flow.Model.Styles.Core;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.Model
{
	public sealed class TableRow : DocumentElementBase, IElementWithProperties
	{
		public TableRow(RadFlowDocument document)
			: base(document)
		{
			this.cells = new TableCellCollection(this);
			this.properties = new TableRowProperties(this);
		}

		DocumentElementPropertiesBase IElementWithProperties.Properties
		{
			get
			{
				return this.properties;
			}
		}

		public TableRowProperties Properties
		{
			get
			{
				return this.properties;
			}
		}

		public TableCellCollection Cells
		{
			get
			{
				return this.cells;
			}
		}

		public Table Table
		{
			get
			{
				return (Table)base.Parent;
			}
		}

		public int GridRowIndex
		{
			get
			{
				if (this.Table != null)
				{
					this.Table.AssureTableGrid();
				}
				return this.gridRowIndex;
			}
			internal set
			{
				this.gridRowIndex = value;
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

		public bool RepeatOnEveryPage
		{
			get
			{
				return this.Properties.RepeatOnEveryPage.GetActualValue().Value;
			}
			set
			{
				this.Properties.RepeatOnEveryPage.LocalValue = new bool?(value);
			}
		}

		public bool CanSplit
		{
			get
			{
				return this.Properties.CanSplit.GetActualValue().Value;
			}
			set
			{
				this.Properties.CanSplit.LocalValue = new bool?(value);
			}
		}

		public TableRowHeight Height
		{
			get
			{
				return this.Properties.Height.GetActualValue();
			}
			set
			{
				this.Properties.Height.LocalValue = value;
			}
		}

		internal override IEnumerable<DocumentElementBase> Children
		{
			get
			{
				return this.Cells;
			}
		}

		internal override DocumentElementType Type
		{
			get
			{
				return DocumentElementType.TableRow;
			}
		}

		public TableRow Clone()
		{
			return this.CloneInternal(null);
		}

		public TableRow Clone(RadFlowDocument document)
		{
			Guard.ThrowExceptionIfNull<RadFlowDocument>(document, "document");
			return this.CloneInternal(document);
		}

		internal override DocumentElementBase CloneCore(CloneContext cloneContext)
		{
			Guard.ThrowExceptionIfNull<CloneContext>(cloneContext, "cloneContext");
			TableRow tableRow = new TableRow(cloneContext.Document);
			tableRow.Properties.CopyPropertiesFrom(this.Properties);
			tableRow.Cells.AddClonedChildrenFrom(this.Cells, cloneContext);
			return tableRow;
		}

		internal void InvalidateTableGrid()
		{
			if (this.Table != null)
			{
				this.Table.InvalidateTableGrid();
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

		TableRow CloneInternal(RadFlowDocument document = null)
		{
			return (TableRow)this.CloneCore(new CloneContext(document ?? this.Document));
		}

		public static readonly StylePropertyDefinition<double?> TableCellSpacingPropertyDefinition = new StylePropertyDefinition<double?>("TableCellSpacing", new double?(DocumentDefaultStyleSettings.TableRowTableCellSpacing), StylePropertyType.TableRow);

		public static readonly StylePropertyDefinition<bool?> RepeatOnEveryPagePropertyDefinition = new StylePropertyDefinition<bool?>("RepeatOnEveryPage", new bool?(DocumentDefaultStyleSettings.TableRowRepeatOnEveryPage), StylePropertyType.TableRow);

		public static readonly StylePropertyDefinition<TableRowHeight> HeightPropertyDefinition = new StylePropertyDefinition<TableRowHeight>("Height", DocumentDefaultStyleSettings.TableRowHeight, StylePropertyType.TableRow);

		public static readonly StylePropertyDefinition<bool?> CanSplitPropertyDefinition = new StylePropertyDefinition<bool?>("CanSplit", new bool?(DocumentDefaultStyleSettings.TableRowCanSplit), StylePropertyType.TableRow);

		readonly TableRowProperties properties;

		readonly TableCellCollection cells;

		int gridRowIndex;
	}
}
