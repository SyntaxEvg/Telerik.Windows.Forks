using System;

namespace Telerik.Windows.Documents.Flow.Model.Styles.Core
{
	public interface IStyleProperty
	{
		IStylePropertyDefinition PropertyDefinition { get; }

		bool HasLocalValue { get; }

		object GetLocalValueAsObject();

		object GetActualValueAsObject();

		void SetValueAsObject(object value);

		void ClearValue();
	}
}
