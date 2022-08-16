using System;

namespace Telerik.Windows.Documents.Flow.Model.Styles.Core
{
	class ValidationRule<TValue> : IValidationRule
	{
		public ValidationRule(Func<TValue, bool> rule)
		{
			this.rule = rule;
		}

		public bool IsValid(TValue value)
		{
			return this.rule(value);
		}

		public bool IsValid(object value)
		{
			return this.IsValid((TValue)((object)value));
		}

		readonly Func<TValue, bool> rule;
	}
}
