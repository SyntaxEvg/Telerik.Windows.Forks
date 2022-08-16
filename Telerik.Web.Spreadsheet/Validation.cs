using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Telerik.Web.Spreadsheet
{
	[DataContract]
	public class Validation
	{
		[DataMember(Name = "type", EmitDefaultValue = false)]
		public string Type { get; set; }

		[DataMember(Name = "comparerType", EmitDefaultValue = false)]
		public string ComparerType { get; set; }

		[DataMember(Name = "dataType", EmitDefaultValue = false)]
		public string DataType { get; set; }

		[DataMember(Name = "from", EmitDefaultValue = false)]
		public string From { get; set; }

		[DataMember(Name = "to", EmitDefaultValue = false)]
		public string To { get; set; }

		[DataMember(Name = "showButton", EmitDefaultValue = false)]
		public bool? ShowButton { get; set; }

		[DataMember(Name = "allowNulls", EmitDefaultValue = false)]
		public bool? AllowNulls { get; set; }

		[DataMember(Name = "messageTemplate", EmitDefaultValue = false)]
		public string MessageTemplate { get; set; }

		[DataMember(Name = "titleTemplate", EmitDefaultValue = false)]
		public string TitleTemplate { get; set; }

		protected Dictionary<string, object> SerializeSettings()
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			if (this.Type != null)
			{
				dictionary["type"] = this.Type;
			}
			if (this.ComparerType != null)
			{
				dictionary["comparerType"] = this.ComparerType;
			}
			if (this.DataType != null)
			{
				dictionary["dataType"] = this.DataType;
			}
			if (this.From != null)
			{
				dictionary["from"] = this.From;
			}
			if (this.To != null)
			{
				dictionary["to"] = this.To;
			}
			if (this.ShowButton != null)
			{
				dictionary["showButton"] = this.ShowButton;
			}
			if (this.AllowNulls != null)
			{
				dictionary["allowNulls"] = this.AllowNulls;
			}
			if (this.MessageTemplate != null)
			{
				dictionary["messageTemplate"] = this.MessageTemplate;
			}
			if (this.TitleTemplate != null)
			{
				dictionary["titleTemplate"] = this.TitleTemplate;
			}
			return dictionary;
		}

		internal Dictionary<string, object> Serialize()
		{
			return this.SerializeSettings();
		}
	}
}
