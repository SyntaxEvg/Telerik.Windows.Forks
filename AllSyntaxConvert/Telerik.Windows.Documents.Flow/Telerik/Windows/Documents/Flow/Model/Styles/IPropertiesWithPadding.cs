using System;
using Telerik.Windows.Documents.Flow.Model.Styles.Core;
using Telerik.Windows.Documents.Primitives;

namespace Telerik.Windows.Documents.Flow.Model.Styles
{
	interface IPropertiesWithPadding
	{
		IStyleProperty<Padding> Padding { get; }
	}
}
