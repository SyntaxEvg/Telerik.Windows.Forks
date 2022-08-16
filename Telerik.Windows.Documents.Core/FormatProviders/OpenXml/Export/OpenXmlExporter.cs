using System;
using System.IO;
using System.Linq;
using System.Xml;
using Telerik.Windows.Documents.Common.FormatProviders.OpenXml.Utilities;
using Telerik.Windows.Documents.Common.Model.Data;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Parts;
using Telerik.Windows.Documents.Utilities;
using Telerik.Windows.Zip;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Export
{
	abstract class OpenXmlExporter<T, TContext> where T : OpenXmlPartsManager where TContext : IOpenXmlExportContext
	{
		public OpenXmlExporter()
		{
			this.zipPackageCreationDateTime = DateTime.Now;
		}

		public void Export(Stream output, TContext context, OpenXmlExportSettings settings)
		{
			Guard.ThrowExceptionIfNull<Stream>(output, "output");
			Guard.ThrowExceptionIfNull<TContext>(context, "context");
			Guard.ThrowExceptionIfNull<OpenXmlExportSettings>(settings, "settings");
			using (ZipArchive zipArchive = new ZipArchive(output, ZipArchiveMode.Create, true, null))
			{
				T partsManager = this.CreatePartsManager();
				this.InitializeParts(partsManager, context);
				foreach (IResource resource in context.Resources)
				{
					string name = this.CreateResourceName(resource);
					partsManager.RegisterResource(resource.Extension);
					using (Stream stream = new MemoryStream(resource.Data))
					{
						OpenXmlExporter<T, TContext>.AddResourceToArchive(zipArchive, name, stream);
					}
				}
				int level;
				for (level = 6; level >= 0; level--)
				{
					OpenXmlPartBase[] array = (from p in partsManager.Parts
						where p.Level == level
						select p).ToArray<OpenXmlPartBase>();
					foreach (OpenXmlPartBase openXmlPartBase in array)
					{
						this.AddOpenPartToArchive(zipArchive, OpenXmlExporter<T, TContext>.ExportPart(openXmlPartBase, context), openXmlPartBase.Name);
					}
				}
			}
		}

		protected static void AddResourceToArchive(ZipArchive archive, string name, Stream stream)
		{
			Guard.ThrowExceptionIfNull<ZipArchive>(archive, "package");
			Guard.ThrowExceptionIfNullOrEmpty(name, "name");
			Guard.ThrowExceptionIfNull<Stream>(stream, "stream");
			using (ZipArchiveEntry zipArchiveEntry = archive.CreateEntry(PathHelper.NormalizePathForExport(name)))
			{
				using (Stream stream2 = zipArchiveEntry.Open())
				{
					stream.CopyTo(stream2);
				}
			}
		}

		protected static Stream ExportPart(OpenXmlPartBase part, TContext context)
		{
			Guard.ThrowExceptionIfNull<OpenXmlPartBase>(part, "part");
			Stream result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (XmlWriter xmlWriter = XmlWriter.Create(memoryStream))
				{
					IOpenXmlWriter writer = new OpenXmlPartWriter(xmlWriter);
					part.Export(writer, context);
					xmlWriter.Flush();
					result = new MemoryStream(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
				}
			}
			return result;
		}

		protected abstract T CreatePartsManager();

		protected abstract string CreateResourceName(IResource resource);

		protected virtual void InitializeParts(T partsManager, TContext context)
		{
			Guard.ThrowExceptionIfNull<T>(partsManager, "partsManager");
			Guard.ThrowExceptionIfNull<TContext>(context, "context");
			partsManager.AddPart(new ContentTypesPart(partsManager));
		}

		protected void AddOpenPartToArchive(ZipArchive archive, Stream partStream, string partName)
		{
			Guard.ThrowExceptionIfNull<ZipArchive>(archive, "archive");
			DeflateSettings deflateSettings = new DeflateSettings();
			deflateSettings.CompressionLevel = CompressionLevel.Fastest;
			using (ZipArchiveEntry zipArchiveEntry = archive.CreateEntry(PathHelper.NormalizePathForExport(partName), deflateSettings))
			{
				using (Stream stream = zipArchiveEntry.Open())
				{
					zipArchiveEntry.LastWriteTime = this.zipPackageCreationDateTime;
					partStream.CopyTo(stream);
				}
			}
		}

		readonly DateTime zipPackageCreationDateTime;
	}
}
