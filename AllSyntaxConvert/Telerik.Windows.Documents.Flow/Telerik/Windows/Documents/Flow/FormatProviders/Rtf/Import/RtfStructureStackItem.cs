using System;
using Telerik.Windows.Documents.Flow.Model;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import
{
	class RtfStructureStackItem
	{
		public RtfStructureStackItem(DocumentElementBase element, int nestingLevel)
		{
			this.Element = element;
			this.NestingLevel = nestingLevel;
		}

		public DocumentElementBase Element { get; set; }

		public int NestingLevel { get; set; }

		public bool IsRowOrParagraph
		{
			get
			{
				return this.Element is TableRow || this.Element is Paragraph;
			}
		}

		public override string ToString()
		{
			return string.Format("Nesting level: {0} type:{1}", this.NestingLevel.ToString(), this.Element.GetType().ToString());
		}
	}
}
