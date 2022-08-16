using System;
using System.Text;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.Model;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.RtfTypes.Fonts
{
	sealed class RtfFont : RtfElementIteratorBase
	{
		public string Id { get; set; }

		public string Name { get; set; }

		public string NameAlt { get; set; }

		public RtfFontFamily FontFamily { get; set; }

		public RtfFontPitch Pitch { get; set; }

		public int CharSet { get; set; }

		public int CodePage
		{
			get
			{
				if (this.codePage == 0)
				{
					return CharSetHelper.GetCodePage(this.CharSet);
				}
				return this.codePage;
			}
		}

		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			RtfFont rtfFont = obj as RtfFont;
			return rtfFont != null && this.Id.Equals(rtfFont.Id) && this.FontFamily == rtfFont.FontFamily && this.Pitch == rtfFont.Pitch && this.CharSet == rtfFont.CharSet && this.codePage == rtfFont.codePage && this.Name.Equals(rtfFont.Name);
		}

		public override int GetHashCode()
		{
			return HashTool.AddHashCode(base.GetType().GetHashCode(), this.ComputeHashCode());
		}

		public override string ToString()
		{
			return string.Format("{0}:{1}", this.Id, this.Name);
		}

		public void FillFont(RtfGroup group)
		{
			this.ResetState();
			base.VisitGroupChildren(group, false);
			this.Name = this.GetFontName();
		}

		protected override void DoVisitGroup(RtfGroup group)
		{
			string destination;
			switch (destination = group.Destination)
			{
			case "f":
			case "flomajor":
			case "fhimajor":
			case "fdbmajor":
			case "fbimajor":
			case "flominor":
			case "fhiminor":
			case "fdbminor":
			case "fbiminor":
				base.VisitGroupChildren(group, false);
				return;
			case "falt":
				this.GetFontAlt(group);
				break;

				return;
			}
		}

		protected override void DoVisitTag(RtfTag tag)
		{
			string name;
			switch (name = tag.Name)
			{
			case "flomajor":
			case "fhimajor":
			case "fdbmajor":
			case "fbimajor":
			case "flominor":
			case "fhiminor":
			case "fdbminor":
			case "fbiminor":
				break;
			case "f":
				this.Id = tag.FullName;
				return;
			case "fnil":
				this.FontFamily = RtfFontFamily.Nil;
				return;
			case "froman":
				this.FontFamily = RtfFontFamily.Roman;
				return;
			case "fswiss":
				this.FontFamily = RtfFontFamily.Swiss;
				return;
			case "fmodern":
				this.FontFamily = RtfFontFamily.Modern;
				return;
			case "fscript":
				this.FontFamily = RtfFontFamily.Script;
				return;
			case "fdecor":
				this.FontFamily = RtfFontFamily.Decor;
				return;
			case "ftech":
				this.FontFamily = RtfFontFamily.Tech;
				return;
			case "fbidi":
				this.FontFamily = RtfFontFamily.Bidi;
				return;
			case "fcharset":
				this.CharSet = tag.ValueAsNumber;
				return;
			case "cpg":
				this.codePage = tag.ValueAsNumber;
				return;
			case "fprq":
				switch (tag.ValueAsNumber)
				{
				case 0:
					this.Pitch = RtfFontPitch.Default;
					return;
				case 1:
					this.Pitch = RtfFontPitch.Fixed;
					return;
				case 2:
					this.Pitch = RtfFontPitch.Variable;
					break;
				default:
					return;
				}
				break;

				return;
			}
		}

		protected override void DoVisitText(RtfText text)
		{
			this.fontNameBuffer.Append(text.Text);
		}

		int ComputeHashCode()
		{
			int hash = this.Id.GetHashCode();
			hash = HashTool.AddHashCode(hash, this.FontFamily);
			hash = HashTool.AddHashCode(hash, this.Pitch);
			hash = HashTool.AddHashCode(hash, this.CharSet);
			hash = HashTool.AddHashCode(hash, this.codePage);
			return HashTool.AddHashCode(hash, this.Name);
		}

		string GetFontName()
		{
			string text = null;
			int length = this.fontNameBuffer.Length;
			if (length > 0)
			{
				if (this.fontNameBuffer[length - 1] == ';')
				{
					text = this.fontNameBuffer.ToString().Substring(0, length - 1).Trim();
				}
				else
				{
					text = this.fontNameBuffer.ToString().Trim();
				}
				if (text.Length == 0)
				{
					text = null;
				}
			}
			return text;
		}

		void ResetState()
		{
			this.CharSet = 0;
			this.codePage = 0;
			this.FontFamily = RtfFontFamily.Nil;
			this.Pitch = RtfFontPitch.Default;
			this.fontNameBuffer.Length = 0;
		}

		void GetFontAlt(RtfGroup group)
		{
			this.NameAlt = RtfHelper.GetGroupText(group, true);
		}

		readonly StringBuilder fontNameBuffer = new StringBuilder();

		int codePage;
	}
}
