using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.Fixed.Model.Fonts;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Model.Text
{
	class TextCollection
	{
		public TextCollection()
		{
			this.isInvalidated = false;
		}

		public TextCollection(FontBase font, string text)
		{
			Guard.ThrowExceptionIfNull<FontBase>(font, "font");
			Guard.ThrowExceptionIfNull<string>(text, "text");
			this.Font = font;
			this.Text = text;
		}

		public TextCollection(TextCollection other)
		{
			Guard.ThrowExceptionIfNull<TextCollection>(other, "other");
			this.font = other.font;
			this.text = other.text;
			this.charInfos = other.charInfos;
			this.isInvalidated = other.isInvalidated;
		}

		public FontBase Font
		{
			get
			{
				return this.font;
			}
			set
			{
				Guard.ThrowExceptionIfNull<FontBase>(value, "Font");
				if (this.font != value)
				{
					this.font = value;
					this.Invalidate();
				}
			}
		}

		public string Text
		{
			get
			{
				return this.text;
			}
			set
			{
				Guard.ThrowExceptionIfNull<string>(value, "value");
				if (this.text != value)
				{
					this.text = value;
					this.Invalidate();
				}
			}
		}

		public IEnumerable<CharInfo> Characters
		{
			get
			{
				if (this.isInvalidated)
				{
					if (this.Font == null)
					{
						this.charInfos = Enumerable.Empty<CharInfo>();
					}
					else
					{
						this.charInfos = this.Font.CalculateCharacters(this.Text);
					}
					this.isInvalidated = false;
				}
				return this.charInfos;
			}
			set
			{
				if (this.charInfos != value)
				{
					this.charInfos = value;
					this.text = string.Join("", from g in this.charInfos
						select g.Unicode);
					this.isInvalidated = false;
				}
			}
		}

		void Invalidate()
		{
			this.isInvalidated = true;
			this.charInfos = null;
		}

		FontBase font;

		string text;

		IEnumerable<CharInfo> charInfos;

		bool isInvalidated;
	}
}
