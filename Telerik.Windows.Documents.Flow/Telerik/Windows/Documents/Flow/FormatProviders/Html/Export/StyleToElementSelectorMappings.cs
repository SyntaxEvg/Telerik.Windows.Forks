using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.Core.Data;
using Telerik.Windows.Documents.Flow.Model.Styles;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Export
{
	static class StyleToElementSelectorMappings
	{
		static StyleToElementSelectorMappings()
		{
			StyleToElementSelectorMappings.elementStylesNames.AddPair("NormalWeb", "p");
			StyleToElementSelectorMappings.elementStylesNames.AddPair(BuiltInStyleNames.GetHeadingStyleIdByIndex(1), "h1");
			StyleToElementSelectorMappings.elementStylesNames.AddPair(BuiltInStyleNames.GetHeadingStyleIdByIndex(2), "h2");
			StyleToElementSelectorMappings.elementStylesNames.AddPair(BuiltInStyleNames.GetHeadingStyleIdByIndex(3), "h3");
			StyleToElementSelectorMappings.elementStylesNames.AddPair(BuiltInStyleNames.GetHeadingStyleIdByIndex(4), "h4");
			StyleToElementSelectorMappings.elementStylesNames.AddPair(BuiltInStyleNames.GetHeadingStyleIdByIndex(5), "h5");
			StyleToElementSelectorMappings.elementStylesNames.AddPair(BuiltInStyleNames.GetHeadingStyleIdByIndex(6), "h6");
			StyleToElementSelectorMappings.elementStylesNames.AddPair("Hyperlink", "a");
		}

		public static IEnumerable<string> GetSelectors()
		{
			return from selector in StyleToElementSelectorMappings.elementStylesNames.ToValues
				where selector != "p"
				select selector;
		}

		public static bool TryGetElementSelector(string styleId, out string selector)
		{
			return StyleToElementSelectorMappings.elementStylesNames.TryGetToValue(styleId, out selector);
		}

		public static bool TryGetStyleId(string selector, out string styleId)
		{
			return StyleToElementSelectorMappings.elementStylesNames.TryGetFromValue(selector, out styleId);
		}

		static readonly ValueMapper<string, string> elementStylesNames = new ValueMapper<string, string>();
	}
}
