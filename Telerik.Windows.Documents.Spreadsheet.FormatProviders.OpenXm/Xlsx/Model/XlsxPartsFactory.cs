using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Parts;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model
{
	static class XlsxPartsFactory
	{
		static XlsxPartsFactory()
		{
			XlsxPartsFactory.RegisterFactoryMethod(XlsxContentTypeNames.SharedStringsContentType, (XlsxPartsManager pm, string pn) => new SharedStringsPart(pm, pn));
			XlsxPartsFactory.RegisterFactoryMethod(XlsxContentTypeNames.StylesContentType, (XlsxPartsManager pm, string pn) => new StylesPart(pm, pn));
			XlsxPartsFactory.RegisterFactoryMethod(XlsxContentTypeNames.WorkbookContentType, (XlsxPartsManager pm, string pn) => new WorkbookPart(pm, pn));
			XlsxPartsFactory.RegisterFactoryMethod(XlsxContentTypeNames.MacroEnabledWorkbookContentType, (XlsxPartsManager pm, string pn) => new WorkbookPart(pm, pn));
			XlsxPartsFactory.RegisterFactoryMethod(XlsxContentTypeNames.WorksheetContentType, (XlsxPartsManager pm, string pn) => new WorksheetPart(pm, pn));
			XlsxPartsFactory.RegisterFactoryMethod(XlsxContentTypeNames.DrawingContentType, (XlsxPartsManager pm, string pn) => new DrawingPart(pm, pn));
		}

		public static XlsxPartBase CreateInstance(string partType, XlsxPartsManager partsManager, string partName)
		{
			Guard.ThrowExceptionIfNullOrEmpty(partType, "partType");
			Guard.ThrowExceptionIfNull<XlsxPartsManager>(partsManager, "partsManager");
			Func<XlsxPartsManager, string, XlsxPartBase> func;
			if (!XlsxPartsFactory.elementNameToFactoryMethod.TryGetValue(partType, out func))
			{
				return null;
			}
			return func(partsManager, partName);
		}

		public static bool CanCreateInstance(string partType)
		{
			Guard.ThrowExceptionIfNullOrEmpty(partType, "partType");
			return XlsxPartsFactory.elementNameToFactoryMethod.ContainsKey(partType);
		}

		static void RegisterFactoryMethod(string partType, Func<XlsxPartsManager, string, XlsxPartBase> factoryMethod)
		{
			Guard.ThrowExceptionIfNullOrEmpty(partType, "partType");
			Guard.ThrowExceptionIfNull<Func<XlsxPartsManager, string, XlsxPartBase>>(factoryMethod, "factoryMethod");
			XlsxPartsFactory.elementNameToFactoryMethod.Add(partType, factoryMethod);
		}

		static readonly Dictionary<string, Func<XlsxPartsManager, string, XlsxPartBase>> elementNameToFactoryMethod = new Dictionary<string, Func<XlsxPartsManager, string, XlsxPartBase>>();
	}
}
