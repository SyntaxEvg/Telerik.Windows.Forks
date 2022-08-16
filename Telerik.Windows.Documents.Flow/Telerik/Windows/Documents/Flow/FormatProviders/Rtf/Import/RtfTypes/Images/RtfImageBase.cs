using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.Model;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.RtfTypes.Images
{
	abstract class RtfImageBase
	{
		public RtfImageBase()
		{
			this.Properties = new Dictionary<string, RtfGroup>();
		}

		public Dictionary<string, RtfGroup> Properties { get; set; }

		public void ReadInstructionGroup(RtfGroup group)
		{
			RtfGroup rtfGroup = group.Elements.FirstOrDefault((RtfElement e) => e.Type == RtfElementType.Group && ((RtfGroup)e).Destination == "sn") as RtfGroup;
			RtfGroup rtfGroup2 = group.Elements.FirstOrDefault((RtfElement e) => e.Type == RtfElementType.Group && ((RtfGroup)e).Destination == "sv") as RtfGroup;
			if (rtfGroup == null || rtfGroup2 == null)
			{
				return;
			}
			RtfText rtfText = rtfGroup.Elements.FirstOrDefault((RtfElement e) => e.Type == RtfElementType.Text) as RtfText;
			if (rtfText == null)
			{
				return;
			}
			this.Properties[rtfText.Text] = rtfGroup2;
		}

		public string GetPropertyStringValue(string propertyName)
		{
			return this.GetPropertyStringValue(propertyName, string.Empty);
		}

		public string GetPropertyStringValue(string propertyName, string defalutValue)
		{
			string result = defalutValue;
			RtfGroup rtfGroup = null;
			this.Properties.TryGetValue(propertyName, out rtfGroup);
			if (rtfGroup != null && rtfGroup.Elements.Count > 1 && rtfGroup.Elements[1] is RtfText)
			{
				result = ((RtfText)rtfGroup.Elements[1]).Text;
			}
			return result;
		}

		public int GetPropertyIntValue(string propertyName, int defaultValue)
		{
			string propertyStringValue = this.GetPropertyStringValue(propertyName, string.Empty);
			int result;
			if (!int.TryParse(propertyStringValue, out result))
			{
				result = defaultValue;
			}
			return result;
		}

		public bool GetPropertyBoolValue(string propertyName, bool defalutValue)
		{
			bool result = defalutValue;
			string propertyStringValue = this.GetPropertyStringValue(propertyName, string.Empty);
			if (!string.IsNullOrEmpty(propertyStringValue))
			{
				result = propertyStringValue == "1";
			}
			return result;
		}
	}
}
