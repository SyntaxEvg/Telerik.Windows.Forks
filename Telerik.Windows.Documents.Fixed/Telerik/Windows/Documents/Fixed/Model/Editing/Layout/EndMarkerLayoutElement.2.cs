using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Fixed.Model.Fonts;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Model.Editing.Layout
{
	class EndMarkerLayoutElement<TStart> : EndMarkerLayoutElement where TStart : StartMarkerLayoutElement
	{
		public EndMarkerLayoutElement(TStart start, FontBase font, double fontSize)
			: base(font, fontSize)
		{
			Guard.ThrowExceptionIfNull<TStart>(start, "start");
			this.start = start;
		}

		public TStart Start
		{
			get
			{
				return this.start;
			}
		}

		internal override bool ShouldSplitToBlocks(Block block)
		{
			Guard.ThrowExceptionIfNull<Block>(block, "block");
			return !block.IsPending(this.Start);
		}

		internal override void SplitToBlocks(Block block, out LayoutElementBase element, out IEnumerable<LayoutElementBase> elementsToInclude, out IEnumerable<LayoutElementBase> pendingElements)
		{
			Guard.ThrowExceptionIfNull<Block>(block, "block");
			TStart tstart = this.Start;
			StartMarkerLayoutElement startMarkerLayoutElement = tstart.Clone();
			element = startMarkerLayoutElement.CreateEnd();
			elementsToInclude = new LayoutElementBase[] { this };
			pendingElements = new LayoutElementBase[] { startMarkerLayoutElement };
		}

		readonly TStart start;
	}
}
