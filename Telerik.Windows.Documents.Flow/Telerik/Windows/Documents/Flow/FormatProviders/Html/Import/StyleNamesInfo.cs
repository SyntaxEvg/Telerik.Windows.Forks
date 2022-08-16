using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Import
{
	class StyleNamesInfo
	{
		public IEnumerable<string> CharacterStyles
		{
			get
			{
				return this.characterStyles;
			}
			set
			{
				Guard.ThrowExceptionIfNull<IEnumerable<string>>(value, "value");
				this.characterStyles = value;
			}
		}

		public IEnumerable<string> ParagraphStyles
		{
			get
			{
				return this.paragraphStyles;
			}
			set
			{
				Guard.ThrowExceptionIfNull<IEnumerable<string>>(value, "value");
				this.paragraphStyles = value;
			}
		}

		public IEnumerable<string> TableStyles
		{
			get
			{
				return this.tableStyles;
			}
			set
			{
				Guard.ThrowExceptionIfNull<IEnumerable<string>>(value, "value");
				this.tableStyles = value;
			}
		}

		IEnumerable<string> characterStyles;

		IEnumerable<string> paragraphStyles;

		IEnumerable<string> tableStyles;
	}
}
