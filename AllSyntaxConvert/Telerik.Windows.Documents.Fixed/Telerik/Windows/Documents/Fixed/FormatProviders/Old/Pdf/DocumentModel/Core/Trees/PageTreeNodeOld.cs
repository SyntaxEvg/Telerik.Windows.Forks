using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.PdfConverters;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Utils;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.Trees
{
	[PdfClass(TypeName = "Pages")]
	class PageTreeNodeOld : PdfObjectOld, ITreeNode<PageTreeNodeOld>
	{
		public PageTreeNodeOld(PdfContentManager contentManager)
			: base(contentManager)
		{
			this.parent = base.CreateLoadOnDemandProperty<PageTreeNodeOld>(new PdfPropertyDescriptor
			{
				Name = "Parent",
				IsRequired = true,
				State = PdfPropertyState.MustBeIndirectReference
			});
			this.children = base.CreateLoadOnDemandProperty<PdfArrayOld>(new PdfPropertyDescriptor
			{
				Name = "Kids",
				IsRequired = true,
				State = PdfPropertyState.ContainsOnlyIndirectReferences
			});
			this.count = base.CreateInstantLoadProperty<PdfIntOld>(new PdfPropertyDescriptor
			{
				Name = "Count",
				IsRequired = true
			});
			this.resources = base.CreateLoadOnDemandProperty<PdfResourceOld>(new PdfPropertyDescriptor
			{
				Name = "Resources",
				IsRequired = true,
				IsInheritable = true
			}, Converters.PdfDictionaryToPdfObjectConverter);
			this.mediaBox = base.CreateLoadOnDemandProperty<PdfArrayOld>(new PdfPropertyDescriptor
			{
				Name = "MediaBox",
				IsRequired = true,
				IsInheritable = true
			});
			this.cropBox = base.CreateLoadOnDemandProperty<PdfArrayOld>(new PdfPropertyDescriptor
			{
				Name = "CropBox",
				IsInheritable = true
			});
			this.rotate = base.CreateInstantLoadProperty<PdfIntOld>(new PdfPropertyDescriptor
			{
				Name = "Rotate",
				IsInheritable = true
			});
		}

		public PageTreeNodeOld Parent
		{
			get
			{
				return this.parent.GetValue();
			}
			set
			{
				this.parent.SetValue(value);
			}
		}

		public PdfArrayOld Children
		{
			get
			{
				return this.children.GetValue();
			}
			set
			{
				this.children.SetValue(value);
			}
		}

		public PdfIntOld Count
		{
			get
			{
				return this.count.GetValue();
			}
			set
			{
				this.count.SetValue(value);
			}
		}

		public PdfResourceOld Resources
		{
			get
			{
				return this.GetInheritableProperty((PageTreeNodeOld pageTreeNode) => pageTreeNode.resources.GetValue(), null);
			}
			set
			{
				this.resources.SetValue(value);
			}
		}

		public PdfIntOld Rotate
		{
			get
			{
				return this.GetInheritableProperty((PageTreeNodeOld pageTreeNode) => pageTreeNode.rotate.GetValue(), null);
			}
			set
			{
				this.rotate.SetValue(value);
			}
		}

		public PdfArrayOld MediaBox
		{
			get
			{
				return this.GetInheritableProperty((PageTreeNodeOld pageTreeNode) => pageTreeNode.mediaBox.GetValue(), null);
			}
			set
			{
				this.mediaBox.SetValue(value);
			}
		}

		public PdfArrayOld CropBox
		{
			get
			{
				return this.GetInheritableProperty((PageTreeNodeOld pageTreeNode) => pageTreeNode.cropBox.GetValue(), this.MediaBox);
			}
			set
			{
				this.cropBox.SetValue(value);
			}
		}

		PageTreeNodeOld ITreeNode<PageTreeNodeOld>.NodeValue
		{
			get
			{
				return this;
			}
		}

		ITreeNode<PageTreeNodeOld> ITreeNode<PageTreeNodeOld>.ParentNode
		{
			get
			{
				return this.Parent;
			}
		}

		readonly LoadOnDemandProperty<PageTreeNodeOld> parent;

		readonly LoadOnDemandProperty<PdfArrayOld> children;

		readonly InstantLoadProperty<PdfIntOld> count;

		readonly LoadOnDemandProperty<PdfResourceOld> resources;

		readonly LoadOnDemandProperty<PdfArrayOld> mediaBox;

		readonly LoadOnDemandProperty<PdfArrayOld> cropBox;

		readonly InstantLoadProperty<PdfIntOld> rotate;
	}
}
