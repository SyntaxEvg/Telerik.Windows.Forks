using System;
using Telerik.Windows.Documents.Flow.Model.Styles.Core;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Flow.Model.Styles
{
	public interface IPropertiesWithShading
	{
		IStyleProperty<ThemableColor> BackgroundColor { get; }

		IStyleProperty<ThemableColor> ShadingPatternColor { get; }

		IStyleProperty<ShadingPattern?> ShadingPattern { get; }

		RadFlowDocument Document { get; }
	}
}
