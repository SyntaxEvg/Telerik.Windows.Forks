using System;

namespace Telerik.Windows.Documents.Flow.Model
{
	public abstract class BlockBase : DocumentElementBase
	{
		internal BlockBase(RadFlowDocument document)
			: base(document)
		{
		}

		public BlockContainerBase BlockContainer
		{
			get
			{
				return (BlockContainerBase)base.Parent;
			}
		}
	}
}
