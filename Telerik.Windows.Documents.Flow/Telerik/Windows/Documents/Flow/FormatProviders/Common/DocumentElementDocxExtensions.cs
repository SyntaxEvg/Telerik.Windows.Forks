using System;
using Telerik.Windows.Documents.Flow.Model;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Common
{
	static class DocumentElementDocxExtensions
	{
		public static bool IsLastInSectionButNotLastInDocument(this Paragraph paragraph, Section parentSection)
		{
			if (parentSection == null)
			{
				return false;
			}
			if (parentSection.IsLastSectionInDocument())
			{
				return false;
			}
			if (parentSection.Blocks.Count == 0)
			{
				return false;
			}
			int index = parentSection.Blocks.Count - 1;
			return paragraph == parentSection.Blocks[index];
		}

		static bool IsLastSectionInDocument(this Section section)
		{
			if (section.Document.Sections.Count == 0)
			{
				return false;
			}
			int index = section.Document.Sections.Count - 1;
			return section == section.Document.Sections[index];
		}
	}
}
