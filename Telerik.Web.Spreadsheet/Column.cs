using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Telerik.Web.Spreadsheet
{
	[DataContract]
	public class Column
	{
		internal Dictionary<string, object> Serialize()
		{
			return this.SerializeSettings();
		}

		[DataMember(Name = "index", EmitDefaultValue = false)]
		public int? Index { get; set; }

		[DataMember(Name = "width", EmitDefaultValue = false)]
		public double? Width { get; set; }

		[DataMember(Name = "hidden", EmitDefaultValue = false)]
		public double? Hidden { get; set; }

		protected Dictionary<string, object> SerializeSettings()
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			if (this.Index != null)
			{
				dictionary["index"] = this.Index;
			}
			if (this.Hidden != null)
			{
				dictionary["hidden"] = this.Hidden;
			}
			if (this.Width != null)
			{
				dictionary["width"] = this.Width;
			}
			return dictionary;
		}
	}
}
