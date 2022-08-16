using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Telerik.Web.Spreadsheet
{
	[DataContract]
	public class NamedRange
	{
		[DataMember(Name = "name", EmitDefaultValue = false)]
		public string Name { get; set; }

		[DataMember(Name = "value", EmitDefaultValue = false)]
		public string Value { get; set; }

		[DataMember(Name = "hidden", EmitDefaultValue = false)]
		public bool? Hidden { get; set; }

		internal Dictionary<string, object> Serialize()
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			if (this.Name != null)
			{
				dictionary["name"] = this.Name;
			}
			if (this.Value != null)
			{
				dictionary["value"] = this.Value;
			}
			if (this.Hidden != null)
			{
				dictionary["hiden"] = this.Hidden;
			}
			return dictionary;
		}
	}
}
