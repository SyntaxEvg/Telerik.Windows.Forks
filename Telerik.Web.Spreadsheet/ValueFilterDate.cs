using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Telerik.Web.Spreadsheet
{
	[DataContract]
	public class ValueFilterDate
	{
		[DataMember(Name = "year", EmitDefaultValue = false)]
		public int Year { get; set; }

		[DataMember(Name = "month", EmitDefaultValue = false)]
		public int Month { get; set; }

		[DataMember(Name = "day", EmitDefaultValue = false)]
		public int Day { get; set; }

		[DataMember(Name = "hours", EmitDefaultValue = false)]
		public int Hours { get; set; }

		[DataMember(Name = "minutes", EmitDefaultValue = false)]
		public int Minutes { get; set; }

		[DataMember(Name = "seconds", EmitDefaultValue = false)]
		public int Seconds { get; set; }

		internal Dictionary<string, object> Serialize()
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["year"] = this.Year;
			dictionary["month"] = this.Month;
			dictionary["day"] = this.Day;
			dictionary["hours"] = this.Hours;
			dictionary["minutes"] = this.Minutes;
			dictionary["seconds"] = this.Seconds;
			return dictionary;
		}
	}
}
