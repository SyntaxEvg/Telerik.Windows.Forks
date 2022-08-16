using System;
using System.Windows;
using Telerik.Windows.Documents.Fixed.Model.Collections;

namespace Telerik.Windows.Documents.Fixed.Model.Common
{
	class EmptyContentRoot : IContentRootElement, IContainerElement, IFixedDocumentElement
	{
		public EmptyContentRoot()
		{
			this.content = new ContentElementCollection(this);
		}

		bool IContentRootElement.SupportsAnnotations
		{
			get
			{
				return false;
			}
		}

		AnnotationCollection IContentRootElement.Annotations
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		ContentElementCollection IContainerElement.Content
		{
			get
			{
				return this.content;
			}
		}

		IFixedDocumentElement IFixedDocumentElement.Parent
		{
			get
			{
				return null;
			}
		}

		Size IContentRootElement.Size
		{
			get
			{
				return Size.Empty;
			}
		}

		readonly ContentElementCollection content;
	}
}
