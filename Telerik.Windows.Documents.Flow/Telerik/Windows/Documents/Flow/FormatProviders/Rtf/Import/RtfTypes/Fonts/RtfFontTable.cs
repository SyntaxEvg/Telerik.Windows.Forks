using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.Model;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.RtfTypes.Fonts
{
	sealed class RtfFontTable : RtfElementIteratorBase
	{
		public bool ContainsFontWithId(string fontId)
		{
			return this.fontByIdMap.ContainsKey(fontId);
		}

		public FontFamily GetFontFamily(string fontId)
		{
			RtfFont rtfFont = this.fontByIdMap[fontId];
			string text = rtfFont.Name;
			if (string.IsNullOrEmpty(text))
			{
				text = rtfFont.NameAlt;
			}
			return new FontFamily(text);
		}

		public void FillFontTable(RtfGroup group)
		{
			Util.EnsureGroupDestination(group, "fonttbl");
			if (group.Elements.Count > 1)
			{
				if (group.Elements.Any((RtfElement e) => e.Type == RtfElementType.Group))
				{
					base.VisitGroupChildren(group, false);
					return;
				}
				int count = group.Elements.Count;
				RtfGroup group2 = new RtfGroup();
				for (int i = 1; i < count; i++)
				{
					RtfElement rtfElement = group.Elements[i];
					if (rtfElement.Type == RtfElementType.Text && ((RtfText)rtfElement).Text == ";")
					{
						this.BuildFontFromGroup(group2);
					}
					group2 = new RtfGroup();
				}
			}
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
				this.BuildFontFromGroup(group);
				break;

				return;
			}
		}

		void BuildFontFromGroup(RtfGroup group)
		{
			RtfFont rtfFont = new RtfFont();
			rtfFont.FillFont(group);
			this.Add(rtfFont);
		}

		void Add(RtfFont font)
		{
			if (font == null)
			{
				throw new ArgumentNullException("font");
			}
			this.fontByIdMap[font.Id] = font;
		}

		readonly Dictionary<string, RtfFont> fontByIdMap = new Dictionary<string, RtfFont>();
	}
}
