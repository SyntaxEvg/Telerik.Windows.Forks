using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Telerik.Web.Spreadsheet
{
	[DataContract]
	public class Cell
	{
		[DataMember(Name = "borderBottom", EmitDefaultValue = false)]
		public BorderStyle BorderBottom { get; set; }

		[DataMember(Name = "borderLeft", EmitDefaultValue = false)]
		public BorderStyle BorderLeft { get; set; }

		[DataMember(Name = "borderTop", EmitDefaultValue = false)]
		public BorderStyle BorderTop { get; set; }

		[DataMember(Name = "borderRight", EmitDefaultValue = false)]
		public BorderStyle BorderRight { get; set; }

		[DataMember(Name = "validation", EmitDefaultValue = false)]
		public Validation Validation { get; set; }

		internal Dictionary<string, object> Serialize()
		{
			Dictionary<string, object> dictionary = this.SerializeSettings();
			if (this.BorderBottom != null)
			{
				dictionary["borderBottom"] = this.BorderBottom.Serialize();
			}
			if (this.BorderLeft != null)
			{
				dictionary["borderLeft"] = this.BorderLeft.Serialize();
			}
			if (this.BorderTop != null)
			{
				dictionary["borderTop"] = this.BorderTop.Serialize();
			}
			if (this.BorderRight != null)
			{
				dictionary["borderRight"] = this.BorderRight.Serialize();
			}
			if (this.Validation != null)
			{
				dictionary["validation"] = this.Validation.Serialize();
			}
			return dictionary;
		}

		[DataMember(Name = "background", EmitDefaultValue = false)]
		public string Background { get; set; }

		[DataMember(Name = "color", EmitDefaultValue = false)]
		public string Color { get; set; }

		[DataMember(Name = "fontFamily", EmitDefaultValue = false)]
		public string FontFamily { get; set; }

		[DataMember(Name = "fontSize", EmitDefaultValue = false)]
		public double? FontSize { get; set; }

		[DataMember(Name = "italic", EmitDefaultValue = false)]
		public bool? Italic { get; set; }

		[DataMember(Name = "bold", EmitDefaultValue = false)]
		public bool? Bold { get; set; }

		[DataMember(Name = "enable", EmitDefaultValue = false)]
		public bool? Enable { get; set; }

		[DataMember(Name = "format", EmitDefaultValue = false)]
		public string Format { get; set; }

		[DataMember(Name = "formula", EmitDefaultValue = false)]
		public string Formula { get; set; }

		[DataMember(Name = "index", EmitDefaultValue = false)]
		public int? Index { get; set; }

		[DataMember(Name = "link", EmitDefaultValue = false)]
		public string Link { get; set; }

		[DataMember(Name = "textAlign", EmitDefaultValue = false)]
		public string TextAlign { get; set; }

		[DataMember(Name = "underline", EmitDefaultValue = false)]
		public bool? Underline { get; set; }

		[DataMember(Name = "value", EmitDefaultValue = false)]
		public object Value { get; set; }

		[DataMember(Name = "verticalAlign", EmitDefaultValue = false)]
		public string VerticalAlign { get; set; }

		[DataMember(Name = "wrap", EmitDefaultValue = false)]
		public bool? Wrap { get; set; }

		[DataMember(Name = "editor", EmitDefaultValue = false)]
		public string Editor { get; set; }

		protected Dictionary<string, object> SerializeSettings()
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			if (this.Background != null)
			{
				dictionary["background"] = this.Background;
			}
			if (this.Color != null)
			{
				dictionary["color"] = this.Color;
			}
			if (this.FontFamily != null)
			{
				dictionary["fontFamily"] = this.FontFamily;
			}
			if (this.FontSize != null)
			{
				dictionary["fontSize"] = this.FontSize;
			}
			if (this.Italic != null)
			{
				dictionary["italic"] = this.Italic;
			}
			if (this.Bold != null)
			{
				dictionary["bold"] = this.Bold;
			}
			if (this.Enable != null)
			{
				dictionary["enable"] = this.Enable;
			}
			if (this.Format != null)
			{
				dictionary["format"] = this.Format;
			}
			if (this.Formula != null)
			{
				dictionary["formula"] = this.Formula;
			}
			if (this.Index != null)
			{
				dictionary["index"] = this.Index;
			}
			if (this.Link != null)
			{
				dictionary["link"] = this.Link;
			}
			if (this.TextAlign != null)
			{
				dictionary["textAlign"] = this.TextAlign;
			}
			if (this.Underline != null)
			{
				dictionary["underline"] = this.Underline;
			}
			if (this.Value != null)
			{
				dictionary["value"] = this.Value;
			}
			if (this.VerticalAlign != null)
			{
				dictionary["verticalAlign"] = this.VerticalAlign;
			}
			if (this.Wrap != null)
			{
				dictionary["wrap"] = this.Wrap;
			}
			if (this.Editor != null)
			{
				dictionary["editor"] = this.Editor;
			}
			return dictionary;
		}
	}
}
