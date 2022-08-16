using System;
using Telerik.Windows.Documents.Common.FormatProviders.OpenXml.Utilities;
using Telerik.Windows.Documents.Core.Data;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Export;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Import;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Parts
{
	abstract class OpenXmlPartBase : IStackCollectionElement
	{
		public OpenXmlPartBase(OpenXmlPartsManager partsManager, string name)
		{
			Guard.ThrowExceptionIfNull<OpenXmlPartsManager>(partsManager, "partsManager");
			Guard.ThrowExceptionIfNullOrEmpty(name, "name");
			this.partsManager = partsManager;
			this.name = name;
		}

		public string Name
		{
			get
			{
				return this.name;
			}
		}

		public OpenXmlPartsManager PartsManager
		{
			get
			{
				return this.partsManager;
			}
		}

		public virtual bool OverrideDefaultContentType
		{
			get
			{
				return true;
			}
		}

		public abstract string ContentType { get; }

		public abstract int Level { get; }

		public abstract OpenXmlElementBase RootElement { get; }

		public virtual void Import(IOpenXmlReader reader, IOpenXmlImportContext context)
		{
			this.RootElement.Read(reader, context);
		}

		public virtual void Export(IOpenXmlWriter writer, IOpenXmlExportContext context)
		{
			this.RootElement.Write(writer, context);
		}

		public string GetResourceName(string target)
		{
			return PathHelper.Combine(this.Name, target);
		}

		public override string ToString()
		{
			return this.Name;
		}

		readonly OpenXmlPartsManager partsManager;

		readonly string name;
	}
}
