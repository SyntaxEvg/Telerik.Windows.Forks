using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Telerik.Web.Spreadsheet
{
	[DataContract]
	public class Worksheet
	{
		public void AddRows(IEnumerable<Row> rows)
		{
			if (this.Rows == null)
			{
				this.Rows = new List<Row>();
			}
			this.Rows.AddRange(rows);
		}

		public void AddRow(Row row)
		{
			this.AddRows(new Row[] { row });
		}

		public void AddMergedCells(string range)
		{
			List<string> orDefault = this.MergedCells.GetOrDefault<string>();
			orDefault.Add(range);
			this.MergedCells = orDefault;
		}

		internal Dictionary<string, object> Serialize()
		{
			return this.SerializeSettings();
		}

		[DataMember(Name = "activeCell", EmitDefaultValue = false)]
		public string ActiveCell { get; set; }

		[DataMember(Name = "name", EmitDefaultValue = false)]
		public string Name { get; set; }

		[DataMember(Name = "columns", EmitDefaultValue = false)]
		public List<Column> Columns { get; set; }

		[DataMember(Name = "filter", EmitDefaultValue = false)]
		public Filter Filter { get; set; }

		[DataMember(Name = "frozenColumns", EmitDefaultValue = false)]
		public int? FrozenColumns { get; set; }

		[DataMember(Name = "frozenRows", EmitDefaultValue = false)]
		public int? FrozenRows { get; set; }

		[DataMember(Name = "rows", EmitDefaultValue = false)]
		public List<Row> Rows { get; set; }

		[DataMember(Name = "selection", EmitDefaultValue = false)]
		public string Selection { get; set; }

		[DataMember(Name = "showGridLines", EmitDefaultValue = false)]
		public bool? ShowGridLines { get; set; }

		[DataMember(Name = "sort", EmitDefaultValue = false)]
		public Sort Sort { get; set; }

		[DataMember(Name = "mergedCells", EmitDefaultValue = false)]
		public List<string> MergedCells { get; set; }

		protected Dictionary<string, object> SerializeSettings()
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			if (this.ActiveCell != null)
			{
				dictionary["activeCell"] = this.ActiveCell;
			}
			if (this.Name != null)
			{
				dictionary["name"] = this.Name;
			}
			if (this.Columns != null)
			{
				dictionary["columns"] = from item in this.Columns
					select item.Serialize();
			}
			if (this.Filter != null)
			{
				dictionary["filter"] = this.Filter.Serialize();
			}
			if (this.FrozenColumns != null)
			{
				dictionary["frozenColumns"] = this.FrozenColumns;
			}
			if (this.FrozenRows != null)
			{
				dictionary["frozenRows"] = this.FrozenRows;
			}
			if (this.Rows != null)
			{
				dictionary["rows"] = from item in this.Rows
					select item.Serialize();
			}
			if (this.Selection != null)
			{
				dictionary["selection"] = this.Selection;
			}
			if (this.ShowGridLines != null)
			{
				dictionary["showGridLines"] = this.ShowGridLines;
			}
			if (this.Sort != null)
			{
				dictionary["sort"] = this.Sort.Serialize();
			}
			if (this.MergedCells != null)
			{
				dictionary["mergedCells"] = this.MergedCells;
			}
			return dictionary;
		}
	}
}
