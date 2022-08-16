using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Telerik.Web.Spreadsheet
{
	[DataContract]
	public class Sort
	{
		internal Dictionary<string, object> Serialize()
		{
			return this.SerializeSettings();
		}

		[DataMember(Name = "columns", EmitDefaultValue = false)]
		public List<SortColumn> Columns { get; set; }

		[DataMember(Name = "ref", EmitDefaultValue = false)]
		public string Ref { get; set; }

		protected Dictionary<string, object> SerializeSettings()
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			if (this.Columns != null)
			{
				dictionary["columns"] = from item in this.Columns
					select item.Serialize();
			}
			if (this.Ref != null)
			{
				dictionary["ref"] = this.Ref;
			}
			return dictionary;
		}
	}
}
