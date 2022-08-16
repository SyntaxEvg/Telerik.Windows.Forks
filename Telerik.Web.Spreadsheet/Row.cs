using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Telerik.Web.Spreadsheet
{
	[DataContract]
	public class Row
	{
		public void AddCells(IEnumerable<Cell> cells)
		{
			if (this.Cells == null)
			{
				this.Cells = new List<Cell>();
			}
			this.Cells.AddRange(cells);
		}

		public void AddCell(Cell cell)
		{
			this.AddCells(new Cell[] { cell });
		}

		internal Dictionary<string, object> Serialize()
		{
			return this.SerializeSettings();
		}

		[DataMember(Name = "cells", EmitDefaultValue = false)]
		public List<Cell> Cells { get; set; }

		[DataMember(Name = "height", EmitDefaultValue = false)]
		public double? Height { get; set; }

		[DataMember(Name = "hidden", EmitDefaultValue = false)]
		public double? Hidden { get; set; }

		[DataMember(Name = "index", EmitDefaultValue = false)]
		public int? Index { get; set; }

		protected Dictionary<string, object> SerializeSettings()
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			if (this.Cells != null)
			{
				dictionary["cells"] = from item in this.Cells
					select item.Serialize();
			}
			if (this.Hidden != null)
			{
				dictionary["hidden"] = this.Hidden;
			}
			if (this.Height != null)
			{
				dictionary["height"] = this.Height;
			}
			if (this.Index != null)
			{
				dictionary["index"] = this.Index;
			}
			return dictionary;
		}
	}
}
