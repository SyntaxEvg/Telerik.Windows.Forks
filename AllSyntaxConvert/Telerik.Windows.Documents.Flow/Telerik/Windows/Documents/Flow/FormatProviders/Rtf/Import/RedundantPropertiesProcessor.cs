using System;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Flow.Model.Lists;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.Flow.Model.Styles.Core;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import
{
	static class RedundantPropertiesProcessor
	{
		public static void ClearRedundantLocalProperties(RadFlowDocument document)
		{
			RedundantPropertiesProcessor.ClearRedundantLocalPropertiesRecursive(document);
			foreach (List list in document.Lists)
			{
				foreach (ListLevel listLevel in list.Levels)
				{
					RedundantPropertiesProcessor.ClearRedundantLocalProperties(listLevel.CharacterProperties);
					RedundantPropertiesProcessor.ClearRedundantLocalProperties(listLevel.ParagraphProperties);
				}
			}
		}

		static void ClearRedundantLocalPropertiesRecursive(DocumentElementBase element)
		{
			foreach (DocumentElementBase element2 in element.Children)
			{
				RedundantPropertiesProcessor.ClearRedundantLocalPropertiesRecursive(element2);
			}
			Run run = element as Run;
			if (run != null)
			{
				RedundantPropertiesProcessor.ClearRedundantLocalProperties(run.Properties);
				return;
			}
			Paragraph paragraph = element as Paragraph;
			if (paragraph != null)
			{
				RedundantPropertiesProcessor.ClearRedundantLocalProperties(paragraph.Properties);
				return;
			}
			TableCell tableCell = element as TableCell;
			if (tableCell != null)
			{
				RedundantPropertiesProcessor.ClearRedundantLocalProperties(tableCell.Properties);
				return;
			}
			TableRow tableRow = element as TableRow;
			if (tableRow != null)
			{
				RedundantPropertiesProcessor.ClearRedundantLocalProperties(tableRow.Properties);
				return;
			}
			Table table = element as Table;
			if (table != null)
			{
				RedundantPropertiesProcessor.ClearRedundantLocalProperties(table.Properties);
				return;
			}
			Section section = element as Section;
			if (section != null)
			{
				HeaderFooterBase headerFooterBase = section.Headers.First;
				if (headerFooterBase != null)
				{
					RedundantPropertiesProcessor.ClearRedundantLocalPropertiesRecursive(headerFooterBase);
				}
				headerFooterBase = section.Headers.Default;
				if (headerFooterBase != null)
				{
					RedundantPropertiesProcessor.ClearRedundantLocalPropertiesRecursive(headerFooterBase);
				}
				headerFooterBase = section.Headers.Even;
				if (headerFooterBase != null)
				{
					RedundantPropertiesProcessor.ClearRedundantLocalPropertiesRecursive(headerFooterBase);
				}
				headerFooterBase = section.Footers.First;
				if (headerFooterBase != null)
				{
					RedundantPropertiesProcessor.ClearRedundantLocalPropertiesRecursive(headerFooterBase);
				}
				headerFooterBase = section.Footers.Default;
				if (headerFooterBase != null)
				{
					RedundantPropertiesProcessor.ClearRedundantLocalPropertiesRecursive(headerFooterBase);
				}
				headerFooterBase = section.Footers.Even;
				if (headerFooterBase != null)
				{
					RedundantPropertiesProcessor.ClearRedundantLocalPropertiesRecursive(headerFooterBase);
				}
			}
		}

		static void ClearRedundantLocalProperties(DocumentElementPropertiesBase documentElementProperties)
		{
			foreach (IStyleProperty styleProperty in documentElementProperties.StyleProperties)
			{
				if (styleProperty.HasLocalValue)
				{
					object actualValueAsObject = styleProperty.GetActualValueAsObject();
					if (actualValueAsObject != null)
					{
						styleProperty.ClearValue();
						object actualValueAsObject2 = styleProperty.GetActualValueAsObject();
						if (!actualValueAsObject.Equals(actualValueAsObject2))
						{
							styleProperty.SetValueAsObject(actualValueAsObject);
						}
					}
				}
			}
		}
	}
}
