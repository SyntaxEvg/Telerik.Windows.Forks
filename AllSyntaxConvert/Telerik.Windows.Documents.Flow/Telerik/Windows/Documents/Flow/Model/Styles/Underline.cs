using System;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Flow.Model.Styles
{
	public class Underline
	{
		public Underline(CharacterProperties characterProperties)
		{
			this.characterProperties = characterProperties;
		}

		public ThemableColor Color
		{
			get
			{
				return this.characterProperties.UnderlineColor.GetActualValue();
			}
			set
			{
				this.characterProperties.UnderlineColor.LocalValue = value;
			}
		}

		public UnderlinePattern Pattern
		{
			get
			{
				return this.characterProperties.UnderlinePattern.GetActualValue().Value;
			}
			set
			{
				this.characterProperties.UnderlinePattern.LocalValue = new UnderlinePattern?(value);
			}
		}

		readonly CharacterProperties characterProperties;
	}
}
