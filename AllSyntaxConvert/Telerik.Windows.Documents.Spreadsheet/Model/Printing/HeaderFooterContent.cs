using System;
using System.Collections.Generic;
using System.Text;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model.Printing
{
	public class HeaderFooterContent
	{
		internal HeaderFooterContent(Action updateOnChange)
		{
			this.leftSection = new HeaderFooterSection(updateOnChange);
			this.centerSection = new HeaderFooterSection(updateOnChange);
			this.rightSection = new HeaderFooterSection(updateOnChange);
		}

		internal HeaderFooterContent(HeaderFooterContent other, Action updateOnChange)
		{
			this.leftSection = new HeaderFooterSection(other.leftSection, updateOnChange);
			this.centerSection = new HeaderFooterSection(other.centerSection, updateOnChange);
			this.rightSection = new HeaderFooterSection(other.rightSection, updateOnChange);
		}

		public HeaderFooterSection LeftSection
		{
			get
			{
				return this.leftSection;
			}
		}

		public HeaderFooterSection CenterSection
		{
			get
			{
				return this.centerSection;
			}
		}

		public HeaderFooterSection RightSection
		{
			get
			{
				return this.rightSection;
			}
		}

		public bool HasValidLength
		{
			get
			{
				string text = this.BuildContentText();
				return text.Length <= 255;
			}
		}

		internal IEnumerable<HeaderFooterSection> Sections
		{
			get
			{
				yield return this.LeftSection;
				yield return this.CenterSection;
				yield return this.RightSection;
				yield break;
			}
		}

		internal bool IsEmpty
		{
			get
			{
				string value = this.BuildContentText();
				return string.IsNullOrEmpty(value);
			}
		}

		internal string BuildContentText()
		{
			HeaderFooterSectionTextSanitizer headerFooterSectionTextSanitizer = new HeaderFooterSectionTextSanitizer();
			string text = headerFooterSectionTextSanitizer.Sanitize(this.LeftSection.Text);
			string text2 = headerFooterSectionTextSanitizer.Sanitize(this.CenterSection.Text);
			string text3 = headerFooterSectionTextSanitizer.Sanitize(this.RightSection.Text);
			StringBuilder stringBuilder = new StringBuilder();
			if (!string.IsNullOrEmpty(text))
			{
				stringBuilder.AppendFormat("{0}{1}{2}", '&', 'L', text);
			}
			if (!string.IsNullOrEmpty(text2))
			{
				stringBuilder.AppendFormat("{0}{1}{2}", '&', 'C', text2);
			}
			if (!string.IsNullOrEmpty(text3))
			{
				stringBuilder.AppendFormat("{0}{1}{2}", '&', 'R', text3);
			}
			return stringBuilder.ToString();
		}

		internal void SetFromContentText(string contentText)
		{
			contentText = contentText ?? string.Empty;
			StringBuilder stringBuilder = new StringBuilder();
			StringBuilder stringBuilder2 = new StringBuilder();
			StringBuilder stringBuilder3 = new StringBuilder();
			StringBuilder stringBuilder4 = null;
			bool flag = false;
			foreach (char c in contentText)
			{
				if (flag)
				{
					char c2 = c;
					if (c2 != 'C')
					{
						if (c2 != 'L')
						{
							if (c2 != 'R')
							{
								if (stringBuilder4 != null)
								{
									stringBuilder4.AppendFormat("{0}{1}", '&', c);
								}
							}
							else
							{
								stringBuilder4 = stringBuilder3;
							}
						}
						else
						{
							stringBuilder4 = stringBuilder;
						}
					}
					else
					{
						stringBuilder4 = stringBuilder2;
					}
					flag = false;
				}
				else if (c == '&')
				{
					flag = true;
				}
				else if (stringBuilder4 != null)
				{
					stringBuilder4.Append(c);
				}
			}
			HeaderFooterSectionTextSanitizer headerFooterSectionTextSanitizer = new HeaderFooterSectionTextSanitizer();
			this.LeftSection.Text = headerFooterSectionTextSanitizer.Sanitize(stringBuilder.ToString());
			this.CenterSection.Text = headerFooterSectionTextSanitizer.Sanitize(stringBuilder2.ToString());
			this.RightSection.Text = headerFooterSectionTextSanitizer.Sanitize(stringBuilder3.ToString());
		}

		internal void CopyFrom(HeaderFooterContent fromHeaderFooterContent)
		{
			this.LeftSection.CopyFrom(fromHeaderFooterContent.LeftSection);
			this.CenterSection.CopyFrom(fromHeaderFooterContent.CenterSection);
			this.RightSection.CopyFrom(fromHeaderFooterContent.RightSection);
		}

		internal void Clear()
		{
			this.LeftSection.Clear();
			this.CenterSection.Clear();
			this.RightSection.Clear();
		}

		public override bool Equals(object obj)
		{
			HeaderFooterContent headerFooterContent = obj as HeaderFooterContent;
			return headerFooterContent != null && (this.LeftSection.Equals(headerFooterContent.LeftSection) && this.CenterSection.Equals(headerFooterContent.CenterSection)) && this.RightSection.Equals(headerFooterContent.RightSection);
		}

		public override int GetHashCode()
		{
			return TelerikHelper.CombineHashCodes(this.LeftSection.GetHashCode(), this.CenterSection.GetHashCode(), this.RightSection.GetHashCode());
		}

		internal const int MaxLength = 255;

		readonly HeaderFooterSection leftSection;

		readonly HeaderFooterSection centerSection;

		readonly HeaderFooterSection rightSection;
	}
}
