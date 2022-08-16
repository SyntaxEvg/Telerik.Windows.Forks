using System;
using Telerik.Windows.Documents.Flow.Model.Lists;
using Telerik.Windows.Documents.Flow.Model.Styles;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Lists.ListsInfo
{
	class ListLevelInfo
	{
		public ListLevelInfo()
		{
		}

		public ListLevelInfo(ListLevel listLevel)
		{
			this.Alignment = new Alignment?(listLevel.Alignment);
			this.IsLegal = new bool?(listLevel.IsLegal);
			this.NumberingStyle = new NumberingStyle?(listLevel.NumberingStyle);
			this.NumberTextFormat = listLevel.NumberTextFormat;
			this.StartIndex = new int?(listLevel.StartIndex);
			this.RestartAfterLevel = new int?(listLevel.RestartAfterLevel);
			this.StyleId = listLevel.StyleId;
			if (listLevel.ParagraphProperties != null && listLevel.ParagraphProperties.HasLocalValues())
			{
				this.ParagraphProperties.CopyPropertiesFrom(listLevel.ParagraphProperties);
			}
			if (listLevel.CharacterProperties != null && listLevel.CharacterProperties.HasLocalValues())
			{
				this.CharacterProperties.CopyPropertiesFrom(listLevel.CharacterProperties);
			}
		}

		public ListLevelInfo(ListLevelInfo listLevelInfo)
		{
			this.ListLevelId = listLevelInfo.ListLevelId;
			this.Alignment = listLevelInfo.Alignment;
			this.IsLegal = listLevelInfo.IsLegal;
			this.NumberingStyle = listLevelInfo.NumberingStyle;
			this.NumberTextFormat = listLevelInfo.NumberTextFormat;
			this.StartIndex = listLevelInfo.StartIndex;
			this.RestartAfterLevel = listLevelInfo.RestartAfterLevel;
			this.StyleId = listLevelInfo.StyleId;
			if (listLevelInfo.ParagraphProperties != null && listLevelInfo.ParagraphProperties.HasLocalValues())
			{
				this.ParagraphProperties.CopyPropertiesFrom(listLevelInfo.ParagraphProperties);
			}
			if (listLevelInfo.CharacterProperties != null && listLevelInfo.CharacterProperties.HasLocalValues())
			{
				this.CharacterProperties.CopyPropertiesFrom(listLevelInfo.CharacterProperties);
			}
		}

		public int ListLevelId { get; set; }

		public int? StartIndex { get; set; }

		public int? RestartAfterLevel { get; set; }

		public string NumberTextFormat { get; set; }

		public NumberingStyle? NumberingStyle { get; set; }

		public bool? IsLegal { get; set; }

		public string StyleId { get; set; }

		public Alignment? Alignment { get; set; }

		public ParagraphProperties ParagraphProperties
		{
			get
			{
				return this.listLevel.ParagraphProperties;
			}
		}

		public CharacterProperties CharacterProperties
		{
			get
			{
				return this.listLevel.CharacterProperties;
			}
		}

		public ListLevel GetListLevel()
		{
			ListLevel listLevel = new ListLevel(null);
			if (this.Alignment != null)
			{
				listLevel.Alignment = this.Alignment.Value;
			}
			if (this.IsLegal != null)
			{
				listLevel.IsLegal = this.IsLegal.Value;
			}
			if (this.NumberingStyle != null)
			{
				listLevel.NumberingStyle = this.NumberingStyle.Value;
			}
			if (!string.IsNullOrEmpty(this.NumberTextFormat))
			{
				listLevel.NumberTextFormat = this.NumberTextFormat;
			}
			if (this.StartIndex != null)
			{
				listLevel.StartIndex = this.StartIndex.Value;
			}
			if (this.RestartAfterLevel != null)
			{
				listLevel.RestartAfterLevel = this.RestartAfterLevel.Value;
			}
			if (!string.IsNullOrEmpty(this.StyleId))
			{
				listLevel.StyleId = this.StyleId;
			}
			if (this.ParagraphProperties.HasLocalValues())
			{
				listLevel.ParagraphProperties.CopyPropertiesFrom(this.ParagraphProperties);
			}
			if (this.CharacterProperties.HasLocalValues())
			{
				listLevel.CharacterProperties.CopyPropertiesFrom(this.CharacterProperties);
			}
			return listLevel;
		}

		public void CopyPropertiesFrom(ListLevelInfo listLevelInfo)
		{
			this.ListLevelId = listLevelInfo.ListLevelId;
			if (this.Alignment != null)
			{
				this.Alignment = listLevelInfo.Alignment;
			}
			if (listLevelInfo.IsLegal != null)
			{
				this.IsLegal = listLevelInfo.IsLegal;
			}
			if (listLevelInfo.NumberingStyle != null)
			{
				this.NumberingStyle = listLevelInfo.NumberingStyle;
			}
			if (!string.IsNullOrEmpty(listLevelInfo.NumberTextFormat))
			{
				this.NumberTextFormat = listLevelInfo.NumberTextFormat;
			}
			if (listLevelInfo.StartIndex != null)
			{
				this.StartIndex = listLevelInfo.StartIndex;
			}
			if (listLevelInfo.RestartAfterLevel != null)
			{
				this.RestartAfterLevel = listLevelInfo.RestartAfterLevel;
			}
			if (!string.IsNullOrEmpty(listLevelInfo.StyleId))
			{
				this.StyleId = listLevelInfo.StyleId;
			}
			if (listLevelInfo.ParagraphProperties != null && listLevelInfo.ParagraphProperties.HasLocalValues())
			{
				this.ParagraphProperties.CopyPropertiesFrom(listLevelInfo.ParagraphProperties);
			}
			if (listLevelInfo.CharacterProperties != null && listLevelInfo.CharacterProperties.HasLocalValues())
			{
				this.CharacterProperties.CopyPropertiesFrom(listLevelInfo.CharacterProperties);
			}
		}

		readonly ListLevel listLevel = new ListLevel(new List());
	}
}
