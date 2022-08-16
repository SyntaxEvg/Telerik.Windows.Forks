using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.TextBased.Csv;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.TextBased.Txt;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders
{
	public class WorkbookFormatProvidersManager
	{
		public static IEnumerable<IWorkbookFormatProvider> FormatProviders
		{
			get
			{
				return WorkbookFormatProvidersManager.Instance.formatProviders;
			}
		}

		WorkbookFormatProvidersManager()
		{
			this.formatProviders = new List<IWorkbookFormatProvider>();
			this.InitBuiltInProviders();
		}

		internal static IEnumerable<string> GetSupportedExtensions(IEnumerable<IWorkbookFormatProvider> formatProviders)
		{
			return WorkbookFormatProvidersManager.GetSupportedExtensionsInternal(formatProviders);
		}

		void InitBuiltInProviders()
		{
			this.RegisterFormatProviderInternal(new TxtFormatProvider());
			this.RegisterFormatProviderInternal(new CsvFormatProvider());
		}

		static IEnumerable<string> GetSupportedExtensionsInternal(IEnumerable<IWorkbookFormatProvider> formatProviders)
		{
			return formatProviders.Reverse<IWorkbookFormatProvider>().SelectMany((IWorkbookFormatProvider p) => p.SupportedExtensions).Distinct<string>();
		}

		IWorkbookFormatProvider GetProviderByNameInternal(string providerName)
		{
			return this.formatProviders.FirstOrDefault((IWorkbookFormatProvider p) => p.Name == providerName);
		}

		static IWorkbookFormatProvider GetProviderByExtensionInternal(string extension, IEnumerable<IWorkbookFormatProvider> formatProviders)
		{
			Guard.ThrowExceptionIfNullOrEmpty(extension, "extension");
			extension = extension.ToLowerInvariant();
			if (!extension.StartsWith("."))
			{
				extension = "." + extension;
			}
			return formatProviders.FirstOrDefault((IWorkbookFormatProvider fp) => fp.SupportedExtensions.Contains(extension));
		}

		void RegisterFormatProviderInternal(IWorkbookFormatProvider provider)
		{
			Guard.ThrowExceptionIfNull<IWorkbookFormatProvider>(provider, "provider");
			this.RemoveFormatProviderByName(provider.Name);
			this.formatProviders.Add(provider);
		}

		void UnregisterFormatProviderInternal(IWorkbookFormatProvider provider)
		{
			Guard.ThrowExceptionIfNull<IWorkbookFormatProvider>(provider, "provider");
			this.RemoveFormatProviderByName(provider.Name);
		}

		void RemoveFormatProviderByName(string name)
		{
			IWorkbookFormatProvider providerByNameInternal = this.GetProviderByNameInternal(name);
			if (providerByNameInternal != null)
			{
				this.formatProviders.Remove(providerByNameInternal);
			}
		}

		public static IEnumerable<string> GetSupportedExtensions()
		{
			return WorkbookFormatProvidersManager.GetSupportedExtensions(WorkbookFormatProvidersManager.FormatProviders);
		}

		public static IWorkbookFormatProvider GetProviderByName(string providerName)
		{
			return WorkbookFormatProvidersManager.Instance.GetProviderByNameInternal(providerName);
		}

		public static IWorkbookFormatProvider GetProviderByExtension(string extension)
		{
			return WorkbookFormatProvidersManager.GetProviderByExtensionInternal(extension, WorkbookFormatProvidersManager.FormatProviders);
		}

		public static void RegisterFormatProvider(IWorkbookFormatProvider provider)
		{
			WorkbookFormatProvidersManager.Instance.RegisterFormatProviderInternal(provider);
		}

		public static void UnregisterFormatProvider(IWorkbookFormatProvider provider)
		{
			WorkbookFormatProvidersManager.Instance.UnregisterFormatProviderInternal(provider);
		}

		public static Workbook Import(string extension, Stream input)
		{
			return WorkbookFormatProvidersManager.Import(extension, input, WorkbookFormatProvidersManager.FormatProviders);
		}

		public static Workbook Import(string extension, Stream input, IEnumerable<IWorkbookFormatProvider> formatProviders)
		{
			IWorkbookFormatProvider providerByExtensionInternal = WorkbookFormatProvidersManager.GetProviderByExtensionInternal(extension, formatProviders);
			if (providerByExtensionInternal == null)
			{
				throw new UnsupportedFileFormatException(extension);
			}
			return providerByExtensionInternal.Import(input);
		}

		public static void Export(Workbook workbook, string extension, Stream output)
		{
			WorkbookFormatProvidersManager.Export(workbook, extension, output, WorkbookFormatProvidersManager.FormatProviders);
		}

		public static void Export(Workbook workbook, string extension, Stream output, IEnumerable<IWorkbookFormatProvider> formatProviders)
		{
			IWorkbookFormatProvider providerByExtensionInternal = WorkbookFormatProvidersManager.GetProviderByExtensionInternal(extension, formatProviders);
			if (providerByExtensionInternal == null)
			{
				throw new UnsupportedFileFormatException(extension);
			}
			providerByExtensionInternal.Export(workbook, output);
		}

		static readonly WorkbookFormatProvidersManager Instance = new WorkbookFormatProvidersManager();

		readonly List<IWorkbookFormatProvider> formatProviders;
	}
}
