using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Flow.Model.Styles.Core
{
	public class StylePropertyValidation
	{
		internal StylePropertyValidation()
		{
			this.rules = new List<IValidationRule>();
		}

		internal IEnumerable<IValidationRule> Rules
		{
			get
			{
				return this.rules;
			}
		}

		public bool IsValid(object value)
		{
			foreach (IValidationRule validationRule in this.Rules)
			{
				if (!validationRule.IsValid(value))
				{
					return false;
				}
			}
			return true;
		}

		internal void AddRule(IValidationRule rule)
		{
			this.rules.Add(rule);
		}

		internal void RemoveRule(IValidationRule rule)
		{
			this.rules.Remove(rule);
		}

		internal void ClearRules()
		{
			this.rules.Clear();
		}

		readonly List<IValidationRule> rules;
	}
}
