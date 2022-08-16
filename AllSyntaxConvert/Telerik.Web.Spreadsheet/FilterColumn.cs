using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Telerik.Web.Spreadsheet
{
	[DataContract]
	public class FilterColumn
	{
		[DataMember(Name = "dates", EmitDefaultValue = false)]
		public List<ValueFilterDate> Dates { get; set; }

		internal Dictionary<string, object> Serialize()
		{
			Dictionary<string, object> dictionary = this.SerializeSettings();
			if (this.Dates != null)
			{
				dictionary["dates"] = from item in this.Dates
					select item.Serialize();
			}
			return dictionary;
		}

		[DataMember(Name = "criteria", EmitDefaultValue = false)]
		public List<Criteria> Criteria { get; set; }

		[DataMember(Name = "filter", EmitDefaultValue = false)]
		public string Filter { get; set; }

		[DataMember(Name = "index", EmitDefaultValue = false)]
		public double? Index { get; set; }

		[DataMember(Name = "logic", EmitDefaultValue = false)]
		public string Logic { get; set; }

		[DataMember(Name = "type", EmitDefaultValue = false)]
		public string Type { get; set; }

		[DataMember(Name = "value", EmitDefaultValue = false)]
		public double? Value { get; set; }

		[DataMember(Name = "values", EmitDefaultValue = false)]
		public List<object> Values { get; set; }

		[DataMember(Name = "blanks", EmitDefaultValue = false)]
		public bool? Blanks { get; set; }

		protected Dictionary<string, object> SerializeSettings()
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			if (this.Criteria != null)
			{
				dictionary["criteria"] = from item in this.Criteria
					select item.Serialize();
			}
			if (this.Filter != null)
			{
				dictionary["filter"] = this.Filter;
			}
			if (this.Index != null)
			{
				dictionary["index"] = this.Index;
			}
			if (this.Logic != null)
			{
				dictionary["logic"] = this.Logic;
			}
			if (this.Type != null)
			{
				dictionary["type"] = this.Type;
			}
			if (this.Value != null)
			{
				dictionary["value"] = this.Value;
			}
			if (this.Values != null)
			{
				dictionary["values"] = this.Values;
			}
			if (this.Blanks != null)
			{
				dictionary["blanks"] = this.Blanks;
			}
			return dictionary;
		}
	}
}
