using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Flow.Model.Styles.Core;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html
{
	static class StylePropertyHelper
	{
		static StylePropertyHelper()
		{
			StylePropertyHelper.stylePropertiesWithDefaultValuesDifferentThanHtml.Add(Run.FontFamilyPropertyDefinition);
			StylePropertyHelper.stylePropertiesWithDefaultValuesDifferentThanHtml.Add(Run.FontSizePropertyDefinition);
			StylePropertyHelper.stylePropertiesWithDefaultValuesDifferentThanHtml.Add(Paragraph.SpacingAfterPropertyDefinition);
			StylePropertyHelper.stylePropertiesWithDefaultValuesDifferentThanHtml.Add(Paragraph.SpacingBeforePropertyDefinition);
			StylePropertyHelper.stylePropertiesWithDefaultValuesDifferentThanHtml.Add(Paragraph.LineSpacingPropertyDefinition);
		}

		internal static bool IsStylePropertyWithNonDefaultHtmlActualValue(IStylePropertyDefinition stylePropertyDefinition)
		{
			return StylePropertyHelper.stylePropertiesWithDefaultValuesDifferentThanHtml.Contains(stylePropertyDefinition);
		}

		static readonly HashSet<IStylePropertyDefinition> stylePropertiesWithDefaultValuesDifferentThanHtml = new HashSet<IStylePropertyDefinition>();
	}
}
