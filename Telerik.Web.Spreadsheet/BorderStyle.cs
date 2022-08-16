using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Telerik.Web.Spreadsheet
{
	[DataContract]
	public class BorderStyle
	{
		internal Dictionary<string, object> Serialize()
		{
			return this.SerializeSettings();
		}

		[DataMember(Name = "size", EmitDefaultValue = false)]
		public double? Size { get; set; }

		[DataMember(Name = "color", EmitDefaultValue = false)]
		public string Color { get; set; }

		protected Dictionary<string, object> SerializeSettings()
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			if (this.Size != null)
			{
				dictionary["size"] = this.Size;
			}
			if (this.Color != null)
			{
				dictionary["color"] = this.Color;
			}
			return dictionary;
		}
	}
}
