using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Telerik.Web.Spreadsheet
{
	[DataContract]
	public class Pdf
	{
		internal Dictionary<string, object> Serialize()
		{
			return this.SerializeSettings();
		}

		[DataMember(Name = "area", EmitDefaultValue = false)]
		public string Area { get; set; }

		[DataMember(Name = "author", EmitDefaultValue = false)]
		public string Author { get; set; }

		[DataMember(Name = "creator", EmitDefaultValue = false)]
		public string Creator { get; set; }

		[DataMember(Name = "date", EmitDefaultValue = false)]
		public DateTime? Date { get; set; }

		[DataMember(Name = "fileName", EmitDefaultValue = false)]
		public string FileName { get; set; }

		[DataMember(Name = "fitWidth", EmitDefaultValue = false)]
		public bool? FitWidth { get; set; }

		[DataMember(Name = "forceProxy", EmitDefaultValue = false)]
		public bool? ForceProxy { get; set; }

		[DataMember(Name = "guidelines", EmitDefaultValue = false)]
		public bool? Guidelines { get; set; }

		[DataMember(Name = "hCenter", EmitDefaultValue = false)]
		public bool? HCenter { get; set; }

		[DataMember(Name = "keywords", EmitDefaultValue = false)]
		public string Keywords { get; set; }

		[DataMember(Name = "landscape", EmitDefaultValue = false)]
		public bool? Landscape { get; set; }

		[DataMember(Name = "margin", EmitDefaultValue = false)]
		public Margin Margin { get; set; }

		[DataMember(Name = "paperSize", EmitDefaultValue = false)]
		public string PaperSize { get; set; }

		[DataMember(Name = "proxyURL", EmitDefaultValue = false)]
		public string ProxyURL { get; set; }

		[DataMember(Name = "proxyTarget", EmitDefaultValue = false)]
		public string ProxyTarget { get; set; }

		[DataMember(Name = "subject", EmitDefaultValue = false)]
		public string Subject { get; set; }

		[DataMember(Name = "title", EmitDefaultValue = false)]
		public string Title { get; set; }

		[DataMember(Name = "vCenter", EmitDefaultValue = false)]
		public bool? VCenter { get; set; }

		protected Dictionary<string, object> SerializeSettings()
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			if (this.Area != null)
			{
				dictionary["area"] = this.Area;
			}
			if (this.Author != null)
			{
				dictionary["author"] = this.Author;
			}
			if (this.Creator != null)
			{
				dictionary["creator"] = this.Creator;
			}
			if (this.Date != null)
			{
				dictionary["date"] = this.Date;
			}
			if (this.FileName != null)
			{
				dictionary["fileName"] = this.FileName;
			}
			if (this.FitWidth != null)
			{
				dictionary["fitWidth"] = this.FitWidth;
			}
			if (this.ForceProxy != null)
			{
				dictionary["forceProxy"] = this.ForceProxy;
			}
			if (this.Guidelines != null)
			{
				dictionary["guidelines"] = this.Guidelines;
			}
			if (this.HCenter != null)
			{
				dictionary["hCenter"] = this.HCenter;
			}
			if (this.Keywords != null)
			{
				dictionary["keywords"] = this.Keywords;
			}
			if (this.Landscape != null)
			{
				dictionary["landscape"] = this.Landscape;
			}
			if (this.Margin != null)
			{
				dictionary["margin"] = this.Margin.Serialize();
			}
			if (this.PaperSize != null)
			{
				dictionary["paperSize"] = this.PaperSize;
			}
			if (this.ProxyURL != null)
			{
				dictionary["proxyURL"] = this.ProxyURL;
			}
			if (this.ProxyTarget != null)
			{
				dictionary["proxyTarget"] = this.ProxyTarget;
			}
			if (this.Subject != null)
			{
				dictionary["subject"] = this.Subject;
			}
			if (this.Title != null)
			{
				dictionary["title"] = this.Title;
			}
			if (this.VCenter != null)
			{
				dictionary["vCenter"] = this.VCenter;
			}
			return dictionary;
		}
	}
}
