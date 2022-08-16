using System;

namespace Telerik.Windows.Documents.Flow.Model.Styles.Core
{
	public interface IStyleProperty<TValue> : IStyleProperty
	{
		TValue LocalValue { get; set; }

		TValue GetActualValue();
	}
}
