using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Flow.Model.Collections;

namespace Telerik.Windows.Documents.Flow.Model
{
	public abstract class BlockContainerBase : DocumentElementBase
	{
		internal BlockContainerBase(RadFlowDocument document)
			: base(document)
		{
			this.blocks = new BlockCollection(this);
		}

		public BlockCollection Blocks
		{
			get
			{
				return this.blocks;
			}
		}

		internal override IEnumerable<DocumentElementBase> Children
		{
			get
			{
				return this.Blocks;
			}
		}

		readonly BlockCollection blocks;
	}
}
