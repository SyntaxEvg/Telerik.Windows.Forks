using System;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.Model.Lists
{
	public sealed class ListLevel
	{
		internal ListLevel(List ownerList)
		{
			this.OwnerList = ownerList;
			this.characterProperties = new CharacterProperties(this, false);
			this.paragraphProperties = new ParagraphProperties(this, false);
		}

		public int StartIndex
		{
			get
			{
				return this.startIndex;
			}
			set
			{
				Guard.ThrowExceptionIfLessThan<int>(0, value, "value");
				this.startIndex = value;
			}
		}

		public int RestartAfterLevel
		{
			get
			{
				return this.restartAfterLevel;
			}
			set
			{
				Guard.ThrowExceptionIfLessThan<int>(-1, value, "value");
				Guard.ThrowExceptionIfGreaterThan<int>(8, value, "value");
				this.restartAfterLevel = value;
			}
		}

		public string NumberTextFormat
		{
			get
			{
				return this.numberTextFormat;
			}
			set
			{
				this.numberTextFormat = value;
			}
		}

		public NumberingStyle NumberingStyle
		{
			get
			{
				return this.numberingStyle;
			}
			set
			{
				this.numberingStyle = value;
			}
		}

		public bool IsLegal
		{
			get
			{
				return this.isLegal;
			}
			set
			{
				this.isLegal = value;
			}
		}

		public string StyleId
		{
			get
			{
				return this.styleId;
			}
			set
			{
				this.styleId = value;
				if (this.Document != null)
				{
					Style style;
					if (BuiltInStyles.IsBuiltInStyle(this.styleId))
					{
						style = this.Document.StyleRepository.AddBuiltInStyle(this.styleId);
					}
					else
					{
						style = this.Document.StyleRepository.GetStyle(this.styleId);
					}
					Guard.ThrowExceptionIfTrue(style.StyleType != StyleType.Paragraph, "Only paragraph styles can be related to list levels.");
					int listLevelId = this.GetListLevelId();
					if (style != null && style.ParagraphProperties != null && !style.ParagraphProperties.ListId.HasLocalValue && !style.ParagraphProperties.ListLevel.HasLocalValue && this.OwnerList.Id != Paragraph.ListIdPropertyDefinition.DefaultValue && listLevelId != Paragraph.ListLevelPropertyDefinition.DefaultValue)
					{
						style.ParagraphProperties.ListLevel.LocalValue = new int?(listLevelId);
						style.ParagraphProperties.ListId.LocalValue = new int?(this.OwnerList.Id);
					}
				}
			}
		}

		public Alignment Alignment
		{
			get
			{
				return this.levelAlignment;
			}
			set
			{
				this.levelAlignment = value;
			}
		}

		public CharacterProperties CharacterProperties
		{
			get
			{
				return this.characterProperties;
			}
		}

		public ParagraphProperties ParagraphProperties
		{
			get
			{
				return this.paragraphProperties;
			}
		}

		public RadFlowDocument Document
		{
			get
			{
				if (this.OwnerList == null)
				{
					return null;
				}
				return this.OwnerList.Document;
			}
		}

		internal List OwnerList
		{
			get
			{
				return this.ownerList;
			}
			set
			{
				this.ownerList = value;
			}
		}

		public ListLevel Clone(List ownerList)
		{
			Guard.ThrowExceptionIfNull<List>(ownerList, "ownerList");
			ListLevel listLevel = new ListLevel(ownerList);
			listLevel.StartIndex = this.StartIndex;
			listLevel.RestartAfterLevel = this.RestartAfterLevel;
			listLevel.NumberTextFormat = this.NumberTextFormat;
			listLevel.NumberingStyle = this.NumberingStyle;
			listLevel.IsLegal = this.IsLegal;
			listLevel.StyleId = this.StyleId;
			listLevel.Alignment = this.Alignment;
			listLevel.CharacterProperties.CopyPropertiesFrom(this.CharacterProperties);
			listLevel.ParagraphProperties.CopyPropertiesFrom(this.ParagraphProperties);
			return listLevel;
		}

		int GetListLevelId()
		{
			int result = -1;
			if (this.OwnerList == null)
			{
				return result;
			}
			for (int i = 0; i < this.OwnerList.Levels.Count; i++)
			{
				if (this.OwnerList.Levels[i] == this)
				{
					result = i;
					break;
				}
			}
			return result;
		}

		readonly CharacterProperties characterProperties;

		readonly ParagraphProperties paragraphProperties;

		List ownerList;

		int startIndex = DocumentDefaultStyleSettings.StartIndex;

		int restartAfterLevel = DocumentDefaultStyleSettings.RestartAfterLevel;

		NumberingStyle numberingStyle = NumberingStyle.Bullet;

		bool isLegal;

		string numberTextFormat;

		Alignment levelAlignment;

		string styleId;
	}
}
