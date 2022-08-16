using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.Model;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import
{
	abstract class RtfElementIteratorBase
	{
		protected void VisitGroupChildren(RtfGroup group, bool recursive)
		{
			foreach (RtfElement element in group.Elements)
			{
				this.VisitElement(element, recursive);
			}
		}

		protected void VisitElement(RtfElement element, bool recursive)
		{
			switch (element.Type)
			{
			case RtfElementType.Tag:
				this.DoVisitTag((RtfTag)element);
				return;
			case RtfElementType.Group:
			{
				RtfGroup group = (RtfGroup)element;
				this.DoVisitGroup(group);
				if (recursive)
				{
					this.VisitGroupChildren(group, recursive);
					return;
				}
				break;
			}
			case RtfElementType.Text:
				this.DoVisitText((RtfText)element);
				return;
			case RtfElementType.Binary:
				this.DoVisitBinary((RtfBinary)element);
				break;
			default:
				return;
			}
		}

		protected virtual void DoVisitGroup(RtfGroup group)
		{
		}

		protected virtual void DoVisitTag(RtfTag tag)
		{
		}

		protected virtual void DoVisitText(RtfText text)
		{
		}

		protected virtual void DoVisitBinary(RtfBinary bin)
		{
		}
	}
}
