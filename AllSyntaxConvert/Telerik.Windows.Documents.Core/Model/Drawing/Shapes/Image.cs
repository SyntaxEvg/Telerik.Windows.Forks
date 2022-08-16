using System;
using Telerik.Windows.Documents.Core.Imaging;
using Telerik.Windows.Documents.Media;
using Telerik.Windows.Documents.Model.Data;

namespace Telerik.Windows.Documents.Model.Drawing.Shapes
{
	public sealed class Image : ShapeBase
	{
		internal Image()
		{
			base.LockAspectRatio = true;
		}

		internal Image(Image other)
			: this(other, true)
		{
		}

		Image(Image other, bool copyImageSource)
			: base(other)
		{
			if (copyImageSource)
			{
				this.imageSource = other.ImageSource;
			}
			this.PreferRelativeToOriginalResize = other.PreferRelativeToOriginalResize;
		}

		public ImageSource ImageSource
		{
			get
			{
				return this.imageSource;
			}
			set
			{
				if (this.imageSource != value)
				{
					ImageSource oldValue = this.imageSource;
					this.imageSource = value;
					this.OnImageSourceChanged(oldValue, this.imageSource);
				}
			}
		}

		public bool PreferRelativeToOriginalResize
		{
			get
			{
				return this.preferRelativeToOriginalResize;
			}
			set
			{
				if (this.preferRelativeToOriginalResize != value)
				{
					this.preferRelativeToOriginalResize = value;
				}
			}
		}

		protected override void InitializeSize()
		{
			if (this.ImageSource != null)
			{
				base.Size = ImageSizeDecodersManager.GetSize(this.ImageSource.Data, this.ImageSource.Extension);
			}
		}

		internal event EventHandler<ResourceChangedEventArgs> ImageSourceChanged;

		internal Image Clone(bool cloneImageSource)
		{
			Image image = new Image(this, !cloneImageSource);
			if (cloneImageSource && this.ImageSource != null)
			{
				image.imageSource = this.ImageSource.Clone();
			}
			return image;
		}

		void OnImageSourceChanged(ImageSource oldValue, ImageSource newValue)
		{
			if (this.ImageSourceChanged != null)
			{
				this.ImageSourceChanged(this, new ResourceChangedEventArgs
				{
					OldValue = oldValue,
					NewValue = newValue
				});
			}
		}

		ImageSource imageSource;

		bool preferRelativeToOriginalResize = true;
	}
}
