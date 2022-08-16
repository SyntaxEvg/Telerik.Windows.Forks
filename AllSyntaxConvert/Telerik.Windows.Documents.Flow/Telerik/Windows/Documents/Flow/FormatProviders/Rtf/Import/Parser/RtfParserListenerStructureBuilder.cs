using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Exceptions;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.Model;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.Parser
{
	sealed class RtfParserListenerStructureBuilder : RtfParserListenerBase
	{
		public RtfGroup StructureRoot
		{
			get
			{
				return this.structureRoot;
			}
		}

		protected override void DoParseBegin()
		{
			this.openGroupStack.Clear();
			this.curGroup = null;
			this.structureRoot = null;
		}

		protected override void DoGroupBegin()
		{
			RtfGroup item = new RtfGroup();
			if (this.curGroup != null)
			{
				this.openGroupStack.Push(this.curGroup);
				this.curGroup.Elements.Add(item);
			}
			this.curGroup = item;
		}

		protected override void DoTagFound(RtfTag tag)
		{
			if (this.curGroup == null)
			{
				throw new RtfStructureException("MissingGroupForNewTag ");
			}
			this.curGroup.Elements.Add(tag);
		}

		protected override void DoTextFound(RtfText text)
		{
			if (this.curGroup == null)
			{
				throw new RtfStructureException("MissingGroupForNewText");
			}
			this.curGroup.Elements.Add(text);
		}

		protected override void DoBinaryFound(RtfBinary bin)
		{
			if (this.curGroup == null)
			{
				throw new RtfStructureException("MissingGroupForNewText");
			}
			this.curGroup.Elements.Add(bin);
		}

		protected override void DoGroupEnd()
		{
			if (this.openGroupStack.Count > 0)
			{
				this.curGroup = this.openGroupStack.Pop();
				return;
			}
			if (this.structureRoot != null)
			{
				throw new RtfStructureException("MultipleRootLevelGroups ");
			}
			this.structureRoot = this.curGroup;
			this.curGroup = null;
		}

		protected override void DoParseEnd()
		{
			if (this.openGroupStack.Count > 0)
			{
				throw new RtfBraceNestingException("UnclosedGroups");
			}
		}

		readonly Stack<RtfGroup> openGroupStack = new Stack<RtfGroup>();

		RtfGroup curGroup;

		RtfGroup structureRoot;
	}
}
