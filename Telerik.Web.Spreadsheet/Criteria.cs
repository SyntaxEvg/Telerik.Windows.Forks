using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Telerik.Web.Spreadsheet
{
	[DataContract]
	public class Criteria
	{
		internal Dictionary<string, object> Serialize()
		{
			return this.SerializeSettings();
		}

		[DataMember(Name = "operator", EmitDefaultValue = false)]
		public string Operator { get; set; }

		[DataMember(Name = "value", EmitDefaultValue = false)]
		public object Value { get; set; }

		protected Dictionary<string, object> SerializeSettings()
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			if (this.Operator != null)
			{
				dictionary["operator"] = this.Operator;
			}
			if (this.Value != null)
			{
				dictionary["value"] = this.Value;
			}
			return dictionary;
		}
	}
}
