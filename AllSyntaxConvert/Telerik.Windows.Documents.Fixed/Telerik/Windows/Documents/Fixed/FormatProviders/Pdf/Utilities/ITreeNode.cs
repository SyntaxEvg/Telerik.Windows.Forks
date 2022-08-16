using System;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities
{
	interface ITreeNode<T>
	{
		T NodeValue { get; }

		ITreeNode<T> ParentNode { get; }
	}
}
