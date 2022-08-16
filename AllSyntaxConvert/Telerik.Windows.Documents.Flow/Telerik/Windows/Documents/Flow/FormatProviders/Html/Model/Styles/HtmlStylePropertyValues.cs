using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Media;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Styles
{
	class HtmlStylePropertyValues
	{
		public HtmlStylePropertyValues(string value)
		{
			this.unparesedValue = value;
			this.hasRelativeValues = false;
			string[] array = value.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
			this.values = new List<HtmlStylePropertyValue>(array.Length);
			for (int i = 0; i < array.Length; i++)
			{
				HtmlStylePropertyValue htmlStylePropertyValue = new HtmlStylePropertyValue(array[i]);
				if (htmlStylePropertyValue.UnitType != null && Unit.IsRelativeUnitType(htmlStylePropertyValue.UnitType.Value))
				{
					this.hasRelativeValues = true;
				}
				this.values.Add(htmlStylePropertyValue);
			}
			if (this.values.Count == 0)
			{
				this.values.Add(new HtmlStylePropertyValue(string.Empty));
			}
		}

		public bool HasRelativeValues
		{
			get
			{
				return this.hasRelativeValues;
			}
		}

		public string UnparsedValue
		{
			get
			{
				return this.unparesedValue;
			}
		}

		public int Count
		{
			get
			{
				return this.values.Count;
			}
		}

		public HtmlStylePropertyValue this[int index]
		{
			get
			{
				return this.values[index];
			}
		}

		readonly string unparesedValue;

		readonly List<HtmlStylePropertyValue> values;

		readonly bool hasRelativeValues;
	}
}
