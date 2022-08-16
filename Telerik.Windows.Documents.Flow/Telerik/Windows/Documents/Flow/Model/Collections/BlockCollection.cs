using System;

namespace Telerik.Windows.Documents.Flow.Model.Collections
{
	public sealed class BlockCollection : DocumentElementCollection<BlockBase, BlockContainerBase>
	{
		internal BlockCollection(BlockContainerBase parent)
			: base(parent)
		{
		}

		public Paragraph AddParagraph()
		{
			Paragraph paragraph = new Paragraph(base.Owner.Document);
			base.Add(paragraph);
			return paragraph;
		}

		public Table AddTable()
		{
			Table table = new Table(base.Owner.Document);
			base.Add(table);
			return table;
		}
	}
}
