using System;
using System.Collections.Generic;
using System.IO;
using Telerik.Windows.Documents.Media;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Common.Model.Data
{
	class ResourcesFactory
	{
		static ResourcesFactory()
		{
			ResourcesFactory.RegisterFactoryMethod("jpg", (Stream stream, string extension) => new ImageSource(stream, extension));
			ResourcesFactory.RegisterFactoryMethod("jpeg", (Stream stream, string extension) => new ImageSource(stream, extension));
			ResourcesFactory.RegisterFactoryMethod("png", (Stream stream, string extension) => new ImageSource(stream, extension));
			ResourcesFactory.RegisterFactoryMethod("bmp", (Stream stream, string extension) => new ImageSource(stream, extension));
			ResourcesFactory.RegisterFactoryMethod("tiff", (Stream stream, string extension) => new ImageSource(stream, extension));
			ResourcesFactory.RegisterFactoryMethod("tif", (Stream stream, string extension) => new ImageSource(stream, extension));
			ResourcesFactory.RegisterFactoryMethod("gif", (Stream stream, string extension) => new ImageSource(stream, extension));
			ResourcesFactory.RegisterFactoryMethod("icon", (Stream stream, string extension) => new ImageSource(stream, extension));
			ResourcesFactory.RegisterFactoryMethod("wmf", (Stream stream, string extension) => new ImageSource(stream, extension));
			ResourcesFactory.RegisterFactoryMethod("emf", (Stream stream, string extension) => new ImageSource(stream, extension));
		}

		public static IResource CreateInstance(Stream stream, string extension)
		{
			extension = extension.ToLowerInvariant();
			return ResourcesFactory.extensionToResourceMapping[extension](stream, extension);
		}

		public static bool CanCreateInstance(string extension)
		{
			Guard.ThrowExceptionIfNullOrEmpty(extension, "extension");
			extension = extension.ToLowerInvariant();
			return ResourcesFactory.extensionToResourceMapping.ContainsKey(extension);
		}

		static void RegisterFactoryMethod(string extension, Func<Stream, string, IResource> factoryMethod)
		{
			Guard.ThrowExceptionIfNullOrEmpty(extension, "extension");
			Guard.ThrowExceptionIfNull<Func<Stream, string, IResource>>(factoryMethod, "factoryMethod");
			ResourcesFactory.extensionToResourceMapping.Add(extension, factoryMethod);
		}

		static readonly Dictionary<string, Func<Stream, string, IResource>> extensionToResourceMapping = new Dictionary<string, Func<Stream, string, IResource>>();
	}
}
