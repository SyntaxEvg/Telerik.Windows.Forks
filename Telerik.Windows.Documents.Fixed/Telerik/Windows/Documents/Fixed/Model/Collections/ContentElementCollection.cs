using System;
using Telerik.Windows.Documents.Fixed.Model.Common;
using Telerik.Windows.Documents.Fixed.Model.Graphics;
using Telerik.Windows.Documents.Fixed.Model.Objects;
using Telerik.Windows.Documents.Fixed.Model.Resources;
using Telerik.Windows.Documents.Fixed.Model.Text;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Model.Collections
{
	public sealed class ContentElementCollection : DocumentElementCollection<ContentElementBase, IContainerElement>
	{
		public ContentElementCollection(IContainerElement parent)
			: base(parent)
		{
		}

		public Path AddPath()
		{
			Path path = new Path();
			base.Add(path);
			return path;
		}

		public TextFragment AddTextFragment()
		{
			TextFragment textFragment = new TextFragment();
			base.Add(textFragment);
			return textFragment;
		}

		public TextFragment AddTextFragment(string text)
		{
			TextFragment textFragment = this.AddTextFragment();
			textFragment.Text = text;
			return textFragment;
		}

		public Image AddImage()
		{
			Image image = new Image();
			base.Add(image);
			return image;
		}

		public Image AddImage(ImageSource source)
		{
			Guard.ThrowExceptionIfNull<ImageSource>(source, "source");
			Image image = this.AddImage();
			image.ImageSource = source;
			return image;
		}

		public Form AddForm()
		{
			Form form = new Form();
			base.Add(form);
			return form;
		}

		public Form AddForm(FormSource source)
		{
			Guard.ThrowExceptionIfNull<FormSource>(source, "source");
			Form form = this.AddForm();
			form.FormSource = source;
			return form;
		}
	}
}
