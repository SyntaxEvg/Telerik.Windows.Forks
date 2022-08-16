using System;
using System.IO;
using Telerik.Windows.Documents.Fixed.Model.Objects;
using Telerik.Windows.Documents.Fixed.Model.Resources;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Model.Editing
{
	class ImageEditor : FixedContentElementEditorBase
	{
		public ImageEditor(FixedContentEditorBase editor)
			: base(editor)
		{
		}

		public void DrawImage(Stream stream)
		{
			Guard.ThrowExceptionIfNull<Stream>(stream, "stream");
			this.DrawImage(stream, null, null);
		}

		public void DrawImage(Stream stream, double? width, double? height)
		{
			Guard.ThrowExceptionIfNull<Stream>(stream, "stream");
			ImageSource source = new ImageSource(stream);
			this.DrawImageInternal(source, width, height);
		}

		public void DrawImage(ImageSource source)
		{
			Guard.ThrowExceptionIfNull<ImageSource>(source, "source");
			this.DrawImageInternal(source, null, null);
		}

		public void DrawImage(ImageSource source, double width, double height)
		{
			Guard.ThrowExceptionIfNull<ImageSource>(source, "source");
			this.DrawImageInternal(source, new double?(width), new double?(height));
		}

		void DrawImageInternal(ImageSource source, double? width, double? height)
		{
			Image image = new Image();
			image.ImageSource = source;
			if (width != null)
			{
				image.Width = width.Value;
			}
			if (height != null)
			{
				image.Height = height.Value;
			}
			base.Editor.Append(image);
		}
	}
}
