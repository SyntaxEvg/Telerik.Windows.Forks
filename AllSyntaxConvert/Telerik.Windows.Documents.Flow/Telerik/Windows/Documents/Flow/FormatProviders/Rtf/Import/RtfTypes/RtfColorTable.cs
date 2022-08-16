using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Exceptions;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.Model;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Utilities;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.RtfTypes
{
	sealed class RtfColorTable : RtfElementIteratorBase
	{
		public int Count
		{
			get
			{
				return this.innerList.Count;
			}
		}

		public RtfColor this[int index]
		{
			get
			{
				return this.innerList[index];
			}
		}

		public bool TryGetColor(int index, out RtfColor rtfColor)
		{
			rtfColor = null;
			if (this.innerList.Count > index && index >= 0)
			{
				rtfColor = this.innerList[index];
				return true;
			}
			return false;
		}

		public void FillColorTable(RtfGroup group)
		{
			Util.EnsureGroupDestination(group, "colortbl");
			this.ResetState();
			base.VisitGroupChildren(group, false);
		}

		protected override void DoVisitTag(RtfTag tag)
		{
			string name;
			if ((name = tag.Name) != null)
			{
				if (name == "red")
				{
					this.currentRed = tag.ValueAsNumber;
					this.isColorDefined = true;
					return;
				}
				if (name == "green")
				{
					this.currentGreen = tag.ValueAsNumber;
					this.isColorDefined = true;
					return;
				}
				if (!(name == "blue"))
				{
					return;
				}
				this.currentBlue = tag.ValueAsNumber;
				this.isColorDefined = true;
			}
		}

		protected override void DoVisitText(RtfText text)
		{
			if (text.Text.StartsWith(";"))
			{
				int num = text.Text.Count((char c) => c == ';');
				for (int i = 0; i < num; i++)
				{
					this.Add(Color.FromArgb(byte.MaxValue, (byte)this.currentRed, (byte)this.currentGreen, (byte)this.currentBlue));
					this.ResetState();
				}
				return;
			}
			throw new RtfColorTableFormatException("ColorTableUnsupportedText: " + text.Text);
		}

		void Add(Color item)
		{
			Guard.ThrowExceptionIfNull<Color>(item, "item");
			this.innerList.Add(new RtfColor(item, !this.isColorDefined));
		}

		void ResetState()
		{
			this.currentRed = 0;
			this.currentGreen = 0;
			this.currentBlue = 0;
			this.isColorDefined = false;
		}

		readonly List<RtfColor> innerList = new List<RtfColor>();

		int currentRed;

		int currentGreen;

		int currentBlue;

		bool isColorDefined;
	}
}
