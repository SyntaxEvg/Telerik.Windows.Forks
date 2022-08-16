using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Parts;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model
{
	class DocxPartsFactory
	{
		static DocxPartsFactory()
		{
			DocxPartsFactory.RegisterFactoryMethod(DocxContentTypeNames.DocumentContentType, (DocxPartsManager pm, string pn) => new DocumentPart(pm, pn));
			DocxPartsFactory.RegisterFactoryMethod(DocxContentTypeNames.HeaderContentType, (DocxPartsManager pm, string pn) => new HeaderPart(pm, pn));
			DocxPartsFactory.RegisterFactoryMethod(DocxContentTypeNames.FooterContentType, (DocxPartsManager pm, string pn) => new FooterPart(pm, pn));
			DocxPartsFactory.RegisterFactoryMethod(DocxContentTypeNames.DocumentSettingsContentType, (DocxPartsManager pm, string pn) => new DocumentSettingsPart(pm, pn));
			DocxPartsFactory.RegisterFactoryMethod(DocxContentTypeNames.StylesContentType, (DocxPartsManager pm, string pn) => new StylesPart(pm, pn));
			DocxPartsFactory.RegisterFactoryMethod(DocxContentTypeNames.CommentsContentType, (DocxPartsManager pm, string pn) => new CommentsPart(pm, pn));
			DocxPartsFactory.RegisterFactoryMethod(DocxContentTypeNames.ListsContentType, (DocxPartsManager pm, string pn) => new ListsPart(pm, pn));
		}

		public static DocxPartBase CreateInstance(string partType, DocxPartsManager partsManager, string partName)
		{
			Guard.ThrowExceptionIfNullOrEmpty(partType, "partType");
			Guard.ThrowExceptionIfNull<DocxPartsManager>(partsManager, "partsManager");
			Guard.ThrowExceptionIfNullOrEmpty(partName, "partName");
			return DocxPartsFactory.elementNameToFactoryMethod[partType](partsManager, partName);
		}

		public static bool CanCreateInstance(string partType)
		{
			Guard.ThrowExceptionIfNullOrEmpty(partType, "partType");
			return DocxPartsFactory.elementNameToFactoryMethod.ContainsKey(partType);
		}

		static void RegisterFactoryMethod(string partType, Func<DocxPartsManager, string, DocxPartBase> factoryMethod)
		{
			Guard.ThrowExceptionIfNullOrEmpty(partType, "partType");
			Guard.ThrowExceptionIfNull<Func<DocxPartsManager, string, DocxPartBase>>(factoryMethod, "factoryMethod");
			DocxPartsFactory.elementNameToFactoryMethod.Add(partType, factoryMethod);
		}

		static readonly Dictionary<string, Func<DocxPartsManager, string, DocxPartBase>> elementNameToFactoryMethod = new Dictionary<string, Func<DocxPartsManager, string, DocxPartBase>>();
	}
}
