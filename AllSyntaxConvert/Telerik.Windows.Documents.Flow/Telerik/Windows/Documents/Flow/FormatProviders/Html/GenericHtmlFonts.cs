using System;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html
{
	public class GenericHtmlFonts
	{
		internal GenericHtmlFonts()
		{
			this.Cursive = new ThemableFontFamily("Comic Sans MS");
			this.Fantasy = new ThemableFontFamily("Algerian");
			this.Monospace = new ThemableFontFamily("Courier New");
			this.SansSerif = new ThemableFontFamily("Arial");
			this.Serif = new ThemableFontFamily("Times New Roman");
		}

		public ThemableFontFamily Cursive
		{
			get
			{
				return this.cursive;
			}
			set
			{
				Guard.ThrowExceptionIfNull<ThemableFontFamily>(value, "value");
				this.cursive = value;
			}
		}

		public ThemableFontFamily Fantasy
		{
			get
			{
				return this.fantasy;
			}
			set
			{
				Guard.ThrowExceptionIfNull<ThemableFontFamily>(value, "value");
				this.fantasy = value;
			}
		}

		public ThemableFontFamily Monospace
		{
			get
			{
				return this.monospace;
			}
			set
			{
				Guard.ThrowExceptionIfNull<ThemableFontFamily>(value, "value");
				this.monospace = value;
			}
		}

		public ThemableFontFamily SansSerif
		{
			get
			{
				return this.sansSerif;
			}
			set
			{
				Guard.ThrowExceptionIfNull<ThemableFontFamily>(value, "value");
				this.sansSerif = value;
			}
		}

		public ThemableFontFamily Serif
		{
			get
			{
				return this.serif;
			}
			set
			{
				Guard.ThrowExceptionIfNull<ThemableFontFamily>(value, "value");
				this.serif = value;
			}
		}

		ThemableFontFamily cursive;

		ThemableFontFamily fantasy;

		ThemableFontFamily monospace;

		ThemableFontFamily sansSerif;

		ThemableFontFamily serif;
	}
}
