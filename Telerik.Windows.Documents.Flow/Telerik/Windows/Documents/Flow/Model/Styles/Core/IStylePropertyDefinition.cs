using System;
using System.ComponentModel;

namespace Telerik.Windows.Documents.Flow.Model.Styles.Core
{
	public interface IStylePropertyDefinition
	{
		[EditorBrowsable(EditorBrowsableState.Never)]
		int GlobalPropertyIndex { get; }

		[EditorBrowsable(EditorBrowsableState.Never)]
		Type PropertyType { get; }

		string PropertyName { get; }

		StylePropertyType StylePropertyType { get; }

		StylePropertyValidation Validation { get; }

		object GetDefaultValueAsObject();
	}
}
