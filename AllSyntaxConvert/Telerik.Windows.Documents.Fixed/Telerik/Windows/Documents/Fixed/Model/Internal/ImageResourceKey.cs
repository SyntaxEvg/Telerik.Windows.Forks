using System;
using Telerik.Windows.Documents.Core.Imaging;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Model.Internal
{
	class ImageResourceKey : ResourceKey
	{
		public Color? StencilColor { get; set; }

		public override bool Equals(object obj)
		{
			ImageResourceKey imageResourceKey = obj as ImageResourceKey;
			if (imageResourceKey == null)
			{
				return false;
			}
			if (base.Type == ResourceType.Local)
			{
				return base.Equals(obj);
			}
			return base.Equals(obj) && ObjectExtensions.EqualsOfT<Color?>(this.StencilColor, imageResourceKey.StencilColor);
		}

		public override int GetHashCode()
		{
			if (base.Type == ResourceType.Local)
			{
				return base.GetHashCode();
			}
			return ObjectExtensions.CombineHashCodes(base.GetHashCode(), this.StencilColor.GetHashCodeOrZero());
		}
	}
}
