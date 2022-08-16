using System;
using Telerik.Windows.Documents.Fixed.Model.Collections;

namespace Telerik.Windows.Documents.Fixed.Model.Common
{
	public interface IContainerElement : IFixedDocumentElement
	{
		ContentElementCollection Content { get; }
	}
}
