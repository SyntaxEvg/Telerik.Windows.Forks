using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using Telerik.Windows.Documents.Common.FormatProviders.OpenXml.Utilities;
using Telerik.Windows.Documents.Common.Model.Data;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Import;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Parts;
using Telerik.Windows.Documents.Utilities;
using Telerik.Windows.Zip;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml
{
	abstract class OpenXmlImporter<T> where T : OpenXmlPartsManager
	{
		public void Import(Stream input, IOpenXmlImportContext context)
		{
			Guard.ThrowExceptionIfNull<Stream>(input, "input");
			Guard.ThrowExceptionIfNull<IOpenXmlImportContext>(context, "context");
			using (ZipArchive zipArchive = new ZipArchive(input, ZipArchiveMode.Read, true, null))
			{
				Dictionary<string, ZipArchiveEntry> zipEntries = OpenXmlImporter<T>.GetZipEntries(zipArchive);
				T t = this.CreatePartsManager();
				context.BeginImport();
				ContentTypesPart contentTypesPart = new ContentTypesPart(t);
				t.AddPart(contentTypesPart);
				OpenXmlImporter<T>.ImportXlsxPartFromArchive(zipEntries[PathHelper.NormalizePathForExport(contentTypesPart.Name)], contentTypesPart, context);
				OpenXmlImporter<T>.PrepareAllParts(t, zipEntries, contentTypesPart);
				int level;
				for (level = 1; level <= 6; level++)
				{
					foreach (OpenXmlPartBase openXmlPartBase in from p in t.Parts
						where p.Level == level
						select p)
					{
						OpenXmlImporter<T>.ImportXlsxPartFromArchive(zipEntries[PathHelper.NormalizePathForExport(openXmlPartBase.Name)], openXmlPartBase, context);
					}
					if (level == 2)
					{
						OpenXmlImporter<T>.ImportResources(context, zipEntries);
					}
				}
				context.EndImport();
				foreach (KeyValuePair<string, ZipArchiveEntry> keyValuePair in zipEntries)
				{
					keyValuePair.Value.Dispose();
				}
			}
		}

		protected abstract T CreatePartsManager();

		static Dictionary<string, ZipArchiveEntry> GetZipEntries(ZipArchive archive)
		{
			Guard.ThrowExceptionIfNull<ZipArchive>(archive, "archive");
			Dictionary<string, ZipArchiveEntry> dictionary = new Dictionary<string, ZipArchiveEntry>();
			foreach (ZipArchiveEntry zipArchiveEntry in archive.Entries)
			{
				dictionary[PathHelper.NormalizePath(zipArchiveEntry.FullName)] = zipArchiveEntry;
			}
			return dictionary;
		}

		static void GetStreamFromZipPackage(ZipArchiveEntry entry, out Stream stream)
		{
			Guard.ThrowExceptionIfNull<ZipArchiveEntry>(entry, "entry");
			stream = new MemoryStream();
			using (Stream stream2 = entry.Open())
			{
				stream2.CopyTo(stream);
			}
			stream.Seek(0L, SeekOrigin.Begin);
		}

		static void ImportXlsxPartFromArchive(ZipArchiveEntry zipEntry, OpenXmlPartBase part, IOpenXmlImportContext context)
		{
			Guard.ThrowExceptionIfNull<ZipArchiveEntry>(zipEntry, "zipEntry");
			Guard.ThrowExceptionIfNull<OpenXmlPartBase>(part, "part");
			Stream stream;
			OpenXmlImporter<T>.GetStreamFromZipPackage(zipEntry, out stream);
			using (stream)
			{
				XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
				xmlReaderSettings.IgnoreComments = true;
				xmlReaderSettings.IgnoreWhitespace = true;
				IOpenXmlReader reader = new OpenXmlPartReader(XmlReader.Create(stream, xmlReaderSettings));
				part.Import(reader, context);
			}
		}

		static void ImportResources(IOpenXmlImportContext context, Dictionary<string, ZipArchiveEntry> zipEntries)
		{
			foreach (KeyValuePair<string, ZipArchiveEntry> keyValuePair in zipEntries)
			{
				string text = PathHelper.NormalizePathFromImport(keyValuePair.Key);
				string extension = OpenXmlHelper.GetExtension(text);
				if (ResourcesFactory.CanCreateInstance(extension))
				{
					Stream stream = null;
					try
					{
						OpenXmlImporter<T>.GetStreamFromZipPackage(keyValuePair.Value, out stream);
						context.RegisterResource(text, ResourcesFactory.CreateInstance(stream, extension));
					}
					finally
					{
						if (stream != null)
						{
							stream.Close();
						}
					}
				}
			}
		}

		static void PrepareAllParts(OpenXmlPartsManager partsManager, Dictionary<string, ZipArchiveEntry> zipEntries, ContentTypesPart contentTypesPart)
		{
			foreach (KeyValuePair<string, ZipArchiveEntry> keyValuePair in zipEntries)
			{
				string partName = PathHelper.NormalizePathFromImport(keyValuePair.Key);
				if (!partsManager.ContainsPart(partName))
				{
					OpenXmlPartBase openXmlPartBase = contentTypesPart.CreatePart(partName);
					if (openXmlPartBase != null)
					{
						partsManager.AddPart(openXmlPartBase);
					}
				}
			}
		}

		const int ResourcesLevel = 2;
	}
}
