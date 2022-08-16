using System;
using Telerik.Windows.Documents.Fixed.Model.Editing.Flow;
using Telerik.Windows.Documents.Fixed.Model.Editing.Lists;
using Telerik.Windows.Documents.Fixed.Model.Editing.Tables;

namespace Telerik.Windows.Documents.Fixed.Model.Editing.Collections
{
	public class BlockCollection : CollectionBase<IBlockElement>
	{
		public Table AddTable()
		{
			Table table = new Table();
			base.Add(table);
			return table;
		}

		public Block AddBlock()
		{
			Block block = new Block();
			base.Add(block);
			return block;
		}

		public Block AddBlock(List list, int listLevel)
		{
			Block block = this.AddBlock();
			block.SetBullet(list, listLevel);
			return block;
		}
	}
}
