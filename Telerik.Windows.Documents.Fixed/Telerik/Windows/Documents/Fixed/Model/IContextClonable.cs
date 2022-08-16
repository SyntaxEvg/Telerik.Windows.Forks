using System;

namespace Telerik.Windows.Documents.Fixed.Model
{
	interface IContextClonable<T>
	{
		T Clone(RadFixedDocumentCloneContext cloneContext);
	}
}
