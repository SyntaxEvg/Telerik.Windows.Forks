using System;
using System.Globalization;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Import.Parser;
using Telerik.Windows.Documents.Media;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Styles
{
	class HtmlStylePropertyValue
	{
		public HtmlStylePropertyValue(string value)
		{
			this.value = value;
		}

		public string Value
		{
			get
			{
				return this.value;
			}
		}

		public double ValueAsDouble
		{
			get
			{
				this.AssureValuesAreParsed();
				return this.valueAsDouble;
			}
		}

		public int ValueAsInt
		{
			get
			{
				this.AssureValuesAreParsed();
				return (int)Math.Round(this.valueAsDouble);
			}
		}

		public UnitType? UnitType
		{
			get
			{
				this.AssureValuesAreParsed();
				return this.unitType;
			}
		}

		void AssureValuesAreParsed()
		{
			if (!this.isParsed)
			{
				double num = 0.0;
				UnitType? unitType = null;
				string s;
				HtmlUnitParser.Parse(this.Value, out s, out unitType);
				if (double.TryParse(s, NumberStyles.Number, CultureInfo.InvariantCulture, out num))
				{
					this.valueAsDouble = num;
				}
				else
				{
					this.valueAsDouble = double.NaN;
				}
				this.unitType = unitType;
				this.isParsed = true;
			}
		}

		readonly string value;

		bool isParsed;

		double valueAsDouble;

		UnitType? unitType;
	}
}
