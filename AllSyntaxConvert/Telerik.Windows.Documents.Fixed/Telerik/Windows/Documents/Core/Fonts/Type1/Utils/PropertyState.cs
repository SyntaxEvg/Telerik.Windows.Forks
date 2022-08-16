using System;

namespace Telerik.Windows.Documents.Core.Fonts.Type1.Utils
{
	enum PropertyState
	{
		None,
		MustBeIndirectReference,
		MustBeDirectObject,
		ContainsOnlyIndirectReferences
	}
}
