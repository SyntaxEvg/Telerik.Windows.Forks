using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Common;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.DocumentStructure
{
	class PageTreeNode : PdfObject, IResourceHolder, ITreeNode<PageTreeNode>
	{
		public PageTreeNode()
		{
			this.children = base.RegisterReferenceProperty<PdfArray>(new PdfPropertyDescriptor("Kids", false, PdfPropertyRestrictions.ContainsOnlyIndirectReferences));
			this.parent = base.RegisterReferenceProperty<PageTreeNode>(new PdfPropertyDescriptor("Parent", false, PdfPropertyRestrictions.MustBeIndirectReference));
			this.count = base.RegisterDirectProperty<PdfInt>(new PdfPropertyDescriptor("Count"));
			this.mediaBox = base.RegisterDirectProperty<PdfArray>(new PdfPropertyDescriptor("MediaBox", true));
			this.cropBox = base.RegisterDirectProperty<PdfArray>(new PdfPropertyDescriptor("CropBox"));
			this.resources = base.RegisterDirectProperty<PdfResource>(new PdfPropertyDescriptor("Resources"));
			this.rotate = base.RegisterDirectProperty<PdfInt>(new PdfPropertyDescriptor("Rotate"));
		}

		public virtual PageTreeNodeType PageTreeNodeType
		{
			get
			{
				return PageTreeNodeType.PageTreeNode;
			}
		}

		public PdfArray Children
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

		public PageTreeNode Parent
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

		public PdfInt Count
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

		public PdfArray MediaBox
		{
			get
			{
				return this.GetInheritableProperty((PageTreeNode pageTreeNode) => pageTreeNode.mediaBox.GetValue(), null);
			}
			set
			{
				this.mediaBox.SetValue(value);
			}
		}

		public PdfArray CropBox
		{
			get
			{
				PdfArray inheritableProperty = this.GetInheritableProperty((PageTreeNode pageTreeNode) => pageTreeNode.cropBox.GetValue(), null);
				if (inheritableProperty == null)
				{
					inheritableProperty = this.MediaBox;
				}
				return inheritableProperty;
			}
			set
			{
				this.cropBox.SetValue(value);
			}
		}

		public PdfResource Resources
		{
			get
			{
				return this.GetInheritableProperty((PageTreeNode pageTreeNode) => pageTreeNode.resources.GetValue(), null);
			}
			set
			{
				this.resources.SetValue(value);
			}
		}

		public PdfInt Rotate
		{
			get
			{
				return this.GetInheritableProperty((PageTreeNode pageTreeNode) => pageTreeNode.rotate.GetValue(), 0.ToPdfInt());
			}
			set
			{
				this.rotate.SetValue(value);
			}
		}

		PageTreeNode ITreeNode<PageTreeNode>.NodeValue
		{
			get
			{
				return this;
			}
		}

		ITreeNode<PageTreeNode> ITreeNode<PageTreeNode>.ParentNode
		{
			get
			{
				return this.Parent;
			}
		}

		public static PageTreeNode CreateInstance(PdfName typeName)
		{
			Guard.ThrowExceptionIfNull<PdfName>(typeName, "typeName");
			string value;
			if ((value = typeName.Value) != null)
			{
				if (value == "Page")
				{
					return new Page();
				}
				if (value == "Pages")
				{
					return new PageTreeNode();
				}
			}
			throw new NotSupportedException("Type is not supported.");
		}

		public IList<Page> EnumeratePages(PostScriptReader reader, IPdfImportContext context)
		{
			Guard.ThrowExceptionIfNull<PostScriptReader>(reader, "reader");
			Guard.ThrowExceptionIfNull<IPdfImportContext>(context, "context");
			List<Page> list = new List<Page>();
			for (int i = 0; i < this.Children.Count; i++)
			{
				PageTreeNode pageTreeNode;
				if (this.Children.TryGetElement<PageTreeNode>(reader, context, i, out pageTreeNode))
				{
					if (pageTreeNode.PageTreeNodeType == PageTreeNodeType.Page)
					{
						list.Add((Page)pageTreeNode);
					}
					else if (pageTreeNode.PageTreeNodeType == PageTreeNodeType.PageTreeNode)
					{
						IList<Page> collection = pageTreeNode.EnumeratePages(reader, context);
						list.AddRange(collection);
					}
				}
			}
			return list;
		}

		public const string PageName = "Page";

		public const string PageTreeNodeName = "Pages";

		public const string ResourcesName = "Resources";

		public const string MediaBoxName = "MediaBox";

		public const string CropBoxName = "CropBox";

		public const string RotateName = "Rotate";

		readonly ReferenceProperty<PdfArray> children;

		readonly ReferenceProperty<PageTreeNode> parent;

		readonly DirectProperty<PdfInt> count;

		readonly DirectProperty<PdfArray> mediaBox;

		readonly DirectProperty<PdfArray> cropBox;

		readonly DirectProperty<PdfResource> resources;

		readonly DirectProperty<PdfInt> rotate;
	}
}
