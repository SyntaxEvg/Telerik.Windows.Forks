using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Telerik.Windows.Zip;

namespace Telerik.Documents.SpreadsheetStreaming.Utilities
{
	static class ResourcesHelper
	{
		static ResourcesHelper()
		{
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			string text = executingAssembly.FullName;
			int length = text.IndexOf(',');
			text = text.Substring(0, length);
			ResourcesHelper.resources = new Dictionary<string, string>();
			ResourcesHelper.resources.Add(text + ".Resources.xlsxContents.xl.theme.theme1.xml", "xl/theme/theme1.xml");
			ResourcesHelper.resources.Add(text + ".Resources.xlsxContents._rels..rels", "_rels/.rels");
		}

		internal static void CopyResourcesToZipArchive(ZipArchive archive, ContentTypesRepository contentTypes)
		{
			contentTypes.Register("xl/theme/theme1.xml", "application/vnd.openxmlformats-officedocument.theme+xml");
			using (Dictionary<string, string>.Enumerator enumerator = ResourcesHelper.resources.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<string, string> pair = enumerator.Current;
					if (!archive.Entries.Where(delegate(ZipArchiveEntry p)
					{
						string fullName = p.FullName;
						KeyValuePair<string, string> pair3 = pair;
						return fullName == pair3.Value;
					}).Any<ZipArchiveEntry>())
					{
						Assembly executingAssembly = Assembly.GetExecutingAssembly();
						KeyValuePair<string, string> pair4 = pair;
						using (ZipArchiveEntry zipArchiveEntry = archive.CreateEntry(pair4.Value))
						{
							using (Stream stream = zipArchiveEntry.Open())
							{
								Assembly assembly = executingAssembly;
								KeyValuePair<string, string> pair2 = pair;
								using (Stream manifestResourceStream = assembly.GetManifestResourceStream(pair2.Key))
								{
									manifestResourceStream.CopyTo(stream);
								}
							}
						}
					}
				}
			}
		}

		static readonly Dictionary<string, string> resources;
	}
}
