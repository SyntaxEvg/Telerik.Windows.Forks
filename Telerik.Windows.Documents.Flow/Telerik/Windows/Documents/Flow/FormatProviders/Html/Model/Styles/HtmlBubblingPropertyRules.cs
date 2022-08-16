using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.BubblingRules;
using Telerik.Windows.Documents.Flow.Model;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Styles
{
	static class HtmlBubblingPropertyRules
	{
		static HtmlBubblingPropertyRules()
		{
			HtmlBubblingPropertyRules.InitializeRules();
		}

		internal static IEnumerable<HtmlBubblingPropertyRule> TryGetPropertyRule(string propertyName)
		{
			if (HtmlBubblingPropertyRules.htmlPropertiesToBubblingRules.ContainsKey(propertyName))
			{
				foreach (HtmlBubblingPropertyRule rule in HtmlBubblingPropertyRules.htmlPropertiesToBubblingRules[propertyName])
				{
					yield return rule;
				}
			}
			yield break;
		}

		static void InitializeRules()
		{
			HtmlBubblingPropertyRules.AddRule(HtmlStylePropertyDescriptors.RowHeightPropertyDescriptor.Name, new TableRowHeightBubblingRule(TableRow.HeightPropertyDefinition));
		}

		static void AddRule(string propertyName, TableRowHeightBubblingRule bubblingRule)
		{
			if (!HtmlBubblingPropertyRules.htmlPropertiesToBubblingRules.ContainsKey(propertyName))
			{
				HtmlBubblingPropertyRules.htmlPropertiesToBubblingRules.Add(propertyName, new List<HtmlBubblingPropertyRule>());
			}
			HtmlBubblingPropertyRules.htmlPropertiesToBubblingRules[propertyName].Add(bubblingRule);
		}

		static readonly Dictionary<string, List<HtmlBubblingPropertyRule>> htmlPropertiesToBubblingRules = new Dictionary<string, List<HtmlBubblingPropertyRule>>();
	}
}
