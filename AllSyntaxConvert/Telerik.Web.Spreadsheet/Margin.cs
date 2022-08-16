using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Telerik.Web.Spreadsheet
{
	[DataContract]
	public class Margin
	{
		internal Dictionary<string, object> Serialize()
		{
			return this.SerializeSettings();
		}

		[DataMember(Name = "bottom", EmitDefaultValue = false)]
		public double? Bottom { get; set; }

		[DataMember(Name = "left", EmitDefaultValue = false)]
		public double? Left { get; set; }

		[DataMember(Name = "right", EmitDefaultValue = false)]
		public double? Right { get; set; }

		[DataMember(Name = "top", EmitDefaultValue = false)]
		public double? Top { get; set; }

		protected Dictionary<string, object> SerializeSettings()
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			if (this.Bottom != null)
			{
				dictionary["bottom"] = this.Bottom;
			}
			if (this.Left != null)
			{
				dictionary["left"] = this.Left;
			}
			if (this.Right != null)
			{
				dictionary["right"] = this.Right;
			}
			if (this.Top != null)
			{
				dictionary["top"] = this.Top;
			}
			return dictionary;
		}
	}
}
