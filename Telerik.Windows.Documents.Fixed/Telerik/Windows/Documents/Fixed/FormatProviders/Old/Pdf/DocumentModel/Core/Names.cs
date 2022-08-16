using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.Trees;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Utils;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core
{
	class Names : PdfObjectOld
	{
		public Names(PdfContentManager contentManager)
			: base(contentManager)
		{
			this.dests = base.CreateLoadOnDemandProperty<NameTreeNode>(new PdfPropertyDescriptor
			{
				Name = "Dests"
			});
		}

		public NameTreeNode Dests
		{
			get
			{
				return this.dests.GetValue();
			}
			set
			{
				this.dests.SetValue(value);
			}
		}

		readonly LoadOnDemandProperty<NameTreeNode> dests;
	}
}
