using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Export;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Import;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Common;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Parts;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.ContentTypes
{
	class TypesElement : OpenXmlPartRootElementBase
	{
		public TypesElement(OpenXmlPartsManager partsManager, OpenXmlPartBase part)
			: base(partsManager, part)
		{
			this.defaults = new Dictionary<string, string>();
			this.overrides = new Dictionary<string, string>();
		}

		public override OpenXmlNamespace Namespace
		{
			get
			{
				return OpenXmlNamespaces.ContentTypesNamespace;
			}
		}

		public override string ElementName
		{
			get
			{
				return "Types";
			}
		}

		public OpenXmlPartBase CreatePart(string partName)
		{
			Guard.ThrowExceptionIfNullOrEmpty(partName, "partName");
			string partType;
			if (!this.overrides.TryGetValue(partName, out partType))
			{
				string extension = OpenXmlHelper.GetExtension(partName);
				if (!this.defaults.TryGetValue(extension, out partType))
				{
					return null;
				}
			}
			return base.PartsManager.CreatePart(partType, partName);
		}

		public void RegisterDefault(string extension, string contentType)
		{
			Guard.ThrowExceptionIfNullOrEmpty(extension, "extension");
			Guard.ThrowExceptionIfNullOrEmpty(contentType, "contentType");
			this.defaults[extension] = contentType;
		}

		public void RegisterOverride(string partName, string contentType)
		{
			Guard.ThrowExceptionIfNullOrEmpty(partName, "partName");
			Guard.ThrowExceptionIfNullOrEmpty(contentType, "contentType");
			this.overrides[partName] = contentType;
		}

		protected override IEnumerable<OpenXmlElementBase> EnumerateChildElements(IOpenXmlExportContext context)
		{
			Guard.ThrowExceptionIfNull<IOpenXmlExportContext>(context, "context");
			foreach (KeyValuePair<string, string> d in this.defaults)
			{
				DefaultElement defaultElement = base.CreateElement<DefaultElement>("Default");
				DefaultElement defaultElement2 = defaultElement;
				KeyValuePair<string, string> keyValuePair = d;
				defaultElement2.Extension = keyValuePair.Key;
				DefaultElement defaultElement3 = defaultElement;
				KeyValuePair<string, string> keyValuePair2 = d;
				defaultElement3.ContentType = keyValuePair2.Value;
				yield return defaultElement;
			}
			foreach (KeyValuePair<string, string> o in this.overrides)
			{
				OverrideElement overrideElement = base.CreateElement<OverrideElement>("Override");
				OverrideElement overrideElement2 = overrideElement;
				KeyValuePair<string, string> keyValuePair3 = o;
				overrideElement2.ContentType = keyValuePair3.Value;
				OverrideElement overrideElement3 = overrideElement;
				KeyValuePair<string, string> keyValuePair4 = o;
				overrideElement3.PartName = keyValuePair4.Key;
				yield return overrideElement;
			}
			yield break;
		}

		protected override void OnAfterReadChildElement(IOpenXmlImportContext context, OpenXmlElementBase element)
		{
			Guard.ThrowExceptionIfNull<IOpenXmlImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<OpenXmlElementBase>(element, "element");
			string elementName;
			if ((elementName = element.ElementName) != null)
			{
				if (elementName == "Default")
				{
					DefaultElement defaultElement = (DefaultElement)element;
					this.RegisterDefault(defaultElement.Extension, defaultElement.ContentType);
					return;
				}
				if (!(elementName == "Override"))
				{
					return;
				}
				OverrideElement overrideElement = (OverrideElement)element;
				this.RegisterOverride(overrideElement.PartName, overrideElement.ContentType);
			}
		}

		protected override void ClearOverride()
		{
			this.defaults.Clear();
			this.overrides.Clear();
		}

		readonly Dictionary<string, string> defaults;

		readonly Dictionary<string, string> overrides;
	}
}
