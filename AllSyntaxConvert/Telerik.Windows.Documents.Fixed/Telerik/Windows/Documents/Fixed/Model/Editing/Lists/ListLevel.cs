using System;
using Telerik.Windows.Documents.Fixed.Model.Editing.Flow;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Model.Editing.Lists
{
	public class ListLevel
	{
		public ListLevel()
			: this(BulletNumberingFormats.EmptyNumbering)
		{
		}

		public ListLevel(IBulletNumberingFormat numberingStyleFormat)
		{
			this.IsAddedToListLevelCollection = false;
			this.BulletNumberingFormat = numberingStyleFormat;
			this.characterProperties = new CharacterProperties();
			this.paragraphProperties = new ParagraphProperties();
			this.StartIndex = FixedDocumentDefaults.DefaultListLevelStartIndex;
			this.RestartAfterLevel = FixedDocumentDefaults.DefaultListLevelRestartAfterLevel;
			this.IndentAfterBullet = FixedDocumentDefaults.DefaultIndentAfterBullet;
		}

		public int StartIndex
		{
			get
			{
				return this.startIndex;
			}
			set
			{
				if (this.startIndex != value)
				{
					this.startIndex = value;
					this.RestartCurrentNumber();
				}
			}
		}

		public int RestartAfterLevel { get; set; }

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

		public double IndentAfterBullet
		{
			get
			{
				return this.indentAfterBullet;
			}
			set
			{
				Guard.ThrowExceptionIfLessThan<double>(0.0, value, "value");
				this.indentAfterBullet = value;
			}
		}

		public IBulletNumberingFormat BulletNumberingFormat
		{
			get
			{
				return this.numberingStyleFormat;
			}
			set
			{
				Guard.ThrowExceptionIfNull<IBulletNumberingFormat>(value, "value");
				this.numberingStyleFormat = value;
			}
		}

		internal bool IsAddedToListLevelCollection { get; set; }

		internal int LastInsertedIndex { get; set; }

		internal void RestartCurrentNumber()
		{
			this.LastInsertedIndex = this.StartIndex - 1;
		}

		readonly CharacterProperties characterProperties;

		readonly ParagraphProperties paragraphProperties;

		IBulletNumberingFormat numberingStyleFormat;

		double indentAfterBullet;

		int startIndex;
	}
}
