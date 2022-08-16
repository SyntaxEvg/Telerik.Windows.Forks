using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Telerik.Web.Spreadsheet
{
	[DataContract]
	public class SortColumn
	{
		internal Dictionary<string, object> Serialize()
		{
			return this.SerializeSettings();
		}

		[DataMember(Name = "ascending", EmitDefaultValue = false)]
		public bool? Ascending { get; set; }

		[DataMember(Name = "index", EmitDefaultValue = false)]
		public double? Index { get; set; }

		protected Dictionary<string, object> SerializeSettings()
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			if (this.Ascending != null)
			{
				dictionary["ascending"] = this.Ascending;
			}
			if (this.Index != null)
			{
				dictionary["index"] = this.Index;
			}
			return dictionary;
		}
	}
}
