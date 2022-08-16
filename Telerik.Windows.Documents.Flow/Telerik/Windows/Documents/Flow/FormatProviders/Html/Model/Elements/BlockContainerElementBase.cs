using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Export;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Elements
{
	abstract class BlockContainerElementBase : BodyElementBase
	{
		public BlockContainerElementBase(HtmlContentManager contentManager)
			: base(contentManager)
		{
		}

		protected BlockContainerBase AssociatedBlockContainer
		{
			get
			{
				return this.blockContainer;
			}
		}

		public void SetAssociatedFlowElement(BlockContainerBase blockContainer)
		{
			Guard.ThrowExceptionIfNull<BlockContainerBase>(blockContainer, "blockContainer");
			this.blockContainer = blockContainer;
		}

		protected override IEnumerable<HtmlElementBase> OnEnumerateInnerElements(IHtmlWriter writer, IHtmlExportContext context)
		{
			Guard.ThrowExceptionIfNull<IHtmlWriter>(writer, "writer");
			Guard.ThrowExceptionIfNull<IHtmlExportContext>(context, "context");
			foreach (HtmlElementBase element in base.ContentManager.ExportBlockContainer(context, this.blockContainer))
			{
				yield return element;
			}
			yield break;
		}

		protected override void ClearOverride()
		{
			base.ClearOverride();
			this.blockContainer = null;
		}

		BlockContainerBase blockContainer;
	}
}
