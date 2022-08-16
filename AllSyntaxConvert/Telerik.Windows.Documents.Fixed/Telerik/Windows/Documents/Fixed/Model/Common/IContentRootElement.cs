using System;
using System.Windows;
using Telerik.Windows.Documents.Fixed.Model.Collections;

namespace Telerik.Windows.Documents.Fixed.Model.Common
{
	public interface IContentRootElement : IContainerElement, IFixedDocumentElement
	{
		Size Size { get; }

		bool SupportsAnnotations { get; }

		AnnotationCollection Annotations { get; }
	}
}
