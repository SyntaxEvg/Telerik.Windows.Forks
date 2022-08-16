using System;
using System.Windows;

namespace Telerik.Windows.Documents.Fixed.Model.Editing.Flow
{
	public interface IBlockElement
	{
		bool HasPendingContent { get; }

		Size DesiredSize { get; }

		Size Measure(Size availableSize);

		void Draw(FixedContentEditor editor, Rect boundingRect);

		IBlockElement Split();
	}
}
