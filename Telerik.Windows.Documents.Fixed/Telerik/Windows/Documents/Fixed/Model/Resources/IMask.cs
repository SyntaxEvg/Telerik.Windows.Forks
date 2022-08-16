using System;
using Telerik.Windows.Documents.Primitives;

namespace Telerik.Windows.Documents.Fixed.Model.Resources
{
	interface IMask
	{
		bool IsColorKeyMask { get; }

		SizeI GetMaskedImageSize(IImageDescriptor image);

		PixelContainer MaskImage(IImageDescriptor image, PixelContainer pixels);

		bool ShouldMaskColorComponents(int[] components);
	}
}
