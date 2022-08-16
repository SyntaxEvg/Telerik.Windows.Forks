using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Parts;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model
{
	class OpenXmlPartsFactory
	{
		static OpenXmlPartsFactory()
		{
			OpenXmlPartsFactory.RegisterFactoryMethod(OpenXmlContentTypeNames.RelationshipContentType, (OpenXmlPartsManager pm, string pn) => new RelationshipsPart(pm, pn));
			OpenXmlPartsFactory.RegisterFactoryMethod(OpenXmlContentTypeNames.ThemeContentType, (OpenXmlPartsManager pm, string pn) => new ThemePart(pm, pn));
			OpenXmlPartsFactory.RegisterFactoryMethod(OpenXmlContentTypeNames.ChartContentType, (OpenXmlPartsManager pm, string pn) => new ChartPart(pm, pn));
		}

		public static OpenXmlPartBase CreateInstance(string partType, OpenXmlPartsManager partsManager, string partName)
		{
			Guard.ThrowExceptionIfNullOrEmpty(partType, "partType");
			Guard.ThrowExceptionIfNull<OpenXmlPartsManager>(partsManager, "partsManager");
			Guard.ThrowExceptionIfNullOrEmpty(partName, "partName");
			return OpenXmlPartsFactory.elementNameToFactoryMethod[partType](partsManager, partName);
		}

		public static bool CanCreateInstance(string partType)
		{
			Guard.ThrowExceptionIfNullOrEmpty(partType, "partType");
			return OpenXmlPartsFactory.elementNameToFactoryMethod.ContainsKey(partType);
		}

		static void RegisterFactoryMethod(string partType, Func<OpenXmlPartsManager, string, OpenXmlPartBase> factoryMethod)
		{
			Guard.ThrowExceptionIfNullOrEmpty(partType, "partType");
			Guard.ThrowExceptionIfNull<Func<OpenXmlPartsManager, string, OpenXmlPartBase>>(factoryMethod, "factoryMethod");
			OpenXmlPartsFactory.elementNameToFactoryMethod.Add(partType, factoryMethod);
		}

		static readonly Dictionary<string, Func<OpenXmlPartsManager, string, OpenXmlPartBase>> elementNameToFactoryMethod = new Dictionary<string, Func<OpenXmlPartsManager, string, OpenXmlPartBase>>();
	}
}
