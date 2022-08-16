using System;

namespace Telerik.Windows.Documents.Flow.Model.Styles.Core
{
	public interface IValidationRule
	{
		bool IsValid(object value);
	}
}
