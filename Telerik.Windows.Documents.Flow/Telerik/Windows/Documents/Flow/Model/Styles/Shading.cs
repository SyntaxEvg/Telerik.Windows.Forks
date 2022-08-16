using System;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Flow.Model.Styles
{
	public class Shading
	{
		public Shading(IPropertiesWithShading properties)
		{
			this.properties = properties;
		}

		public ThemableColor BackgroundColor
		{
			get
			{
				return this.properties.BackgroundColor.GetActualValue();
			}
			set
			{
				this.properties.BackgroundColor.LocalValue = value;
			}
		}

		public ThemableColor PatternColor
		{
			get
			{
				return this.properties.ShadingPatternColor.GetActualValue();
			}
			set
			{
				this.properties.ShadingPatternColor.LocalValue = value;
			}
		}

		public ShadingPattern Pattern
		{
			get
			{
				return this.properties.ShadingPattern.GetActualValue().Value;
			}
			set
			{
				this.properties.ShadingPattern.LocalValue = new ShadingPattern?(value);
			}
		}

		readonly IPropertiesWithShading properties;
	}
}
