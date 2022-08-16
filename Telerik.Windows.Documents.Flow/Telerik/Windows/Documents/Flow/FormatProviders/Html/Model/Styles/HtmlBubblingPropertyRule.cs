using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.Flow.Model.Styles.Core;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Styles
{
	abstract class HtmlBubblingPropertyRule
	{
		public HtmlBubblingPropertyRule(IStylePropertyDefinition propertyDefinition)
		{
			Guard.ThrowExceptionIfNull<IStylePropertyDefinition>(propertyDefinition, "propertyDefinition");
			this.propertyDefinition = propertyDefinition;
		}

		public IStylePropertyDefinition PropertyDefinition
		{
			get
			{
				return this.propertyDefinition;
			}
		}

		public void SetMostSuitableCandidateValue(DocumentElementPropertiesBase properties, IEnumerable<object> candidateValues)
		{
			Guard.ThrowExceptionIfNull<DocumentElementPropertiesBase>(properties, "properties");
			Guard.ThrowExceptionIfNull<IEnumerable<object>>(candidateValues, "candidateValues");
			IStyleProperty styleProperty = properties.GetStyleProperty(this.PropertyDefinition);
			if (styleProperty != null)
			{
				object obj = styleProperty.GetActualValueAsObject();
				foreach (object candidateValue in candidateValues)
				{
					object obj2;
					if (this.IsRuleSatisfied(obj, candidateValue, out obj2))
					{
						obj = obj2;
					}
				}
				styleProperty.SetValueAsObject(obj);
			}
		}

		public virtual bool ShouldRegisterProperty(DocumentElementPropertiesBase properties)
		{
			return true;
		}

		protected abstract bool IsRuleSatisfied(object value, object candidateValue, out object result);

		readonly IStylePropertyDefinition propertyDefinition;
	}
}
